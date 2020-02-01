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

    public Player player { get; set; }

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

    public void QuitGame()
    {
        Application.Quit();
    }

	void InitialiseLevel()
	{
		SceneManager.LoadScene(levelScene);

        // TODO: instantiate stuff
    }

    public void playerToggleTools()
    {
        if(GameObject.Find("Player") != null)
        {
            player.ToggleMyTools();
        }
    }

    public void playerUseTool()
    {
        if (GameObject.Find("Player") != null)
        {
            player.UseMyTools();
        }
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
            player = GameObject.FindObjectOfType<Player>();
            GameUI.onLevelStarted();
        }
	}
}
