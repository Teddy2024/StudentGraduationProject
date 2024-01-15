using UnityEngine;

public class Projectile21 : Projectile
{
    [Header("旋轉角度"), SerializeField] float _angle = 0f;//旋轉角度
    [Header("離玩家距離"), SerializeField] float _radius = 0f;//離玩家距離
    [Header("距離幅度"), SerializeField] float radiusFloat = 3f;//離玩家距離

    private void Update() 
    {
        if (!player) return;
        _radius += Time.deltaTime * radiusFloat * 0.25f;
        _angle += Time.deltaTime * _speed;
        float x = Mathf.Cos(_angle) * _radius;
        float y = Mathf.Sin(_angle) * _radius;
        Vector3 offset = new Vector3(x, y, 0f);
        Vector3 pos = player.position + offset;
        transform.Rotate(new Vector3(0,0,10) * Time.deltaTime * 50);

        transform.position = pos;
    }
}
