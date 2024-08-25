using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _duration;

    private Vector3 _initialPosition;
    private Vector3 _targetPosition;
    private float _lerpWeight;
    
    private void Start()
    {
        _initialPosition = transform.position;
        _targetPosition = _initialPosition + (Vector3.right * _maxDistance);
    }

    private void FixedUpdate()
    {
        if (_lerpWeight < 1f)
        {
            _lerpWeight += Time.fixedDeltaTime / _duration;
            transform.position = Vector3.Lerp(_initialPosition, _targetPosition, _lerpWeight);
        }
        else
        {
            _lerpWeight = 0f;
            (_targetPosition, _initialPosition) = (_initialPosition, _targetPosition);
        }
    }
}
