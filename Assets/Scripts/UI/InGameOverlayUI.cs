using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameOverlayUI : MonoBehaviour
{
    public UIDocument uIDocument;

    private Label frogCountLabel;
    
    private Label timerLabel;

    private GameManager _gameManager;

    private int catchedFrogs = 0;

    private void OnValidate()
    {
        

    }

    private void OnEnable()
    {
        _gameManager = GameManager.Instance;
        var root = uIDocument.rootVisualElement;
        frogCountLabel = root.Query<Label>("FrogCount");
        timerLabel = root.Query<Label>("Timer");

        frogCountLabel.text = catchedFrogs.ToString();
    }

    private void Update()
    {
        int minutes = Mathf.FloorToInt(_gameManager.Timer / 60);  
        int seconds = Mathf.FloorToInt(_gameManager.Timer % 60);

        // Formatear como "MM:SS"
        string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        timerLabel.text = formattedTime;

        catchedFrogs = _gameManager.catchedFrogs;
        frogCountLabel.text = catchedFrogs.ToString();

    }
}
