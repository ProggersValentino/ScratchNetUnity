using UnityEngine;
using System.Runtime.InteropServices;



public class Test : MonoBehaviour
{


    [DllImport("SocketLibrary.dll")]
    private static extern bool InitializeSockets();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bool init = false;
        init = InitializeSockets();

        Debug.Log(init);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
