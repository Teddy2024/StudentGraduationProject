using UnityEngine;
using UnityEngine.Events;
using System;

public class UiManager : Singleton<UiManager>
{
    [SerializeField] private GameObject exitMenu;
    [SerializeField] private AudioClip ButtonClip;
    [SerializeField] private AudioClip NoButtonClip;
    
    public GameObject deadPart;
    public UnityEvent Winning;
    public UnityEvent Loseing;

    private void Update() 
    {
        OnExit();
    }

    private void OnExit()
    {
        if(Input.GetKeyDown("escape"))
        {
            if(GameManager.Instance.State == GameState.InPlay)
            {
                GameManager.Instance.ChangeGameState(GameState.ExitMenu);
                exitMenu.SetActive(true);
            }
            else if(GameManager.Instance.State == GameState.ExitMenu)
            {
                ResumeGame();
            }
        }
    }
    public void ResumeGame()
    {
        GameManager.Instance.ChangeGameState(GameState.InPlay);
        exitMenu.SetActive(false);
    }
    public void ExitGame()
    {
        GameManager.Instance.ChangeGameState(GameState.MainMenu);
        LevelManager.Instance.BackToMainMenu();
    }

    void OnEnable() 
    {
        GameManager.OnGameStateChange += OnWin;
        GameManager.OnGameStateChange += OnLose;
    }

    void OnDestroy() 
    {
        GameManager.OnGameStateChange -= OnWin;
        GameManager.OnGameStateChange -= OnLose;
    }

    private void OnWin(GameState state)
    {
        if(state == GameState.Win)
        {
            Winning?.Invoke();
        }
    }
    private void OnLose(GameState state)
    {
        if(state == GameState.Lose)
        {
            Loseing?.Invoke();
        }
    }

    public void ButtonClick() => AudioManager.Instance.PlaySound(ButtonClip);
    public void NoButtonClick() => AudioManager.Instance.PlaySound(NoButtonClip);
}
