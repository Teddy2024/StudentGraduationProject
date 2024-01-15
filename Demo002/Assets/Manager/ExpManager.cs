using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public class ExpManager : Singleton<ExpManager>
{
    [SerializeField]  AudioClip LevelUpSound;
    [Header("升級所需經驗"), SerializeField, Range(0, 2000)] private int levelUpRamp = 500;
    [Header("升級條"), SerializeField] private ExperienceBar _experienceBar;
    [Space(20)][Header("元素升級按鈕"), SerializeField] private List<ElementButton> AllButtons;
    [Header("化合物按鈕"), SerializeField] private List<CompoundButton> ComButtons;

    List<ElementData> AllElementData;//升級元素選項
    [HideInInspector]public List<ElementData> ButtonCompoundData = new List<ElementData>();//升級化合物選項

    [Space(25)]public UnityEvent LevelUp;
    public int level{ get; private set; }
    int experience;
    int TO_LEVEL_UP { get => level * levelUpRamp + levelUpRamp; }
    public int LevelUpExp
    {
        get
        {
            return TO_LEVEL_UP - experience;
        }
    }

    void Start() 
    {
        _experienceBar.UpdateExperienceSlider(experience, TO_LEVEL_UP);
        _experienceBar.SetLevelText(level);
        AllElementData = ResourceManager.Instance.ElementDataLibrary;
    }

    #region GameManager溝通  
    void OnEnable() 
    {
        GameManager.OnGameStateChange += OnLevelUp;
    }

    void OnDestroy() 
    {
        GameManager.OnGameStateChange -= OnLevelUp;
    }

    private void OnLevelUp(GameState state)
    {
        if(state == GameState.OnUpgrade)
        {
            LevelUp?.Invoke();
        }
    }
    #endregion
    #region 經驗值管理
    public void AddExperience(int amount)
    {
        experience += amount;
        CheckLevelUp();
        _experienceBar.UpdateExperienceSlider(experience, TO_LEVEL_UP);
    }

    public void CheckLevelUp()
    {
        if(experience >= TO_LEVEL_UP)
        {
            experience -= TO_LEVEL_UP;
            level += 1;
            _experienceBar.SetLevelText(level);
            if(GameManager.Instance.State != GameState.Win || GameManager.Instance.State != GameState.Lose)
            {
                GameManager.Instance.ChangeGameState(GameState.OnUpgrade);
            }
        }
    }
    #endregion
    #region 升級按鈕
    private List<ElementData> GetButtonElement(int count)
    {   //取得隨機三個元素
        List<ElementData> randomElements = new List<ElementData>();
        for (int i = 0; i < count; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = UnityEngine.Random.Range(0, AllElementData.Count);
            } while (randomElements.Contains(AllElementData[randomIndex]));

            randomElements.Add(AllElementData[randomIndex]);
        }
        return randomElements;
    }

    public void SetAllButton()
    {   //設定元素按鈕
        for (int i = 0; i < AllButtons.Count; i++)
        {
            AllButtons[i].CurrentElement = null;
            AllButtons[i].gameObject.SetActive(false);
        }

        int getElementButtonNumber = 4 - ButtonCompoundData.Count;

        if(getElementButtonNumber > 0)
        {
            List<ElementData> randomElements = GetButtonElement(getElementButtonNumber);
            for (int i = 0; i < randomElements.Count; i++)
            {
                AllButtons[i].gameObject.SetActive(true);
                AllButtons[i].SetButton(randomElements[i]);
            }
        }
    }  
    public void SetCompoundButton()
    {   //設定化合物按鈕
        // ComButtons.CurrentCompound = null;
        // if (ButtonCompoundData != null && ButtonCompoundData.Count > 0)
        // {
        //     if (ButtonCompoundData.Count == 1)
        //     {
        //         ComButtons.SetButton(ButtonCompoundData[0]);
        //     }
        //     else if (ButtonCompoundData.Count >= 2)
        //     {
        //         ComButtons.SetButton(ButtonCompoundData[UnityEngine.Random.Range(0, ButtonCompoundData.Count)]);
        //     }
        //     ComButtons.gameObject.SetActive(true);
        // }
        // else
        // {
        //     ComButtons.gameObject.SetActive(false);
        // }
        for (int i = 0; i < ComButtons.Count; i++)
        {
            ComButtons[i].CurrentCompound = null;
            ComButtons[i].gameObject.SetActive(false);
        }
        
        if (ButtonCompoundData != null && ButtonCompoundData.Count > 0)
        {
            int getCompoundButtonNumber = Mathf.Min(ButtonCompoundData.Count, 4);
            for (int i = 0; i < getCompoundButtonNumber; i++)
            {
                ComButtons[i].gameObject.SetActive(true);
                ComButtons[i].SetButton(ButtonCompoundData[i]);
            }
        }
    }
    public void LevelUpSoundPlay()
    {
        AudioManager.Instance.PlaySound(LevelUpSound, 1);
    }
    #endregion
}
