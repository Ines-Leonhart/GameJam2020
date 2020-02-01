using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : Singleton
{
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
    [SerializeField] float levelDuration;

    public State CurrentState { get; set; }

	bool mainSceneLoaded;
    float levelStartTimestamp;

	protected override void Awake()
	{
		GameObject.DontDestroyOnLoad(gameObject);
		Application.targetFrameRate = 60;
		base.Awake();
	}

    void Start()
    {
		CurrentState = State.Frontend;
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public void StartGame()
	{
		mainSceneLoaded = false;
		CurrentState = State.Play;
		InitialiseLevel();
        levelStartTimestamp = 0;
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
        }
    }

    public void playerToggleTools(string tool)
    {
        if(GameObject.Find("Player") != null)
        {
            if (tool == "watercan")
            {
                Debug.Log("Player changed to " + tool);
            }
            else if (tool == "pulverizer")
            {
                Debug.Log("Player changed to " + tool);
            }
        }
    }

    public void playerUseTool(string tool)
    {
        if (GameObject.Find("Player") != null)
        {
            if (GameObject.Find("Player") != null)
            {
                GameObject player = GameObject.Find("Player");
                if (tool == "watercan")
                {
                    Debug.Log("Player used " + tool + " on cell " + player.GetComponent<Player>().currentCell.GetInstanceID());
                    
                }
                else if (tool == "pulverizer")
                {
                    Debug.Log("Player used " + tool + " on cell " + player.GetComponent<Player>().currentCell.GetInstanceID());
                }
            }
        }
    }

    private void Update()
    {
		if (CurrentState == State.Play && mainSceneLoaded)
		{
			var numPlantsAlive = FindObjectsOfType<Plant>().Length;
			if (numPlantsAlive == 0)
			{
                EndLevel(false);
			}

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

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name.Equals(levelScene))
		{
			mainSceneLoaded = true;
            levelStartTimestamp = Time.realtimeSinceStartup;
        }
	}
}
