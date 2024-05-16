using UnityEngine;

public class HandTrackingAndControllerSwitcher : MonoBehaviour
{
    private bool leftHandUsingController = false;
    private bool rightHandUsingController = false;

    // A gameobject will be on 2 different positions and rotations for controller and hand tracking, so we need to switch between them, write a script to do that

    public GameObject menu;
    public GameObject menuSwitch;
    public GameObject keyboardSwitch;

    public Transform menuInController;
    public Transform menuInHand;
    public Transform menuSwitchInController;
    public Transform menuSwitchInHand;
    public Transform keyboardSwitchInController;
    public Transform keyboardSwitchInHand;

    void Start()
    {
        OVRManager.InputFocusAcquired += HandleInputFocusAcquired;
        OVRManager.InputFocusLost += HandleInputFocusLost;

        bool leftControllerConnected = (OVRInput.GetControllerIsInHandState(OVRInput.Hand.HandLeft) == OVRInput.ControllerInHandState.ControllerInHand);
        bool rightControllerConnected = (OVRInput.GetControllerIsInHandState(OVRInput.Hand.HandRight) == OVRInput.ControllerInHandState.ControllerInHand);

        if (leftControllerConnected)
        {
            leftHandUsingController = true;
            FingerColliderCreator.LeftHandInstance.sphere.transform.SetParent(FingerColliderCreator.LeftHandInstance.controllerParent, false);
            Debug.Log("Left hand switched to Controller Tracking");
            UpdateTransform(menu, menuInController);
            UpdateTransform(menuSwitch, menuSwitchInController);
        }
        else if (!leftControllerConnected)
        {
            leftHandUsingController = false;
            FingerColliderCreator.LeftHandInstance.created = false;
            Debug.Log("Left hand switched to Hand Tracking");
            UpdateTransform(menu, menuInHand);
            UpdateTransform(menuSwitch, menuSwitchInHand);
        }

        // Update right hand usage
        if (rightControllerConnected)
        {
            rightHandUsingController = true;
            FingerColliderCreator.RightHandInstance.sphere.transform.SetParent(FingerColliderCreator.RightHandInstance.controllerParent, false);
            Debug.Log("Right hand switched to Controller Tracking");
            UpdateTransform(keyboardSwitch, keyboardSwitchInController);
        }
        else if (!rightControllerConnected)
        {
            rightHandUsingController = false;
            FingerColliderCreator.RightHandInstance.created = false;
            Debug.Log("Right hand switched to Hand Tracking");
            UpdateTransform(keyboardSwitch, keyboardSwitchInHand);
        }
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
        //bool leftControllerConnected = OVRInput.IsControllerConnected(OVRInput.Controller.LTouch);
        //bool rightControllerConnected = OVRInput.IsControllerConnected(OVRInput.Controller.RTouch);
        bool leftControllerConnected = (OVRInput.GetControllerIsInHandState(OVRInput.Hand.HandLeft) == OVRInput.ControllerInHandState.ControllerInHand);
        bool rightControllerConnected = (OVRInput.GetControllerIsInHandState(OVRInput.Hand.HandRight) == OVRInput.ControllerInHandState.ControllerInHand);

        // Update left hand usage
        if (leftControllerConnected && !leftHandUsingController)
        {
            leftHandUsingController = true;
            FingerColliderCreator.LeftHandInstance.sphere.transform.SetParent(FingerColliderCreator.LeftHandInstance.controllerParent, false);
            Debug.Log("Left hand switched to Controller Tracking");
            UpdateTransform(menu, menuInController);
            UpdateTransform(menuSwitch, menuSwitchInController);
        }
        else if (!leftControllerConnected && leftHandUsingController)
        {
            leftHandUsingController = false;
            FingerColliderCreator.LeftHandInstance.created = false;
            Debug.Log("Left hand switched to Hand Tracking");
            UpdateTransform(menu, menuInHand);
            UpdateTransform(menuSwitch, menuSwitchInHand);
        }

        // Update right hand usage
        if (rightControllerConnected && !rightHandUsingController)
        {
            rightHandUsingController = true;
            FingerColliderCreator.RightHandInstance.sphere.transform.SetParent(FingerColliderCreator.RightHandInstance.controllerParent, false);
            Debug.Log("Right hand switched to Controller Tracking");
            UpdateTransform(keyboardSwitch, keyboardSwitchInController);
        }
        else if (!rightControllerConnected && rightHandUsingController)
        {
            rightHandUsingController = false;
            FingerColliderCreator.RightHandInstance.created = false;
            Debug.Log("Right hand switched to Hand Tracking");
            UpdateTransform(keyboardSwitch, keyboardSwitchInHand);
        }
    }

    void UpdateTransform(GameObject obj, Transform target)
    {
        obj.transform.position = target.position;
        obj.transform.rotation = target.rotation;
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
