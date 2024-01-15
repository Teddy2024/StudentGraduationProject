using UnityEngine;

public class Projectile5 : Projectile
{
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
                transform.Rotate(0, 0, 45);
                break;
            case 2:
                transform.Rotate(0, 0, 45);
                transform.localScale += new Vector3(1f,1f,0);
                _stayTime = 5f;
                break;
            case 3:
                transform.Rotate(0, 0, 45);
                transform.localScale += new Vector3(5f,5f,0);
                _stayTime = 10;
                break;
        }
    }
}
