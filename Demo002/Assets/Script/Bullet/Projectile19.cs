using UnityEngine;

public class Projectile19 : Projectile
{
    public override void ApplyAmountBuff(int level)
    {
        level = Mathf.Clamp(level, 1, 3);
        _applyValueBuffLevel = level;
        switch (level)
        {
            case 1:
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position + new Vector3(0,0.5f,0), Quaternion.Euler(0f, 0f, 10f));
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position + new Vector3(0,-0.5f,0), Quaternion.Euler(0f, 0f, -10f));
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
