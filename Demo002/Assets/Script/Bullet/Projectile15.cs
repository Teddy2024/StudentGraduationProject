using UnityEngine;

public class Projectile15 : Projectile
{
    protected override void Move()
    {
        if (!player) return;
        Vector3 initialScale = player.localScale;

        // 获取生成点的旋转角度
        float spawnRotation = transform.rotation.eulerAngles.z;

        // 将生成点的旋转角度应用于移动方向向量
        Quaternion rotation = Quaternion.Euler(0f, 0f, spawnRotation);
        Vector2 movementDirection = rotation * new Vector2(0f, initialScale.y * 1).normalized;

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

                float angleOffset = Random.Range(-30f, 30f);
                Quaternion rotation = Quaternion.Euler(0f, 0f, angleOffset);
                reflectDirection = rotation * reflectDirection;

                rb.velocity = reflectDirection * _speed;
            }
        }
    }

    public override void ApplyAmountBuff(int level)
    {
        if(level >= 3)level = 3;
        _applyValueBuffLevel = level;
        switch (level)
        {
            case 1:
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 140f));  
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 180f));  
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 90f));  
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 270f));  
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 220f));  
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 40f));  
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 320f));  
                break;
            case 2:
                break;
            case 3:
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 140f));  
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 180f));  
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 90f));  
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 270f));  
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 220f));  
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 40f));  
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 320f));  
                break;
        }
    }
}
