using UnityEngine;

public class KeyboardPlacer : MonoBehaviour
{
    public Transform character;
    public Transform keyboard;
    public Collider browser;

    public float fixedDistanceToCharacter = 1.0f;
    public float minDistanceToBrowser = 1.0f;

    private bool isAdjusting = false; // Flag to prevent recursive position adjustments

    void Update()
    {
        if (keyboard.gameObject.activeInHierarchy && !isAdjusting)
        {
            // Start adjusting to prevent recursive calls
            isAdjusting = true;

            // Position the keyboard and adjust its height to match the character's
            Vector3 keyboardPosition = PositionKeyboard();
            keyboardPosition.y = character.position.y;
            keyboard.position = keyboardPosition;

            // Rotate the keyboard to face the character
            keyboard.LookAt(new Vector3(browser.transform.position.x, keyboard.position.y, browser.transform.position.z));

            // Check and adjust keyboard's position relative to the browser
            AdjustPositionFromBrowser();

            // End adjusting
            isAdjusting = false;
        }
    }

    Vector3 PositionKeyboard()
    {
        // Calculate the direction from the character towards the browser
        Vector3 directionToBrowser = (browser.transform.position - character.position).normalized;

        // Return the position that is 1 unit away from the character in the direction of the browser
        return character.position + directionToBrowser * fixedDistanceToCharacter;
    }

    void AdjustPositionFromBrowser()
    {
        // Keep checking and adjusting the position until it's no longer too close to the browser
        int safetyCounter = 0; // Prevent infinite loop by limiting the number of adjustments
        while (IsTooCloseToBrowser(keyboard.position) && safetyCounter < 100)
        {
            Vector3 escapeDirection = (keyboard.position - browser.ClosestPoint(keyboard.position)).normalized;
            keyboard.position += escapeDirection * 0.1f;  // Incrementally move away from the browser
            keyboard.position = new Vector3(keyboard.position.x, character.position.y, keyboard.position.z);
            safetyCounter++;
        }
    }

    bool IsTooCloseToBrowser(Vector3 position)
    {
        // Calculate the closest point on the browser's collider to the given position
        Vector3 closestPoint = browser.ClosestPoint(position);
        float distance = Vector3.Distance(closestPoint, position);
        return distance < minDistanceToBrowser;
    }
}
