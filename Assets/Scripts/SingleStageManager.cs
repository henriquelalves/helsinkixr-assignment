using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleStageManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private CanvasUi _canvasUi;

    private void Start()
    {
        
    }

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
}
