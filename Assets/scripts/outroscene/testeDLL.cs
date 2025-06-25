using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class BroadphaseTest : MonoBehaviour
{
    [DllImport("Broadmark_Debug_x64.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void HelloWorld();

    [DllImport("Broadmark_Debug_x64.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int HelloSum(int a, int b);

    void Start()
    {
        Debug.Log("Chamando DLL...");

        HelloWorld();

        int result = HelloSum(5, 7);
        Debug.Log($"Resultado da soma: {result}");
    }
}
