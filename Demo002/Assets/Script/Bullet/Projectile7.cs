using UnityEngine;

public class Projectile7 : Projectile
{
    protected override void Move()
    {
        if (!player) return;
        Vector3 initialScale = player.localScale;

        // 获取生成点的旋转角度
        float spawnRotation = transform.rotation.eulerAngles.z;

        // 将生成点的旋转角度应用于移动方向向量
        Quaternion rotation = Quaternion.Euler(0f, 0f, spawnRotation);
        Vector2 movementDirection = rotation * new Vector2(initialScale.x * 1, initialScale.y * 1).normalized;

        rb.velocity = movementDirection * _speed;
    }

   public override void ApplyAmountBuff(int level)
    {
        if(level >= 3)level = 3;
        _applyValueBuffLevel = level;
        switch (level)
        {
            case 1:
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 90f));
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 180f));
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 270f));
                break;
            case 2:  
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 90f));
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 180f));
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 270f));
                break;
            case 3:
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 90f));
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 180f));
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 270f));
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
                transform.localScale += new Vector3(0.5f,0.5f,0);
                _damage *= 1.5f;
                _knockBackForce *= 1.2f;
                break;
            case 3:
                transform.localScale += new Vector3(1.5f,1.5f,0);
                _penetrate = true;
                _knockBackForce *= 1.2f;
                _damage *= 2;
                break;
        }
    }
}
