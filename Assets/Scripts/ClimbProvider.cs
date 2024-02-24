using UnityEngine;

public class ClimbProvider : MonoBehaviour
{
    public static ClimbProvider Instance;

    public CharacterController controller;
    public float gravity = -9.81f;

    private Vector3 _verticalVelocity = Vector3.zero;

    // Hand activation flags
    public bool _leftHandActive = false;
    public bool _rightHandActive = false;

    // Tracking initial grab positions for movement calculation
    private Vector3 _initialLeftHandPosition;
    private Vector3 _initialRightHandPosition;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Update()
    {
        Vector3 movement = Vector3.zero;

        if (_leftHandActive)
        {
            movement += CalculateClimbingMovement(_initialLeftHandPosition, HandSphereClimb.LeftHandInstance.transform.position);
        }

        if (_rightHandActive)
        {
            movement += CalculateClimbingMovement(_initialRightHandPosition, HandSphereClimb.RightHandInstance.transform.position);
        }

        ApplyGravityIfNeeded();

        if (!_leftHandActive && !_rightHandActive)
        {
            movement += _verticalVelocity * Time.deltaTime;
        }

        controller.Move(movement);
    }

    private Vector3 CalculateClimbingMovement(Vector3 initialPosition, Vector3 currentPosition)
    {
        // Calculate the movement based on the difference from the initial grab position
        Vector3 movementSinceGrab = initialPosition - currentPosition;

        // Invert the movement to simulate climbing (moving the character opposite to the hand's movement)
        return movementSinceGrab;
    }

    private void ApplyGravityIfNeeded()
    {
        if (!_leftHandActive && !_rightHandActive)
        {
            if (controller.isGrounded)
            {
                _verticalVelocity = Vector3.zero;
            }
            else
            {
                _verticalVelocity.y += gravity * Time.deltaTime;
            }
        }
        else
        {
            _verticalVelocity = Vector3.zero;
        }
    }

    public void HandActivated(OVRPlugin.SkeletonType skeletonType)
    {
        if (skeletonType == OVRPlugin.SkeletonType.HandLeft)
        {
            _leftHandActive = true;
            // Capture the initial hand position at the moment of activation
            _initialLeftHandPosition = HandSphereClimb.LeftHandInstance.transform.position;
        }
        else if (skeletonType == OVRPlugin.SkeletonType.HandRight)
        {
            _rightHandActive = true;
            // Capture the initial hand position at the moment of activation
            _initialRightHandPosition = HandSphereClimb.RightHandInstance.transform.position;
        }
    }

    public void HandDeactivated(OVRPlugin.SkeletonType skeletonType)
    {
        if (skeletonType == OVRPlugin.SkeletonType.HandLeft)
        {
            _leftHandActive = false;
        }
        else if (skeletonType == OVRPlugin.SkeletonType.HandRight)
        {
            _rightHandActive = false;
        }
    }
}













//using System;
//using UnityEngine;

//public class ClimbProvider : MonoBehaviour
//{
//    public static ClimbProvider Instance; // Singleton for easy access

//    public CharacterController controller;
//    public float gravity = -9.81f; // Gravity force

//    private Vector3 _verticalVelocity = Vector3.zero;

//    private Vector3 _lastLeftHandPosition;
//    private Vector3 _lastRightHandPosition;
//    public bool _leftHandActive = false;
//    public bool _rightHandActive = false;

//    void Awake()
//    {
//        if (Instance != null && Instance != this)
//        {
//            Destroy(this);
//        }
//        else
//        {
//            Instance = this;
//        }
//    }

//    //public void RegisterLeftHand(Transform leftHand)
//    //{
//    //    _leftHandTransform = leftHand;
//    //}

//    //public void RegisterRightHand(Transform rightHand)
//    //{
//    //    _rightHandTransform = rightHand;
//    //}

//    void Update()
//    {
//        Vector3 movement = Vector3.zero; // Initialize movement vector

//        if (_leftHandActive && HandSphereClimb.LeftHandInstance != null)
//        {
//            Vector3 currentLeftHandPosition = HandSphereClimb.LeftHandInstance.transform.position;
//            Vector3 leftHandMovement = _lastLeftHandPosition - currentLeftHandPosition;

//            // Apply left hand movement to the character controller
//            movement += leftHandMovement;

//            // Update last left hand position for next frame calculations
//            _lastLeftHandPosition = currentLeftHandPosition;
//        }

//        if (_rightHandActive && HandSphereClimb.RightHandInstance != null)
//        {
//            Vector3 currentRightHandPosition = HandSphereClimb.RightHandInstance.transform.position;
//            Vector3 rightHandMovement = _lastRightHandPosition - currentRightHandPosition;

//            // Apply right hand movement to the character controller
//            movement += rightHandMovement;

//            // Update last right hand position for next frame calculations
//            _lastRightHandPosition = currentRightHandPosition;
//        }

//        // Normalize the movement vector if both hands are active to avoid doubling the speed
//        if (_leftHandActive && _rightHandActive)
//        {
//            movement *= 0.5f;
//        }

//        if (!_leftHandActive && !_rightHandActive)
//        {
//            if (controller.isGrounded)
//            {
//                _verticalVelocity.y = 0; // Reset vertical velocity when grounded
//            }
//            else
//            {
//                _verticalVelocity.y += gravity * Time.deltaTime; // Apply gravity to vertical velocity
//            }
//            movement += _verticalVelocity * Time.deltaTime;
//        } else
//        {
//            _verticalVelocity = Vector3.zero;
//        }

//        controller.Move(movement);
//    }

//    public void HandActivated(OVRPlugin.SkeletonType skeletonType)
//    {
//        if (skeletonType == OVRPlugin.SkeletonType.HandLeft)
//        {
//            _leftHandActive = true;
//            _lastLeftHandPosition = HandSphereClimb.LeftHandInstance.transform.position;
//        }
//        else if (skeletonType == OVRPlugin.SkeletonType.HandRight)
//        {
//            _rightHandActive = true;
//            _lastRightHandPosition = HandSphereClimb.RightHandInstance.transform.position;
//        }
//    }

//    public void HandDeactivated(OVRPlugin.SkeletonType skeletonType)
//    {
//        if (skeletonType == OVRPlugin.SkeletonType.HandRight)
//        {
//            _rightHandActive = false;
//        }
//        else if (skeletonType == OVRPlugin.SkeletonType.HandLeft)
//        {
//            _leftHandActive = false;
//        }
//        //CheckIfClimbingIsActive();
//    }

//    private void CheckIfClimbingIsActive()
//    {
//        if (!_leftHandActive && !_rightHandActive)
//        {
//            // Optionally notify that climbing has stopped
//        }
//    }
//}
