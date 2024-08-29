using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private AudioSource[] _audioSources;

    [Header("Sound Clips")] 
    [SerializeField] private AudioClip _powerUpAudio;
    [SerializeField] private AudioClip _starAudio;
    [SerializeField] private AudioClip _jumpAudio;
    [SerializeField] private AudioClip _gameOverAudio;

    private int _nextAudioSourceIdx;
    
    private void Start()
    {
        _player.PlayerKilled += () => { PlayAudio(_gameOverAudio); };
        _player.Jumped += () => { PlayAudio(_jumpAudio); };
        _player.StarCollected += () => { PlayAudio(_starAudio); };
        _player.PowerUpCollected += () => { PlayAudio(_powerUpAudio); };
    }

    private void PlayAudio(AudioClip audioClip)
    {
        _audioSources[_nextAudioSourceIdx].PlayOneShot(audioClip);
        _nextAudioSourceIdx = (_nextAudioSourceIdx + 1) % _audioSources.Length;
    }
}
