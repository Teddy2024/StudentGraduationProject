using UnityEngine;

public class Projectile8 : Projectile
{
    public float bounceHeight = 1f;
    public float bounceSpeed = 1f;
    public float minAlpha = 0.5f;
    public float maxAlpha = 0.2f;

    Transform startPos;
    Vector3 initialPosition;
    SpriteRenderer objectRenderer;
    float timer;

    private void OnEnable() 
    {
        startPos = transform;
        objectRenderer = GetComponent<SpriteRenderer>();
        initialPosition = startPos.position;
        timer = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        float y = initialPosition.y + Mathf.Sin(timer * bounceSpeed) * bounceHeight;
        Vector3 newPosition = new Vector3(initialPosition.x, y, initialPosition.z);
        transform.position = newPosition;
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, (y - initialPosition.y + bounceHeight) / (2 * bounceHeight));
        Color color = objectRenderer.color;
        color.a = alpha;
        objectRenderer.color = color;
    }

    public override void ApplyAmountBuff(int level)
    {
        level = Mathf.Clamp(level, 1, 3);
        _applyValueBuffLevel = level;
        switch (level)
        {
            case 1:
                break;
            case 2:  
                break;
            case 3:
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position + new Vector3(1,0,0), Quaternion.Euler(0f, 0f, Random.Range(0,90)));
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position + new Vector3(-1,0,0), Quaternion.Euler(0f, 0f, Random.Range(0,90)));
                break;
        }
    }
}
