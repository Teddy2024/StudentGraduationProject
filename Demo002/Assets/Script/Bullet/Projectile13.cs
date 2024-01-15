using UnityEngine;

public class Projectile13 : Projectile
{
    [SerializeField] private float _angle;

    protected override void Move()
    {
        if (!player) return;
        Vector3 initialScale = player.localScale;

        // 获取生成点的旋转角度
        float spawnRotation = transform.rotation.eulerAngles.z;

        // 将生成点的旋转角度应用于移动方向向量
        Vector2 movementDirection = GameManager.Instance.GetPlayer().movement.normalized;
        if(movementDirection == Vector2.zero)movementDirection = new Vector2(-1,0);
        movementDirection = Quaternion.Euler(0f, 0f, spawnRotation + Random.Range(-_angle, _angle)) * movementDirection;

        rb.velocity = movementDirection * _speed;
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
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 180f));
                break;
        }
    }
}
