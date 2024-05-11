using UnityEngine;

public class HandSphereClimb : MonoBehaviour
{
    public OVRPlugin.SkeletonType skeletonType;

    public static HandSphereClimb LeftHandInstance;
    public static HandSphereClimb RightHandInstance;

    public HandPlayerController handController;

    bool handActivated = false;

    bool isColliding = false;

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
        if (skeletonType == OVRPlugin.SkeletonType.HandLeft)
        {
            if (LeftHandInstance != null && LeftHandInstance != this)
            {
                Destroy(this);
            }
            else
            {
                LeftHandInstance = this;
                handController = HandPlayerController.LeftHandInstance;
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
                handController = HandPlayerController.RightHandInstance;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isColliding) return;
        isColliding = true;

        if (other.CompareTag("Climbable") && handController.handShape == HandPlayerController.HandShape.Rock)
        {
            ClimbProvider.Instance.HandActivated(skeletonType);
            handActivated = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Climbable") && handActivated == false && handController.handShape == HandPlayerController.HandShape.Rock)
        {
            ClimbProvider.Instance.HandActivated(skeletonType);
            handActivated = true;
        }
        else if (handActivated == true && handController.handShape != HandPlayerController.HandShape.Rock)
        {
            ClimbProvider.Instance.HandDeactivated(skeletonType);
            handActivated = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isColliding) return;
        isColliding = false;

        if (other.CompareTag("Climbable"))
        {
            ClimbProvider.Instance.HandDeactivated(skeletonType);
            handActivated = false;
        }
    }
}