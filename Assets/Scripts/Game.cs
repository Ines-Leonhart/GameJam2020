using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : Singleton
{
	public enum State
	{
        Frontend,
        Play,
        LevelEnd
	}

	[SerializeField] string levelScene;

    public State CurrentState { get; set; }

	protected override void Awake()
	{
		GameObject.DontDestroyOnLoad(gameObject);
		Application.targetFrameRate = 60;
		base.Awake();
	}

    void Start()
    {
		CurrentState = State.Frontend;    
    }

	public void StartGame()
	{
		CurrentState = State.Play;
		InitialiseLevel();
    }

	void InitialiseLevel()
	{
		SceneManager.LoadScene(levelScene);

        // TODO: instantiate stuff
    }
}
