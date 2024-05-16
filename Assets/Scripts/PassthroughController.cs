using UnityEngine;

public class PassthroughController : MonoBehaviour
{
    OVRPassthroughLayer passthroughLayer;

    public GameObject groundGameObject;

    void Start()
    {
        passthroughLayer = GetComponent<OVRPassthroughLayer>();

        if (OVRManager.IsPassthroughRecommended())
        {
            passthroughLayer.enabled = true;

            OVRCameraRig ovrCameraRig = GameObject.Find("OVRCameraRig").GetComponent<OVRCameraRig>();
            var centerCamera = ovrCameraRig.centerEyeAnchor.GetComponent<Camera>();
            centerCamera.clearFlags = CameraClearFlags.SolidColor;
            centerCamera.backgroundColor = Color.clear;
        }
        else
        {
            passthroughLayer.enabled = false;
            /// TODO: Activate VR environment
        }
    }

    public void TogglePassthrough()
    {
        PassthroughSwitch(!passthroughLayer.enabled);
    }

    public void PassthroughSwitch(bool enabled)
    {
        if (enabled)
        {
            passthroughLayer.enabled = true;
            GetComponent<OVRManager>().isInsightPassthroughEnabled = true;

            OVRCameraRig ovrCameraRig = GameObject.Find("OVRCameraRig").GetComponent<OVRCameraRig>();
            var centerCamera = ovrCameraRig.centerEyeAnchor.GetComponent<Camera>();
            centerCamera.clearFlags = CameraClearFlags.SolidColor;
            centerCamera.backgroundColor = Color.clear;
            groundGameObject.SetActive(false);
        }
        else
        {
            passthroughLayer.enabled = false;
            GetComponent<OVRManager>().isInsightPassthroughEnabled = false;

            OVRCameraRig ovrCameraRig = GameObject.Find("OVRCameraRig").GetComponent<OVRCameraRig>();
            var centerCamera = ovrCameraRig.centerEyeAnchor.GetComponent<Camera>();
            centerCamera.clearFlags = CameraClearFlags.Skybox;
            groundGameObject.SetActive(true);
        }
    }
}