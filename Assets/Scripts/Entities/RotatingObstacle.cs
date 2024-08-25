using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObstacle : MonoBehaviour
{
    [SerializeField] private float _rotatingVelocity;
    void FixedUpdate()
    {
        transform.Rotate(Vector3.forward, _rotatingVelocity);
    }
}
