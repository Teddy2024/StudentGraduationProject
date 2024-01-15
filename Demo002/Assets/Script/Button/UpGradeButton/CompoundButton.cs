using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;

public class CompoundButton : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI desc;
    [SerializeField] SpawnButton spawnButton;
    [SerializeField] AudioClip _sound;

    public ElementData CurrentCompound { private get; set; }

    public void SetButton(ElementData elementData)
    {
        CurrentCompound = elementData;
        icon.sprite = elementData.BaseStats.elementSp;
        desc.text = elementData.BaseStats.description;
    }

    public void CompoundButtonClick()
    {
        Debug.Log(CurrentCompound.BaseStats.elementName);
        DictionaryManager.Instance.ElementAdd(CurrentCompound);
        spawnButton.SpawnCompoundGrid(CurrentCompound);
        ExpManager.Instance.ButtonCompoundData.Remove(CurrentCompound);
        if (CurrentCompound.ButtonEffects.Any())
        {
            foreach (var effect in CurrentCompound.ButtonEffects)
            {
                effect.Apply();
            }
        }
        AudioManager.Instance.PlaySound(_sound, 1);
        GameManager.Instance.ChangeGameState(GameState.InPlay);//處理
    }
}
