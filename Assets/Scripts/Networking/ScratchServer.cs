using System;
using System.Data;
using System.Runtime.InteropServices;
using UnityEngine;

public static class NativeServerPlugin
{
    [DllImport("ScratchServerLibrary.dll")]
    public static extern IntPtr InitializeScratchServer();

    [DllImport("ScratchServerLibrary.dll")]
    public static extern void BeginServerProcess(IntPtr serverObject);

    [DllImport("ScratchServerLibrary.dll")]
    public static extern void ShutdownServer(IntPtr serverObject);
}

public class ScratchServer : MonoBehaviour
{
    public IntPtr serverObject { get; private set; }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        serverObject = NativeServerPlugin.InitializeScratchServer();

        NativeServerPlugin.BeginServerProcess(serverObject);

        Debug.LogWarning($"Server Ptr Value: {serverObject.ToString()}");
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Cleaning up server...");
        NativeServerPlugin.ShutdownServer(serverObject); //safely eliminate threads
        serverObject = IntPtr.Zero;
    }
}
