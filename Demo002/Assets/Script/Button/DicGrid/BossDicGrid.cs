using UnityEngine;
using UnityEngine.UI;

public class BossDicGrid : DicGrid<EnemyDataBase>
{
    BossDicPanel panel;

    private void Start() => panel = GameObject.FindObjectOfType<BossDicPanel>();

    public override void SetButton(EnemyDataBase DataBase)
    {
        CurrentData = DataBase;
        icon.sprite = DataBase.BaseStats.enemySp;
    }

    public override void DicGridButtonClick()
    {
        AudioManager.Instance.PlaySound(CurrentData.BaseStats.hitSound);
        panel.SetDicPanel(CurrentData);
    }
}
