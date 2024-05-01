using UnityEngine;

public class HandTrackingAndControllerSwitcher : MonoBehaviour
{
    private bool leftHandUsingController = false;
    private bool rightHandUsingController = false;

    void Start()
    {
        OVRManager.InputFocusAcquired += HandleInputFocusAcquired;
        OVRManager.InputFocusLost += HandleInputFocusLost;
    }

    void Update()
    {
        CheckLeftController();
        CheckRightController();
        UpdateInputMethod();
    }

    void CheckLeftController()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
        {
            HandPlayerController.LeftHandInstance.SetPointing(true);
            Debug.Log("Left Index Finger Button Pressed");
        }

        if (OVRInput.GetUp(OVRInput.RawButton.LIndexTrigger))
        {
            HandPlayerController.LeftHandInstance.SetPointing(false);
            Debug.Log("Left Index Finger Button Released");
        }

        if (OVRInput.GetDown(OVRInput.RawButton.LHandTrigger))
        {
            HandPlayerController.LeftHandInstance.SetRock(true);
            Debug.Log("Left Grip Button Pressed");
        }

        if (OVRInput.GetUp(OVRInput.RawButton.LHandTrigger))
        {
            HandPlayerController.LeftHandInstance.SetRock(false);
            Debug.Log("Left Grip Button Released");
        }
    }

    void CheckRightController()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            HandPlayerController.RightHandInstance.SetPointing(true);
            Debug.Log("Right Index Finger Button Pressed");
        }

        if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger))
        {
            HandPlayerController.RightHandInstance.SetPointing(false);
            Debug.Log("Right Index Finger Button Released");
        }

        if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
        {
            HandPlayerController.RightHandInstance.SetRock(true);
            Debug.Log("Right Grip Button Pressed");
        }

        if (OVRInput.GetUp(OVRInput.RawButton.RHandTrigger))
        {
            HandPlayerController.RightHandInstance.SetRock(false);
            Debug.Log("Right Grip Button Released");
        }
    }

    void UpdateInputMethod()
    {
        bool leftControllerConnected = OVRInput.IsControllerConnected(OVRInput.Controller.LTouch);
        bool rightControllerConnected = OVRInput.IsControllerConnected(OVRInput.Controller.RTouch);

        // Update left hand usage
        if (leftControllerConnected && !leftHandUsingController)
        {
            leftHandUsingController = true;
            FingerColliderCreator.LeftHandInstance.sphere.transform.SetParent(FingerColliderCreator.LeftHandInstance.controllerParent, false);
            Debug.Log("Left hand switched to Controller Tracking");
        }
        else if (!leftControllerConnected && leftHandUsingController)
        {
            leftHandUsingController = false;
            FingerColliderCreator.LeftHandInstance.created = false;
            Debug.Log("Left hand switched to Hand Tracking");
        }

        // Update right hand usage
        if (rightControllerConnected && !rightHandUsingController)
        {
            rightHandUsingController = true;
            FingerColliderCreator.RightHandInstance.sphere.transform.SetParent(FingerColliderCreator.RightHandInstance.controllerParent, false);

            Debug.Log("Right hand switched to Controller Tracking");
        }
        else if (!rightControllerConnected && rightHandUsingController)
        {
            rightHandUsingController = false;
            FingerColliderCreator.RightHandInstance.created = false;
            Debug.Log("Right hand switched to Hand Tracking");
        }
    }

    private void HandleInputFocusAcquired()
    {
        // Optionally handle input focus gained (e.g., app resumed)
    }

    private void HandleInputFocusLost()
    {
        // Optionally handle input focus lost (e.g., app paused)
    }

    void OnDestroy()
    {
        OVRManager.InputFocusAcquired -= HandleInputFocusAcquired;
        OVRManager.InputFocusLost -= HandleInputFocusLost;
    }
}
