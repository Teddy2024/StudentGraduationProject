using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElementGrid : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI count;
    public ElementData CurrentCompound { private get; set; }

    public void SetButton(ElementData elementData)
    {
        CurrentCompound = elementData;
        icon.sprite = elementData.BaseStats.elementSp;
    }

    private string GetDicValue(ElementData elementData)
    {
        string finallValue;
        int value;

        if (DictionaryManager.Instance.CurrentElementDictionary.TryGetValue(elementData, out value))
        {
            finallValue = value.ToString();
        }
        else finallValue = "0";

        return finallValue;
    }
    
    void OnEnable() 
    {
        GameManager.OnGameStateChange += OnLevelUpOrPaused;
    }

    void OnDestroy() 
    {
        GameManager.OnGameStateChange -= OnLevelUpOrPaused;
    }

    private void OnLevelUpOrPaused(GameState state)
    {
        if(state == GameState.OnUpgrade || state == GameState.ExitMenu)
        {
            count.text = GetDicValue(CurrentCompound);
        }
        else
        {
            count.text = "";
        }
    }
}
