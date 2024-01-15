using UnityEngine;

public class TooltipManager : Singleton<TooltipManager>
{
    [SerializeField] private Tooltip _toolTip;

    public void Show(string content, string header = "")
    {
        _toolTip.SetToolText(content, header);
        _toolTip.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _toolTip.gameObject.SetActive(false);
    }
}
