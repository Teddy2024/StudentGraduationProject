using UnityEngine;

public class Projectile22 : Projectile
{
    [Header("震幅"), SerializeField] float amplitude = 15f;
    [Header("頻率"), SerializeField] float frequency = 0.7f;
    [Header("鏡像"), SerializeField] bool inverted = false;

    protected override void Move()
    {
        if (!player) return;
        Vector3 initialScale = player.localScale;

        // 获取生成点的旋转角度
        float spawnRotation = transform.rotation.eulerAngles.z;

        // 将生成点的旋转角度应用于移动方向向量
        Quaternion rotation = Quaternion.Euler(0f, 0f, spawnRotation);
        Vector2 movementDirection = rotation * new Vector2(0, initialScale.y * 1).normalized;

        rb.velocity = movementDirection * _speed;
    }

    private void Update() 
    {
        Vector2 pos = transform.position;
        float sin = Mathf.Sin(pos.y * frequency) * amplitude *Time.deltaTime;
        if(inverted)
        {
            sin *= -1f;
        }
        pos.x = transform.position.x + sin;
        transform.position = pos;
    }

    public override void ApplyAmountBuff(int level)
    {
        if(level >= 3)level = 3;
        _applyValueBuffLevel = level;
        switch (level)
        {
            case 1:
                for (int i = 0; i < 3; i++)
                {  
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, Random.Range(100f, -100f)));
                }
                break;
            case 2:  
                break;
            case 3:
                break;
        }
    }
}
