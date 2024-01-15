using UnityEngine;
using System;
using UnityEngine.Pool;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.VFX;

public class EnemyAIManager : Singleton<EnemyAIManager>
{
    [Header("所有敵人")]
    public List<EnemyDefault> Enemies = new List<EnemyDefault>();
    [Header("所有關主")]
    public List<EnemyDefault> Bosses = new List<EnemyDefault>();

    #region 物件池的變數
    [Header("經驗值寶石")]
    [SerializeField] private ItemDefault Gem;
    public ItemDefault BossGem;
    [SerializeField] private int MaxPoolCount = 500;

    [Header("傷害數字")]
    [SerializeField] private PopDamage PopupDamage;
    [SerializeField] private int MaxPopPoolCount = 100;

    [Header("死亡動畫")]
    [SerializeField] private VFXscript DeathAnim;
    [SerializeField] private int MaxDeadPoolCount = 100;
    #endregion
    #region 物件池的設定
    //寶石物件池
    public ObjectPool<ItemDefault> _pool;
    public int CurrentPoolCount { get; set; }
    public bool CanSpawnGem => CurrentPoolCount < MaxPoolCount;

    //PopDamage物件池
    public ObjectPool<PopDamage> _popPool;
    public int CurrentPopPoolCount { get; set; }
    public bool CanSpawnPop => CurrentPopPoolCount < MaxPopPoolCount;

    //VFXscript死亡特效池
    public ObjectPool<VFXscript> _deadPool;
    public int CurrentDeadPoolCount { get; set; }
    public bool CanSpawnDead => CurrentDeadPoolCount < MaxDeadPoolCount;
    #endregion

    [Header("狀態傷害")]
    public int _poisonDamage = 3;
    public int _FireDamage = 8;

    Transform _player;

    [Space(10)]
    [SerializeField] private EffectColor _effectColor;
    public EffectColor BaseColor => _effectColor;

    [Space(10)]
    [SerializeField] private PopDamageColor _popDamageColor;
    public PopDamageColor BasePopupDamageColor => _popDamageColor;


    protected override void Awake() 
    {
        base.Awake();
        _pool = PoolSomething<ItemDefault>(Gem, MaxPoolCount);
        _popPool = PoolSomething<PopDamage>(PopupDamage, MaxPopPoolCount);
        _deadPool = PoolSomething<VFXscript>(DeathAnim, MaxDeadPoolCount);
    }

    private void Start() 
    {
        _player = GameManager.Instance.GetPlayer().transform;
    }

    private void Update()
    {
        HandleEnemyUpdate(Enemies);
        HandleEnemyUpdate(Bosses);
    }

    private void FixedUpdate()
    {
        HandleEnemyFixedUpdate(Enemies);
        HandleEnemyFixedUpdate(Bosses);
    }

    #region 處理敵人追縱，移動
    private void HandleEnemyUpdate(List<EnemyDefault> list)
    {
        if(_player == null)return;
        for(int i = list.Count - 1; i >= 0; i--)
        {
            if(list[i] == null)
            {
                list.RemoveAt(i);
            }
            else
            {
                list[i].DeBuffApply();
            }
        }
    }
    private void HandleEnemyFixedUpdate(List<EnemyDefault> list)
    {
        if(_player == null)return;
        for(int i = list.Count - 1; i >= 0; i--)
        {
            if(list[i] == null)
            {
                list.RemoveAt(i);
            }
            else
            {
                list[i].Chase();
            }
        }
    }
    #endregion
    #region 物件池設定
    ObjectPool<T> PoolSomething<T>(T Item, int maxPoolCount)where T: MonoBehaviour
    {
        var pool = new ObjectPool<T>  (() => {return Instantiate(Item);},
                                            Item => {Item.gameObject.SetActive(true);},
                                            Item => {Item.gameObject.SetActive(false);},
                                            Item => {Destroy(Item.gameObject);},
                                            false, 100, maxPoolCount); 

        List<T> itemList = new List<T>();
        for (int i = 0; i < maxPoolCount; i++)
        {
            var newItem = pool.Get();
            newItem.transform.parent = transform;
            itemList.Add(newItem); 
        }

        foreach (var item in itemList)
        {
            item.gameObject.SetActive(false); // 在游戏运行时取消激活对象
            pool.Release(item);
        }
        itemList.Clear();
        return pool;
    }
    #endregion
    #region 敵人增減
    public void OnEnemiesAdd(EnemyDefault enemyDefault)
    {
        Enemies.Add(enemyDefault);
    }
    public void OnEnemiesDelete(EnemyDefault enemyDefault)
    {
        if(Enemies.Contains(enemyDefault))
        {
            Enemies.Remove(enemyDefault);
        }
    }
    public void OnBossesAdd(EnemyDefault bossDefault)
    {
        Bosses.Add(bossDefault);
    }
    public void OnBossesDelete(EnemyDefault bossDefault)
    {
        if(Bosses.Contains(bossDefault))
        {
            Bosses.Remove(bossDefault);
        }
    }
    #endregion
    #region 生成寶石
    public void SpawnGem(Vector3 spawnPosition)
    {
        if(CanSpawnGem)
        {
            CurrentPoolCount++;
            var newGem = _pool.Get();
            newGem.transform.position = spawnPosition;
            newGem.transform.parent = transform;
        }
    }
    public void ReleaseGem(ItemDefault gem)
    {
        CurrentPoolCount--;
        _pool.Release(gem);
    }
    #endregion
    #region 生成傷害數字
    public void DamagePopup(float damage, Transform enemyTram)
    {
        if(damage <= 0 || !CanSpawnPop)return;

        Color damColor = Color.white;
        Vector3 damSize = Vector3.zero;

        damage = UpDownNum(damage);
        if(damage <= 10)
        {
            damColor = BasePopupDamageColor.Light;
            damSize = new Vector3(.35f, .35f, .35f);
        }
        else if(damage > 10 && damage <= 20)
        {
            damColor = BasePopupDamageColor.Medial;
            damSize = new Vector3(.5f, .5f, .5f);
        }
        else
        {
            damSize = new Vector3(.7f, .7f, .7f);
            damColor = BasePopupDamageColor.Heavy;
        }
        HandlePopup(damage, damColor, enemyTram).transform.localScale = damSize;
    }

