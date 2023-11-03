using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    Vector2 moveInputvector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;
    CharacterMovementHandler CharacterMovementHandler;
    bool isJumpButtonPressed;
    private void Awake()
    {
        CharacterMovementHandler = GetComponent<CharacterMovementHandler>();
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
        CharacterMovementHandler.SetViewInputVector(viewInputVector);
        moveInputvector.x = Input.GetAxis("Horizontal");
        moveInputvector.y = Input.GetAxis("Vertical");
        isJumpButtonPressed = Input.GetKeyDown(KeyCode.Space);
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();
        networkInputData.rotationInput = viewInputVector.x;
        networkInputData.movementInput = moveInputvector;

        networkInputData.isJumped = isJumpButtonPressed;
        return networkInputData;
    }
}
