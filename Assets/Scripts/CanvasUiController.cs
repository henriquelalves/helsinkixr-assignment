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
    [SerializeField] private CanvasGroup _wonScreenCanvas;
    [SerializeField] private Text _starsLabel;
    void Start()
    {
        _player.FirstJumped += OnPlayerFirstJumped;
        _player.PlayerKilled += OnPlayerKilled;
        _player.StarCollected += OnStarCollected;
        _player.ArrivedFinishLine += OnArrivedFinishLine;
    }

    private void OnDestroy()
    {
        _player.FirstJumped -= OnPlayerFirstJumped;
        _player.PlayerKilled -= OnPlayerKilled;
        _player.StarCollected -= OnStarCollected;
        _player.ArrivedFinishLine -= OnArrivedFinishLine;
    }

    void OnStarCollected()
    {
        _starsLabel.text = $"Stars: {_player.CollectedStars}";
    }

    void OnArrivedFinishLine()
    {
        Utils.FadeCanvasGroup(this, _wonScreenCanvas, 1f, 0.3f);
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