    float UpDownNum(float damage)
    {
        float updown = UnityEngine.Random.Range(1.2f, 0.8f);
        updown = (float)(Mathf.Round(updown * 100))/100;
        damage *= updown;
        damage = (float)(Mathf.Round(damage * 100))/100;
        return damage;
    }

    public void DeBuffDamagePopup(float damage, Transform enemyTram, DeBuff deBuff)
    {
        if(damage <= 0 || !CanSpawnPop)return;
        Color damColor = Color.white;

        if(deBuff == DeBuff.Poison)
        {
            damColor = BasePopupDamageColor.Poison;
        }
        else if(deBuff == DeBuff.Fire)
        {
            damColor = BasePopupDamageColor.Fire;
        }
        var pop = HandlePopup(damage, damColor, enemyTram);
    }

    PopDamage HandlePopup(float damage, Color damColor, Transform enemyTram)
    {
        CurrentPopPoolCount++;
        var newPop = _popPool.Get();
        newPop.SetPopupDamage(damage, damColor);
        newPop.transform.position = new Vector3(enemyTram.position.x, enemyTram.position.y, -9);
        newPop.transform.parent = transform;
        return newPop;
    }

    public void ReleasePop(PopDamage popDamage)
    {
        CurrentPopPoolCount--;
        _popPool.Release(popDamage);
    }
    #endregion
    #region 生成死亡特效
    public void SpawnDead(Vector3 spawnPosition)
    {
        if(CanSpawnDead)
        {
            CurrentDeadPoolCount++;
            var newDead = _deadPool.Get();
            newDead.PlaySomeDead();
            newDead.transform.position = spawnPosition;
            newDead.transform.parent = transform;
        }
    }
    public void ReleaseDead(VFXscript newDead)
    {
        CurrentDeadPoolCount--;
        _deadPool.Release(newDead);
    }
    #endregion
    #region 改變敵人顏色
    public void ColorChange(SpriteRenderer _sp, DeBuff _currentDeBuff)
    {
        switch (_currentDeBuff)
        {
            case DeBuff.SpeedDown:
                _sp.color = BaseColor.SpeedDownClor;
                break;
            case DeBuff.Corrosive:
                _sp.color = BaseColor.CorrosiveClor;
                break;
            case DeBuff.Poison:
                _sp.color = BaseColor.PoisonClor;
                break;
            case DeBuff.Fire:
                _sp.color = BaseColor.FireClor;
                break;
            case DeBuff.Freeze:
                _sp.color = BaseColor.FreezeClor;
                break;
            default:
                _sp.color = Color.white;
                break;
        }
    }
    #endregion

}

[Serializable] public struct EffectColor
{
    public Color SpeedDownClor;
    public Color CorrosiveClor;
    public Color PoisonClor;
    public Color FireClor;
    public Color FreezeClor;
}

[Serializable] public struct PopDamageColor
{
    public Color Light;
    public Color Medial;
    public Color Heavy;
    public Color Fire;
    public Color Poison;
}

public enum DeBuff
{
    None,//沒
    SpeedDown,//速度下降
    Corrosive,//腐蝕
    Poison,//毒
    Fire,//燃燒
    Freeze//靜止
}
