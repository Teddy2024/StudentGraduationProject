using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgScrolling : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    Vector2Int currentTitlePosition;
    [SerializeField] Vector2Int playerTitlePosition;
    [SerializeField] float pixelSize = 8f;
    GameObject[, ] terrarinTitles;

    [SerializeField] int terrarinTitleHorizontal;
    [SerializeField] int terrarinTitleVertical;

    private void Awake() 
    {
        terrarinTitles = new GameObject[terrarinTitleHorizontal, terrarinTitleVertical];
    }

    private void Update() 
    {
        playerTitlePosition.x = (int)(playerTransform.position.x / pixelSize);
        playerTitlePosition.y = (int)(playerTransform.position.y / pixelSize);
    }

    public void Add(GameObject titleGameObject, Vector2Int titlePosition)
    {
        terrarinTitles[titlePosition.x, titlePosition.y] = titleGameObject;
    }
}
