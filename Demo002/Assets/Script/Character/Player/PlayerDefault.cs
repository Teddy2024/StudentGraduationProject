using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class PlayerDefault : MonoBehaviour
{
    [Header("玩家數值")] public PlayerDataBase playerDataBase;
    float maxHealth;
    float currentHealth; 
    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            _healthBar.HealthSet(CurrentHealth);
        }
    }
    [HideInInspector] public float speed;
    Gender gender;

    [Header("玩家鋼體"), SerializeField] private Rigidbody2D _rb;
    [Header("玩家撿取道具"), SerializeField] public PlayerPickUp _playerPickUp;
    [Header("玩家生命條"), SerializeField] private HealthBar _healthBar;
    [Header("玩家受傷特效"), SerializeField] private ParticleSystem _blood;

    [HideInInspector] public Vector2 movement;
    List<GameObject> collidingEnemies = new List<GameObject>();//碰撞的敵人
    bool facingRight = false;
    public bool dead = false;
    public bool IsTakingDamage => collidingEnemies.Count > 0;

    private void Start() => PlayerDataSet(playerDataBase);

    private void Update()
    {
        if(dead)return;
        PlayerInput();
        FlipControl();
        BloodCheck();
    }

    private void FixedUpdate()
    {
        if(dead)return;
        EnemyMissingCheck();
        PlayerMovement();
    }

    #region 玩家資料設定
    private void PlayerDataSet(PlayerDataBase playerDataBase)
    {
        maxHealth = playerDataBase.BaseStats.maxHealth;
        currentHealth = maxHealth;
        _healthBar.MaxHealthSet(maxHealth);
        speed = playerDataBase.BaseStats.speed;
        gender = playerDataBase.gender;

        AudioManager.Instance._hurtSource.Play();
    }
    #endregion
    #region 玩家移動
    protected virtual void PlayerInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    protected virtual void PlayerMovement()
    {
        float moveAmount = speed * Time.fixedDeltaTime;
        float moveMagnitude = movement.magnitude;

        if (moveMagnitude > 1f)movement.Normalize();
        _rb.MovePosition(_rb.position + movement * moveAmount);
    }
    #endregion
    #region 玩家翻轉
    private void FlipControl()
    {
        if((movement.x > 0 && !facingRight) || (movement.x < 0 && facingRight))
        {
            Flip(transform);
            Flip(_healthBar.transform);
            facingRight = !facingRight;
        }

        void Flip(Transform flipTransform)
        {
            Vector3 currentScale = flipTransform.localScale;
            currentScale.x *= -1;
            flipTransform.localScale = currentScale;
        }
    }
    #endregion
    #region 玩家碰到敵人或道具
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.TryGetComponent(out EnemyDefault enemy))
        {
            collidingEnemies.Add(other.gameObject);
        }
    }
    private void OnCollisionStay2D(Collision2D other) 
    {
        GameObject otherGameObject = other.gameObject;
        if (collidingEnemies.Contains(otherGameObject))
        {
            if (otherGameObject.TryGetComponent(out EnemyDefault enemy))
            {
                TakeDamage(enemy.enemyDamage);
            }
        }
    }
    private void OnCollisionExit2D(Collision2D other) 
    {
        if(other.gameObject.TryGetComponent(out EnemyDefault enemy))
        {
            collidingEnemies.Remove(other.gameObject);
        }
    }
    void EnemyMissingCheck()
    {
        for (int i = collidingEnemies.Count - 1; i >= 0; i--)
        {
            if(collidingEnemies[i] == null)collidingEnemies.RemoveAt(i);
        }
    }
    #endregion
    #region 玩家回血  
    public void HealthRecover(float amount)
    {
        CurrentHealth += amount;
    }
    #endregion
    #region 玩家受傷與死亡
    protected virtual void TakeDamage(float enemyDamage)
    {
        if(dead)return;
        CurrentHealth -= enemyDamage;
        if(CurrentHealth <= 0)Death();
    }

    void BloodCheck()
    {
        if(GameManager.Instance.GameIsPaused)return;
        if(IsTakingDamage)
        {
            _blood.Play();
            AudioManager.Instance._hurtSource.UnPause();
        }
        else
        {
            _blood.Stop();
            AudioManager.Instance._hurtSource.Pause();
        }
    }

    private void Death()
    {
        dead = true;
        var go = UiManager.Instance?.deadPart;
        if(go != null)Instantiate(go, transform.position, go.transform.rotation);
        AudioManager.Instance.StopAllAudio();
        Destroy(gameObject);
    }
    #endregion

}