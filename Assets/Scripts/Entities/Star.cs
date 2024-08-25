using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] private GameObject _starParticles;
    
    public void Kill()
    {
        var particles = Instantiate(_starParticles);
        particles.transform.position = transform.position;
        Destroy(this.gameObject);
    }
}
