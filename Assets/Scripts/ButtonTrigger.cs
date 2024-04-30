using System.Collections;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    // This event is public, allowing you to assign actions in the Unity editor or from other scripts.
    public UnityEngine.Events.UnityEvent onButtonPress;
    public UnityEngine.Events.UnityEvent onButtonHold;

    float triggerTimer = 0.1f;
    bool canBeTriggered = true;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<HandPlayerController>() != null && other.gameObject.name == "Sphere" && canBeTriggered)
        {
            Debug.Log("Button pressed by " + other.name);
            onButtonPress.Invoke();
            canBeTriggered = false;
            StartCoroutine(ResetTriggerBool());
        }
    }

    IEnumerator ResetTriggerBool()
    {
        yield return new WaitForSeconds(triggerTimer);
        canBeTriggered = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<HandPlayerController>() != null && other.gameObject.name == "Sphere")
        {
            onButtonHold.Invoke();
        }
    }
}