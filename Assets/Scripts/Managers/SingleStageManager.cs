using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleStageManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private CanvasUi _canvasUiController;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!_player.Alive)
            {
                SceneManager.LoadScene("SingleStage");
            }
        }
    }

    private void Start()
    {
        _player.PlayerKilled += OnPlayerKilled;
        _player.ArrivedFinishLine += OnArrivedFinishLine;
    }

    private void OnPlayerKilled()
    {
        _canvasUiController.FadeInGameOverScreen();
    }

    private void OnArrivedFinishLine()
    {
        _canvasUiController.FadeInFinishScreen();
    }
}
