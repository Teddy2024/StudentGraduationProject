using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : Singleton<MainMenuManager>
{
    [SerializeField] private GameObject playerSelect;
    [SerializeField] private GameObject _dictionaryBook;
    [SerializeField] private GameObject sceneSelect;
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject setting;
    [SerializeField] private GameObject scenePlayButton;
    [SerializeField] private AudioClip ButtonClip;
    [SerializeField] private AudioClip NoButtonClip;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleEscapeKey();
        }
    }

    public void HandleEscapeKey()
    {
        if (playerSelect.activeSelf)
        {
            DeactivateUI(playerSelect);
            ActivateUI(main);
        }
        else if (_dictionaryBook.activeSelf)
        {
            DeactivateUI(_dictionaryBook);
            ActivateUI(main);
        }
        else if (setting.activeSelf)
        {
            DeactivateUI(setting);
            ActivateUI(main);
        }
        else if (sceneSelect.activeSelf)
        {
            DeactivateUI(sceneSelect);
            ActivateUI(playerSelect);
            scenePlayButton.SetActive(false);
        }
        else
        {
            ExitGame();
            Debug.Log("Quit");
        }
    }

    private void ActivateUI(GameObject uiObject)
    {
        uiObject?.SetActive(true);
    }

    private void DeactivateUI(GameObject uiObject)
    {
        uiObject?.SetActive(false);
    }

    public void ButtonClick() => AudioManager.Instance.PlaySound(ButtonClip);
    public void NoButtonClick() => AudioManager.Instance.PlaySound(NoButtonClip);
    public void ExitGame() => Application.Quit();
}
