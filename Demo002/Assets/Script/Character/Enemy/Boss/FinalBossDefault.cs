using UnityEngine;

public class FinalBossDefault : BossDefault
{
    public override void TakeDamage(float playerDamage , float knockbackForce, bool isDeBuff)
    {
        if(isDead)return;
        _sp.material = _whiteFlash;
        enemyHealth -= playerDamage;
        if(!isDeBuff)EnemyAIManager.Instance.DamagePopup(playerDamage, transform);//生成傷害數字        
        
        if(enemyHealth <= 0)
        {
            isDead = true;
            Death();
        }
        else Invoke("ResetMaterial", 0.05f);
    }

    protected override void Death()
    {
        GameManager.Instance.ChangeGameState(GameState.Win);
    }
}
