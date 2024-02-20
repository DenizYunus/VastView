using Unity.VisualScripting;
using UnityEngine;

public class HandSphereClimb : MonoBehaviour
{
    public OVRPlugin.SkeletonType skeletonType;

    public static HandSphereClimb LeftHandInstance;
    public static HandSphereClimb RightHandInstance;

    HandPlayerController handController;

    private void OnEnable()
    {
        //if (skeletonType == OVRPlugin.SkeletonType.HandLeft)
        //{
        //    ClimbProvider.Instance.RegisterLeftHand(this.transform);
        //}
        //else if (skeletonType == OVRPlugin.SkeletonType.HandRight)
        //{
        //    ClimbProvider.Instance.RegisterRightHand(this.transform);
        //}
    }

    public void Initialize()
    {
        if (LeftHandInstance != null && LeftHandInstance != this && skeletonType == OVRPlugin.SkeletonType.HandLeft)
        {
            print("aaaaa");
            Destroy(this);
        }
        else
        {
            LeftHandInstance = this;
            handController = HandPlayerController.LeftHandInstance;
        }

        if (RightHandInstance != null && RightHandInstance != this && skeletonType == OVRPlugin.SkeletonType.HandRight)
        {
            print("aaaaab");
            Destroy(this);
        }
        else
        {
            RightHandInstance = this;
            handController = HandPlayerController.RightHandInstance;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Climbable") && handController.handShape == HandPlayerController.HandShape.Rock)
        {
            ClimbProvider.Instance.HandActivated(skeletonType);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (handController.handShape == HandPlayerController.HandShape.Rock)
        {
            ClimbProvider.Instance.HandActivated(skeletonType);
        }
        else
        {
            ClimbProvider.Instance.HandDeactivated(skeletonType);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            ClimbProvider.Instance.HandDeactivated(skeletonType);
        }
    }
}