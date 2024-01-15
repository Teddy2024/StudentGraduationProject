using UnityEngine;

public class Cow : EnemyDefault
{
    float t = 0f; 

    public override void TakeDamage(float playerDamage , float knockbackForce, bool isDeBuff)
    {
        if(isDead)return;
        _sp.material = _whiteFlash;
        if(player != null)
        {
            Vector2 direction = (transform.position - player.position).normalized;
            if(rb != null)rb.AddForce(direction * knockbackForce * 0.5f);//擊退效果
        }
        enemyHealth -= playerDamage;
        if(!isDeBuff)EnemyAIManager.Instance.DamagePopup(playerDamage, transform);//生成傷害數字
        
        t += 0.2f;
        transform.localScale = Vector3.Lerp(new Vector3(1f, 1f, 0), new Vector3(2f, 2f, 0), t);
        if(enemyHealth <= 0)
        {
            isDead = true;
            Death();
        }
        else
        {
            Invoke("ResetMaterial", 0.1f);
        }
    }
}
