using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("GameManager Instance started");
                _instance = FindObjectOfType<GameManager>();
                if (_instance)
                {
                    Debug.Log("GameManager Instance found");
                }
                else
                {
                    Debug.LogError("No GameManager in the scene!");
                }
            }
            return _instance;
        }
    }

    private UiManager uiManager;

    public int frogCount = 5;


    private List<GameObject> frogs;
    public List<GameObject> crocodiles;
    public List<GameObject> bees;

    private GameObject player;

    public GameObject frogPrefab;

    public float minSpawnDistance = 5f;  // Distancia mínima desde el jugador
    public float maxSpawnDistance = 10f; // Distancia máxima desde el jugador

    public float levelTimer = 60f;
    private float timer;

    public Vector3 playerStartPos = new Vector3(0, 0, 0);


    public float mapLimitX = 17f;
    public float mapLimitZ = 13f;
    public float mapLimitY = 20f;

    private int maxScore = 0;
    public int MaxScore
    {
        get { return maxScore; }
    }
   
    public float Timer
    {
        get { return timer; }
    }

    private bool gameOver;
    public bool IsGameOver
    {
        get { return gameOver; }
    }

    [HideInInspector]
    public int catchedFrogs = 0;

    public int level = 1;

    public bool isPaused;

    private bool matchStarted;

    private void OnValidate()
    {
        player = GameObject.Find("Player");
        if (player)
        {
            player.transform.position = playerStartPos;

        }
    }

    private void Awake()
    {
        if (Application.isMobilePlatform)
        {
            Application.targetFrameRate = 30;
        }

        uiManager = GameObject.Find("UI").GetComponent<UiManager>();
        player = GameObject.Find("Player");
        if (player)
        {
            player.SetActive(false);
        }
        else
        {
            Debug.Log("Player Not Found");
        }
    }

    void Start()
    {
        CheckMaxScore();
        matchStarted = false;
        if (uiManager)
        {
            uiManager.ToggleUI("MainMenu", true);
        }
    }

    void Update()
    {
        if (matchStarted)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    timer = 0;
                    GameOver();
                }
            }
            
            if (timer < levelTimer / 2)
            {
                if (crocodiles != null)
                {
                    ToggleEntities(crocodiles,true);
                }
                else
                {
                    foreach (var croc in crocodiles)
                    {
                        croc.SetActive(false);
                    }
                }
            }

            uiManager.EnableMobileControls(player.activeSelf);
        }
    }

    public GameObject InstantiateFrog(Vector3 spawnPos)
    {
        GameObject frog = Instantiate(frogPrefab, spawnPos, frogPrefab.transform.rotation);

        return frog;
    }

    private void CheckMaxScore()
    {

        maxScore = PlayerPrefs.GetInt("MaxScore", 0);

        if (maxScore < catchedFrogs)
        {
            Debug.Log($"New record! {catchedFrogs} frogs!");
            PlayerPrefs.SetInt("MaxScore", catchedFrogs);
        }
    }

    public void GameOver()
    {
        if (!gameOver)
        {
            timer = 0;
            Debug.Log("Game Over");
            CheckMaxScore();
            
            player.SetActive(false);

            gameOver = true;
            matchStarted = false;
            uiManager.ToggleUI("GameOver", true);
        }
    }

    public void StartGame(int level)
    {
        if (isPaused)
        {
            TogglePauseGame();
        }
        gameOver = false;
        Debug.Log($"Level {level} Start");
        if (uiManager)
        {
            uiManager.ToggleUI("InGameOverlay", true);
        }

        timer = levelTimer + 0.9f;
        catchedFrogs = 0;

        frogs = GameObject.FindGameObjectsWithTag("Frog").ToList();

        if (frogs.Count < frogCount)
        {
            while (frogs.Count < frogCount)
            {
                GameObject frog = InstantiateFrog(transform.position);
                frogs.Add(frog);
            }

        }
        ToggleEntities(frogs,false);
        ToggleEntities(bees, false);
        ToggleEntities(crocodiles, false);

        if (player)
        {
         

            player.transform.position = playerStartPos;
            player.SetActive(true);
        }

        ToggleEntities(bees,true);
        ToggleEntities(frogs,true);


        matchStarted = true;
    }

    private void ToggleEntities(List<GameObject> entities, bool active)
    {
        if (entities != null)
        {
            foreach (var entity in entities)
            {
                if (entity.activeSelf == !active)
                {
                    entity.transform.position = RandomVector();
                    entity.SetActive(active);
                }
            }
        }

    }

    private Vector3 RandomVector()
    {
        Vector3 spawnPosition = Vector3.zero;
        float randomAngle = Random.Range(0f, 360f);
        float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);

        if (player)
        {
            spawnPosition = new Vector3(
                        player.transform.position.x + randomDistance * Mathf.Cos(randomAngle * Mathf.Deg2Rad),
                        player.transform.position.y,
                        player.transform.position.z + randomDistance * Mathf.Sin(randomAngle * Mathf.Deg2Rad)
                    );
        }

        return spawnPosition;


    }

    public void OpenMainMenu()
    {
        player.SetActive(false);
        ToggleEntities(frogs,false);
        ToggleEntities(crocodiles,false);
        ToggleEntities(bees,false);
        uiManager.ToggleUI("MainMenu", true);
    }

    public void TogglePauseGame()
    {
        if (!gameOver)
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                uiManager.ToggleUI("GameOver", true);
                Time.timeScale = 0;
            }
            else
            {
                uiManager.ToggleUI("InGameOverlay", true);
                Time.timeScale = 1;
            }
        }
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        Debug.Log("Pause Called");
        TogglePauseGame();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    private void OnDrawGizmosSelected()
    {
       Vector3 size = new Vector3(mapLimitX, mapLimitY, mapLimitZ)*2;
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position, size); 
    }
}
