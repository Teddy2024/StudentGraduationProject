using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class CompoundGrid : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image icon;
    string touchHead;
    string touchText;
    public ElementData CurrentCompound { private get; set; }

    public void SetButton(ElementData elementData)
    {
        CurrentCompound = elementData;
        icon.sprite = elementData.BaseStats.elementSp;
        touchHead = elementData.BaseStats.elementName;
        touchText = elementData.BaseStats.description;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       TooltipManager.Instance.Show(touchText, touchHead);
       Cursor.visible = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       TooltipManager.Instance.Hide();
       Cursor.visible = true;
    }
}
