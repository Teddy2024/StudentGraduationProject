using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossDicPanel : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] private TextMeshProUGUI bossName;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI speed;
    [SerializeField] private TextMeshProUGUI attack;
    [SerializeField] private TextMeshProUGUI description;

    public void SetDicPanel(EnemyDataBase enemyDataBase)
    {
        icon.sprite = enemyDataBase.BaseStats.enemySp;
        bossName.text = enemyDataBase.BaseStats.enemyName;
        health.text = enemyDataBase.BaseStats.enemyHealth.ToString();
        speed.text = enemyDataBase.BaseStats.enemySpeed.ToString();
        attack.text = enemyDataBase.BaseStats.enemyDamage.ToString();
        description.text = enemyDataBase.BaseStats.enemyDescription;
    }
}
