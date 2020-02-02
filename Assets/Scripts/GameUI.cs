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
    [SerializeField] Image progressFill;

    [SerializeField] Image levelEndImage;
    [SerializeField] Sprite WinImage;
    [SerializeField] Sprite LoseImage;
    [SerializeField] Button restartButton;
    [SerializeField] Button nextButton;

    [SerializeField] Text playerLevel;

    Game Game
    {
        get
        {
            return Singleton.Get<Game>();
        }
    }

	protected override void Awake()
	{
		base.Awake();
	}

    void Start()
    {
        playButton.onClick.AddListener(() => { Game.StartGame(); Game.GetComponent<AudioSource>().Play(); });
        quitButton.onClick.AddListener(() => { Game.QuitGame(); Game.GetComponent<AudioSource>().Play(); });
        toolButton.onClick.AddListener(() => { toggleTools(); Game.GetComponent<AudioSource>().Play(); });
        actionButton.onClick.AddListener(() => { useTool(); });

        restartButton.onClick.AddListener(() => { Game.RestartLevel(); Game.GetComponent<AudioSource>().Play(); });
        nextButton.onClick.AddListener(() => { Game.NextLevel(); Game.GetComponent<AudioSource>().Play(); });
    }

    public void onLevelStarted()
    {
        UpdateToolButton();
        UpdateCountdown(Game.levelDuration);
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

        playerLevel.text = string.Format("Level {0}", Game.PlayerLevel + 1);
    }

    public void OnLevelEnd(bool win)
    {
        levelEndImage.sprite = win ? WinImage : LoseImage;
        restartButton.gameObject.SetActive(!win);
        nextButton.gameObject.SetActive(win);
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

        progressFill.fillAmount = Mathf.Clamp(1 - (time / Game.levelDuration), 0, 1);
    }
}
