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
        Instantiate(ServerObject, Vector3.zero, Quaternion.identity);
        Instantiate(ClientObject, Vector3.zero, Quaternion.identity);
    }

    public void ConnectToLobby()
    {
        Instantiate(ClientObject, Vector3.zero, Quaternion.identity);
    }

    private void OnApplicationQuit()
    {
        NativeSocketPlugin.ShutdownSockets();
    }
}
