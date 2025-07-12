using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class TracyManager : MonoBehaviour
{//Layout q utilizo nos 3 ancoras,scripts nos gameobjects, para fujncionar a troca de dll e csharp
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Vec3 //strucklayout [in] [out] para correçao de bugs, de garbagecollector e gerenciar memoria
    {
        public float x, y, z, w;
        public Vec3(float x, float y, float z) //precisa do w, pois na dll sao 4 valores o vec, q eh como o broadmark funciona
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 0f;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Aabb
    {
        public Vec3 min;
        public Vec3 max;
    }
     //conjunto de dllimport, as funcoes q o algoritmo usa
    [DllImport("BFnTracy", CallingConvention = CallingConvention.Cdecl)]
    private static extern void TRACY_Create();

    [DllImport("BFnTracy", CallingConvention = CallingConvention.Cdecl)]
    private static extern void TRACY_InitializeWithVecs(
        int numberOfObjects, [In] Aabb[] aabbs,
        float worldMinX, float worldMinY, float worldMinZ,
        float worldMaxX, float worldMaxY, float worldMaxZ,
        float marginX, float marginY, float marginZ
    );

    [DllImport("BFnTracy", CallingConvention = CallingConvention.Cdecl)]
    private static extern void TRACY_UpdateObjects([In] Aabb[] aabbs);

    [DllImport("BFnTracy", CallingConvention = CallingConvention.Cdecl)]
    private static extern void TRACY_CleanCache();

    [DllImport("BFnTracy", CallingConvention = CallingConvention.Cdecl)]
    private static extern void TRACY_SearchOverlaps();

    [DllImport("BFnTracy", CallingConvention = CallingConvention.Cdecl)]
    private static extern UIntPtr TRACY_GetPairCount();

    [DllImport("BFnTracy", CallingConvention = CallingConvention.Cdecl)]
    private static extern void TRACY_Destroy();

    private ulong totalOverlaps = 0;
    private int lastObjectCount = 0;
    private bool isInitialized = false;

    [Header("UI")]
    public CollisionUIController collisionUIController;

    void Start()
    {
        StartCoroutine(UpdateOverlapsLoop());
    }

    IEnumerator UpdateOverlapsLoop()
    {
        while (true)
        {
            RunTracyTest();
            yield return new WaitForSeconds(0.02f); // Atualiza a cada 1 segundo
        }
    }

    void RunTracyTest()
    {
        try
        {
            var objects = FindObjectsOfType<AABBObjectController>();

            if (objects.Length == 0)
            {
                Debug.LogWarning("Nenhum objeto AABB encontrado na cena.");
                isInitialized = false;

                if (collisionUIController != null)
                    collisionUIController.UpdateCollisionCount(0);

                return;
            }

            Aabb[] aabbs = new Aabb[objects.Length];

            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].UpdateAABB();
                aabbs[i] = new Aabb
                {
                    min = new Vec3(objects[i].min.x, objects[i].min.y, objects[i].min.z),
                    max = new Vec3(objects[i].max.x, objects[i].max.y, objects[i].max.z)
                };
            }

            // Resetar cor dos objetos para verde (sem colisão)
            foreach (var obj in objects)
                obj.SetColor(Color.green);

            if (!isInitialized || objects.Length != lastObjectCount)
            {
                TRACY_Destroy();
                TRACY_Create();

                TRACY_InitializeWithVecs(
                    aabbs.Length, aabbs,
                    -100f, -100f, -100f,
                    100f, 100f, 100f,
                    0.01f, 0.01f, 0.01f
                );

                lastObjectCount = aabbs.Length;
                isInitialized = true;
            }
            else
            {
                TRACY_UpdateObjects(aabbs);
            }

            // Limpar cache antes de buscar colisões
            TRACY_CleanCache();

            TRACY_SearchOverlaps();

            ulong pairCount = TRACY_GetPairCount().ToUInt64();
            totalOverlaps = pairCount; // Atualiza o valor, não acumula

            if (collisionUIController != null)
                collisionUIController.UpdateCollisionCount(pairCount);

            /* Validação visual no Unity: pinta de vermelho os objetos em colisão
            for (int i = 0; i < objects.Length; i++)
            {
                for (int j = i + 1; j < objects.Length; j++)
                {
                    if (CheckAABBOverlap(objects[i], objects[j]))
                    {
                        objects[i].SetColor(Color.red);
                        objects[j].SetColor(Color.red);
                    }
                }
            }*/
        }
        catch (Exception e)
        {
            Debug.LogError($" Erro no Tracy: {e.Message}");
        }
    }

    bool CheckAABBOverlap(AABBObjectController a, AABBObjectController b)
    {
        return (a.min.x <= b.max.x && a.max.x >= b.min.x) &&
               (a.min.y <= b.max.y && a.max.y >= b.min.y) &&
               (a.min.z <= b.max.z && a.max.z >= b.min.z);
    }

    void OnDestroy()
    {
        Debug.Log(" Limpando Tracy na destruição.");
        TRACY_Destroy();
    }
}
