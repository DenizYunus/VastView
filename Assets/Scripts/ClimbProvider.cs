using System;
using UnityEngine;

public class ClimbProvider : MonoBehaviour
{
    public static ClimbProvider Instance; // Singleton for easy access

    public CharacterController controller;

    private Vector3 _lastLeftHandPosition;
    private Vector3 _lastRightHandPosition;
    private bool _leftHandActive = false;
    private bool _rightHandActive = false;

    //Transform _leftHandTransform;
    //Transform _rightHandTransform;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    //public void RegisterLeftHand(Transform leftHand)
    //{
    //    _leftHandTransform = leftHand;
    //}

    //public void RegisterRightHand(Transform rightHand)
    //{
    //    _rightHandTransform = rightHand;
    //}

    void Update()
    {
        Vector3 movement = Vector3.zero; // Initialize movement vector

        if (_leftHandActive && HandSphereClimb.LeftHandInstance != null)
        {
            Vector3 currentLeftHandPosition = HandSphereClimb.LeftHandInstance.transform.position;
            Vector3 leftHandMovement = _lastLeftHandPosition - currentLeftHandPosition;

            // Apply left hand movement to the character controller
            movement += leftHandMovement;

            // Update last left hand position for next frame calculations
            _lastLeftHandPosition = currentLeftHandPosition;
        }

        if (_rightHandActive && HandSphereClimb.RightHandInstance != null)
        {
            Vector3 currentRightHandPosition = HandSphereClimb.RightHandInstance.transform.position;
            Vector3 rightHandMovement = _lastRightHandPosition - currentRightHandPosition;

            // Apply right hand movement to the character controller
            movement += rightHandMovement;

            // Update last right hand position for next frame calculations
            _lastRightHandPosition = currentRightHandPosition;
        }

        // Normalize the movement vector if both hands are active to avoid doubling the speed
        if (_leftHandActive && _rightHandActive)
        {
            movement *= 0.5f;
        }

        // Apply the calculated movement to your character controller or Rigidbody here
        controller.Move(movement);
    }

    public void HandActivated(OVRPlugin.SkeletonType skeletonType)
    {
        if (skeletonType == OVRPlugin.SkeletonType.HandLeft)
        {
            _leftHandActive = true;
            _lastLeftHandPosition = HandSphereClimb.LeftHandInstance.transform.position;
        }
        else if (skeletonType == OVRPlugin.SkeletonType.HandRight)
        {
            _rightHandActive = true;
            _lastRightHandPosition = HandSphereClimb.RightHandInstance.transform.position;
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
        //CheckIfClimbingIsActive();
    }

    private void CheckIfClimbingIsActive()
    {
        if (!_leftHandActive && !_rightHandActive)
        {
            // Optionally notify that climbing has stopped
        }
    }
}
