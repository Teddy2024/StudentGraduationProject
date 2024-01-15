using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _header;
    [SerializeField] private TextMeshProUGUI _content;
    [SerializeField] private LayoutElement _layoutElement;
    [SerializeField] private int characterWrapLimit;
    [SerializeField] private RectTransform _rectTransform;

    private void Awake() 
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update() 
    {
        // if(Application.isEditor)//記得刪掉
        {
            int headerLength = _header.text.Length;
            int contentLength = _content.text.Length;

            _layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
        

            Vector2 position = Input.mousePosition;

            float pivotX = position.x / Screen.width;
            float pivotY = position.y / Screen.height;

            _rectTransform.pivot = new Vector2(pivotX, pivotY);

            transform.position = position;
        }
    }

    public void SetToolText(string content, string header = "")
    {
        if(string.IsNullOrEmpty(header))
        {
            _header.gameObject.SetActive(false);
        }
        else
        {
            _header.gameObject.SetActive(true);
            _header.text = header;
        }

        _content.text = content;

        int headerLength = _header.text.Length;
        int contentLength = _content.text.Length;

        _layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
    }
}
