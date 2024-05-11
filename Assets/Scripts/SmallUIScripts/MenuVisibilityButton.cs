using UnityEngine;

public class MenuVisibilityButton : MonoBehaviour
{
    public GameObject menuGameObject;

    public void ToggleMenuVisibility()
    {
        menuGameObject.SetActive(!menuGameObject.activeSelf);
    }
}