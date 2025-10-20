using System;
using System.Runtime.InteropServices;
using UnityEngine;

public static class NativeClientPlugin
{
    [DllImport("ClientLibrary.dll")]
    public static extern IntPtr InitializeClient();

    [DllImport("ClientLibrary.dll")]
    public static extern void BeginClientProcess(IntPtr clientObject);

    [DllImport("ClientLibrary.dll")]
    public static extern void CleanupClient(IntPtr clientObject);

    [DllImport("ClientLibrary.dll")]
    public static extern int GetObjectID(IntPtr clientObject);

    [DllImport("ClientLibrary.dll")]
    public static extern void QueuePositionToClient(IntPtr client, int objectID, float posX, float posY, float posZ);
    
    [DllImport("ClientLibrary.dll")]
    public static extern float RetrieveBaselinePacketPosX(IntPtr client);

    [DllImport("ClientLibrary.dll")]
    public static extern float RetrieveBaselinePacketPosY(IntPtr client);

    [DllImport("ClientLibrary.dll")]
    public static extern float RetrieveBaselinePacketPosZ(IntPtr client);

}

public class ScratchClient : MonoBehaviour
{

    public IntPtr ClientObject { get; private set; }
    int ObjectID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ClientObject = NativeClientPlugin.InitializeClient();

        NativeClientPlugin.BeginClientProcess(ClientObject); //start running the threads 

        ObjectID = NativeClientPlugin.GetObjectID(ClientObject);

        Debug.LogWarning($"Client Ptr Value: {ClientObject.ToString()}");
        Debug.LogWarning(ObjectID.ToString());
    }

    public void QueueNewPositionToClient(Vector3 newPos)
    {
        NativeClientPlugin.QueuePositionToClient(ClientObject, ObjectID, newPos.x, newPos.y, newPos.z);
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Cleaning up client...");
        NativeClientPlugin.CleanupClient(ClientObject); //to ensure we safely eliminate any active threads 
    }
}
