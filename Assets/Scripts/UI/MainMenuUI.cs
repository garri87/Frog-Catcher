using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    private UIDocument uiDocument;
    private Button startGameButton;
    private Button exitGameButton;
    private Button optionsButton;
    private Label maxScoreLabel;

    private GameManager gameManager;
    private UiManager uiManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        uiManager = GameObject.FindObjectOfType<UiManager>();
    }
    private void OnEnable()
    {
        uiDocument= GetComponent<UIDocument>();

        VisualElement root = uiDocument.rootVisualElement;

        startGameButton = root.Q<Button>("StartButton");
        optionsButton = root.Q<Button>("OptionsButton");
        exitGameButton = root.Q<Button>("ExitButton");
        maxScoreLabel = root.Q<Label>("MaxScoreLabel");

        startGameButton.RegisterCallback<ClickEvent>( evt => ShowTutorial());
        exitGameButton.RegisterCallback<ClickEvent>(evt => gameManager.QuitGame());

        UpdateMaxScoreLabel();

    }

    private void UpdateMaxScoreLabel()
    {
      maxScoreLabel.text = $"Max Score: {PlayerPrefs.GetInt("MaxScore",0)}"; 
    }

    private void ShowTutorial()
    {
        if (uiManager)
        {
            uiManager.ToggleUI("Tutorial",true);
        }
        else
        {
            Debug.Log("No UI Manager Found");
        }
    }
}
