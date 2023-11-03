using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class CharacterMovementHandler : NetworkBehaviour
{
    Camera localCamera;
    NetworkCharacterControllerPrototypeCustom networkCharacterControllerPrototypeCustom;
    Vector2 viewInput;

    float cameraRotationX = 0;

    private void Awake()
    {
        networkCharacterControllerPrototypeCustom = GetComponent<NetworkCharacterControllerPrototypeCustom>();
        localCamera = GetComponentInChildren<Camera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cameraRotationX += viewInput.y * Time.deltaTime * networkCharacterControllerPrototypeCustom.viewUpDown;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -90, 90);
        localCamera.transform.localRotation = Quaternion.Euler(cameraRotationX, 0, 0);
    }
    public override void FixedUpdateNetwork()
    {
        
        if (GetInput(out NetworkInputData networkInputData))
        {
            Vector3 moveDirection = (transform.forward * networkInputData.movementInput.y + transform.right * networkInputData.movementInput.x);
            moveDirection.Normalize();
            networkCharacterControllerPrototypeCustom.Move(moveDirection);
            if (networkInputData.isJumped)
            {
                networkCharacterControllerPrototypeCustom.Jump();
                Debug.Log("Jump hua");
            }
            networkCharacterControllerPrototypeCustom.Rotate(networkInputData.rotationInput);
        }
    }

    public void SetViewInputVector(Vector2 viewInputVector) 
    {
        this.viewInput = viewInputVector;
    }
}
