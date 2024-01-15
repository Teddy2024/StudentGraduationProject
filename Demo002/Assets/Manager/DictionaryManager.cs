using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System;

public class DictionaryManager : Singleton<DictionaryManager>
{
    List<ElementData> CurrentElementList;
    List<ElementData> CompoundList;
    public Dictionary<ElementData, int> CurrentElementDictionary = new Dictionary<ElementData, int>();
    
    List<Coroutine> _activeEffectCoroutines = new List<Coroutine>();
    public UnityEvent OnElementDicChange;
    Transform _player;

    [SerializeField] GameObject element;
    [SerializeField] SpawnButton spawnButton;
    [SerializeField] CompoundNotice compoundNotice;
    public float delayTimeBuff = 0f;
    #region 投射物總加成變數
    public Action<Projectile> buffFuction;
    [HideInInspector] public Vector3 scaleBuff;
    [HideInInspector] public DeBuff AllDeBuff;
    [HideInInspector] public float _damageBuff;
    [HideInInspector] public float _speedBuff;
    [HideInInspector] public float _knockBackForceBuff;
    [HideInInspector] public float _stayTimeBuff;  
    #endregion
    
    private void Start()
    {          
        _player = GameManager.Instance.GetPlayer().transform;
        CurrentElementList = GameManager.Instance.GetPlayer().playerDataBase.PlayerElementList.ToList();
        CurrentElementDictionary = CurrentElementList.ToElementDic();
        DealElementButton();

        StartCoroutine(SpawnSomethingWithDelay(element, _player, 20, 120));//每兩分鐘生一個元素
       
        CompoundList = ResourceManager.Instance.CompoundDataLibrary;
        OnElementDicChange?.Invoke();
    }

    #region 不重複元素分配到按鈕
    void DealElementButton()
    {
        var noDouble = NoRepeat(CurrentElementList);

        for (int i = 0; i < noDouble.Count; i++)
        {
            if(noDouble[i].elementType == ElementType.Element)
            {
                spawnButton.SpawnElementGrid(noDouble[i]);
            }
        }

        List<ElementData> NoRepeat(List<ElementData> target)
        {
            var newList = new List<ElementData>();
            for(var i = 0; i < target.Count; i++)
            {
                if(!newList.Contains(target[i]))
                {
                    newList.Add(target[i]);
                }
            }
            return newList;
        }
    }
    #endregion
    #region 元素生成Grid
    public void CheckSpawnElementGrid(ElementData elementData)
    {
        if (GetDicValue(elementData) == "1")
        {
            spawnButton.SpawnElementGrid(elementData);
        }
    }
    public string GetDicValue(ElementData target)
    {
        string finallValue;
        int value;

        if (CurrentElementDictionary.TryGetValue(target, out value))
        {
            finallValue = value.ToString();
        }
        else finallValue = "0";

        return finallValue;
    }
    #endregion
    
