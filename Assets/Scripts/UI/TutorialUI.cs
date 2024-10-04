using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialUI : MonoBehaviour
{
    private UIDocument uiDocument;

    private VisualElement root;

    private Button startButton;

    private GameManager gameManager;

    private void Awake()
    {
        uiDocument= GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        startButton = root.Q<Button>("StartButton");

        startButton.RegisterCallback<ClickEvent>(evt => gameManager.StartGame(gameManager.level));
    }

}
