using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyDefault : MonoBehaviour, IDamageable, IDebuffable
{
    [Header("敵人數值")]
    protected float enemyHealth;
    [HideInInspector] public float enemyDamage;
    protected float enemySpeed;
    int rewardExperience;
    protected bool isDead;
    AudioClip enemySound;
    EnemyType enemyType;
    Faction faction;
    float _current, _target = 5, _rate = 20f;

    protected Transform player;
    protected Material _whiteFlash;
    protected Material _defaultMaterial;
    protected DeBuff _currentDeBuff;
    protected float deBuffDamage;
    protected float damageInterval = 1f; 
    protected float timeAccumulator = 0f;
    
    [Header("敵人鋼體")]
    [SerializeField] protected Rigidbody2D rb;
    [Header("敵人圖片")]
    [SerializeField] protected SpriteRenderer _sp;
    [Header("敵人動畫")]
    [SerializeField] protected Animator _anim;
    [Header("敵人影子")]
    [SerializeField] protected Transform _shadow;

    private void Awake() 
    {
        _whiteFlash = Resources.Load<Material>("WhiteFlash");
        _defaultMaterial = _sp.material;
    }
    private void Start() 
    {
        player = GameManager.Instance.GetPlayer().transform;
    }
    private void OnDestroy() 
    {
        KillAllFuckingDotween();
    }

    #region 敵人資料設定
    public void EnemyDataSet(EnemyDataBase enemyDataBase)
    {
        enemyHealth = enemyDataBase.BaseStats.enemyHealth;
        enemyDamage = enemyDataBase.BaseStats.enemyDamage;
        enemySpeed = enemyDataBase.BaseStats.enemySpeed;
        rewardExperience = enemyDataBase.BaseStats.rewardExperience;
        enemySound = enemyDataBase.BaseStats.hitSound;
        enemyType = enemyDataBase.enemyType;
        faction = enemyDataBase.faction;

        enemyDataBase = null;
    }
    #endregion
    #region Ai敵人追逐
    public virtual void Chase()
    {
        if(player == null || _currentDeBuff == DeBuff.Freeze)return;
        LookAtTarget();
        MoveTowardsTarget();
        Wave();
    }
    #endregion
    #region 敵人移動與轉向
    void MoveTowardsTarget()
    {
        Vector2 newPos = Vector2.MoveTowards(transform.position, player.position, enemySpeed * Time.fixedDeltaTime);
        transform.GetComponent<Rigidbody2D>().MovePosition(newPos);
    }

    void Wave()
    {
        _current = Mathf.MoveTowards(_current, _target, _rate * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(0, 0, _current);
        if(_current == _target)_target = _target == 5 ? -5 : 5;
    }

    void LookAtTarget()
    {
        float relativePos = Mathf.Sign(player.position.x - _sp.transform.position.x);
        _sp.flipX = relativePos > 0;

        float shadowScaleX = _sp.flipX ? -Mathf.Abs(_shadow.localScale.x) : Mathf.Abs(_shadow.localScale.x);
        float shadowPositionX = _sp.flipX ? -Mathf.Abs(_shadow.localPosition.x) : Mathf.Abs(_shadow.localPosition.x);

        _shadow.localScale = new Vector3(shadowScaleX, _shadow.localScale.y, _shadow.localScale.z);
        _shadow.localPosition = new Vector3(shadowPositionX, _shadow.localPosition.y, _shadow.localPosition.z);
    }
    #endregion
    #region 敵人受傷與死亡
    public virtual void TakeDamage(float playerDamage , float knockbackForce, bool isDeBuff)
    {
        if(isDead)return;
        _sp.material = _whiteFlash;
        enemyHealth -= playerDamage;
        if(!isDeBuff)EnemyAIManager.Instance.DamagePopup(playerDamage, transform);//生成傷害數字
        if(!isDeBuff)HitBack(knockbackForce);//擊退效果
        
        if(enemyHealth <= 0)
        {
            isDead = true;
            Death();
        }
        else
        {
            Invoke("ResetMaterial", 0.05f);
        }
    }

    protected virtual void Death()
    {
        AudioManager.Instance.PlaySound(enemySound, 1);//發出聲音
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce).OnComplete(() => 
        {
            EnemyAIManager.Instance.SpawnDead(transform.position);

            if(enemyType == EnemyType.normalEnemy)
            {
                if(rewardExperience != 0)
                {
                    for (int i = 0; i < rewardExperience / 5 ; i++)
                    {
                        Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * 1;
                        Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);
                        EnemyAIManager.Instance.SpawnGem(spawnPosition);//生普通寶石
                    }
                }
                EnemyAIManager.Instance.OnEnemiesDelete(this);
            }
            Destroy(gameObject, 0.2f);
        });
    }

    private void ResetMaterial() => _sp.material = _defaultMaterial;
    private void HitBack(float knockbackForce)
    {
        if(player != null && enemyType == EnemyType.normalEnemy)
        {
            Vector2 direction = (transform.position - player.position).normalized;
            Vector2 goal = (Vector2)transform.position + direction * 1f;

            transform.DOLocalMove(goal, knockbackForce).SetSpeedBased(true).SetEase(Ease.InOutSine);

            transform?.DOShakePosition(.5f, .5f);
            transform?.DOShakeRotation(.5f, .5f);
            transform?.DOShakeScale(.5f, .1f);
        }
    }
    #endregion
    #region 敵人負面狀態 
    public virtual void ChangeDeBuff(DeBuff debuff)
    {
        if(debuff == DeBuff.None || _currentDeBuff == debuff)return;
        _currentDeBuff = debuff;
        switch (debuff)
        {
            case DeBuff.SpeedDown:
            enemySpeed *= 0.75f;
            break;
            case DeBuff.Corrosive:
            rewardExperience *= 2;
            break;
            case DeBuff.Poison:
            deBuffDamage = EnemyAIManager.Instance._poisonDamage;
            break;
            case DeBuff.Fire:
            deBuffDamage = EnemyAIManager.Instance._FireDamage;
            break;
            case DeBuff.Freeze:
            rb.mass = 100;
            break;
        }
        EnemyAIManager.Instance.ColorChange(_sp, _currentDeBuff);
    }

    public virtual void DeBuffApply()
    {
        if (_currentDeBuff == DeBuff.Poison || _currentDeBuff == DeBuff.Fire)
        {
            timeAccumulator += Time.deltaTime; // 累計時間

            while (timeAccumulator >= damageInterval) // 檢查是否到達掉血時間
            {
                TakeDamage(deBuffDamage, 1000, true);
                EnemyAIManager.Instance.DeBuffDamagePopup(deBuffDamage, transform, _currentDeBuff);
                timeAccumulator -= damageInterval; // 減去已處理的時間
            }
        }

        if(_anim != null)
        {
            if(_currentDeBuff == DeBuff.SpeedDown)_anim.speed = 0.5f;
            else if(_currentDeBuff == DeBuff.Freeze)_anim.speed = 0f;
            else _anim.speed = 1f;
        }
    }
    #endregion
    #region 清除DOTween
    void KillAllFuckingDotween()
    {
        transform.DOKill(false);
    }
    #endregion
}