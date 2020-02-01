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

    [SerializeField] Text levelEndText;

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
        toolButton.onClick.AddListener(() => toggleTools());
        actionButton.onClick.AddListener(() => useTool());
    }

    public void onLevelStarted()
    {
        UpdateToolButton();
    }

    void UpdateToolButton()
    {
        ColorBlock my_colors = toolButton.colors;
        if (Game.player.currentTool == Player.Tools.Watercan)
        {
            my_colors.normalColor = new Color(0, 0, 255, 255);
            my_colors.highlightedColor = new Color(0, 0, 255, 255);
            my_colors.selectedColor = new Color(0, 0, 255, 255);
            my_colors.pressedColor = new Color(100, 100, 255, 255);
            toolButton.GetComponentInChildren<Text>().text = "WATERCAN";
        }
        else if (Game.player.currentTool == Player.Tools.Pulverizer)
        {
            my_colors.normalColor = new Color(255, 0, 0, 255);
            my_colors.highlightedColor = new Color(255, 0, 0, 255);
            my_colors.selectedColor = new Color(255, 0, 0, 255);
            my_colors.pressedColor = new Color(255, 100, 100, 255);
            toolButton.GetComponentInChildren<Text>().text = "PULVERIZER";
        }
        toolButton.colors = my_colors;
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

    void toggleTools()
    {
        Game.playerToggleTools();
        UpdateToolButton();
    }

    void useTool()
    {
        Game.playerUseTool();
    }
}
