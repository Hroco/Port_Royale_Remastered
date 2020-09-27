using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using PhysHelper;

public class ShipMovement : MonoBehaviour
{
    [SerializeField] private bool sail;
    [SerializeField] private bool isSelected;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float defaultTurnSpeed; 
    [SerializeField] private float currentTurnSpeed;
    [SerializeField] private int maxSailStage;
    [SerializeField] private int sailStage;
    [SerializeField] private Transform helm;

    [SerializeField] private Vector3 target;
    [SerializeField] private Vector3 movingDirection;
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private new Camera camera;
    [SerializeField] private ShipDataModel shipData;


    // Start is called before the first frame update
    void Start()
    {
        maxSpeed = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (speed < maxSpeed && sail)
        {
            speed += Time.deltaTime / acceleration;
        }
        else
        {
            if ((speed - maxSpeed > 0.2f) && sail)
            {
                speed -= Time.deltaTime / acceleration;
            }
        }
            

        if (Input.GetMouseButton(1))
        {
            CalculateMovement();
        }
        
        if (sail)
        {
            var forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);
            PhysicsHelper.ApplyForceToReachVelocity(rigidbody, forward * maxSpeed, acceleration);

            if (target != Vector3.zero)
            {
                if ((int)(speed / 3) == 0)
                {
                    currentTurnSpeed = defaultTurnSpeed;
                }
                else
                {
                    currentTurnSpeed = defaultTurnSpeed / (int)(speed / 3);
                }

                transform.Rotate(transform.rotation.x, currentTurnSpeed * AngleDirection(transform.forward, movingDirection, transform.up), transform.rotation.z);

                if (target != Vector3.zero && Vector3.Angle(transform.up, target) <= 10)
                {
                    Debug.Log("WA");
                    target = Vector3.zero;
                    rigidbody.angularVelocity = Vector3.zero;
                }
            }
        }

        GetComponent<WaterFloat>().CalculateFloating();
    }

    private void CalculateMovement()
    {
        RaycastHit _hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out _hit);
        movingDirection = _hit.point - transform.position;

        target = _hit.point;
    }

    //calculate if target is on right = 1 or left = -1
    private float AngleDirection(Vector3 fwd, Vector3 targetDirection, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDirection);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            return 1f;
        }
        else
        {
            if (dir < 0f)
            {
                return -1f;
            }
            else
            {
                return 0;
            }
        }
    }

    public void sailsUp()
    {
        if (sailStage == maxSailStage)
            return;

        sailStage++;

        if (sailStage == maxSailStage)
        {
            maxSpeed = shipData.MaxSpeed;
        }
        else
        {
            maxSpeed = (int)(shipData.MaxSpeed / maxSailStage) * sailStage;
        }       
    }

    public void sailsDown()
    {
        if (sailStage == 0)
            return;

        sailStage--;

        if (sailStage == 0)
        {
            maxSpeed = 0;
        }
        else
        {
            maxSpeed = (int)(shipData.MaxSpeed / maxSailStage) * sailStage;
        }
    }
}
