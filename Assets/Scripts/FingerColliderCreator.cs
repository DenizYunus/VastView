using System.Linq;
using TLab.Android.WebView;
using UnityEngine;

public class FingerColliderCreator : MonoBehaviour
{
    OVRSkeleton skeleton;
    OVRBone bone;

    public GameObject sphere;

    public TLabWebView m_tlabWebView;
    public RectTransform m_webViewRect;

    public bool created = false;

    public OVRPlugin.SkeletonType skeletonType;

    HandPlayerController handPlayerController;
    public Transform controllerParent;

    public static FingerColliderCreator LeftHandInstance;
    public static FingerColliderCreator RightHandInstance;

    void CreateInstance()
    {
        if (skeletonType == OVRPlugin.SkeletonType.HandLeft)
        {
            if (LeftHandInstance != null && LeftHandInstance != this)
            {
                Destroy(this);
            }
            else
            {
                LeftHandInstance = this;
            }
        }

        if (skeletonType == OVRPlugin.SkeletonType.HandRight)
        {
            if (RightHandInstance != null && RightHandInstance != this)
            {
                Destroy(this);
            }
            else
            {
                RightHandInstance = this;
            }
        }
    }

    void Start()
    {
        skeleton = GetComponent<OVRSkeleton>();

        CreateInstance();

        if (skeleton == null)
        {
            Debug.LogError("OVRSkeleton component not found on the GameObject.");
            return;
        }

        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.AddComponent<BoxCollider>();
        //sphere.GetComponent<MeshRenderer>().enabled = false;
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





        sphere.transform.SetParent(transform, false);
        sphere.transform.localScale = Vector3.one / 30;
        sphere.transform.localPosition = Vector3.zero;

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
                    sphere.transform.localScale = Vector3.one / 30;
                    sphere.transform.localPosition = Vector3.zero;
                    created = true;
                    //sphere.transform.position = bone.Transform.position;
                }
            }
        }
    }
}