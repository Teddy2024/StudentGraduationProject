using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using System;

public class TimeManager : Singleton<TimeManager>
{
    [Header("生成行事曆"), SerializeField] private List<SpawnData> spawnDataList;
    Dictionary<int, SpawnData> spawnDataDictionary; // 使用字典存储时间点数据
    HashSet<int> appliedTimes;
    
    [SerializeField] private ProgressBar _progressBar;
    [SerializeField] private TextMeshProUGUI _timeText;

    float sceneStartTime;
    float elapsedTimeInMinutes;
    float currentTime;

    private void Start()
    {
        SetAllTimeData();
        _progressBar.GetAllBossTime(spawnDataList);
        _progressBar.UpdateProgressSlider(elapsedTimeInMinutes);

    }

    private void Update()
    {
        UpdateTimeData();
        UpdateTimeText();
        _progressBar.UpdateProgressSlider(elapsedTimeInMinutes);
    }

    void SetAllTimeData()
    {
        sceneStartTime = Time.time;
        appliedTimes = new HashSet<int>();

        // 将时间点数据转换为字典
        spawnDataDictionary = new Dictionary<int, SpawnData>();
        foreach (SpawnData spawnData in spawnDataList)
        {
            if (!spawnDataDictionary.ContainsKey(spawnData.ApplyTime))
            {
                spawnDataDictionary.Add(spawnData.ApplyTime, spawnData);
            }
        }
    }

    void UpdateTimeData()
    {
        float elapsedTimeInSeconds = Time.time - sceneStartTime;
        elapsedTimeInMinutes = elapsedTimeInSeconds / 60f;

        int elapsedTimeMinutesRounded = Mathf.RoundToInt(elapsedTimeInMinutes);

        if (!appliedTimes.Contains(elapsedTimeMinutesRounded))
        {
            if (spawnDataDictionary.TryGetValue(elapsedTimeMinutesRounded, out SpawnData spawnData))
            {
                SpawnManager.Instance.AddSpawnData(spawnData);
                appliedTimes.Add(elapsedTimeMinutesRounded);
            }
        }
    }

    void UpdateTimeText()
    {
        currentTime += Time.deltaTime;
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        _timeText.text = time.Hours.ToString("00") + ":" + time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneStartTime = Time.time;
        appliedTimes?.Clear();
    }

    [ContextMenu("CatchAllTimeEvent")]
    public void CatchAllTimeEvent()
    {
        //之後改
        spawnDataList = Resources.LoadAll<SpawnData>("ScriptableObjects/ForSpawnData/Level001").ToList();
    }
}