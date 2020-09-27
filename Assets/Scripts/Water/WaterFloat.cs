using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhysHelper;

public class WaterFloat : MonoBehaviour
{

    [SerializeField] private float airDrag = 10;
    [SerializeField] private float waterDrag = 10;
    [SerializeField] private float waterLine;
    [SerializeField] private Transform[] floatPoints;
    [SerializeField] private bool attachToSurface;

    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private Waves waves;

    private Vector3[] waterLinePoints;
    private Vector3 centerOffset;
    private Vector3 smoothVectorRotation;
    private Vector3 targetUp;
    public Vector3 center { get { return transform.position + centerOffset; } }

    void Awake()
    {
        waves = FindObjectOfType<Waves>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;

        //calculate center
        waterLinePoints = new Vector3[floatPoints.Length];
        for (int i = 0; i < floatPoints.Length; i++)
        {
            waterLinePoints[i] = floatPoints[i].position;
            centerOffset = PhysicsHelper.GetCenter(waterLinePoints) - transform.position;
        }
    }

    void Update()
    {

    }

    public void CalculateFloating()
    {
        //default water surface
        var newWaterLine = 0f;
        var pointUnderWater = false;

        //set WaterLinePoints and WaterLine
        for (int i = 0; i < floatPoints.Length; i++)
        {
            //height
            waterLinePoints[i] = floatPoints[i].position;
            waterLinePoints[i].y = waves.GetHeight(floatPoints[i].position);
            newWaterLine += waterLinePoints[i].y / floatPoints.Length;
            if (waterLinePoints[i].y > floatPoints[i].position.y)
                pointUnderWater = true;
        }

        var waterLineDelta = newWaterLine - waterLine;
        waterLine = newWaterLine;

        //gravity
        var gravity = Physics.gravity * 2;
        rigidbody.drag = airDrag;
        if (waterLine > center.y)
        {
            rigidbody.drag = waterDrag;
            //under water
            if (attachToSurface)
            {
                //attach to water surface
                rigidbody.position = new Vector3(rigidbody.position.x, waterLine - centerOffset.y, rigidbody.position.z);
            }
            else
            {
                //go up
                gravity = -Physics.gravity;
                transform.Translate(Vector3.up * waterLineDelta * 0.9f);
            }
        }
        rigidbody.AddForce(gravity * Mathf.Clamp(Mathf.Abs(waterLine - center.y), 0, 1));

        //calculate up vector
        targetUp = PhysicsHelper.GetNormal(waterLinePoints);

        //rotation
        if (pointUnderWater)
        {
            //attach to water surface
            targetUp = Vector3.SmoothDamp(transform.up, targetUp, ref smoothVectorRotation, 0.2f);
            rigidbody.rotation = Quaternion.FromToRotation(transform.up, targetUp) * rigidbody.rotation;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (floatPoints == null)
            return;

        for (int i = 0; i < floatPoints.Length; i++)
        {
            if (floatPoints[i] == null)
                continue;

            if (waves != null)
            {
                //draw cube
                Gizmos.color = Color.red;
                Gizmos.DrawCube(waterLinePoints[i], Vector3.one * 0.3f);
            }

            //draw cube
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(floatPoints[i].position, 0.1f);
        }

        //draw center
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(new Vector3(center.x, waterLine, center.z), Vector3.one * 1f);
        }
    }
}
