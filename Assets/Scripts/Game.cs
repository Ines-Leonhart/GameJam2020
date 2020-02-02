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

    public State CurrentState { get; set; }
    public int PlayerLevel { get; private set; }

	bool mainSceneLoaded;
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
		GameObject.DontDestroyOnLoad(gameObject);
		Application.targetFrameRate = 60;
		base.Awake();
        LoadProgress();
	}

    void Start()
    {
		CurrentState = State.Frontend;
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public void StartGame()
	{
        mainSceneLoaded = false;
        gameStarted = false;
        gameStartedDone = false;
		CurrentState = State.Play;
		InitialiseLevel();
        levelStartTimestamp = 0;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

	void InitialiseLevel()
	{
		SceneManager.LoadScene(levelScene);

        // TODO: instantiate stuff
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
		if (CurrentState == State.Play && mainSceneLoaded)
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

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name.Equals(levelScene))
		{
			mainSceneLoaded = true;
            player = Instantiate(playerPrefab);
            GameUI.onLevelStarted();
        }
	}

    public void RestartLevel()
    {
        mainSceneLoaded = false;
        StartGame();
    }

    public void NextLevel()
    {
        mainSceneLoaded = false;
        StartGame();
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
