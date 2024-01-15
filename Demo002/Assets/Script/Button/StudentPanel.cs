using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StudentPanel : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] private TextMeshProUGUI studentName;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI speed;
    [SerializeField] private TextMeshProUGUI description;

    public void SetStudentPanel(PlayerDataBase playerDataBase)
    {
        icon.sprite = playerDataBase.BaseStats.playerSp;
        studentName.text = playerDataBase.BaseStats.playerName;
        health.text = playerDataBase.BaseStats.maxHealth.ToString();
        speed.text = playerDataBase.BaseStats.speed.ToString();
        description.text = playerDataBase.BaseStats.playerDescription;
    }
}
