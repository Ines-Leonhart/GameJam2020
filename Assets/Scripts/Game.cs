﻿using UnityEngine;
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
    public Player player { get; set; }

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
        }
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
            player = GameObject.FindObjectOfType<Player>();
            GameUI.onLevelStarted();
        }
	}
}
