using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public event Action PlayerKilled;
    public event Action FirstJumped;
    public event Action StarCollected;
    public event Action ArrivedFinishLine;

    public int CollectedStars => _collectedStars;
    public bool Alive => _alive;
    
    [SerializeField] private GameObject _playerDeadParticles;
    [SerializeField] private float _gravity;
    [SerializeField] private float _jumpForce;

    private SpriteRenderer _spriteRenderer;
    private ColorEntity _colorEntity;
    private bool _firstJump;
    private float _velocity;
    private bool _alive;
    private int _collectedStars;
    
    void Start()
    {
        _alive = true;
        _colorEntity = GetComponent<ColorEntity>();
        _colorEntity.SetColorType(Globals.ColorsData.GetRandomColorType());
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_alive)
            {
                _velocity = _jumpForce;
                if (!_firstJump)
                {
                    FirstJumped?.Invoke();
                    _firstJump = true;
                }   
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_alive)
        {
            return;
        }
        if (other.CompareTag("DeathCollider"))
        {
            PlayerKilled?.Invoke();
            Kill();
        }
        else if (other.CompareTag("StarCollider"))
        {
            other.GetComponent<Star>().Kill();
            _collectedStars += 1;
            StarCollected?.Invoke();
        }
        else if (other.CompareTag("PowerUpCollider"))
        {
            Destroy(other.gameObject);
            _colorEntity.SetColorType(Globals.ColorsData.GetRandomColorType(_colorEntity.CurrentColorType));
        }
        else if (other.CompareTag("FinishLineCollider"))
        {
            ArrivedFinishLine?.Invoke();
            Kill();
        }
        else
        {
            var otherColorEntity = other.GetComponent<ColorEntity>();
            if (otherColorEntity.CurrentColorType != _colorEntity.CurrentColorType)
            {
                PlayerKilled?.Invoke();
                Kill();
            }
        }
    }

    private void Kill()
    {
        _alive = false;
        var deadParticles = Instantiate(_playerDeadParticles);
        deadParticles.transform.position = transform.position;
        _spriteRenderer.enabled = false;
    }
    
    void FixedUpdate()
    {
        if (!_alive || !_firstJump)
        {
            return;
        }
        
        _velocity += _gravity;
        transform.position += Vector3.up * _velocity;
    }
}
