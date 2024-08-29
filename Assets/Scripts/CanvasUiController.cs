using UnityEngine;
using UnityEngine.UI;

public class CanvasUi : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private CanvasGroup _initialScreenCanvas;
    [SerializeField] private CanvasGroup _gameOverScreenCanvas;
    [SerializeField] private CanvasGroup _wonScreenCanvas;
    [SerializeField] private CanvasGroup _loadingScreenCanvas;
    [SerializeField] private Text _starsLabel;
    [SerializeField] private Text _wonLabel;
    [SerializeField] private Text _startLabel;
    
    void Start()
    {
        _player.FirstJumped += FadeOutStartingScree;
        _player.StarCollected += OnStarCollected;
    }

    void OnStarCollected()
    {
        _starsLabel.text = $"Stars: {_player.CollectedStars}";
    }

    public void SetLoadingScreen()
    {
        _loadingScreenCanvas.alpha = 1f;
        _initialScreenCanvas.alpha = 0f;
    }
    
    public void SetStartScreen()
    {
        _loadingScreenCanvas.alpha = 0f;
        _initialScreenCanvas.alpha = 1f;
    }

    public void SetStartText(string text)
    {
        _startLabel.text = text;
    }
    
    public void SetWonText(string text)
    {
        _wonLabel.text = text;
    }
    
    public void FadeInFinishScreen()
    {
        Utils.FadeCanvasGroup(this, _wonScreenCanvas, 1f, 0.3f);
    }
    
    public void FadeInGameOverScreen()
    {
        Utils.FadeCanvasGroup(this, _gameOverScreenCanvas, 1f, 0.3f);
    }
    
    void FadeOutStartingScree()
    {
        Utils.FadeCanvasGroup(this, _initialScreenCanvas, 0f, 0.3f);
    }
}
