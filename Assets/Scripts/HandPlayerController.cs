using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPlayerController : MonoBehaviour
{
    //public enum HandType {
    //    LeftHand,
    //    RightHand
    //}

    //HandType handType;
    public OVRPlugin.SkeletonType skeletonType;

    public bool isRock = false;
    public bool isPointing = false;

    public enum HandShape
    {
        None,
        Rock,
        Pointing
    }

    public HandShape handShape;

    public static HandPlayerController LeftHandInstance;
    public static HandPlayerController RightHandInstance;

    public void ResetHandShape()
    {
        if (!isRock && !isPointing)
        {
            handShape = HandShape.None;
        }
        else if (isRock)
        {
            handShape = HandShape.Rock;
        }
        else if (isPointing)
        {
            handShape = HandShape.Pointing;
        }
    }

    public void SetRock(bool value)
    {
        isRock = value;
        ResetHandShape();
    }

    public void SetPointing(bool value)
    {
        isPointing = value;
        ResetHandShape();
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

    void OnEnable()
    {
        //if (leftControllerInstance == null)
        //{
        //    if (skeletonType == OVRPlugin.SkeletonType.HandLeft)
        //        leftControllerInstance = this;
        //}
        //if (rightControllerInstance == null)
        //{
        //    if (skeletonType == OVRPlugin.SkeletonType.HandRight)
        //        rightControllerInstance = this;
        //}
    }

    void Update()
    {

    }
}