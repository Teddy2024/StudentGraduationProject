using UnityEngine;

public class Projectile10 : Projectile
{
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.TryGetComponent(out IDamageable enemy))
        {
            enemy.TakeDamage(_damage, _knockBackForce, false);
            if (!_penetrate)
            {
                Vector2 reflectDirection = Vector2.Reflect(rb.velocity.normalized, other.contacts[0].normal);

                float angleOffset = Random.Range(-15f, 15f);
                Quaternion rotation = Quaternion.Euler(0f, 0f, angleOffset);
                reflectDirection = rotation * reflectDirection;

                rb.velocity = reflectDirection * _speed;
            }
        }
    }
    protected override void Attack(IDamageable enemy){}
   
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
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 180f));
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
                _knockBackForce *= 1.5f;
                _damage *= 1.5f;
                break;
            case 3:
                _knockBackForce *= 1.5f;
                _damage *= 1.5f;
                _stayTime *= 1.5f;
                break;
        }
    }

}