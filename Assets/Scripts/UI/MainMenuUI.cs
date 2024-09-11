using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }
    private void OnEnable()
    {
        uiDocument= GetComponent<UIDocument>();

        VisualElement root = uiDocument.rootVisualElement;

        startGameButton = root.Q<Button>("StartButton");
        optionsButton = root.Q<Button>("OptionsButton");
        exitGameButton = root.Q<Button>("ExitButton");
        maxScoreLabel = root.Q<Label>("MaxScoreLabel");

        startGameButton.RegisterCallback<ClickEvent>(evt => gameManager.StartGame(gameManager.level));
        exitGameButton.RegisterCallback<ClickEvent>(evt => gameManager.QuitGame());

        UpdateMaxScoreLabel();

    }

    private void UpdateMaxScoreLabel()
    {
      maxScoreLabel.text = $"Max Score: {PlayerPrefs.GetInt("MaxScore",0)}"; 
    }
}
