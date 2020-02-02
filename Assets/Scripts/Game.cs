using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Game : Singleton
{
#if UNITY_EDITOR
    [MenuItem("Utils / Reset Progress")]
    public static void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
#endif

    GameUI GameUI
	{
		get
		{
			return Singleton.Get<GameUI>();
		}
	}

	public enum State
	{
        Frontend,
        Play,
        LevelEnd
	}

	[SerializeField] string levelScene;
    [SerializeField] public float levelDuration;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] public int minGrid;
    [SerializeField] public int maxGrid;
    [SerializeField] public int maxEnemies;
    [SerializeField] public int minPlants;
    [SerializeField] public int increase;
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject menuCamera;

    public State CurrentState { get; set; }
    public int PlayerLevel { get; private set; }

    public bool gameStarted { get; set; }
    bool gameStartedDone;
    float levelStartTimestamp;

    GameObject player;
    public Player Player
    {
        get
        {
            if (player != null)
            {
                return player.GetComponent<Player>();
            }

            return null;
        }
    }

	protected override void Awake()
	{
		Application.targetFrameRate = 60;
		base.Awake();
        LoadProgress();
	}

    void Start()
    {
        CurrentState = State.Frontend;
        menuCamera.SetActive(true);
        mainCamera.SetActive(false);
        InitialiseLevel();
	}

	public void StartGame()
	{
        gameStarted = false;
        gameStartedDone = false;
		CurrentState = State.Play;
        levelStartTimestamp = 0;
        menuCamera.SetActive(false);
        mainCamera.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void LoadLevel()
    {
        SceneManager.LoadScene(levelScene);
    }

	void InitialiseLevel()
	{
        Destroy(player);
        player = Instantiate(playerPrefab);
        GameUI.onLevelStarted();
    }

    void EndLevel(bool win)
    {
        CurrentState = State.LevelEnd;
        GameUI.OnLevelEnd(win);
        levelStartTimestamp = 0;

        // Explode all enemies!
        if (win)
        {
            var enemies = GameObject.FindObjectsOfType<enemyMovement>();
            for (var i = 0; i < enemies.Length; ++i)
            {
                enemies[i].Kill();
            }

            ++PlayerLevel;
            SaveProgress();
        }
    }

    public void playerToggleTools()
    {
        if(Player != null)
        {
            Player.ToggleMyTools();
        }
    }

    public void playerUseTool()
    {
        if (Player != null)
        {
            Player.UseMyTools();
        }
    }

    private void Update()
    {
		if (CurrentState == State.Play)
		{
            var plants = GameObject.FindObjectsOfType<Plant>();
            if (plants != null)
            {
                for (int i = 0; i < plants.Length; i++)
                {
                    if (plants[i].CurrentState == Plant.State.Grown)
                    {
                        gameStarted = true;
                    }
                }
            }

            if (gameStarted && !gameStartedDone)
            {
                Debug.Log(levelStartTimestamp);
                levelStartTimestamp = Time.realtimeSinceStartup;
                gameStartedDone = true;

            }

            var numPlantsAlive = FindObjectsOfType<Plant>().Length;
			if (numPlantsAlive == 0)
			{
                EndLevel(false);
			}

            if(levelStartTimestamp != 0)
            {
                if (Time.realtimeSinceStartup - levelStartTimestamp > levelDuration)
                {
                    EndLevel(true);
                }
                else
                {
                    GameUI.UpdateCountdown(levelDuration - (Time.realtimeSinceStartup - levelStartTimestamp));
                }
            }
                
		}
    }

    public void RestartLevel()
    {
        LoadLevel();
        InitialiseLevel();
    }

    public void NextLevel()
    {
        LoadLevel();
        InitialiseLevel();
    }

    void LoadProgress()
    {
        PlayerLevel = PlayerPrefs.GetInt("PlayerLevel", 0);
    }

    void SaveProgress()
    {
        PlayerPrefs.SetInt("PlayerLevel", PlayerLevel);
        PlayerPrefs.Save();
    }
}
