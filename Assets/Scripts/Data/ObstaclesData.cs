using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ObstaclesData", order = 1)]
public class ObstaclesData: ScriptableObject
{
    [SerializeField] private List<GameObject> _obstaclesPrefabs;

    public GameObject GetRandomObstacle()
    {
        var randomIndex = Random.Range(0, _obstaclesPrefabs.Count);
        return _obstaclesPrefabs[randomIndex];
    }
}
