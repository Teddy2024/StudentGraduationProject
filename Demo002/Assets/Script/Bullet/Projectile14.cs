using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile14 : Projectile
{
    protected override void Move()
    {
        if (!player) return;
        Vector3 initialScale = player.localScale;

        // 获取生成点的旋转角度
        float spawnRotation = transform.rotation.eulerAngles.z;

        // 将生成点的旋转角度应用于移动方向向量
        Quaternion rotation = Quaternion.Euler(0f, 0f, spawnRotation);
        Vector2 movementDirection = rotation * new Vector2(initialScale.x * -1, 0f).normalized;

        rb.velocity = movementDirection * _speed;
    }
    protected override void Attack(IDamageable enemy)
    {
       
        enemy.TakeDamage(_damage, _knockBackForce, false);
        _damage -= 1;
        transform.localScale -= new Vector3(0.1f,0.1f,0);
        if(!_penetrate || _damage <= 0 || transform.localScale.x <= 0)ProjectileEnd();
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
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position + new Vector3(0,0.5f,0), Quaternion.Euler(0f, 0f, 10f));
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position + new Vector3(0,-0.5f,0), Quaternion.Euler(0f, 0f, -10f));
                break;
        }
    }
}
