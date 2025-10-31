using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public static class NativeSocketPlugin
{
    [DllImport("SocketLibrary.dll")]
    public static extern bool InitializeSockets();

    [DllImport("SocketLibrary.dll")]
    public static extern void ShutdownSockets();
}


public class NetworkManager : MonoBehaviour
{
    public GameObject ServerObject;
    public GameObject ClientObject;
    public GameObject ObjectManagement;

    [SerializeField] private int packetSendRate; //how many packets do we want the client and server sending per second

    private void Awake()
    {
        NativeSocketPlugin.InitializeSockets();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Host()
    {
        GameObject serverObj = Instantiate(ServerObject, Vector3.zero, Quaternion.identity);
        ScratchServer server = serverObj.GetComponent<ScratchServer>();
        server.InitServerSettings(packetSendRate);

        GameObject clientObject = Instantiate(ClientObject, Vector3.zero, Quaternion.identity);
        ScratchClient client = clientObject.GetComponent<ScratchClient>();
        client.InitClientSettings(packetSendRate);

        InitObjectManagement(client);
    }

    public void ConnectToLobby()
    {
        GameObject clientObject = Instantiate(ClientObject, Vector3.zero, Quaternion.identity);
        ScratchClient client = clientObject.GetComponent<ScratchClient>();
        client.InitClientSettings(packetSendRate);

        InitObjectManagement(client);
    }

    private void OnApplicationQuit()
    {
        NativeSocketPlugin.ShutdownSockets();
    }

    void InitObjectManagement(ScratchClient client)
    {
        

        GameObject objectManagement = Instantiate(ObjectManagement, Vector3.zero, Quaternion.identity);
        ScratchNetObjectManager objectManage = objectManagement.GetComponent<ScratchNetObjectManager>();

        objectManage.Init(client.nom);

    }
}
