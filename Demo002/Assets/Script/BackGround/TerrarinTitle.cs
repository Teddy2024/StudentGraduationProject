using UnityEngine;

public class TerrarinTitle : MonoBehaviour
{
    [SerializeField] Vector2Int titlePosition;

    private void Start() 
    {
        GetComponentInParent<BgScrolling>().Add(gameObject, titlePosition);
    }
}
