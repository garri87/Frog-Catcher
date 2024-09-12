using UnityEngine;
using UnityEngine.UIElements;

public class UiManager : MonoBehaviour
{
    public UIDocument[] uiDocuments;
    public Canvas joystickCanvas;

    private void Awake()
    {
        if (uiDocuments.Length == 0 )
        {
            uiDocuments = GetComponentsInChildren<UIDocument>(includeInactive:true);
        }

        if (!joystickCanvas)
        {
            joystickCanvas = transform.Find("JoystickCanvas").GetComponent<Canvas>();
        }
    }

    public void ToggleUI(string name, bool enabled)
    {
        CloseAllUI();
        foreach (var uiDocument in uiDocuments)
        {
            if(uiDocument.gameObject.name == name)
            {
                uiDocument.gameObject.SetActive(enabled);
            }
        }
    }

    public void EnableMobileControls(bool enabled = true)
    {       
       joystickCanvas.enabled = enabled;
    }


    public void CloseAllUI()
    {
        foreach (var uiDocument in uiDocuments)
        {
            uiDocument.gameObject.SetActive(false);
        }
    }
}
