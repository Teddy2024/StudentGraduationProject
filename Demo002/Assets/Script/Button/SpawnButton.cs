using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SpawnButton : MonoBehaviour
{
    [SerializeField] private ButtonType _buttonType;

    [Header("選角色用的")]
    [SerializeField] private GameObject _playerButtonPrefab;
    [SerializeField] private GameObject _playerPlayButton;

    [Header("選場景用的")]
    [SerializeField] private GameObject _sceneButtonPrefab;
    [SerializeField] private GameObject _scenePlayButton;

    [Header("化合物顯示")]
    [SerializeField] private GameObject _compoundGridPrefab;

    [Header("元素顯示")]
    [SerializeField] private GameObject _elementGridPrefab;


    private void Start() 
    {
        if(_buttonType == ButtonType.PlayerSelect)
        {
            SpawnPlayerButton();
        }
        else if(_buttonType == ButtonType.SceneSelect)
        {
            SpawnSceneButton();
        }
    }

    private void SpawnPlayerButton()
    {
        List<PlayerDataBase> AllPlayerData = ResourceManager.Instance.PlayerDataLibrary;
        for (int i = 0; i < AllPlayerData.Count; i++)
        {
            GameObject newButton = Instantiate(_playerButtonPrefab, this.transform);
            newButton.GetComponent<PlayerButton>().SetButton(AllPlayerData[i], _playerPlayButton);
        }
    }

    private void SpawnSceneButton()
    {
        List<LevelData> AllLevelData = ResourceManager.Instance.LevelDataLibrary;
        for (int i = 0; i < AllLevelData.Count; i++)
        {
            GameObject newButton = Instantiate(_sceneButtonPrefab, this.transform);
            newButton.GetComponent<LevelButton>().SetButton(AllLevelData[i], _scenePlayButton);
        }
    }

    public void SpawnCompoundGrid(ElementData elementData)
    {
        GameObject newButton = Instantiate(_compoundGridPrefab, this.transform);
        newButton.GetComponent<CompoundGrid>().SetButton(elementData);
    }

    public void SpawnElementGrid(ElementData elementData)
    {
        GameObject newButton = Instantiate(_elementGridPrefab, this.transform);
        newButton.GetComponent<ElementGrid>().SetButton(elementData);
    }
}
[Serializable] public enum ButtonType
{
    PlayerSelect = 0,
    SceneSelect = 1,
    CompoundGrid = 2,
};