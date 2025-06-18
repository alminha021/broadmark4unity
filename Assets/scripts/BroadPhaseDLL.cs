using System.Runtime.InteropServices;

public static class BroadPhaseDLL {
    [DllImport("testeBF9do6.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void InitBF(int objectCount);

    [DllImport("testeBF9do6.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void UpdateAABB(int index, float minX, float minY, float minZ, float maxX, float maxY, float maxZ);

    [DllImport("testeBF9do6.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void RunBF();

    [DllImport("testeBF9do6.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetOverlapCount();

    [DllImport("testeBF9do6.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void GetOverlapPair(int index, out int idA, out int idB);

    [DllImport("testeBF9do6.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int HelloSum(int a, int b);
}
