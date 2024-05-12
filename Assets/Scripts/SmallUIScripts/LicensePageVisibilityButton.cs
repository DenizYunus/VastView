using UnityEngine;

public class LicensePageVisibilityButton : MonoBehaviour
{
    public GameObject pageGameObject;

    public void TogglePageVisibility()
    {
        pageGameObject.SetActive(!pageGameObject.activeSelf);
    }

    public void ClosePage()
    {
        pageGameObject.SetActive(false);
    }
}