using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfiniteStageManager : MonoBehaviour
{
    [SerializeField] private CanvasUi _canvasUi;
    [SerializeField] private Player _player;
    [SerializeField] private GameObject _starPrefab;
    [SerializeField] private GameObject _powerUpPrefab;

    private List<GameObject> _obstacles = new List<GameObject>();
    private bool _nextPowerUp = false;
    
    private void Start()
    {
        var randomObstacle = Globals.ObstaclesData.GetRandomObstacle();
        var prevObstacle = InstantiateNextObstacle(randomObstacle);
        prevObstacle.transform.position += Vector3.up * Mathf.Abs(prevObstacle.GetComponent<ObstaclePivot>().StartPivot);
        InstantiateNextPowerUp();
        
        for (var i = 0; i < 3; i++)
        {
            prevObstacle = Globals.ObstaclesData.GetRandomObstacle();
            InstantiateNextObstacle(prevObstacle);
            InstantiateNextPowerUp();
        }

        _player.PlayerKilled += OnPlayerKilled;
    }

    private void FixedUpdate()
    {
        if (_player.transform.position.y > _obstacles[1].GetComponent<ObstaclePivot>().EndPivot)
        {
            var nextObstacle = Globals.ObstaclesData.GetRandomObstacle();
            InstantiateNextObstacle(nextObstacle);
            InstantiateNextPowerUp();
            Destroy(_obstacles[0]);
            _obstacles.RemoveAt(0);
        }
    }

    private void OnPlayerKilled()
    {
        _canvasUi.FadeInGameOverScreen();
    }

    private GameObject InstantiateNextObstacle(GameObject obstaclePrefab)
    {
        var newObstacle = Instantiate(obstaclePrefab);
        var newObstaclePivot = newObstacle.GetComponent<ObstaclePivot>();
        if (_obstacles.Count > 0)
        {
            var lastObstacle = _obstacles[_obstacles.Count - 1];
            var lastObstaclePivot = lastObstacle.GetComponent<ObstaclePivot>();

            var pivotDiff = lastObstaclePivot.EndPivot - newObstaclePivot.StartPivot;
            newObstacle.transform.position += pivotDiff * Vector3.up;
        }
        _obstacles.Add(newObstacle);
        
        return newObstacle;
    }

    private void InstantiateNextPowerUp()
    {
        var lastObstaclePivot = _obstacles[_obstacles.Count - 1].GetComponent<ObstaclePivot>();
        if (_nextPowerUp)
        {
            var powerUp = Instantiate(_powerUpPrefab);
            powerUp.transform.position = Vector3.up * lastObstaclePivot.EndPivot;
        }
        else
        {
            var star = Instantiate(_starPrefab);
            star.transform.position = Vector3.up * lastObstaclePivot.EndPivot;
        }

        _nextPowerUp = !_nextPowerUp;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!_player.Alive)
            {
                SceneManager.LoadScene("InfiniteRun");
            }
        }
    }
}
