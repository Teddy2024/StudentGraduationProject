using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    [SerializeField] private GameObject grid;
    [SerializeField] private GameObject nullBox;
    [SerializeField] private Image _handle;

    Dictionary<int, Sprite> bossPointDictionary = new Dictionary<int, Sprite>();
    List<GameObject> nullBoxList = new List<GameObject>();

    [Space(20)]
    [Header("所有Boss圖標"), SerializeField] private BossProgressIcon _bossProgressIcon;

    private void Start() 
    {
        _handle.sprite = GameManager.Instance._playerData?.HeadIcon;
    }

    public void GetAllBossTime(List<SpawnData> allSpawn)
    {
        var finalValue = 0;

        foreach (SpawnData spawnData in allSpawn)
        {
            foreach(EditSpawnEnemyEffect EditSpawnEnemyEffect in spawnData.EditSpawnEnemyEffects)
            {
                if(EditSpawnEnemyEffect is SpawnBoss boss)
                {
                    int time = spawnData.ApplyTime;
                    if(time < 0)break;
                    if(time > finalValue)finalValue = time;
                    if(bossPointDictionary.ContainsKey(time))break;
                    bossPointDictionary.Add(time, _bossProgressIcon.SleepBossIcon);
                }
            }
        }
        _slider.maxValue = finalValue;
        SetAllPoint(finalValue);
    }

    public void UpdateProgressSlider(float current)
    {
        _slider.value = current;
    }

    void SetAllPoint(int finalValue)
    {
        for(var i = 0; i < finalValue + 1; i++)
        {
            var aNullBox = Instantiate(nullBox, grid.transform);
            nullBoxList.Add(aNullBox);
        }

        foreach(var item in bossPointDictionary)
        {
            var uiSprite = nullBoxList[item.Key].GetComponent<Image>();
            uiSprite.sprite = (item.Value);
            
            Color newColor = uiSprite.color;
            newColor.a = 1;
            uiSprite.color = newColor;
        }
    }

    public void AwakePoint(int time)
    {
        nullBoxList[time].GetComponent<Image>().sprite = _bossProgressIcon.AwakeBossIcon;
    }
}
[Serializable] public struct BossProgressIcon
{
    public Sprite SleepBossIcon;
    public Sprite AwakeBossIcon;
}
