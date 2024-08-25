using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField] private Player _player;
    private float _targetHeight;
    
    private void FixedUpdate()
    {
        _targetHeight = Mathf.Max(_targetHeight, _player.transform.position.y);

        var height = Mathf.Lerp(transform.position.y, _targetHeight, 0.1f);
        transform.position = new Vector3(0f, height, -10f);
    }
}
