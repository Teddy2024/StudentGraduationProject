using UnityEngine;

public class BossDefault : EnemyDefault
{
    public override void ChangeDeBuff(DeBuff debuff)
    {
        if(debuff == DeBuff.None || _currentDeBuff == debuff)return;
        _currentDeBuff = debuff;
        switch (debuff)
        {
            case DeBuff.SpeedDown:
            break;
            case DeBuff.Corrosive:
            break;
            case DeBuff.Poison:
            deBuffDamage = 3;
            damageInterval = 1;
            break;
            case DeBuff.Fire:
            deBuffDamage = 8;
            damageInterval = 1f;
            break;
            case DeBuff.Freeze:
            break;
        }
        EnemyAIManager.Instance.ColorChange(_sp, _currentDeBuff);
    }

    protected override void Death()
    {
        Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * 1;
        Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);
        GameObject newGem = Instantiate(EnemyAIManager.Instance.BossGem.gameObject, spawnPosition, Quaternion.identity);
        newGem.transform.parent = EnemyAIManager.Instance.transform;//生BOSS寶石

        EnemyAIManager.Instance.OnBossesDelete(this);

        base.Death();
    }
}
