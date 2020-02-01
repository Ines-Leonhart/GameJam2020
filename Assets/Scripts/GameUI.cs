using UnityEngine;
using UnityEngine.UI;

public class GameUI : Singleton
{
    [SerializeField] GameObject frontend;
    [SerializeField] GameObject game;
    [SerializeField] GameObject levelEnd;

    [SerializeField] Button playButton;
    [SerializeField] Button toolButton;
    [SerializeField] Button actionButton;

    [SerializeField] Text countdown;

    [SerializeField] Text levelEndText;

    string tools = "pulverizer";

    Game Game
    {
        get
        {
            return Singleton.Get<Game>();
        }
    }

	protected override void Awake()
	{
		GameObject.DontDestroyOnLoad(gameObject);
		base.Awake();
	}

    void Start()
    {
        playButton.onClick.AddListener(() => Game.StartGame());
        toolButton.onClick.AddListener(() => toggleTools(ref tools));
        actionButton.onClick.AddListener(() => useTool(tools));
    }

    void Update()
    {
        frontend.SetActive(Game.CurrentState == Game.State.Frontend);
        game.SetActive(Game.CurrentState == Game.State.Play);
        levelEnd.SetActive(Game.CurrentState == Game.State.LevelEnd);
    }

    public void OnLevelEnd(bool win)
    {
        levelEndText.text = win ? "YOU WIN!" : "YOU LOSE!";
    }

    void toggleTools(ref string tool)
    {
        ColorBlock my_colors = toolButton.colors;
        if (tool == "watercan")
        {
            my_colors.normalColor = new Color(255, 0, 0, 255);
            my_colors.highlightedColor = new Color(255, 0, 0, 255);
            my_colors.selectedColor = new Color(255, 0, 0, 255);
            my_colors.pressedColor = new Color(255, 100, 100, 255);
            toolButton.GetComponentInChildren<Text>().text = "WATERCAN";
            tool = "pulverizer";
        }
        else if (tool == "pulverizer")
        {
            my_colors.normalColor = new Color(0, 0, 255, 255);
            my_colors.highlightedColor = new Color(0, 0, 255, 255);
            my_colors.selectedColor = new Color(0, 0, 255, 255);
            my_colors.pressedColor = new Color(100, 100, 255, 255);
            toolButton.GetComponentInChildren<Text>().text = "PULVERIZER";
            tool = "watercan";
        }
        toolButton.colors = my_colors;
        Debug.Log(tool);
        Game.playerToggleTools(tool);
    }

    void useTool(string tool)
    {
        Game.playerUseTool(tool);
    }

    public void UpdateCountdown(float time)
    {
        var t = System.TimeSpan.FromSeconds(time);
        countdown.text = string.Format("{0:00}:{1:00}", t.Minutes, t.Seconds);
    }
}
