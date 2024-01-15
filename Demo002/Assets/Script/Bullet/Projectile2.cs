using UnityEngine;
using DG.Tweening;

public class Projectile2 : Projectile
{
    [Header("震幅"), SerializeField] float amplitude = 15f;
    [Header("頻率"), SerializeField] float frequency = 0.7f;
    [Header("鏡像"), SerializeField] bool inverted = false;

    private void Update() 
    {
        Vector2 pos = transform.position;
        float sin = Mathf.Sin(pos.x * frequency) * amplitude *Time.deltaTime;
        if(inverted)
        {
            sin *= -1f;
        }
        pos.y = transform.position.y + sin;
        transform.position = pos;
    }
    #region 等級BUFF
    public override void ApplyAmountBuff(int level)
    {
        if(level >= 3)level = 3;
        _applyValueBuffLevel = level;
        switch (level)
        {
            case 1:
                break;
            case 2:  
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 180f));
                break;
            case 3:
                for (int i = 0; i < 2; i++)
                {  
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
                }
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
                _damage *= 1.5f;
                frequency *= 1.5f;
                break;
            case 3:
                _damage *= 2f;
                frequency *= 1.5f;
                _penetrate = true;
                break;
        }
    }
    #endregion
}
