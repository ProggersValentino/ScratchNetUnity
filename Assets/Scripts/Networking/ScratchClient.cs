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

    [DllImport("ClientLibrary.dll")]
    public static extern IntPtr ExtractNOM(IntPtr client);

    [DllImport("ClientLibrary.dll")]
    public static extern bool CanPacketBeQueuedToSend(IntPtr client);

    [DllImport("ClientLibrary.dll")]
    public static extern IntPtr ExtractSnapshotFromIndex(IntPtr client, int index);

    [DllImport("ClientLibrary.dll")]
    public static extern int FindRecordIndex(IntPtr client, IntPtr recordToLookFor);

    [DllImport("ClientLibrary.dll")]
    public static extern float RetrievePacketPosX(IntPtr chosenSnapshot);

    [DllImport("ClientLibrary.dll")]
    public static extern float RetrievePacketPosY(IntPtr chosenSnapshot);

    [DllImport("ClientLibrary.dll")]
    public static extern float RetrievePacketPosZ(IntPtr chosenSnapshot);

    [DllImport("ClientLibrary.dll")]
    public static extern void SetPacketSendRate(IntPtr client, int rate);


}

public class ScratchClient : MonoBehaviour
{

    public IntPtr ClientObject { get; private set; }
    public IntPtr nom { get; private set; }

    ScratchNetObjectManager objManager;

    int ObjectID;

    public GameObject playerPref;

    [SerializeField] private int packetSendRate;

    private void Awake()
    {
        ClientObject = NativeClientPlugin.InitializeClient();
        nom = NativeClientPlugin.ExtractNOM(ClientObject);

       
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        NativeClientPlugin.SetPacketSendRate(ClientObject, packetSendRate);


        NativeClientPlugin.BeginClientProcess(ClientObject); //start running the threads 

        ObjectID = NativeClientPlugin.GetObjectID(ClientObject);

        Debug.LogWarning($"Client Ptr Value: {ClientObject.ToString()}");
        Debug.LogWarning($"Network Object Management Ptr Value: {nom.ToString()}");
        Debug.LogWarning(ObjectID.ToString());
        

        /*//init network object manager
        objManager = GetComponent<ScratchNetObjectManager>();
        objManager.Init(nom);*/

        SpawnPlayer();
    }

    public void QueueNewPositionToClient(Vector3 newPos)
    {
        bool canSendRequest = NativeClientPlugin.CanPacketBeQueuedToSend(ClientObject);
        //ensure we're in sync with the send rate of the packets & to ensure we're not bombarding the request pool with a hundreds of packets 
        if (!canSendRequest)
        {
            return;
        }

        NativeClientPlugin.QueuePositionToClient(ClientObject, ObjectID, newPos.x, newPos.y, newPos.z);
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Cleaning up client...");
        NativeClientPlugin.CleanupClient(ClientObject); //to ensure we safely eliminate any active threads 
    }

    void SpawnPlayer()
    {
        float pox = NativeClientPlugin.RetrieveBaselinePacketPosX(ClientObject);
        float poy = NativeClientPlugin.RetrieveBaselinePacketPosY(ClientObject);
        float poz = NativeClientPlugin.RetrieveBaselinePacketPosZ(ClientObject);

        Vector3 startingPos = new Vector3(pox, poy, poz); 

        GameObject player = Instantiate(playerPref, startingPos, Quaternion.identity);

        ScratchNetPlayer playerComp = player.GetComponent<ScratchNetPlayer>();
        playerComp.Init(this);
    }

    public void InitClientSettings(int sendRate)
    {
        packetSendRate = sendRate;
    }
}
