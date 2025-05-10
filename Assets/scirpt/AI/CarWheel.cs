using UnityEngine;

public class CarWheel : MonoBehaviour
{
    public WheelCollider targetWheel;
    private Vector3 wheelPosition=new Vector3();
    private Quaternion wheelRotation=new Quaternion();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetWheel.GetWorldPose(out wheelPosition, out wheelRotation);
        transform.position=wheelPosition;
        transform.rotation=wheelRotation;
    }
}
