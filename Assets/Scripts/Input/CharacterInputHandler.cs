using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    Vector2 moveInputvector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;
    bool isJumpButtonPressed;

    localCameraHandler localCameraHandler;
    private void Awake()
    {
        localCameraHandler = GetComponentInChildren<localCameraHandler>();
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
        viewInputVector.x = Input.GetAxis("Mouse X");
        viewInputVector.y = Input.GetAxis("Mouse Y")  * -1 ;

        localCameraHandler.SetViewInputVector(viewInputVector);

        moveInputvector.x = Input.GetAxis("Horizontal");
        moveInputvector.y = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.Space))
            isJumpButtonPressed = true;
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();

        //Movement Data
        networkInputData.movementInput = moveInputvector;

        networkInputData.aimForwardVector = localCameraHandler.transform.forward;

        //Jump Data
        networkInputData.isJumped = isJumpButtonPressed;
        isJumpButtonPressed = false;

        return networkInputData;
    }
}
