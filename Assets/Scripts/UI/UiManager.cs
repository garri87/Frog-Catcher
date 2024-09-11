using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UiManager : MonoBehaviour
{
    public UIDocument[] uiDocuments;


    private void Awake()
    {
        if (uiDocuments.Length == 0 )
        {
            uiDocuments = GetComponentsInChildren<UIDocument>(includeInactive:true);
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


    public void CloseAllUI()
    {
        foreach (var uiDocument in uiDocuments)
        {
            uiDocument.gameObject.SetActive(false);
        }
    }
}
