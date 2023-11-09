using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class CharacterMovementHandler : NetworkBehaviour
{
    Camera localCamera;
    NetworkCharacterControllerPrototypeCustom networkCharacterControllerPrototypeCustom;
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    GameObject spawnPoint;
    
 

    private void Awake()
    {
        networkCharacterControllerPrototypeCustom = GetComponent<NetworkCharacterControllerPrototypeCustom>();
        localCamera = GetComponentInChildren<Camera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public override void FixedUpdateNetwork()
    {
        //Get input from network 
        if (GetInput(out NetworkInputData networkInputData))
        {

            transform.forward = networkInputData.aimForwardVector;
            Quaternion rotation = transform.rotation;
            rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, rotation.eulerAngles.z);
            this.transform.rotation = rotation;

            //Move 
            Vector3 moveDirection = (transform.forward * networkInputData.movementInput.y + transform.right * networkInputData.movementInput.x);
            moveDirection.Normalize();
            networkCharacterControllerPrototypeCustom.Move(moveDirection);

            //Jump
            if (networkInputData.isJumped)
            {
                networkCharacterControllerPrototypeCustom.Jump();
                Debug.Log("Jump hua");
            }
            if(networkInputData.Shoot)
            {
                Runner.Spawn(bulletPrefab, spawnPoint.transform.position, transform.rotation);
                Debug.Log(networkInputData.aimForwardVector);
            }
        }
    }

}
