using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverUI : MonoBehaviour
{
    private UIDocument _uiDocument;

    private Label _textLabel;
    public Label _infoTextLabel;

    private Button _retryButton;
    private Button _mainMenuButton;
    private Button _exitButton;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        _uiDocument= GetComponent<UIDocument>();
        VisualElement root = _uiDocument.rootVisualElement;

        _textLabel = root.Query<Label>("Text");
        _retryButton = root.Q<Button>("RetryButton");
        _mainMenuButton = root.Q<Button>("MainMenuButton");
        _exitButton = root.Q<Button>("ExitButton");
        _infoTextLabel = root.Q<Label>("InfoText");

        _retryButton.RegisterCallback<ClickEvent>(evt => gameManager.StartGame(gameManager.level) );
        _mainMenuButton.RegisterCallback<ClickEvent>(evt => gameManager.OpenMainMenu());
        _exitButton.RegisterCallback<ClickEvent>(evt => gameManager.QuitGame());

        _infoTextLabel.text = $"Captured Frogs:{gameManager.catchedFrogs}";
    }

    private void Update()
    {
        
        if (gameManager.isPaused)
        {
            _textLabel.text = "Paused";
        }
        else
        {
            _textLabel.text = "Game Over";
        }
        _infoTextLabel.text = $"Captured Frogs:{gameManager.catchedFrogs}";

    }

 
}
