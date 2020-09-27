/*
    DESCRIPTION: class that handles camera movement via wasd, mouse position, map border, camera zooming.


    DATE        USER        ACTION
    19.04.2020  SH          Created
    23.04.2020  SH          Added Camera zooming and adjusting camera location when player zoom out of map in CalculateMovement method.
    24.04.2020  SH          Added statement that disable camera moving with mouse when tab is pressed, wasd controll still works.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private bool LockCamera;

    private GameObject mainCameraGO;
    [SerializeField]
    private GameObject NorthCube;
    [SerializeField]
    private GameObject SouthCube;
    [SerializeField]
    private GameObject EastCube;
    [SerializeField]
    private GameObject WestCube;

    [SerializeField]
    private int screenWidth, screenHeight;
    [SerializeField]
    private float cameraMovementSpeed;
    [SerializeField]
    private float borderPercentage;
    [SerializeField]
    private float MinCamZoom;
    [SerializeField]
    private float MaxCamZoom;
    [SerializeField]
    private Camera mainCameraCMP;
    [SerializeField]
    private float orthographicSize;
    [SerializeField]
    private float northEdge;
    [SerializeField]
    private float southEdge;
    [SerializeField]
    private float eastEdge;
    [SerializeField]
    private float westEdge;
    [SerializeField]
    private float ScreenRatio;

    [SerializeField]
    private Vector2 mousePos;
    [SerializeField]
    private Vector3 movementVector;
    [SerializeField]
    private Vector3 CameraLocation;

    // Start is called before the first frame update
    void Start()
    {
        mainCameraGO = Camera.main.gameObject;  //main camera game object
        mainCameraCMP = Camera.main;            //main camera component


    }

    // Update is called once per frame
    void Update()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        if (!LockCamera)
            mousePos = Input.mousePosition;
        else
            mousePos = new Vector2(screenWidth / 2, screenHeight / 2);
        if (Input.GetKeyUp(KeyCode.Tab))
            LockCamera = !LockCamera;


        CalculateMovement();
        CameraScroll();
        
    }

    //method handles camera movement, map border with wasd and mouse position
    private void CalculateMovement()
    {
        orthographicSize = mainCameraCMP.orthographicSize;
        CameraLocation = mainCameraGO.transform.position;
        movementVector.z = 0;
        movementVector.x = 0;
        northEdge = NorthCube.transform.position.z;
        southEdge = SouthCube.transform.position.z;
        eastEdge = EastCube.transform.position.x;
        westEdge = WestCube.transform.position.x;
        ScreenRatio = (float)screenWidth / (float)screenHeight;

        //Move camera when mouse is on top of screen or key a is pressed and stop mooving when camera hit corner of map
        if (((mousePos.x < ((screenWidth / 100) * borderPercentage)) || (Input.GetKey("a"))) && ((CameraLocation.x - (orthographicSize * ScreenRatio) > westEdge)))
            movementVector.x = -1;

        //Move camera when mouse is on top of screen or key d is pressed and stop mooving when camera hit corner of map
        if (((mousePos.x > ((screenWidth / 100) * (100 - borderPercentage))) || (Input.GetKey("d"))) && (CameraLocation.x + (orthographicSize * ScreenRatio) < eastEdge))
            movementVector.x = 1;

        //Move camera when mouse is on top of screen or key s is pressed and stop mooving when camera hit corner of map
        if (((mousePos.y < ((screenHeight / 100) * borderPercentage)) || (Input.GetKey("s"))) && ((CameraLocation.z - (orthographicSize)) > southEdge))
            movementVector.z = -1;

        //Move camera when mouse is on top of screen or key w is pressed and stop mooving when camera hit corner of map
        if (((mousePos.y > ((screenHeight / 100) * (100 - borderPercentage))) || (Input.GetKey("w"))) && (CameraLocation.z + (orthographicSize) < northEdge))
            movementVector.z = 1;

        //Adjust camera location when zooming out on west site                                                              //<------------SH 23.04.2020-------------
        if (CameraLocation.x - (orthographicSize * 1.78) < westEdge - 1)
            movementVector.x = -((CameraLocation.x - (orthographicSize * ScreenRatio)) - westEdge);
        
        //Adjust camera location when zooming out on east site
        if (CameraLocation.x + (orthographicSize * 1.78) > eastEdge + 1)
            movementVector.x = -((CameraLocation.x + (orthographicSize * ScreenRatio)) - eastEdge);
        
        //Adjust camera location when zooming out on south site
        if (CameraLocation.z - (orthographicSize) < southEdge - 1)
            movementVector.z = -((CameraLocation.z - (orthographicSize)) - southEdge);
        
        //Adjust camera location when zooming out on north site
        if (CameraLocation.z + (orthographicSize) > northEdge + 1)
            movementVector.z = -((CameraLocation.z + (orthographicSize)) - northEdge);

        
        mainCameraGO.transform.position += movementVector * cameraMovementSpeed * Time.deltaTime;
    }
  
    //camera zooming using mouse wheel
    private void CameraScroll()
    {
        if (mainCameraCMP.orthographicSize > MaxCamZoom)
            mainCameraCMP.orthographicSize = MaxCamZoom;

        if (mainCameraCMP.orthographicSize < MinCamZoom)
            mainCameraCMP.orthographicSize = MinCamZoom;

        if ((Input.GetAxis("Mouse ScrollWheel") < 0f) && (mainCameraCMP.orthographicSize < MaxCamZoom))
        {
            mainCameraCMP.orthographicSize += 10;
        }

        if ((Input.GetAxis("Mouse ScrollWheel") > 0f) && (mainCameraCMP.orthographicSize > MinCamZoom))
        {
            mainCameraCMP.orthographicSize -= 10;
        }
    }
}
