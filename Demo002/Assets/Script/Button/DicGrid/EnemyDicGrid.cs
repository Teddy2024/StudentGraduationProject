using UnityEngine;
using UnityEngine.UI;

public class EnemyDicGrid : DicGrid<EnemyDataBase>
{
    EnemyDicPanel panel;

    private void Start() => panel = GameObject.FindObjectOfType<EnemyDicPanel>();

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
