using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _levelText;
    
    public void UpdateExperienceSlider(int current, int target)
    {
        _slider.maxValue = target;
        _slider.value = current;
    }
    public void SetLevelText(int level)
    {
        _levelText.text = "Level: " + level.ToString();
    }
}