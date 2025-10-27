using System;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScratchNetPlayer : MonoBehaviour
{
    public PlayerInput playerMap;
    InputAction moveAction;

    public ScratchClient clientObject { get; set; }

    [SerializeField] private float pSpeed = 15.0f;

    private Vector3 targetPos;

    private float interpolationTimer = 0f;

    IntPtr olderSnap = IntPtr.Zero;
    IntPtr newerSnap = IntPtr.Zero;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = playerMap.actions.FindAction("Move");
        TryToUpdatePosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (clientObject == null) return;

        TryToUpdatePosition(); //check to see if our baseline snapshot updated and move to that location

        Vector2 movementVal = moveAction.ReadValue<Vector2>();

        //Debug.Log(movementVal);

        Vector2 movementValAdjusted = movementVal * pSpeed * Time.deltaTime;

        Vector3 newPos = new Vector3(gameObject.transform.position.x + movementValAdjusted.x, gameObject.transform.position.y, 
            gameObject.transform.position.z + movementValAdjusted.y);

        clientObject.QueueNewPositionToClient(newPos);

        

    }

    void TryToUpdatePosition()
    {
        Vector3 extractedPos = BuildPosition();
        gameObject.transform.position = extractedPos;

        /* olderSnap = NativeClientPlugin.ExtractSnapshotFromIndex(clientObject.ClientObject, 2);
         newerSnap = NativeClientPlugin.ExtractSnapshotFromIndex(clientObject.ClientObject, 1);

         float lerpDuration = 0.1f;

         if (olderSnap == IntPtr.Zero || newerSnap == IntPtr.Zero)
             return;

         Vector3 olderPos = BuildPosition(olderSnap);
         Vector3 newerPos = BuildPosition(newerSnap);

         // Advance timer
         interpolationTimer += Time.deltaTime;

         float t = interpolationTimer / lerpDuration;
         t = Mathf.Clamp01(t);

         transform.position = Vector3.Lerp(olderPos, newerPos, t);

         // Move to next snapshot when done
         if (t >= 1.0f)
         {
             olderSnap = newerSnap;

             int newSnapRecentPosInbacklog = NativeClientPlugin.FindRecordIndex(clientObject.ClientObject, newerSnap);

             if(newSnapRecentPosInbacklog == - 1)
             {
                 return;
             }

             newerSnap = NativeClientPlugin.ExtractSnapshotFromIndex(clientObject.ClientObject, newSnapRecentPosInbacklog - 1); // minus 1 goes up in the record list 

             interpolationTimer = 0f;
         }*/

    }

    Vector3 BuildPosition()
    {
        float posX = 0.0f;
        float posY = 0.0f;
        float posZ = 0.0f;

        posX = NativeClientPlugin.RetrieveBaselinePacketPosX(clientObject.ClientObject);
        posY = NativeClientPlugin.RetrieveBaselinePacketPosY(clientObject.ClientObject);
        posZ = NativeClientPlugin.RetrieveBaselinePacketPosZ(clientObject.ClientObject);

        Vector3 extractedPos = new Vector3(posX, posY, posZ);

        return extractedPos;
    }

    Vector3 BuildPosition(IntPtr snapshot)
    {
        float posX = 0.0f;
        float posY = 0.0f;
        float posZ = 0.0f;

        posX = NativeClientPlugin.RetrievePacketPosX(snapshot);
        posY = NativeClientPlugin.RetrievePacketPosY(snapshot);
        posZ = NativeClientPlugin.RetrievePacketPosZ(snapshot);
    
        Vector3 extractedSnapshot = new Vector3(posX, posY, posZ);

        return extractedSnapshot;
    }

    public void Init(ScratchClient client)
    {
        clientObject = client;
    }
}
