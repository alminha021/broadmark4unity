using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct AABB
{
    public Vector3 min;  // 3 floats
    public Vector3 max;  // 3 floats

    public AABB(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }
}
