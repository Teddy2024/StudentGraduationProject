using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopDamage : MonoBehaviour
{
    [SerializeField] private TextMeshPro _damageText;

    public void DestPop()
    {
        EnemyAIManager.Instance.ReleasePop(this);
    }
    
    public void SetPopupDamage(float damage, Color color)
    {
        _damageText.text = damage.ToString();
        _damageText.color = color;
    }
}

