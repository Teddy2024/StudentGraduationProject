using UnityEngine;
using UnityEngine.UI;

public class CompoundDicGrid : DicGrid<ElementData>
{
    CompoundDicPanel panel;

    private void Start() => panel = GameObject.FindObjectOfType<CompoundDicPanel>();

    public override void SetButton(ElementData DataBase)
    {
        CurrentData = DataBase;
        icon.sprite = DataBase.BaseStats.elementSp;
    }

    public override void DicGridButtonClick()
    {
        MainMenuManager.Instance.ButtonClick();
        panel.SetDicPanel(CurrentData);
    }
}
