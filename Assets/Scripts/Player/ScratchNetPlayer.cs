using UnityEngine;
using UnityEngine.InputSystem;

public class ScratchNetPlayer : MonoBehaviour
{
    public PlayerInput playerMap;
    InputAction moveAction;

    public ScratchClient clientObject { get; set; }

    [SerializeField] private float pSpeed = 15.0f;

    private Vector3 targetPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = playerMap.actions.FindAction("Move");
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

    public void Init(ScratchClient client)
    {
        clientObject = client;
    }
}
