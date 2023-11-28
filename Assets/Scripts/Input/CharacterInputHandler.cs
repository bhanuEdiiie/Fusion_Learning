using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    Vector2 moveInputvector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;
    bool isJumpButtonPressed = false;
    bool isMouseButtonPressed = false;
    CharacterMovementHandler movementHandler;
    localCameraHandler localCameraHandler;
    private void Awake()
    {
        localCameraHandler = GetComponentInChildren<localCameraHandler>();
        movementHandler = GetComponent<CharacterMovementHandler>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!movementHandler.Object.HasInputAuthority)
            return;
        viewInputVector.x = Input.GetAxis("Mouse X");
        viewInputVector.y = Input.GetAxis("Mouse Y")  * -1 ;

        localCameraHandler.SetViewInputVector(viewInputVector);

        moveInputvector.x = Input.GetAxis("Horizontal");
        moveInputvector.y = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.Space))
            isJumpButtonPressed = true;

        if (Input.GetKey(KeyCode.Mouse0))
            isMouseButtonPressed = true;
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();

        //Movement Data
        networkInputData.movementInput = moveInputvector;

        networkInputData.aimForwardVector = localCameraHandler.transform.forward;

        //Jump Data
        networkInputData.isJumped = isJumpButtonPressed;

        //Shoot Data 
        networkInputData.isFired = isMouseButtonPressed;

        //Reset Data 
        isJumpButtonPressed = false;
        isMouseButtonPressed = false;
        return networkInputData;
    }
}
