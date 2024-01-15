using UnityEngine;

public class Projectile20 : Projectile
{
    protected override void Move()
    {
        if (!player) return;
        Vector3 initialScale = player.localScale;

        // 获取生成点的旋转角度
        float spawnRotation = transform.rotation.eulerAngles.z;

        // 将生成点的旋转角度应用于移动方向向量
        Vector2 movementDirection = GameManager.Instance.GetPlayer().movement.normalized;
        if(movementDirection == Vector2.zero)movementDirection = new Vector2(-1,0);
        movementDirection = Quaternion.Euler(0f, 0f, spawnRotation) * movementDirection;

        rb.velocity = movementDirection * _speed; 
    }

   private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.TryGetComponent(out IDamageable enemy))
        {
            enemy.TakeDamage(_damage, _knockBackForce, false);
            if (!_penetrate)
            {
                Vector2 reflectDirection = Vector2.Reflect(rb.velocity.normalized, other.contacts[0].normal);

                float angleOffset = Random.Range(100f, -100f);
                Quaternion rotation = Quaternion.Euler(0f, 0f, angleOffset);
                reflectDirection = rotation * reflectDirection;

                rb.velocity = reflectDirection * _speed;
                _damage *= 2;
            }
        }
    }
    public override void ApplyAmountBuff(int level)
    {
        level = Mathf.Clamp(level, 1, 3);
        _applyValueBuffLevel = level;
        switch (level)
        {
            case 1:
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 180f));
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 90f));
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 270f));
                break;
            case 2:  
                break;
            case 3:
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 180f));
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 90f));
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 270f));
                break;
        }
    }
}
