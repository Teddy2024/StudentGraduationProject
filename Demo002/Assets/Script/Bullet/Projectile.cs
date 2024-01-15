using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileData _projectileData;
    [HideInInspector] public float _damage;
    [HideInInspector] public float _speed;
    [HideInInspector] public float _knockBackForce;
    [HideInInspector] public float _stayTime;
    [HideInInspector] public bool _penetrate;

    protected Transform player;
    protected Rigidbody2D rb;
    public DeBuff debuff;

    [HideInInspector] public int _applyValueBuffLevel;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameManager.Instance.GetPlayer().transform;
        ProjectileData(_projectileData);
        ApplyValueBuff(_applyValueBuffLevel);
        DictionaryManager.Instance.ApplyProjectileBuff(this);
        Move();
        StartCoroutine(DestroyAfterDelay(_stayTime));
    }
    #region 等級BUFF
    public virtual void ApplyAmountBuff(int level)
    {
        level = Mathf.Clamp(level, 1, 3);
        _applyValueBuffLevel = level;
        switch (level)
        {
            case 1:
                break;
            case 2:  
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position, Quaternion.Euler(0f, 0f, 180f));
                break;
            case 3:
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position + new Vector3(0,0.5f,0), Quaternion.Euler(0f, 0f, 10f));
                Instantiate(this, GameManager.Instance.GetPlayer().transform.position + new Vector3(0,-0.5f,0), Quaternion.Euler(0f, 0f, -10f));
                break;
        }
    }
    public virtual void ApplyValueBuff(int level)
    {
        switch (level)
        {
            case 1:
                break;
            case 2:
                _damage *= 1.5f;
                break;
            case 3:
                _knockBackForce *= 1.2f;
                _damage *= 2;
                break;
        }
    }
    #endregion
    #region 投射物資料設定
    public void ProjectileData(ProjectileData projectileData)
    {
        _speed = projectileData.BaseStats.speed;
        _damage = projectileData.BaseStats.damage;
        _knockBackForce = projectileData.BaseStats.knockBackForce;
        _stayTime = projectileData.BaseStats.stayTime;
        _penetrate = projectileData.BaseStats.penetrate;
    }
    #endregion
    #region 投射物移動
    protected virtual void Move()
    {
        if (!player) return;
        Vector3 initialScale = player.localScale;

        // 获取生成点的旋转角度
        float spawnRotation = transform.rotation.eulerAngles.z;

        // 将生成点的旋转角度应用于移动方向向量
        Quaternion rotation = Quaternion.Euler(0f, 0f, spawnRotation);
        Vector2 movementDirection = rotation * new Vector2(initialScale.x * -1, 0f).normalized;

        rb.velocity = movementDirection * _speed;
    }
    #endregion
    #region 投射物攻擊賦予負面狀態
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent(out IDamageable enemy))Attack(enemy);
        if(other.gameObject.TryGetComponent(out IDebuffable debuffEnemy))DeBuffAttack(debuffEnemy);
    }
    protected virtual void Attack(IDamageable enemy)
    {
        enemy.TakeDamage(_damage, _knockBackForce, false);
        if(!_penetrate)ProjectileEnd();
    }
    protected virtual void DeBuffAttack(IDebuffable debuffEnemy)
    {
        if(debuff != DeBuff.None)debuffEnemy.ChangeDeBuff(debuff);
    }

    #endregion
    #region 投射物消失
    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ProjectileEnd();
    }
    protected virtual void ProjectileEnd()
    {
        transform.DOKill(false);
        Destroy(gameObject);
    }
    #endregion
    #region 投射物螢幕外
    private void OnBecameInvisible()
    {
        ProjectileEnd();
    }
    #endregion
}
