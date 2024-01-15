using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;

public class ElementButton : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI desc;
    [SerializeField] TextMeshProUGUI count;
    [SerializeField] AudioClip _sound;

    public ElementData CurrentElement { private get; set; }

    public void SetButton(ElementData elementData)
    {
        CurrentElement = elementData;
        icon.sprite = elementData.BaseStats.elementSp;
        desc.text = elementData.BaseStats.description;
        count.text = DictionaryManager.Instance.GetDicValue(CurrentElement);
    }

    public void ElementButtonClick()
    {
        DictionaryManager.Instance.ElementAdd(CurrentElement);
        DictionaryManager.Instance.CheckSpawnElementGrid(CurrentElement);
        if (CurrentElement.ButtonEffects.Any())
        {
            foreach (var effect in CurrentElement.ButtonEffects)
            {
                effect.Apply();
            }
        }
        AudioManager.Instance.PlaySound(_sound, 1);
        GameManager.Instance.ChangeGameState(GameState.InPlay);//處理
    }
}
