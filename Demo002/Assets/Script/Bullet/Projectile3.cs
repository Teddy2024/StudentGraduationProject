using UnityEngine;

public class Projectile3 : Projectile
{
    [Header("旋轉角度"), SerializeField] float _angle = 0f;//旋轉角度
    [Header("離玩家距離"), SerializeField] float _radius = 0f;//離玩家距離
    [Header("距離幅度"), SerializeField] float radiusFloat = 3f;//離玩家距離

    private void Update() 
    {
        if (!player) return;
        _radius += Time.deltaTime * radiusFloat;
        _angle += Time.deltaTime * _speed;
        float x = Mathf.Cos(_angle) * _radius;
        float y = Mathf.Sin(_angle) * _radius;
        Vector3 offset = new Vector3(x, y, 0f);
        Vector3 pos = player.position + offset;

        transform.position = pos;

        Vector3 lookAtPosition = new Vector3(transform.position.x- 1f, transform.position.y, transform.position.z);
        transform.up = (player.position - lookAtPosition).normalized;
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
                _stayTime *= 1.5f;
                break;
            case 3:
                _angle = Random.Range(0f, 360f);
                radiusFloat *= 0.5f;
                _stayTime *= 3;
                _penetrate = true;
                _damage *= 1.5f;
                break;
        }
    }

}
