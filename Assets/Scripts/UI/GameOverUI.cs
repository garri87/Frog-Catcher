using UnityEngine;
using UnityEngine.UIElements;

public class GameOverUI : MonoBehaviour
{
    private UIDocument _uiDocument;

    private Label _titleLabel;
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

        _titleLabel = root.Query<Label>("Text");
        _retryButton = root.Q<Button>("RetryButton");
        _mainMenuButton = root.Q<Button>("MainMenuButton");
        _exitButton = root.Q<Button>("ExitButton");
        _infoTextLabel = root.Q<Label>("InfoText");

        Debug.Log($"Game Over state: {gameManager.IsGameOver}");

        if (gameManager.IsGameOver)
        {
            _retryButton.text = "Retry";
            _retryButton.UnregisterCallback<ClickEvent>(evt => gameManager.TogglePauseGame());
            _retryButton.RegisterCallback<ClickEvent>(evt => gameManager.StartGame(gameManager.Level));
        }
        else
        {
            _retryButton.text = "Next Level";
            _retryButton.UnregisterCallback<ClickEvent>(evt => gameManager.TogglePauseGame());
            _retryButton.RegisterCallback<ClickEvent>(evt => gameManager.NextLevel());
        }
        _mainMenuButton.RegisterCallback<ClickEvent>(evt => gameManager.OpenMainMenu());
        _exitButton.RegisterCallback<ClickEvent>(evt => gameManager.QuitGame());

        _infoTextLabel.text = $"Captured Frogs:{gameManager.catchedFrogs}";
    }

    private void Update()
    {
        
        if (gameManager.isPaused)
        {
            _titleLabel.text = "Paused";
        }
        else
        {
          _titleLabel.text = (gameManager.IsGameOver) ? gameManager.gameOverCause : "Level Complete";      
        }
        _infoTextLabel.text = $"Captured Frogs:{gameManager.catchedFrogs}";

    }

   
}
