using UnityEngine;

public class JoystickMovement : MonoBehaviour
{
    private void Update()
    {
        if (OVRInput.Get(OVRInput.RawButton.LThumbstickDown))
        {
            ClimbProvider.Instance.MoveBackward();
        }
        if (OVRInput.Get(OVRInput.RawButton.LThumbstickUp))
        {
            ClimbProvider.Instance.MoveForward();
        }
        if (OVRInput.Get(OVRInput.RawButton.LThumbstickLeft))
        {
            ClimbProvider.Instance.MoveLeft();
        }
        if (OVRInput.Get(OVRInput.RawButton.LThumbstickRight))
        {
            ClimbProvider.Instance.MoveRight();
        }
        if (OVRInput.GetDown(OVRInput.RawButton.RThumbstick))
        {
            ClimbProvider.Instance.ToggleFlyMode();
        }
        if (OVRInput.Get(OVRInput.RawButton.RThumbstickUp))
        {
            ClimbProvider.Instance.MoveUp();
        }
        if (OVRInput.Get(OVRInput.RawButton.RThumbstickDown))
        {
            ClimbProvider.Instance.MoveDown();
        }
    }
}