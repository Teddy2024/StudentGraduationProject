using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    float maxScale = 20.0f;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.TryGetComponent(out ItemDefault item))
        {
            item.OnPickUp();
        }
    }

    public void ChangePickUpRange() 
    {
        transform.localScale *= 1.3f;
        Vector3 newScale = transform.localScale;
        newScale.x = Mathf.Min(newScale.x, maxScale);
        newScale.y = Mathf.Min(newScale.y, maxScale);
        transform.localScale = newScale;
    }

}
