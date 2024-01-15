using UnityEngine;

public class Projectile4 : Projectile
{
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private float  _rotationSpeed;

    private void Update() 
    {
        transform.Rotate(_rotation * Time.deltaTime * _rotationSpeed);
    }

    protected override void Move()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector2 movementDirection = randomDirection * _speed;

        rb.velocity = movementDirection;
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
                for (int i = 0; i < 1; i++)
                {  
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.identity);
                }
                break;
            case 3:
                for (int i = 0; i < 3; i++)
                {  
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.identity);
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
                break;
            case 3:
                _damage *= 2f;
                break;
        }
    }
    #endregion

   
}
