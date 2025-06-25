using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class BFTester : MonoBehaviour
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vec3
    {
        public float x, y, z;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AABB
    {
        public Vec3 min;
        public Vec3 max;
    }

    [DllImport("BFonlyff", CallingConvention = CallingConvention.Cdecl)]
    private static extern void BF_Create();

    [DllImport("BFonlyff", CallingConvention = CallingConvention.Cdecl)]
    private static extern void BF_Initialize(int numberOfObjects, [In] AABB[] aabbs);

    [DllImport("BFonlyff", CallingConvention = CallingConvention.Cdecl)]
    private static extern void BF_SearchOverlaps();

    [DllImport("BFonlyff", CallingConvention = CallingConvention.Cdecl)]
    private static extern UIntPtr BF_GetPairCount();

    [DllImport("BFonlyff", CallingConvention = CallingConvention.Cdecl)]
    private static extern void BF_Destroy();

    void Start()
{
    try
    {
        Debug.Log("Iniciando teste hardcoded...");

        BF_Create();

        // Criando 2 AABBs que se sobrepõem
        AABB[] testAABBs = new AABB[2];
        testAABBs[0] = new AABB
        {
            min = new Vec3 { x = 0f, y = 0f, z = 0f },
            max = new Vec3 { x = 1f, y = 1f, z = 1f }
        };

        testAABBs[1] = new AABB
        {
            min = new Vec3 { x = 0.5f, y = 0.5f, z = 0.5f },
            max = new Vec3 { x = 1.5f, y = 1.5f, z = 1.5f }
        };

        // 🔥 Debug: Verificar os valores que estão sendo passados
        for (int i = 0; i < testAABBs.Length; i++)
        {
            Debug.Log($"AABB {i}: min=({testAABBs[i].min.x}, {testAABBs[i].min.y}, {testAABBs[i].min.z}), " +
                      $"max=({testAABBs[i].max.x}, {testAABBs[i].max.y}, {testAABBs[i].max.z})");
        }

        // Enviando dados para DLL
        BF_Initialize(2, testAABBs);
        BF_SearchOverlaps();

        // Verificando pares detectados
        ulong pairCount = BF_GetPairCount().ToUInt64();
        Debug.Log($"✅ Pares detectados (teste hardcoded): {pairCount}");

        BF_Destroy();
    }
    catch (Exception e)
    {
        Debug.LogError($"Erro: {e}");
    }
}

}
