using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;

    public bool isFinished = false;

    [Header("Input")]
    [SerializeField] private SteeringWheel steeringWheel;
    [SerializeField] private MobileInput mobileInput;
    [SerializeField] private GearShift gearShift;

    [Header("Settings")]
    [SerializeField] private float motorForce = 2000f;
    [SerializeField] private float breakForce = 3000f;
    [SerializeField] private float maxSteerAngle = 30f;

    [Header("Friction Settings")]
    [SerializeField] private float forwardStiffness = 3f;
    [SerializeField] private float sidewaysStiffness = 8f;
    [SerializeField] private float rearWheelPowerRatio = 0.5f;

    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    [Header("Wheel Meshes")]
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    [Header("Audio")]
    [SerializeField] private AudioSource gasAudioSource;
    [SerializeField] private AudioSource brakeAudioSource;

    private Rigidbody carRigidbody;

    private void Start() {
        carRigidbody = GetComponent<Rigidbody>();
        AdjustFrictionSettings();

        // Find the car based on the "Player" tag
    GameObject playerCar = GameObject.FindGameObjectWithTag("Player");
    if (playerCar != null) {
        // Automatically find and assign the wheel transforms by name under the player car
        frontLeftWheelTransform = playerCar.transform.Find("Front_Left_Wheel");
        frontRightWheelTransform = playerCar.transform.Find("Front_Right_Wheel");
        rearLeftWheelTransform = playerCar.transform.Find("Rear_Left_Wheel");
        rearRightWheelTransform = playerCar.transform.Find("Rear_Right_Wheel");

        // If any transform is not found, log a warning
        if (frontLeftWheelTransform == null) {
            Debug.LogWarning("Front_Left_Wheel not found!");
        }
        if (frontRightWheelTransform == null) {
            Debug.LogWarning("Front_Right_Wheel not found!");
        }
        if (rearLeftWheelTransform == null) {
            Debug.LogWarning("Rear_Left_Wheel not found!");
        }
        if (rearRightWheelTransform == null) {
            Debug.LogWarning("Rear_Right_Wheel not found!");
        }
    } else {
        Debug.LogWarning("No player car found with the 'Player' tag.");
    }
}

    private void FixedUpdate() {
        if (isFinished) return;

        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        StabilizeCar();
    }

private void GetInput()
{
    float mobileSteering = steeringWheel.output;
    float keyboardSteering = Input.GetAxis("Horizontal");

    if (Mathf.Abs(keyboardSteering) > 0.01f)
        horizontalInput = keyboardSteering;
    else
        horizontalInput = mobileSteering;

    float keyboardVertical = Input.GetAxis("Vertical");
    float mobileVertical = mobileInput.isAccelerating ? 1f : 0f;

    float combinedVertical = Mathf.Abs(keyboardVertical) > 0.01f ? keyboardVertical : mobileVertical;

    switch (gearShift.currentGear)
    {
        case GearShift.Gear.Park:
            verticalInput = 0f;
            break;

        case GearShift.Gear.Drive:
            verticalInput = Mathf.Max(0f, combinedVertical);
            break;

        case GearShift.Gear.Reverse:
            verticalInput = Mathf.Abs(keyboardVertical) > 0.01f ? Mathf.Min(0f, combinedVertical) : -mobileVertical;
            break;
    }

    isBreaking = mobileInput.isBraking || Input.GetKey(KeyCode.Space);

    // 🎵 GAS SOUND: Play when verticalInput is active and not breaking
    if (verticalInput > 0f && !isBreaking)
    {
        if (gasAudioSource != null && !gasAudioSource.isPlaying)
        {
            gasAudioSource.Play();
        }
    }
    else
    {
        if (gasAudioSource != null && gasAudioSource.isPlaying)
        {
            gasAudioSource.Stop();
        }
    }

    // 🎵 BRAKE SOUND: Play when braking (Space or mobile)
    if (isBreaking)
    {
        if (brakeAudioSource != null && !brakeAudioSource.isPlaying)
        {
            brakeAudioSource.Play();
        }
    }
    else
    {
        if (brakeAudioSource != null && brakeAudioSource.isPlaying)
        {
            brakeAudioSource.Stop();
        }
    }
}



    private void HandleMotor() {
    if (gearShift.currentGear == GearShift.Gear.Park) {
        // Set the car's velocity to zero instantly when in Park
        carRigidbody.linearVelocity = Vector3.zero;
        frontLeftWheelCollider.motorTorque = 0f;
        frontRightWheelCollider.motorTorque = 0f;
        rearLeftWheelCollider.motorTorque = 0f;
        rearRightWheelCollider.motorTorque = 0f;
    } else {
        // Apply motor torque as usual when not in Park
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        rearLeftWheelCollider.motorTorque = verticalInput * motorForce * rearWheelPowerRatio;
        rearRightWheelCollider.motorTorque = verticalInput * motorForce * rearWheelPowerRatio;
    }

    currentbreakForce = isBreaking ? breakForce : 0f;
    ApplyBreaking();
}

    private void ApplyBreaking() {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering() {
        currentSteerAngle = Mathf.Lerp(currentSteerAngle, maxSteerAngle * horizontalInput, Time.deltaTime * 5f);
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels() {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform) {
        Vector3 pos;
        Quaternion rot; 
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }

    private void AdjustFrictionSettings() {
        SetWheelFriction(frontLeftWheelCollider);
        SetWheelFriction(frontRightWheelCollider);
        SetWheelFriction(rearLeftWheelCollider);
        SetWheelFriction(rearRightWheelCollider);
    }

    private void SetWheelFriction(WheelCollider wheel) {
        WheelFrictionCurve forwardFriction = wheel.forwardFriction;
        WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;

        forwardFriction.stiffness = forwardStiffness;
        sidewaysFriction.stiffness = sidewaysStiffness;

        forwardFriction.extremumSlip = 0.4f;
        forwardFriction.extremumValue = 1f;
        forwardFriction.asymptoteSlip = 0.8f;
        forwardFriction.asymptoteValue = 0.5f;

        sidewaysFriction.extremumSlip = 0.2f;
        sidewaysFriction.extremumValue = 1f;
        sidewaysFriction.asymptoteSlip = 0.5f;
        sidewaysFriction.asymptoteValue = 0.75f;

        wheel.forwardFriction = forwardFriction;
        wheel.sidewaysFriction = sidewaysFriction;
    }

    private void StabilizeCar() {
        float angle = Vector3.Angle(transform.up, Vector3.up);
        if (angle > 5f) {
            Vector3 torqueAxis = Vector3.Cross(transform.up, Vector3.up);
            carRigidbody.AddTorque(torqueAxis * angle * 0.1f, ForceMode.VelocityChange);
        }

        Vector3 localVelocity = transform.InverseTransformDirection(carRigidbody.linearVelocity);
        localVelocity.x *= 0.8f;
        carRigidbody.linearVelocity = transform.TransformDirection(localVelocity);

        if (Mathf.Abs(horizontalInput) < 0.1f && verticalInput > 0) {
            carRigidbody.angularVelocity *= 0.95f;
        }
    }

    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Finish"))
    {
        isFinished = true;

        // Optional: Stop all movement instantly
        carRigidbody.linearVelocity = Vector3.zero;
        carRigidbody.angularVelocity = Vector3.zero;

        Debug.Log("Car finished the race!");
    }
}
}
