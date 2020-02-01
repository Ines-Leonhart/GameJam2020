﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : Singleton
{
    [SerializeField] GameObject frontend;
    [SerializeField] GameObject game;
    [SerializeField] GameObject levelEnd;
    [SerializeField] Button playButton;

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
    }

    void Update()
    {
        frontend.SetActive(Game.CurrentState == Game.State.Frontend);
        game.SetActive(Game.CurrentState == Game.State.Play);
        levelEnd.SetActive(Game.CurrentState == Game.State.LevelEnd);
    }
}
