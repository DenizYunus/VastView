using System.Collections.Generic;
using UnityEngine;

public class LicensePageVisibilityButton : MonoBehaviour
{
    public GameObject pageGameObject;
    public List<GameObject> otherButtonsToBeToggled;

    public void TogglePageVisibility()
    {
        pageGameObject.SetActive(!pageGameObject.activeSelf);
        foreach (var button in otherButtonsToBeToggled)
        {
            button.SetActive(!pageGameObject.activeSelf);
        }
    }

    public void ClosePage()
    {
        pageGameObject.SetActive(false);
        foreach (var button in otherButtonsToBeToggled)
        {
            button.SetActive(true);
        }
    }
}