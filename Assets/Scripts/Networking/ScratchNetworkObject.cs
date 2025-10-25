using System;
using System.Runtime.InteropServices;
using UnityEngine;

public static class NativeScratchNetworkObjPlugin
{
    [DllImport("ClientLibrary.dll")]
    public static extern int GetSNObjectID(IntPtr sno);

    [DllImport("ClientLibrary.dll")]
    public static extern int GetSNORecordKeeper(IntPtr sno);
}


public class ScratchNetworkObject : MonoBehaviour
{
    //the ptr to the scratchnetobject will be used to poll positonal updates to the client for it send to sever
    IntPtr scratchNetObject;

    int objectID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TryToUpdatePosition();
    }

    //gets called in the ScratchNetObjectManager upon spawning this object
    public void Init(IntPtr sno)
    {
        scratchNetObject = sno; 

        objectID = NativeScratchNetworkObjPlugin.GetSNObjectID(scratchNetObject);
    }

    void TryToUpdatePosition()
    {
        IntPtr SNOSnapshot = NativeNetObjectManagePlugin.GetBaselineNetworkedObjSnapshot(scratchNetObject);

        if(SNOSnapshot == IntPtr.Zero) //did we properly extract a valid ptr to use
        {
            return;
        }

        Vector3 newPosition = BuildPositionalData(scratchNetObject);

        gameObject.transform.position = newPosition; //set new position

    }


    //returns a vector3 of a position from the baseline snapshot of the scratchnetObeject
    Vector3 BuildPositionalData(IntPtr SNOSnapshot)
    {
        float posX = 0.0f;
        float posY = 0.0f;
        float posZ = 0.0f;

        posX = NativeNetObjectManagePlugin.GetPosXFromNOS(SNOSnapshot);
        posY = NativeNetObjectManagePlugin.GetPosYFromNOS(SNOSnapshot);
        posZ = NativeNetObjectManagePlugin.GetPosZFromNOS(SNOSnapshot);

        Vector3 position = new Vector3(posX, posY, posZ);

        return position;
    }
}
