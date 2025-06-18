using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class BroadmarkTester : MonoBehaviour
{
    // Seu Vec3 com 4 floats para alinhar com C++
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Vec3
    {
        public float x;
        public float y;
        public float z;
        public float w; // sempre zero no C++ (index 3)

        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 0f;
        }
    }

    // Aabb com Vec3 custom
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Aabb
    {
        public Vec3 min;
        public Vec3 max;

        public Aabb(Vec3 min, Vec3 max)
        {
            this.min = min;
            this.max = max;
        }
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

    void Start()
    {
        try
        {
            Debug.Log("🟢 Iniciando teste Unity + DLL...");

            BF_Create();

            Aabb[] testAABBs = new Aabb[6];

            testAABBs[0] = new Aabb(
                new Vec3(0f, 0f, 0f),
                new Vec3(1f, 1f, 1f)
            );
            testAABBs[1] = new Aabb(
                new Vec3(0.5f, 0.5f, 0.5f),
                new Vec3(1.5f, 1.5f, 1.5f)
            );
            testAABBs[2] = new Aabb(
                new Vec3(3.5f, 3.5f, 3.5f),
                new Vec3(4.5f, 4.5f, 4.5f)
            );
            testAABBs[3] = new Aabb(
                new Vec3(3f, 3f, 3f),
                new Vec3(4f, 4f, 4f)
            );
             testAABBs[4] = new Aabb(
                new Vec3(3f, 3f, 3f),
                new Vec3(4f, 4f, 4f)
            );
            testAABBs[5] = new Aabb(
                new Vec3(13f, 13f, 13f),
                new Vec3(14f, 14f, 14f)
            );

            Debug.Log("🚀 Dados enviados. Executando  Initialize...");

            BF_InitializeWithVecs(
                testAABBs.Length, testAABBs,
                -100f, -100f, -100f,   // worldMin
                100f, 100f, 100f,      // worldMax
                0.01f, 0.01f, 0.01f    // margin
            );

            Debug.Log("🔍 Executando busca de colisões...");
            BF_SearchOverlaps();

            ulong pairCount = BF_GetPairCount().ToUInt64();
            Debug.Log($"✅ Pares detectados: {pairCount}");

            BF_Destroy();

            Debug.Log("🛑 Teste finalizado e memória limpa.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro no teste: {e}");
        }
    }
}
