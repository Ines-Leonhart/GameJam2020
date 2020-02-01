using UnityEngine;
using UnityEngine.UI;

public class GameUI : Singleton
{
    [SerializeField] GameObject frontend;
    [SerializeField] GameObject game;
    [SerializeField] GameObject levelEnd;

    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;
    [SerializeField] Button toolButton;
    [SerializeField] Image toolImage;
    [SerializeField] Sprite waterSprite;
    [SerializeField] Sprite fertilizerSprite;
    [SerializeField] Button actionButton;

    [SerializeField] Text countdown;

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
        quitButton.onClick.AddListener(() => Game.QuitGame());
        toolButton.onClick.AddListener(() => toggleTools());
        actionButton.onClick.AddListener(() => useTool());
    }

    public void onLevelStarted()
    {
        UpdateToolButton();
    }

    void UpdateToolButton()
    {
        toolImage.sprite = Game.Player.currentTool == Player.Tools.Watercan ? waterSprite : fertilizerSprite;
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

    public void UpdateCountdown(float time)
    {
        var t = System.TimeSpan.FromSeconds(time);
        countdown.text = string.Format("{0:00}:{1:00}", t.Minutes, t.Seconds);
    }
}
