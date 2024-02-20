using System.Linq;
using TLab.Android.WebView;
using UnityEngine;

public class FingerColliderCreator : MonoBehaviour
{
    OVRSkeleton skeleton;
    OVRBone bone;

    GameObject sphere;

    public TLabWebView m_tlabWebView;
    public RectTransform m_webViewRect;

    bool created = false;

    public OVRPlugin.SkeletonType skeletonType;

    HandPlayerController handPlayerController;

    void Start()
    {
        skeleton = GetComponent<OVRSkeleton>();
        if (skeleton == null)
        {
            Debug.LogError("OVRSkeleton component not found on the GameObject.");
            return;
        }

        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.AddComponent<SphereCollider>();
        var rb = sphere.AddComponent<Rigidbody>();
        sphere.AddComponent<VRHandBrowserEvents>().SetProperties(m_tlabWebView, m_webViewRect);
        rb.useGravity = false;
        rb.isKinematic = true;
        handPlayerController = sphere.AddComponent<HandPlayerController>();
        handPlayerController.skeletonType = skeletonType;
        handPlayerController.Initialize();
        var hsc = sphere.AddComponent<HandSphereClimb>();
        hsc.skeletonType = skeletonType;
        hsc.Initialize();

    }

    public void SetRock(bool value)
    {
        handPlayerController.SetRock(value);
    }

    public void SetPointing(bool value)
    {
        handPlayerController.SetPointing(value);
    }

    void Update()
    {
        if (!created)
        {
            if (skeleton.IsInitialized && skeleton.Bones != null)
            {
                bone = skeleton.Bones.Where(b => b.Id == OVRSkeleton.BoneId.Hand_IndexTip).FirstOrDefault();
                if (bone != null)
                {
                    sphere.transform.SetParent(bone.Transform, false);
                    sphere.transform.localScale = Vector3.one / 10;
                    sphere.transform.localPosition = Vector3.zero;
                    created = true;
                    //sphere.transform.position = bone.Transform.position;
                }
            }
        }
    }
}