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

    public State CurrentState { get; set; }

	bool mainSceneLoaded;

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
    }

	void InitialiseLevel()
	{
		SceneManager.LoadScene(levelScene);

        // TODO: instantiate stuff
    }

    private void Update()
    {
		if (CurrentState == State.Play && mainSceneLoaded)
		{
			var numPlantsAlive = FindObjectsOfType<Plant>().Length;
			if (numPlantsAlive == 0)
			{
				CurrentState = State.LevelEnd;
				GameUI.OnLevelEnd(false);
			}
		}
    }

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name.Equals(levelScene))
		{
			mainSceneLoaded = true;
        }
	}
}
