using System;
using System.Runtime.InteropServices;
using UnityEngine;

public static class NativeNetObjectManagePlugin
{
    [DllImport("ClientLibrary.dll")]
    public static extern IntPtr RetrieveSpawnRequest(IntPtr nom);

    [DllImport("ClientLibrary.dll")]
    public static extern void CompleteSpawnRequest(IntPtr nom);

    [DllImport("ClientLibrary.dll")]
    public static extern IntPtr RetrieveScratchNetObject(IntPtr nom, int objectID);
    
    [DllImport("ClientLibrary.dll")]
    public static extern IntPtr GetBaselineNetworkedObjSnapshot(IntPtr scratchNetObject);

    [DllImport("ClientLibrary.dll")]
    public static extern float GetPosXFromNOS(IntPtr snapshot);

    [DllImport("ClientLibrary.dll")]
    public static extern float GetPosYFromNOS(IntPtr snapshot);

    [DllImport("ClientLibrary.dll")]
    public static extern float GetPosZFromNOS(IntPtr snapshot);

}


public class ScratchNetObjectManager : MonoBehaviour
{

    public IntPtr nom {  get; private set; } //the network object manager's pointer

    public GameObject networkObjectPref;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(nom == IntPtr.Zero)
        {
            return;
        }
        TryToSpawnNewObject();


    }

    public void Init(IntPtr inputedNom)
    {
        nom = inputedNom;
    }

    //try and grab a spawn request and spawn a object
    void TryToSpawnNewObject()
    {
        IntPtr objectRequestedToSpawn = NativeNetObjectManagePlugin.RetrieveSpawnRequest(nom); //grab the first request from spawn request

        if(objectRequestedToSpawn == IntPtr.Zero)
        {
            return; //dont have anything to spawn
        }

        IntPtr snapshotFromRequested = NativeNetObjectManagePlugin.GetBaselineNetworkedObjSnapshot(objectRequestedToSpawn);

        if(snapshotFromRequested == IntPtr.Zero)
        {
            Debug.LogWarning("Couldnt get object's snapshot");
            return;
        }

        Vector3 pos = BuildPositionFromSnapshot(snapshotFromRequested);

        GameObject newNetObject = Instantiate(networkObjectPref, pos, Quaternion.identity); //spawn the object at the position extracted from 

        ScratchNetworkObject networkComp = newNetObject.GetComponent<ScratchNetworkObject>();

        //line below needs to happen before init otherwise it doesnt get called 
        NativeNetObjectManagePlugin.CompleteSpawnRequest(nom); //pop the request from the queue

        //networkComp.Init(objectRequestedToSpawn);
        networkComp.scratchNetObject = objectRequestedToSpawn;
        networkComp.objectID = NativeScratchNetworkObjPlugin.GetSNObjectID(objectRequestedToSpawn);

        Debug.Log("Created object now cleaning up");

        

    }

    Vector3 BuildPositionFromSnapshot(IntPtr snapshot)
    {
        float posX = 0.0f;
        float posY = 0.0f;
        float posZ = 0.0f;

        posX = NativeNetObjectManagePlugin.GetPosXFromNOS(snapshot);
        posY = NativeNetObjectManagePlugin.GetPosYFromNOS(snapshot);
        posZ = NativeNetObjectManagePlugin.GetPosZFromNOS(snapshot);

        Vector3 pos = new Vector3(posX, posY, posZ);

        return pos;
    }
}
