using UnityEngine;

public class MenuVisibilityButton : MonoBehaviour
{
    public GameObject menuGameObject;

    private void Start()
    {
        menuGameObject.SetActive(false);
    }

    public void ToggleMenuVisibility()
    {
        menuGameObject.SetActive(!menuGameObject.activeSelf);
    }
}