using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class KDManager : MonoBehaviour
{
     [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Vec3
    {
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


    [DllImport("kdthook", CallingConvention = CallingConvention.Cdecl)]
    private static extern void KD_Create();

    [DllImport("kdthook", CallingConvention = CallingConvention.Cdecl)]
    private static extern void KD_InitializeWithVecs(
        UIntPtr numberOfObjects, [In] Aabb[] aabbs,
        float worldMinX, float worldMinY, float worldMinZ,
        float worldMaxX, float worldMaxY, float worldMaxZ,
        float marginX, float marginY, float marginZ
    );

    [DllImport("kdthook", CallingConvention = CallingConvention.Cdecl)]
    private static extern void KD_UpdateObjects([In] Aabb[] aabbs);

    [DllImport("kdthook", CallingConvention = CallingConvention.Cdecl)]
    private static extern void KD_CleanCache();

    [DllImport("kdthook", CallingConvention = CallingConvention.Cdecl)]
    private static extern void KD_SearchOverlaps();

    [DllImport("kdthook", CallingConvention = CallingConvention.Cdecl)]
    private static extern UIntPtr KD_GetPairCount();

    [DllImport("kdthook", CallingConvention = CallingConvention.Cdecl)]
    private static extern void KD_Destroy();

    private ulong totalOverlaps = 0;
    private int lastObjectCount = 0;
    private bool isInitialized = false;

    [Header("UI")]
    public CollisionUIController collisionUIController;

    void Start() => StartCoroutine(UpdateOverlapsLoop());

    IEnumerator UpdateOverlapsLoop()
    {
        while (true)
        {
            RunKDTest();
            yield return new WaitForSeconds(0.02f);
        }
    }

    void RunKDTest()
    {
        try
        {
            var objects = FindObjectsOfType<AABBObjectController>();
            if (objects.Length == 0)
            {
                Debug.LogWarning("❗ Nenhum objeto AABB na cena.");
                isInitialized = false;
                collisionUIController?.UpdateCollisionCount(0);
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
                objects[i].SetColor(Color.green);
            }

            if (!isInitialized || objects.Length != lastObjectCount)
            {
                KD_Destroy();
                KD_Create();

                KD_InitializeWithVecs(
                    (UIntPtr)aabbs.Length, aabbs,
                    -100f, -100f, -100f,
                    100f, 100f, 100f,
                    0.01f, 0.01f, 0.01f
                );

                lastObjectCount = aabbs.Length;
                isInitialized = true;
            }
            else
            {
                KD_UpdateObjects(aabbs);
            }

            KD_CleanCache();
            KD_SearchOverlaps();
             

            ulong pairCount = KD_GetPairCount().ToUInt64();
            totalOverlaps = pairCount; // Atualiza o valor, não acumula

            if (collisionUIController != null)
                collisionUIController.UpdateCollisionCount(pairCount);

            /*for (int i = 0; i < objects.Length; i++)
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
            Debug.LogError($"🚨 KDTree Erro: {e.Message}");
        }
    }

    bool CheckAABBOverlap(AABBObjectController a, AABBObjectController b) =>
        a.min.x <= b.max.x && a.max.x >= b.min.x &&
        a.min.y <= b.max.y && a.max.y >= b.min.y &&
        a.min.z <= b.max.z && a.max.z >= b.min.z;

    void OnDestroy()
    {
        Debug.Log("🧹 Limpando KDTree.");
        KD_Destroy();
    }
}
