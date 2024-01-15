using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    PlayerDefault _player;
    public PlayerDataBase _playerData { get; set; }
    public GameState State{ get; private set; }
    public static event Action<GameState> OnGameStateChange;
    public bool GameIsPaused { get; private set; }

    [SerializeField] private BGM _allBGM;
    public BGM AllBGM => _allBGM;

    [SerializeField] private AudioClip lose;
    [SerializeField] private AudioClip win;

    #region 生成所選玩家
    private void OnEnable() 
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable() 
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if(scene.name.Contains("Level"))
        {
          Instantiate(_playerData.prefab);
        }
    }
    #endregion

    private void Start() 
    {
        HandleMainMenu();
        ChangeGameState(GameState.MainMenu);
    }

    private void Update() 
    {
        Time.timeScale = GameIsPaused ? 0 : 1;
    }

    public void ChangeGameState(GameState newState)
    {
        if(State == newState)return;
        State = newState;
        switch (newState)
        {
            case GameState.MainMenu:
            HandleMainMenu();
            break;
            case GameState.InPlay:
            HandleInPlay();
            break;
            case GameState.OnUpgrade:
            HandleOnUpgrade();
            break;
            case GameState.ExitMenu:
            HandleExitMenu();
            break;
            case GameState.Win:
            HandleWin();
            break;
            case GameState.Lose:
            HandleLose();
            break;
        }
        OnGameStateChange?.Invoke(newState);
    }

    #region 各狀態處理
    private void HandleMainMenu()
    {
        GameIsPaused = false;
        _playerData = null;
        AudioManager.Instance.PlayMusic(AllBGM.MainMenuBGM);
    }
    private void HandleInPlay()
    {
        GameIsPaused = false;
        AudioManager.Instance.PlayMusic(AllBGM.InPlayBGM);
    }
    private void HandleOnUpgrade()
    {
        GameIsPaused = true;
        AudioManager.Instance.StopAllAudio();
    }
    private void HandleExitMenu()
    {
        GameIsPaused = true;
        AudioManager.Instance.StopAllAudio();
    }
    private void HandleWin()
    {
        GameIsPaused = true;
        AudioManager.Instance.StopAllAudio();
        AudioManager.Instance.PlaySound(win);
    }
    private void HandleLose()
    {
        GameIsPaused = true;
        AudioManager.Instance.StopAllAudio();
        AudioManager.Instance.PlaySound(lose);
    }
    #endregion

    public PlayerDefault GetPlayer()
    {
        if (_player == null)
        {
            _player = FindObjectOfType<PlayerDefault>();
            if (_player == null)
            {
                Debug.LogError("Unable to find Player");
            }
        }
        return _player;
    }
}

[Serializable] public enum GameState
{
    MainMenu,
    InPlay,
    OnUpgrade,
    ExitMenu,
    Win,
    Lose
}

[Serializable] public struct BGM
{
    public AudioClip MainMenuBGM;
    public AudioClip InPlayBGM;
}
