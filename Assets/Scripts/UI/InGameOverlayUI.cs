using UnityEngine;
using UnityEngine.UIElements;

public class InGameOverlayUI : MonoBehaviour
{
    public UIDocument uIDocument;

    private Label frogCountLabel;
    
    private Label timerLabel;

    private Button pauseButton;

    private GameManager _gameManager;

    private int catchedFrogs = 0;

    private void OnValidate()
    {
        

    }

    private void OnEnable()
    {
        _gameManager = GameManager.Instance;
        var root = uIDocument.rootVisualElement;
        frogCountLabel = root.Q<Label>("FrogCount");
        timerLabel = root.Q<Label>("Timer");
        pauseButton = root.Q<Button>("PauseButton");

        frogCountLabel.text = catchedFrogs.ToString();

        pauseButton.RegisterCallback<ClickEvent>(evt => _gameManager.TogglePauseGame());
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
