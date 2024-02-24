using System;
using UnityEngine;

public class ClimbProvider : MonoBehaviour
{
    public static ClimbProvider Instance;

    public CharacterController controller;
    public float gravity = -9.81f;

    private Vector3 _verticalVelocity = Vector3.zero;

    private Vector3 _lastLeftHandPosition;
    private Vector3 _lastRightHandPosition;
    public bool _leftHandActive = false;
    public bool _rightHandActive = false;

    private Vector3 _leftHandReferencePosition;
    private Vector3 _rightHandReferencePosition;

    private Vector3 movedLeftHandVector = Vector3.zero;
    private Vector3 movedRightHandVector = Vector3.zero;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        Vector3 movement = Vector3.zero;

        // Apply hand movements
        movement += ApplyHandMovement();

        //print(movement);
        
        // Apply gravity if needed
        ApplyGravityIfNeeded();
        
        // Add vertical velocity if not climbing
        if (!_leftHandActive && !_rightHandActive)
        {
            movement += _verticalVelocity * Time.deltaTime;
        }

        //print("After Gravity: " + movement);

        // Move the character
        controller.Move(movement);
    }

    private Vector3 ApplyHandMovement()
    {
        Vector3 movement = Vector3.zero;

        if (_leftHandActive)
        {
            movement += UpdateHandMovement(ref _lastLeftHandPosition, _leftHandReferencePosition, ref movedLeftHandVector, HandSphereClimb.LeftHandInstance);
        }

        if (_rightHandActive)
        {
            movement += UpdateHandMovement(ref _lastRightHandPosition, _rightHandReferencePosition, ref movedRightHandVector, HandSphereClimb.RightHandInstance);
        }


        // Normalize the movement if both hands are active
        if (_leftHandActive && _rightHandActive) 
        {
            movement *= 0.5f;
        }

        return movement;
    }

    private Vector3 UpdateHandMovement(ref Vector3 lastHandPosition, Vector3 referenceHandPosition, ref Vector3 handMovedVector, HandSphereClimb handInstance)
    {
        if (handInstance == null) return Vector3.zero;

        Vector3 currentHandPosition = handInstance.transform.position;

        Vector3 movementSinceActivation = currentHandPosition - referenceHandPosition;

        Vector3 movement = movementSinceActivation - handMovedVector;

        handMovedVector += movement;

        //lastHandPosition = currentHandPosition;

        return movementSinceActivation;
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

    //private Vector3 CalculateMovement()
    //{
    //    Vector3 movement = Vector3.zero;
    //    // Depending on how you apply the hand movement, accumulate it here.
    //    if (!_leftHandActive && !_rightHandActive)
    //    {
    //        movement += _verticalVelocity * Time.deltaTime;
    //    }
    //    return movement;
    //}

    public void HandActivated(OVRPlugin.SkeletonType skeletonType)
    {
        if (skeletonType == OVRPlugin.SkeletonType.HandLeft)
        {
            _leftHandActive = true;
            _leftHandReferencePosition = HandSphereClimb.LeftHandInstance.transform.position; // Set reference position
            _lastLeftHandPosition = _leftHandReferencePosition; // Synchronize last position with reference position
        }
        else if (skeletonType == OVRPlugin.SkeletonType.HandRight)
        {
            _rightHandActive = true;
            _rightHandReferencePosition = HandSphereClimb.RightHandInstance.transform.position; // Set reference position
            _lastRightHandPosition = _rightHandReferencePosition; // Synchronize last position with reference position
        }
    }


    public void HandDeactivated(OVRPlugin.SkeletonType skeletonType)
    {
        if (skeletonType == OVRPlugin.SkeletonType.HandRight)
        {
            _rightHandActive = false;
        }
        else if (skeletonType == OVRPlugin.SkeletonType.HandLeft)
        {
            _leftHandActive = false;
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
