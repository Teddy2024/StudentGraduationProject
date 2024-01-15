using UnityEngine;

public class Projectile12 : Projectile
{
    [Header("旋轉角度"), SerializeField] float _angle = 0f;//旋轉角度
    [Header("距離幅度"), SerializeField] float radiusFloat = 3f;//離玩家距離
    private SpriteRenderer spriteRenderer;
    private Transform playerTransform;

    protected override void Move()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = GameManager.Instance.GetPlayer().transform;
    }

    private void Update() 
    {
        if (!playerTransform) return;
        _angle += Time.deltaTime * _speed;
        float x = Mathf.Cos(_angle) * radiusFloat;
        float y = Mathf.Sin(_angle) * radiusFloat;
        Vector3 offset = new Vector3(x, y, 0f);
        Vector3 pos = playerTransform.position + offset;

        transform.position = pos;

        Vector3 lookAtPosition = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);
        transform.up = (playerTransform.position - lookAtPosition).normalized;
    }

    public override void ApplyAmountBuff(int level)
    {
        if(level >= 3)level = 3;
        _applyValueBuffLevel = level;
        switch (level)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
    public override void ApplyValueBuff(int level)
    {
        switch (level)
        {
            case 1:
                break;
            case 2:
                transform.localScale = new Vector3(0.7f,0.7f,0);
                _knockBackForce *= 1.5f;
                _damage *= 2f;
                break;
            case 3:
                _knockBackForce *= 2f;
                _damage *= 2f;
                _penetrate = true;
                break;
        }
    }

}
