using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class localCameraHandler : MonoBehaviour
{
    Camera localCamera;
    [SerializeField] Transform cameraAnchorPoint;
    [SerializeField]
    NetworkCharacterControllerPrototypeCustom networkCharacterControllerPrototypeCustom;
    float cameraRotationX = 0;
    float cameraRotationY = 0;


    //Input
    Vector2 viewInput;


    private void Awake()
    {
        localCamera = GetComponent<Camera>();
        networkCharacterControllerPrototypeCustom = GetComponentInParent<NetworkCharacterControllerPrototypeCustom>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(localCamera.enabled)
            localCamera.transform.parent = null;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        if (cameraAnchorPoint == null)
            return;
        if (!localCamera.enabled)
            return;

        //Move camera to the position of player
        localCamera.transform.position = cameraAnchorPoint.position;


        //Calcuate rotation 
        cameraRotationX += viewInput.y * Time.deltaTime * networkCharacterControllerPrototypeCustom.viewUpDown;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -90, 90);
        cameraRotationY += viewInput.x * Time.deltaTime * networkCharacterControllerPrototypeCustom.rotationSpeed;
        localCamera.transform.rotation = Quaternion.Euler(cameraRotationX, cameraRotationY, 0);
    }

    public void SetViewInputVector(Vector2 viewInput)
    {
        this.viewInput = viewInput;
        Debug.Log(this.viewInput);
    }
}
