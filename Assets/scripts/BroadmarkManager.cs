using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class BroadmarkManager : MonoBehaviour
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
            this.w = 0f; // Alinhamento igual ao C++
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Aabb
    {
        public Vec3 min;
        public Vec3 max;
    }

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

    void Start()
    {
        StartCoroutine(UpdateOverlapsLoop());
    }

    IEnumerator UpdateOverlapsLoop()
    {
        while (true)
        {
            RunBroadPhaseTest();
            yield return new WaitForSeconds(0.02f); // Atualiza a cada 20ms
        }
    }

    void RunBroadPhaseTest()
    {
        try
        {
            var objects = FindObjectsOfType<AABBObjectController>();

            if (objects.Length == 0)
            {
                Debug.LogWarning("❗ Nenhum objeto AABB encontrado na cena.");
                isInitialized = false;
                return;
            }

            Aabb[] aabbs = new Aabb[objects.Length];

            // Preenche AABBs
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
            foreach (var obj in objects)
                obj.SetColor(Color.white);

            // Checa se é necessário reinicializar (quando a quantidade muda)
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

            // Executa busca de colisões
            BF_SearchOverlaps();

            ulong pairCount = BF_GetPairCount().ToUInt64();
            totalOverlaps += pairCount;

            Debug.Log($"🟢 Pares detectados neste frame: {pairCount}");

            // ✔️ Validação cruzada: brute force em C# (apenas visual)
            for (int i = 0; i < objects.Length; i++)
            {
                for (int j = i + 1; j < objects.Length; j++)
                {
                    if (CheckAABBOverlap(objects[i], objects[j]))
                    {
                        //objects[i].SetColor(Color.red);
                        //objects[j].SetColor(Color.red);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"🚨 Erro no Broadmark: {e.Message}");
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
        Debug.Log("🧹 Limpando Broadmark na destruição.");
        BF_Destroy();
    }
}
