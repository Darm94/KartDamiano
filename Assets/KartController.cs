using System;
using UnityEngine;

public class KartController : MonoBehaviour
{
    [Header("Car Properties")]
    [SerializeField] float motorTorque = 1000f;
    [SerializeField] float brakeTorque = 2000f;
    [SerializeField] float maxSpeed = 20f;
    [SerializeField] float steeringRange = 45f;
    [SerializeField] float steeringRangeAtMaxSpeed = 10f;
    [SerializeField] float centreOfGravityOffset = -1f;
    [SerializeField] private Transform steer;
    [SerializeField] private float maxSteerRotation = 45f;
    [SerializeField] private float steerRotationSpeed = 90f; // g/s
    [SerializeField] WheelControl[] wheels;

    private InputSystem_Actions _carControls; // Reference to the new input system
    private Rigidbody _rigidBody;

    // Steer informations
    private Quaternion _steerStartRotation;
    private float _steerCurrentYRotation;

    // Input system
    private Vector2 _inputVector;
    private float _vInput; // Forward/backward input
    private float _hInput; // Steering input

    // Notify about car speed
    public static Action<float> CarSpeed;

    // Event function
    void Awake()
    {
        _steerStartRotation = steer.localRotation; // LOCAL ROTATION OF STEER
        _carControls = new InputSystem_Actions(); // Initialize Input Actions
    }
    
    // Event function
    void OnEnable()
    {
        _carControls.Enable();
    }

    // Event function
    void OnDisable()
    {
        _carControls.Disable();
    }

    // Start is called before the first frame update
    // Event function
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();

        // Adjust center of mass to improve stability and prevent rolling
        Vector3 centerOfMass = _rigidBody.centerOfMass;
        centerOfMass.y += centreOfGravityOffset;
        _rigidBody.centerOfMass = centerOfMass;

        // Get all wheel components attached to the car
        wheels = GetComponentsInChildren<WheelControl>();
    }

    // Event function
    private void Update()
    {
        // Read the Vector2 input from the new Input System
        Vector2 inputVector = _carControls.Car.Movement.ReadValue<Vector2>();

        //Debug.Log($"vInput: {_vInput}, hInput: {_hInput}");

        // Get player input for acceleration and steering
        _vInput = inputVector.y; // Forward/backward input
        _hInput = inputVector.x; // Steering input

        // Rotate Steer
        _steerCurrentYRotation = Mathf.MoveTowards(_steerCurrentYRotation, 
            _hInput * maxSteerRotation, 
            steerRotationSpeed * Time.deltaTime);
        //Debug.Log($"_hInput*maxSteerRotation -> {_steerCurrentYRotation}");

        Quaternion targetRotation = Quaternion.Euler(0f, _steerCurrentYRotation, 0f);
        steer.localRotation = _steerStartRotation * targetRotation;
    }
    void FixedUpdate()
    {
        // Calculate current speed along the car's forward axis
        float forwardSpeed = Vector3.Dot(transform.forward, _rigidBody.linearVelocity);
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, Mathf.Abs(forwardSpeed)); // Normalized speed factor

        // Notify the speedometer
        CarSpeed?.Invoke(speedFactor);

        // Reduce motor torque and steering at high speeds for better handling
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        // Determine if the player is accelerating or trying to reverse
        bool isAccelerating = Mathf.Approximately(Mathf.Sign(_vInput), Mathf.Sign(forwardSpeed));

        foreach (var wheel in wheels)
        {
            // Apply steering to wheels that support steering
            if (wheel.Steerable)
            {
                wheel.WheelCollider.steerAngle = _hInput * currentSteerRange;
            }

            if (isAccelerating)
            {
                // Apply torque to motorized wheels
                if (wheel.Motorized)
                {
                    wheel.WheelCollider.motorTorque = _vInput * currentMotorTorque;
                }

                // Release brakes when accelerating
                wheel.WheelCollider.brakeTorque = 0f;
            }
            else
            {
                //Apply Brakes when reversing direction
                wheel.WheelCollider.motorTorque = 0f;
                wheel.WheelCollider.brakeTorque = Mathf.Abs(_vInput) * brakeTorque;
            }
        }
    }
}
