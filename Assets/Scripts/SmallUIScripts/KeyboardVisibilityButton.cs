using UnityEngine;

public class KeyboardVisibilityButton : MonoBehaviour
{
    public GameObject keyboardGameObject;

    private void Start()
    {
        keyboardGameObject.SetActive(false);
    }

    public void ToggleKeyboardVisibility()
    {
        keyboardGameObject.SetActive(!keyboardGameObject.activeSelf);
    }
}