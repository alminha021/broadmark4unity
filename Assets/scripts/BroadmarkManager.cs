using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class BroadmarkManager : MonoBehaviour
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vec3 { public float x, y, z; }

    [StructLayout(LayoutKind.Sequential)]
    public struct Aabb
    {
        public Vec3 min;
        public Vec3 max;
    }

    [DllImport("Broadmarkfix", CallingConvention = CallingConvention.Cdecl)]
    private static extern void BF_Create();

    [DllImport("Broadmarkfix", CallingConvention = CallingConvention.Cdecl)]
    private static extern void BF_InitializeWithVecs(
        int numberOfObjects, [In] Aabb[] aabbs,
        float worldMinX, float worldMinY, float worldMinZ,
        float worldMaxX, float worldMaxY, float worldMaxZ,
        float marginX, float marginY, float marginZ
    );

    [DllImport("Broadmarkfix", CallingConvention = CallingConvention.Cdecl)]
    private static extern void BF_SearchOverlaps();

    [DllImport("Broadmarkfix", CallingConvention = CallingConvention.Cdecl)]
    private static extern UIntPtr BF_GetPairCount();

    [DllImport("Broadmarkfix", CallingConvention = CallingConvention.Cdecl)]
    private static extern void BF_Destroy();

    private ulong totalOverlaps = 0;

    void Start()
    {
        StartCoroutine(UpdateOverlapsLoop());
    }

    IEnumerator UpdateOverlapsLoop()
    {
        while (true)
        {
            RunBroadPhaseTest();
            yield return new WaitForSeconds(1f); // No futuro usar Update() para frame a frame
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
                return;
            }

            Aabb[] aabbs = new Aabb[objects.Length];

            // Resetar as cores antes do teste
            foreach (var obj in objects)
            {
                obj.SetColor(Color.white);
            }

            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].UpdateAABB();
                aabbs[i] = new Aabb
                {
                    min = new Vec3
                    {
                        x = objects[i].min.x,
                        y = objects[i].min.y,
                        z = objects[i].min.z
                    },
                    max = new Vec3
                    {
                        x = objects[i].max.x,
                        y = objects[i].max.y,
                        z = objects[i].max.z
                    }
                };
            }

            // 🔥 Sempre destruir antes de criar, para garantir que a memória não fique suja
            BF_Destroy();
            BF_Create();

            BF_InitializeWithVecs(
                aabbs.Length, aabbs,
                -50f, -50f, -50f,   // World min
                 50f,  50f,  50f,   // World max
                0.01f, 0.01f, 0.01f // Margin
            );

            BF_SearchOverlaps();

            ulong framePairs = BF_GetPairCount().ToUInt64();
            totalOverlaps += framePairs;

            Debug.Log($"🟢 Pares detectados neste frame: {framePairs}");
            Debug.Log($"🔢 Total acumulado até agora: {totalOverlaps}");

            BF_Destroy();
        }
        catch (Exception e)
        {
            Debug.LogError($"🚨 Erro no Broadmark: {e.Message}");
            BF_Destroy();
        }
    }

    void OnDestroy()
    {
        Debug.Log("🧹 Limpando Broadmark na destruição.");
        BF_Destroy();
    }
}
