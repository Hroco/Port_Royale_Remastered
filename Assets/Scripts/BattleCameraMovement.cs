using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCameraMovement : MonoBehaviour
{
    [SerializeField] private float borderPercentage;
    [SerializeField] private float cameraMovementSpeed;
    [SerializeField] private float screenWidth;
    [SerializeField] private float screenHeight;



    [SerializeField] private new Camera camera;

    [SerializeField] private Vector3 movementVector;
    [SerializeField] private Vector2 mousePos;

    [SerializeField] private bool lockCamera;

    private void Start()
    {
       
    }

    private void Update()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        mousePos = Input.mousePosition;

        if (!lockCamera)
            mousePos = Input.mousePosition;
        else
            mousePos = new Vector2(screenWidth / 2, screenHeight / 2);
        if (Input.GetKeyUp(KeyCode.Tab))
            lockCamera = !lockCamera;


        CalculateMovement();
    }

    private void CalculateMovement()
    {
        movementVector.z = 0;
        movementVector.x = 0;

        if ((mousePos.x < ((screenWidth / 100) * borderPercentage)) || (Input.GetKey("a")))
            movementVector.x = -1;

        //Move camera when mouse is on top of screen or key d is pressed and stop mooving when camera hit corner of map
        if ((mousePos.x > ((screenWidth / 100) * (100 - borderPercentage))) || (Input.GetKey("d")))
            movementVector.x = 1;

        //Move camera when mouse is on top of screen or key s is pressed and stop mooving when camera hit corner of map
        if ((mousePos.y < ((screenHeight / 100) * borderPercentage)) || (Input.GetKey("s")))
            movementVector.z = -1;

        //Move camera when mouse is on top of screen or key w is pressed and stop mooving when camera hit corner of map
        if ((mousePos.y > ((screenHeight / 100) * (100 - borderPercentage))) || (Input.GetKey("w")))
            movementVector.z = 1;

        camera.gameObject.transform.position += movementVector * cameraMovementSpeed * Time.deltaTime;
    }
}
