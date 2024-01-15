using UnityEngine;
using UnityEngine.UI;

public class ElementDicGrid : DicGrid<ElementData>
{
    ElementDicPanel panel;

    private void Start() => panel = GameObject.FindObjectOfType<ElementDicPanel>();
    
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
