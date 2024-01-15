using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
   [SerializeField] private Slider _slider;
   [SerializeField] private Gradient _gradient;
   [SerializeField] private Image _fill;

    public void MaxHealthSet(float health)
    {
        _slider.maxValue = health;
        _slider.value = health;

        _fill.color = _gradient.Evaluate(1f);
    }

    public void HealthSet(float health)
    {
        _slider.value = health;

        _fill.color = _gradient.Evaluate(_slider.normalizedValue);

    }
}