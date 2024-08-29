using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePivot : MonoBehaviour
{
    public float StartPivot => _startPivot.position.y;
    public float EndPivot => _endPivot.position.y;
    
    [SerializeField] private Transform _startPivot;
    [SerializeField] private Transform _endPivot;
}
