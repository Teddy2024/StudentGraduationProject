using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CompoundDicPanel : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] private TextMeshProUGUI compoundName;
    [SerializeField] private TextMeshProUGUI description;

    public void SetDicPanel(ElementData elementDataBase)
    {
        icon.sprite = elementDataBase.BaseStats.elementSp;
        compoundName.text = elementDataBase.BaseStats.elementName;
        description.text = elementDataBase.BaseStats.dicDescription;
    }
}
