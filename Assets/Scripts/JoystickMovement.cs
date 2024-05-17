using UnityEngine;

public class JoystickMovement : MonoBehaviour
{
    private void Update()
    {
        if (OVRInput.Get(OVRInput.RawButton.LThumbstickDown))
        {
            ClimbProvider.Instance.MoveBackward();
            ClimbProvider.Instance.MoveBackward();
        }
        if (OVRInput.Get(OVRInput.RawButton.LThumbstickUp))
        {
            ClimbProvider.Instance.MoveForward();
            ClimbProvider.Instance.MoveForward();
        }
        if (OVRInput.Get(OVRInput.RawButton.LThumbstickLeft))
        {
            ClimbProvider.Instance.MoveLeft();
            ClimbProvider.Instance.MoveLeft();
        }
        if (OVRInput.Get(OVRInput.RawButton.LThumbstickRight))
        {
            ClimbProvider.Instance.MoveRight();
            ClimbProvider.Instance.MoveRight();
        }
        if (OVRInput.GetDown(OVRInput.RawButton.RThumbstick))
        {
            ClimbProvider.Instance.ToggleFlyMode();
        }
        if (OVRInput.Get(OVRInput.RawButton.RThumbstickUp))
        {
            ClimbProvider.Instance.MoveUp();
            ClimbProvider.Instance.MoveUp();
        }
        if (OVRInput.Get(OVRInput.RawButton.RThumbstickDown))
        {
            ClimbProvider.Instance.MoveDown();
            ClimbProvider.Instance.MoveDown();
        }
    }
}