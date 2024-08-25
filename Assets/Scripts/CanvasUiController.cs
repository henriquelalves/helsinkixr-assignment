using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUi : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private CanvasGroup _initialScreenCanvas;
    [SerializeField] private CanvasGroup _gameOverScreenCanvas;
    [SerializeField] private Text _starsLabel;
    void Start()
    {
        _player.FirstJumped += OnPlayerFirstJumped;
        _player.PlayerKilled += OnPlayerKilled;
        _player.StarCollected += OnStarCollected;
    }

    private void OnDestroy()
    {
        _player.FirstJumped -= OnPlayerFirstJumped;
        _player.PlayerKilled -= OnPlayerKilled;
        _player.StarCollected -= OnStarCollected;
    }

    void OnStarCollected()
    {
        _starsLabel.text = $"Stars: {_player.CollectedStars}";
    }
    
    void OnPlayerKilled()
    {
        Utils.FadeCanvasGroup(this, _gameOverScreenCanvas, 1f, 0.3f);
    }
    
    void OnPlayerFirstJumped()
    {
        Utils.FadeCanvasGroup(this, _initialScreenCanvas, 0f, 0.3f);
    }
}
