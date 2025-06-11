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

   
           

    public int frogsToSpawn = 5;
    public int beesToSpawn = 1;
    public int maxBeesToSpawn = 5;
    private float levelSpawnMultiplier = 1.5f;
    private int frogsLeft = 0;
    public int FrogsLeft
    {
        get { return frogsLeft; }
    }

    private List<GameObject> frogsList;
   
    public List<GameObject> crocodiles;
    public List<GameObject> bees;

    private GameObject player;

    public GameObject frogPrefab;
    public GameObject beePrefab;
    public GameObject crocodilePrefab;
    public bool enableCrocodiles;
    public GameObject powerUpPrefab;
    public GameObject powerUpGO;

    public float minSpawnDistance = 5f;  // Distancia mínima desde el jugador
    public float maxSpawnDistance = 10f; // Distancia máxima desde el jugador

    public float levelTimer = 60f;
    private float timer;
    public float Timer
    {
        get { return timer; }
    }

    public Vector3 playerStartPos = new Vector3(0, 1, 0);


    public float mapLimitX = 17f;
    public float mapLimitZ = 13f;
    public float mapLimitY = 20f;


    private int maxScore = 0;
    public int MaxScore
    {
        get { return maxScore; }
    }

    public float minPowerUpSpawnRate = 5f;
    public float maxPowerUpSpawnRate = 10f;
    private float powerUpSpawnTimer;

    
   
  

    private bool isGameOver;
    public bool IsGameOver
    {
        get { return isGameOver; }
    }

    [HideInInspector]
    public int catchedFrogs = 0;

    private int level = 1;
    public int Level
    {
        get { return level; }
    }

    public bool isPaused;

    private bool matchStarted;

    public string gameOverCause = "Game Over";

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

        uiManager = GameObject.Find("UI").GetComponent<UiManager>();
        
        if (Application.isMobilePlatform)
        {
            Application.targetFrameRate = 30;
        }
             
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
        powerUpGO = Instantiate(powerUpPrefab,transform.position,transform.rotation);
        powerUpGO.SetActive(false);
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
                    GameOver("Out of time!");
                }
            }
            
            //Enable crocodile event
            if (timer < levelTimer / 2)
            {
                if (crocodiles != null && enableCrocodiles)
                {
                    EnableEntities(crocodiles,true);
                }
            }

            //Enable power up event
            if (!powerUpPrefab.activeSelf)
            {
                powerUpSpawnTimer -= Time.deltaTime;
            
                if (powerUpSpawnTimer < 0)
                {
                    float randomX = Random.Range(-mapLimitX, mapLimitX);
                    float randomY = Random.Range(-mapLimitY, mapLimitY);
                    powerUpGO.transform.position = new Vector3(randomX, 1,randomY);
                    powerUpSpawnTimer = Random.Range(minPowerUpSpawnRate, maxPowerUpSpawnRate);
                    powerUpGO.SetActive(true);
                }
            }
        }
    }

    public GameObject InstantiateEntity(Vector3 spawnPos, GameObject prefab)
    {
        GameObject entity = Instantiate(prefab, spawnPos, prefab.transform.rotation);

        return entity;
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

    public void GameOver(string cause)
    {
        if (!isGameOver)
        {
            timer = 0;
            Debug.Log("Game Over by " + cause);
            gameOverCause = cause;
            CheckMaxScore();
            
            player.SetActive(false);
            powerUpPrefab.SetActive(false);
            isGameOver = true;
            matchStarted = false;

            uiManager.ToggleUI("GameOver", true);
        }
    }

    public void WinEvent()
    {
        timer = 0;
        player.SetActive(false);
        isGameOver = false;
        matchStarted = false;
        uiManager.ToggleUI("GameOver", true);
    }


    public void StartGame(int level)
    {

        uiManager.EnableMobileControls(Application.isMobilePlatform);
        
        if (isPaused)
        {
            TogglePauseGame();
        }
        isGameOver = false;
        Debug.Log($"Level {level} Start");
        if (uiManager)
        {
            uiManager.ToggleUI("InGameOverlay", true);
        }

        timer = levelTimer + 0.9f;
        
        powerUpSpawnTimer = maxPowerUpSpawnRate;
        powerUpPrefab.SetActive(false);

        catchedFrogs = 0;

        frogsList = GameObject.FindGameObjectsWithTag("Frog").ToList();
        bees = GameObject.FindGameObjectsWithTag("Bee").ToList();

        SpawnEntities(frogsList,frogsToSpawn,frogPrefab);
        SpawnEntities(bees,beesToSpawn,beePrefab);
        SpawnEntities(crocodiles,1,crocodilePrefab);

        EnableEntities(frogsList,false);
        EnableEntities(bees, false);
        EnableEntities(crocodiles, false);

        if (player)
        {
            player.transform.position = playerStartPos;
            player.SetActive(true);
        }

        EnableEntities(bees,true);
        EnableEntities(frogsList,true);

        frogsLeft = frogsList.Count;

        matchStarted = true;
    }

    private void SpawnEntities(List<GameObject> entityList, int spawnCount, GameObject prefab)
    {

        if (entityList.Count < spawnCount)
        {
            while (entityList.Count < spawnCount)
            {
                Vector3 pos = new Vector3(transform.position.x,0,transform.position.z);
                GameObject entity = InstantiateEntity(transform.position, prefab);
                entityList.Add(entity);
            }

        }
    }

    private void EnableEntities(List<GameObject> entities, bool active)
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

    /// <summary>
    /// returns a vector3 in a random point around the center of the map
    /// </summary>
    /// <returns></returns>
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
        EnableEntities(frogsList,false);
        EnableEntities(crocodiles,false);
        EnableEntities(bees,false);

        uiManager.ToggleUI("MainMenu", true);
    }

    public void TogglePauseGame()
    {
        if (!isGameOver)
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
        Gizmos.color = new Color(232f,253f,0f,0.22f);
        Gizmos.DrawCube(transform.position, size); 
    }

    public bool CheckOutOfTargets()
    {
        bool outOfTargets = true;
        for (int i = 0; i < frogsList.Count; i++)
        {
            if (frogsList[i].activeSelf)
            {
                outOfTargets = false;
            }
        }
        return outOfTargets;
    }

    

    public void NextLevel()
    {
        if (CheckOutOfTargets())
        {
            level++;
            frogsToSpawn = Mathf.RoundToInt(frogsToSpawn * levelSpawnMultiplier);
            if (beesToSpawn < maxBeesToSpawn)
            {
                beesToSpawn++;
            }
            StartGame(level);

        }
    }
}
