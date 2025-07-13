using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
//Layout q utilizo nos 3 ancoras,scripts nos gameobjects, para fujncionar a troca de dll e csharp
public class BroadmarkManager : MonoBehaviour
{
    //strucklayout [in] [out] para correçao de bugs, de garbagecollector e gerenciar memoria
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Vec3
    {
        //precisa do w, pois na dll sao 4 valores o vec, q eh como o broadmark funciona
        public float x, y, z, w;
        public Vec3(float x, float y, float z)
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
    //conjunto de dllimport, as funcoes q o algoritmousa
    [DllImport("bf", CallingConvention = CallingConvention.Cdecl)]
    private static extern void BF_Create();

    [DllImport("bf", CallingConvention = CallingConvention.Cdecl)]
    private static extern void BF_InitializeWithVecs(
        int numberOfObjects, [In] Aabb[] aabbs,
        float worldMinX, float worldMinY, float worldMinZ,
        float worldMaxX, float worldMaxY, float worldMaxZ,
        float marginX, float marginY, float marginZ
    );

    [DllImport("bf", CallingConvention = CallingConvention.Cdecl)]
    private static extern void BF_UpdateObjects([In] Aabb[] aabbs);

    [DllImport("bf", CallingConvention = CallingConvention.Cdecl)]
    private static extern void BF_SearchOverlaps();

    [DllImport("bf", CallingConvention = CallingConvention.Cdecl)]
    private static extern UIntPtr BF_GetPairCount();

    [DllImport("bf", CallingConvention = CallingConvention.Cdecl)]
    private static extern void BF_Destroy();

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
            RunBroadPhaseTest();
            yield return new WaitForSeconds(0.02f); // 50 fps
        }
    }

    void RunBroadPhaseTest()
    {
        try
        {
            var objects = FindObjectsOfType<AABBObjectController>();

            if (objects.Length == 0)
            {
                //semrpe vai aparecer, o primeiro loop é mais rapido q o spawn
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

            // Resetar cor dos objetos
           // foreach (var obj in objects)
              //  obj.SetColor(Color.green);

            if (!isInitialized || objects.Length != lastObjectCount)
            {
                BF_Destroy();
                BF_Create();

                BF_InitializeWithVecs(
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
                BF_UpdateObjects(aabbs);
            }

            BF_SearchOverlaps();

            ulong pairCount = BF_GetPairCount().ToUInt64();
            totalOverlaps += pairCount;

            if (collisionUIController != null)
                collisionUIController.UpdateCollisionCount(pairCount);

            /* Validação visual C#
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
            Debug.LogError($" Erro no Broadmark: {e.Message}");
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
        Debug.Log(" Limpando Broadmark na destruição.");
        BF_Destroy();
    }
}
