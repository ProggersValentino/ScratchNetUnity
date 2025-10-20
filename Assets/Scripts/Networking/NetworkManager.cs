using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public GameObject ServerObject;
    public GameObject ClientObject;

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
}
