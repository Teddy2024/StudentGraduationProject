using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElementDicPanel : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] private TextMeshProUGUI elementName;
    [SerializeField] private TextMeshProUGUI description;

    public void SetDicPanel(ElementData elementDataBase)
    {
        icon.sprite = elementDataBase.BaseStats.elementSp;
        this.elementName.text = elementDataBase.BaseStats.elementName;
        description.text = elementDataBase.BaseStats.dicDescription;
    }
}
