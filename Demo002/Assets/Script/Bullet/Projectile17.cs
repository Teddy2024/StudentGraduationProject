using UnityEngine;

public class Projectile17 : Projectile
{
     protected override void Move()
    {
        if (!player) return;
        Vector3 initialScale = player.localScale;

        Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(40,-40));
        Vector2 movementDirection = rotation * new Vector2(0f, initialScale.y * -1).normalized;

        rb.velocity = movementDirection * _speed;
    }
    protected override void Attack(IDamageable enemy)
    {
        enemy.TakeDamage(_damage, _knockBackForce, false);
        _penetrate = true;

        rb.velocity = new Vector3(0,1,0) * _speed;
    }

    public override void ApplyAmountBuff(int level)
    {
        if(level >= 3)level = 3;
        _applyValueBuffLevel = level;
        switch (level)
        {
            case 1:
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, Random.Range(40,-40)));  
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
}
