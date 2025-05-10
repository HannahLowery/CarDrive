using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarEngine : MonoBehaviour
{
    public Transform path;
    private List<Transform> nodes;
    private int currentNode=0;
    public float maxSteerAngle = 45f;
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public float MaxMotorTorque=80f;
    public float currentSpeed;
     public float maxSpeed=100f;
     //public Vector3 centerOfMass;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //GetComponent<Rigidbody>().centerOfMass=centerOfMass;
         Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
         nodes = new List<Transform>();

         for(int i=0;i<pathTransforms.Length;i++){
            if(pathTransforms[i]!=path.transform){
                nodes.Add(pathTransforms[i]);
            }
         }
    }

    // Update is called once per frame
     
    void FixedUpdate()
    {
        ApplySteer();
        Drive();
        CheckWaypointDistance();
    }
   

    private void  ApplySteer(){
        Vector3 relativeVector=transform.InverseTransformPoint(nodes[currentNode].position);
        //relativeVector=relativeVector/relativeVector.magnitude;
        float newSteer=(relativeVector.x/relativeVector.magnitude)*maxSteerAngle;
        wheelFL.steerAngle=newSteer;
         wheelFR.steerAngle=newSteer;
    }
    private void Drive(){
        currentSpeed=2*Mathf.PI*wheelFL.radius*wheelFL.rpm*60/1000;
        if(currentSpeed<maxSpeed){
        wheelFL.motorTorque=MaxMotorTorque;
        wheelFR.motorTorque=MaxMotorTorque;
        }
        else{
            wheelFL.motorTorque=0;
            wheelFR.motorTorque=0;
        }
    }
    private void CheckWaypointDistance(){
        if(Vector3.Distance(transform.position,nodes[currentNode].position)<0.5f){
            if(currentNode==nodes.Count-1){
                currentNode=0;
            }
            else{
                currentNode++;
            }
        }
    }
}
