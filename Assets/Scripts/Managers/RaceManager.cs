using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

#if !UNITY_WEBGL
using System.Net.Http;
#endif

public class RaceManager : MonoBehaviour
{
    private const int RecordFrameSkip = 5;
    private float CurrentTimestamp => Time.realtimeSinceStartup - _startingTimestamp;
    
    [Serializable]
    struct Frame
    {
        public float PlayerPosition;
        public bool PlayerKilled;
        public float Timestamp;
    }

    [Serializable]
    struct RaceData
    {
        public string PlayerName;
        public List<Frame> Frames;
    }

    [SerializeField] private bool _recordFrames;
    [SerializeField] private bool _loadRace;
    [SerializeField] private CanvasUi _canvasUiController;
    [SerializeField] private Player _player;
    [SerializeField] private GameObject _playerDeathParticles;
    [SerializeField] private GameObject _opponent;
    [SerializeField] private GameCamera _gameCamera;
    [SerializeField] private string _playerName;

    private RaceData _opponentRaceData;
    private bool _raceRunning;
    private float _startingTimestamp;
    private int _opponentFrameIdx;
    private int _fixedFrameCount;
    private List<Frame> _recordedFrames = new List<Frame>();

    private void OnFirstJumped()
    {
        if (_raceRunning)
        {
            return;
        }
        
        _raceRunning = true;
        _startingTimestamp = Time.realtimeSinceStartup;

        RecordFrame();
    }

    private void OnPlayerKilled()
    {
        RecordFrame(true);
        _player.Respawn();
        _gameCamera.Reset();
    }

    private void OnPlayerArrivedFinishLine()
    {
        RecordFrame();
        SendRecordData();
        _raceRunning = false;
        _canvasUiController.FadeInFinishScreen();
        if (CurrentTimestamp < _opponentRaceData.Frames[^1].Timestamp)
        {
            _canvasUiController.SetWonText("You won first place!");
        }
        else
        {
            _canvasUiController.SetWonText("You won second place!");
        }
    }

    private IEnumerator GetRandomReplay()
    {
        const string backendUri = "https://xrhelsinki-backend.henriquelalves.com/get_random_replay/";
        using UnityWebRequest webRequest = UnityWebRequest.Get(backendUri);
        yield return webRequest.SendWebRequest();
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            _opponentRaceData = JsonUtility.FromJson<RaceData>(webRequest.downloadHandler.text);
        }
    }
    
    private void SendRecordData()
    {
        #if !UNITY_WEBGL
        if (!_recordFrames)
        {
            return;
        }
        async void SendData()
        {
            var client = new HttpClient();
            const string backendUri = "https://xrhelsinki-backend.henriquelalves.com/save_replay/";

            var raceData = new RaceData();
            raceData.Frames = _recordedFrames;
            raceData.PlayerName = _playerName;
        
            var json = JsonUtility.ToJson(raceData);
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(json), "replay");
            await client.PostAsync(backendUri, content);
        }
        
        SendData();
        #endif
    }
    
    private void RecordFrame(bool killed = false)
    {
        if (!_recordFrames)
        {
            return;
        }
        var newFrame = new Frame();
        newFrame.Timestamp = Time.realtimeSinceStartup - _startingTimestamp;
        newFrame.PlayerPosition = _player.transform.position.y;
        newFrame.PlayerKilled = killed;
        _recordedFrames.Add(newFrame);
    }

    IEnumerator Start()
    {
        _player.FirstJumped += OnFirstJumped;
        _player.PlayerKilled += OnPlayerKilled;
        _player.ArrivedFinishLine += OnPlayerArrivedFinishLine;
        
        if (_loadRace)
        {
            _canvasUiController.SetLoadingScreen();

            yield return GetRandomReplay();
            _canvasUiController.SetStartText($"You are competing against {_opponentRaceData.PlayerName}");
        }
        else
        {
            _opponentRaceData = new RaceData();
            _opponentRaceData.Frames = new List<Frame>();
            _opponentRaceData.Frames.Add(new Frame());
        }
        _canvasUiController.SetStartScreen();

    }

    private void FixedUpdate()
    {
        if (_raceRunning)
        {
            _fixedFrameCount += 1;
        }
        
        if (_raceRunning && _fixedFrameCount % RecordFrameSkip == 0)
        {
            RecordFrame();
        }

        if (_raceRunning && _opponentRaceData.Frames != null)
        {
            while (_opponentFrameIdx < _opponentRaceData.Frames.Count - 1 &&
                   _opponentRaceData.Frames[_opponentFrameIdx + 1].Timestamp < CurrentTimestamp)
            {
                _opponentFrameIdx++;
                if (_opponentRaceData.Frames[_opponentFrameIdx].PlayerKilled)
                {
                    var particles = Instantiate(_playerDeathParticles);
                    particles.transform.position = _opponent.transform.position;
                    _opponent.transform.position = Vector3.zero;
                }
            }

            if (_opponentFrameIdx < _opponentRaceData.Frames.Count - 1)
            {
                var currentFrame = _opponentRaceData.Frames[_opponentFrameIdx];
                var nextFrame = _opponentRaceData.Frames[_opponentFrameIdx + 1];

                var timestampLerp = Mathf.InverseLerp(currentFrame.Timestamp, nextFrame.Timestamp, CurrentTimestamp);

                _opponent.transform.position = Vector3.up * Mathf.Lerp(
                    currentFrame.PlayerPosition,
                    nextFrame.PlayerPosition,
                    timestampLerp
                );
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!_player.Alive && !_raceRunning)
            {
                SceneManager.LoadScene("Race");
            }
        }
    }
}
