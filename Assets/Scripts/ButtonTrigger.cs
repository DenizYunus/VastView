using System.Collections;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    // This event is public, allowing you to assign actions in the Unity editor or from other scripts.
    public UnityEngine.Events.UnityEvent onButtonPress;

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


    private void OnCollisionEnter(Collision collision)
    {
        // You can add conditions to check the tag or properties of 'collision' if you need specific objects to trigger the action.
        if (collision.gameObject.GetComponent<HandPlayerController>() != null)  // Ensures only objects tagged as "Player" can trigger the event
        {
            Debug.Log("Button pressed by " + collision.gameObject.name);
            onButtonPress.Invoke();  // Invokes all functions assigned to onButtonPress
        }
    }
}