    #region 元素增減
    public void ElementAdd(ElementData elementData)
    {
        if(CurrentElementDictionary.ContainsKey(elementData))
        {
            CurrentElementDictionary[elementData]++;
        }
        else
        {
            CurrentElementDictionary[elementData] = 1;
        }
        OnElementDicChange?.Invoke();
    }
    public void ElementRemove(ElementData elementData)
    {
        if (CurrentElementDictionary.TryGetValue(elementData, out int count))
        {
            count--;
            if (count <= 0)
            {
                CurrentElementDictionary.Remove(elementData);
            }
            else
            {
                CurrentElementDictionary[elementData] = count;
            }
        }
        OnElementDicChange?.Invoke();
    }
    #endregion
    #region 元素合成
    public void ElementCombine()
    {
        bool canCombine = true;
        while (canCombine)
        {
            canCombine = false;
            foreach (var compound in CompoundList)
            {
                if (!CurrentElementDictionary.ContainsKey(compound) && ElementCombineCheck(CurrentElementDictionary, compound.CompoundMaterials.ToElementDic()))
                {
                    if(!ExpManager.Instance.ButtonCompoundData.Contains(compound))
                    {
                        compoundNotice.ChangeText(compound.BaseStats.elementName);
                        ExpManager.Instance.ButtonCompoundData.Add(compound);
                        canCombine = true;

                        Debug.Log(compound.BaseStats.elementName);
                    }
                }
            }
        }
    }
    #region 元素種類數量要求
    private bool ElementCombineCheck(Dictionary<ElementData, int> dictionary1, Dictionary<ElementData, int> dictionary2)
    {
        foreach (var kvp in dictionary2)
        {
            if (!dictionary1.ContainsKey(kvp.Key) || dictionary1[kvp.Key] < kvp.Value)
            {
                return false;
            }
        }
        return true;
    }
    #endregion
    #endregion
    #region 元素效果觸發
    public void EffectApply()
    {
        StopActiveEffectCoroutines();
        HashSet<ElementData> triggeredElements = new HashSet<ElementData>();
        foreach (var kvp in CurrentElementDictionary)
        {
            ElementData element = kvp.Key;
            int elementCount = kvp.Value;

            if (!element.LevelEffects.Any() && !element.Effects.Any())continue;
            if (triggeredElements.Contains(element))continue;

            foreach (var effect in element.LevelEffects)
            {
                effect.Apply();
            }

            foreach (var effect in element.Effects)
            {
                Coroutine effectCoroutine = StartCoroutine(ApplyEffectWithDelay(effect));
                _activeEffectCoroutines.Add(effectCoroutine);

                if (effect is AddProjectile projectileEffect)
                {   //生成投射物
                    Coroutine projectileCoroutine = StartCoroutine(SpawnProjectilesWithDelay(projectileEffect.ProjectileData.Prefab,
                        _player, projectileEffect.ProjectileData.spawnDelayTime, elementCount));
                    _activeEffectCoroutines.Add(projectileCoroutine);
                }

                if (effect is AddSomething somethingEffect)
                {   //生成道具
                    Coroutine somethingCoroutine = StartCoroutine(SpawnSomethingWithDelay(somethingEffect.something.gameObject,
                    _player, somethingEffect.distance, somethingEffect.DelayTime));
                    // _activeEffectCoroutines.Add(somethingCoroutine);
                }
            }
            triggeredElements.Add(element);
        }
    }
    #endregion
    #region 元素效果生成
    private IEnumerator ApplyEffectWithDelay(ElementEffect effect)
    {
        while (_player != null)
        {
            if (GameManager.Instance.GameIsPaused)
            {
                yield return new WaitUntil(() => !GameManager.Instance.GameIsPaused);
                continue;
            }
            effect.Apply();
            yield return new WaitForSeconds(effect.DelayTime);
        }
    }

    private IEnumerator SpawnProjectilesWithDelay(Projectile projectile, Transform player, float delayTime, int elementCount)
    {
        while (_player!= null)
        {
            if (GameManager.Instance.GameIsPaused)
            {
                yield return new WaitUntil(() => !GameManager.Instance.GameIsPaused);
                continue;
            }
            Projectile newProjectile = Instantiate(projectile, _player.transform.position, Quaternion.identity);
            newProjectile?.ApplyAmountBuff(elementCount);
            newProjectile.transform.parent = transform;
            yield return new WaitForSeconds(delayTime * delayTimeBuff);
        }
    }
    private IEnumerator SpawnSomethingWithDelay(GameObject something, Transform player, float distance, float delayTime)
    {
        while (_player!= null)
        {
            if (GameManager.Instance.GameIsPaused)
            {
                yield return new WaitUntil(() => !GameManager.Instance.GameIsPaused);
                continue;
            }
            Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * distance;
            Vector3 spawnPosition = _player.transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);
            GameObject newSomething = Instantiate(something, spawnPosition, Quaternion.identity);
            newSomething.transform.parent = transform;
            yield return new WaitForSeconds(delayTime);
        }
    }
    #endregion
    #region 元素效果清除
    private void StopActiveEffectCoroutines()
    {
        foreach (var coroutine in _activeEffectCoroutines)
        {
            StopCoroutine(coroutine);
        }
        _activeEffectCoroutines.Clear();
    }
    #endregion
    #region 投射物總加成
    private void ClampAllBuff()
    {
        _damageBuff = Mathf.Clamp(_damageBuff, 0f, 100f);
        _knockBackForceBuff = Mathf.Clamp(_knockBackForceBuff, 0f, 5000f);
    }

    public void ApplyProjectileBuff(Projectile projectile)
    {
        buffFuction?.Invoke(projectile);
        ClampAllBuff();

        projectile.gameObject.transform.localScale += scaleBuff;
        projectile._damage += _damageBuff;
        projectile._knockBackForce += _knockBackForceBuff;

        if(AllDeBuff != DeBuff.None && projectile.debuff == DeBuff.None)projectile.debuff = AllDeBuff;
    } 
    #endregion
}