using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewElement", menuName = "ScriptableObject/Element", order = 2)]
public class ElementData : ScriptableObject
{
    //元素資料
    [SerializeField, Header("元素資料")] private ElementStats _elementStats;
    public ElementStats BaseStats => _elementStats;
    [Header("元素類別")]public ElementType elementType;
    
    // 合成材料元素列表
    [Space(20)]
    [SerializeField, Header("合成物專用")]
    private List<ElementData> _compoundMaterials;
    public List<ElementData> CompoundMaterials => _compoundMaterials;
    
    [Space(30)]
    #region 元素按鈕效果列表 
    [SerializeReference] public List<ElementButtonEffect> ButtonEffects;
    [ContextMenu(nameof(AddButtonPlayerBuff))] void AddButtonPlayerBuff(){ ButtonEffects.Add(new ButtonPlayerBuff());}
    [ContextMenu(nameof(AddButtonProjectBuff))] void AddButtonProjectBuff(){ ButtonEffects.Add(new ButtonProjectBuff());}
    [ContextMenu(nameof(AddButtonSpecificBuff))] void AddButtonSpecificBuff(){ ButtonEffects.Add(new ButtonSpecificBuff());}
    [ContextMenu(nameof(AddButtonProjectChange))] void AddButtonProjectChange(){ ButtonEffects.Add(new ButtonProjectChange());}
    [ContextMenu(nameof(AddButtonAllEnemy))] void AddButtonAllEnemy(){ ButtonEffects.Add(new ButtonAllEnemy());}
    [ContextMenu(nameof(AddButtonAllItem))] void AddButtonAllItem(){ ButtonEffects.Add(new ButtonAllItem());}
    #endregion
    #region 元素等級效果列表
    [SerializeReference] public List<ElementLevelEffect> LevelEffects;
    [ContextMenu(nameof(AddLevelPlayerBuff))] void AddLevelPlayerBuff(){ LevelEffects.Add(new LevelPlayerBuff());}
    [ContextMenu(nameof(AddLevelProjectBuff))] void AddLevelProjectBuff(){ LevelEffects.Add(new LevelProjectBuff());}
    [ContextMenu(nameof(AddLevelAllEnemy))] void AddLevelAllEnemy(){ LevelEffects.Add(new LevelAllEnemy());}
    #endregion
    #region 元素持續效果列表
    [SerializeReference] public List<ElementEffect> Effects;
    [ContextMenu(nameof(AddPlayerBuff))] void AddPlayerBuff(){ Effects.Add(new AddPlayerBuff());}
    [ContextMenu(nameof(AddProjectile))] void AddProjectile(){ Effects.Add(new AddProjectile());}
    [ContextMenu(nameof(AddExp))] void AddExp(){ Effects.Add(new AddExp());}
    [ContextMenu(nameof(AddSomething))] void AddSomething(){ Effects.Add(new AddSomething());}
    #endregion
}
#region 元素資料
[Serializable] public struct ElementStats
{
    public Sprite elementSp;
    public int elementID;
    public string elementName;
    [TextArea(1,3)]public string description;
    [TextArea(1,10)]public string dicDescription;
}
[Serializable] public enum ElementType
{
    Element = 0,
    Compound = 1,
};
#endregion

#region 各種按鈕元素效果
[Serializable] public class ElementButtonEffect
{
    public virtual void Apply(){}
}
[Serializable] public class ButtonPlayerBuff : ElementButtonEffect
{
    public string effectName = "玩家強化";
    public PlayerBuff buff;
    public float BuffAmount;
    public override void Apply()
    {
        switch (buff)
        {
            case PlayerBuff.Heal:
                GameManager.Instance.GetPlayer().HealthRecover(BuffAmount);
                break;
            case PlayerBuff.SpeedUp:
                GameManager.Instance.GetPlayer().speed += BuffAmount;
                break;
            case PlayerBuff.Small:
                if(GameManager.Instance.GetPlayer().transform.localScale.y <= 0.75f)return;
                GameManager.Instance.GetPlayer().transform.localScale *= 0.75f;
                GameManager.Instance.GetPlayer().GetComponent<Rigidbody2D>().mass *= 0.75f;
                break;
            case PlayerBuff.Heavier:
                GameManager.Instance.GetPlayer().GetComponent<Rigidbody2D>().mass *= 2f;
                break;
            case PlayerBuff.Bigger:
                GameManager.Instance.GetPlayer().GetComponent<Rigidbody2D>().mass *= 4f;
                GameManager.Instance.GetPlayer().transform.localScale *= 1.3f;
                break;
            case PlayerBuff.ItemRange:
                GameManager.Instance.GetPlayer()._playerPickUp.ChangePickUpRange();
                break;
        }
    }
}
[Serializable] public class ButtonProjectBuff : ElementButtonEffect
{
    public string effectName = "投射物強化";
    public ProjectileBuff buff;
    public float BuffAmount;
    public Vector3 BuffScale;
    public DeBuff debuff;
    public override void Apply()
    {
        switch (buff)
        {
            case ProjectileBuff.DelayTime:
                DictionaryManager.Instance.delayTimeBuff -= 0.1f;
                if(DictionaryManager.Instance.delayTimeBuff <= 0f)DictionaryManager.Instance.delayTimeBuff = 0.1f;
                break;
            case ProjectileBuff.Attack:
                DictionaryManager.Instance._damageBuff += BuffAmount;
                break;
            case ProjectileBuff.Speed:
                // DictionaryManager.Instance._speedBuff += BuffAmount;
                break;
            case ProjectileBuff.BackForce:
                DictionaryManager.Instance._knockBackForceBuff += BuffAmount;
                break;
            case ProjectileBuff.StayTime:
                // DictionaryManager.Instance._stayTimeBuff += BuffAmount;
                break;
            case ProjectileBuff.Bigger:
                DictionaryManager.Instance.scaleBuff += BuffScale;
                break;
            case ProjectileBuff.DeBuffChange:
                DictionaryManager.Instance.AllDeBuff = this.debuff;
                break;
            case ProjectileBuff.CompoundLevelUp:
                break;
        }
    }
}
[Serializable] public class ButtonSpecificBuff : ElementButtonEffect
{
    public string effectName = "指定投射物強化";
    [Header("特定目標投射物")]public List<string> SpecificTargets;
    public ProjectileBuff buff;
    public float BuffAmount;
    public Vector3 BuffScale;
    public DeBuff debuff;

    public override void Apply()
    {
       DictionaryManager.Instance.buffFuction +=  SpecificTargetBuff;
    }
    private void SpecificTargetBuff(Projectile projectile)
    {
        if(SpecificTargets.Contains(projectile.gameObject.name))
        {
            switch (buff)
            {
                case ProjectileBuff.DelayTime:
                    break;
                case ProjectileBuff.Attack:
                    projectile._damage += BuffAmount;
                    break;
                case ProjectileBuff.Speed:
                    // projectile._speed += BuffAmount;
                    break;
                case ProjectileBuff.BackForce:
                    projectile._knockBackForce += BuffAmount;
                    break;
                case ProjectileBuff.StayTime:
                    // projectile._stayTime += BuffAmount;
                    break;
                case ProjectileBuff.Bigger:
                    projectile.gameObject.transform.localScale += BuffScale;
                    break;
                case ProjectileBuff.DeBuffChange:
                    projectile.debuff = this.debuff;
                    break;
                case ProjectileBuff.CompoundLevelUp:
                    projectile.ApplyAmountBuff(3);
                    break;
            }
        }
    }
}
[Serializable] public class ButtonProjectChange : ElementButtonEffect
{
    public string effectName = "投射物特別變化";
    public Sprite _sprite;

    public override void Apply()
    {
       DictionaryManager.Instance.buffFuction +=  SpecificTargetChange;
    }
    private void SpecificTargetChange(Projectile projectile)
    {
        _sprite = GameManager.Instance.GetPlayer().playerDataBase.BaseStats.playerSp;
        projectile.gameObject.GetComponent<SpriteRenderer>().sprite = _sprite;
        projectile.gameObject.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(UnityEngine.Random.Range(0f, 1f), 1f, 1f);;
    }
}
[Serializable] public class ButtonAllEnemy : ElementButtonEffect
{
    public string effectName = "所有敵人";
    public EnemyEffect enemyEffect;
    public DeBuff debuff;
    public float damage;
    public float backForce;
    public override void Apply()
    {
        foreach (Transform child in SpawnManager.Instance.transform)
        {
            switch (enemyEffect)
            {
                case EnemyEffect.DeBuff:  
                    if (child.TryGetComponent(out IDebuffable enemyDebuff))enemyDebuff.ChangeDeBuff(debuff);
                    break;
                case EnemyEffect.BackForce:  
                    if (child.TryGetComponent(out IDamageable enemyDamage))
                    {
                        enemyDamage.TakeDamage(damage, backForce, false);
                    }
                    break;
            }
        }
    } 
}
[Serializable] public class ButtonAllItem : ElementButtonEffect
{
    public string effectName = "所有寶石";
    public override void Apply()
    {
        foreach (Transform item in EnemyAIManager.Instance.transform)
        {
            if (item.gameObject.activeSelf && item.TryGetComponent(out ItemDefault pickUpItem))
            {
                pickUpItem.OnPickUp();
            }
        }
    } 
}
#endregion
#region 各種等級元素效果
[Serializable] public class ElementLevelEffect
{
    public virtual void Apply(){}
}
[Serializable] public class LevelPlayerBuff : ElementLevelEffect
{
    public string effectName = "玩家強化";
    public PlayerBuff buff;
    public float BuffAmount;
    public override void Apply()
    {
        switch (buff)
        {
            case PlayerBuff.Heal:
                GameManager.Instance.GetPlayer().HealthRecover(BuffAmount);
                break;
            case PlayerBuff.SpeedUp:
                GameManager.Instance.GetPlayer().speed += BuffAmount;
                break;
            case PlayerBuff.Small:
                if(GameManager.Instance.GetPlayer().transform.localScale.y <= 0.75f)return;
                GameManager.Instance.GetPlayer().transform.localScale *= 0.75f;
                GameManager.Instance.GetPlayer().GetComponent<Rigidbody2D>().mass *= 0.75f;
                break;
            case PlayerBuff.Heavier:
                GameManager.Instance.GetPlayer().GetComponent<Rigidbody2D>().mass *= 2f;
                break;
            case PlayerBuff.Bigger:
                GameManager.Instance.GetPlayer().GetComponent<Rigidbody2D>().mass *= 4f;
                GameManager.Instance.GetPlayer().transform.localScale *= 1.3f;
                break;
            case PlayerBuff.ItemRange:
                GameManager.Instance.GetPlayer().GetComponent<PlayerPickUp>().gameObject.transform.localScale *= 1.3f;
                break;
        }
    }
}
[Serializable] public class LevelProjectBuff : ElementLevelEffect
{
    public string effectName = "投射物強化";
    public ProjectileBuff buff;
    public float BuffAmount;
    public Vector3 BuffScale;
    public DeBuff debuff;
    
    public override void Apply()
    {
        switch (buff)
        {
            case ProjectileBuff.DelayTime:
                DictionaryManager.Instance.delayTimeBuff -= 0.05f;
                if(DictionaryManager.Instance.delayTimeBuff <= 0.1f)DictionaryManager.Instance.delayTimeBuff = 0.1f;
                break;
            case ProjectileBuff.Attack:
                DictionaryManager.Instance._damageBuff += BuffAmount;
                break;
            case ProjectileBuff.Speed:
                // DictionaryManager.Instance._speedBuff += BuffAmount;
                break;
            case ProjectileBuff.BackForce:
                DictionaryManager.Instance._knockBackForceBuff += BuffAmount;
                break;
            case ProjectileBuff.StayTime:
                // DictionaryManager.Instance._stayTimeBuff += BuffAmount;
                break;
            case ProjectileBuff.Bigger:
                DictionaryManager.Instance.scaleBuff += BuffScale;
                break;
            case ProjectileBuff.DeBuffChange:
                DictionaryManager.Instance.AllDeBuff = this.debuff;
                break;
        }
    }
}
[Serializable] public class LevelAllEnemy : ElementLevelEffect
{
    public string effectName = "所有敵人";
    public EnemyEffect enemyEffect;
    public DeBuff debuff;
    public float damage;
    public float backForce;
    public override void Apply()
    {
        foreach (Transform child in SpawnManager.Instance.transform)
        {
            switch (enemyEffect)
            {
                case EnemyEffect.DeBuff:  
                    if (child.TryGetComponent(out IDebuffable enemyDebuff))enemyDebuff.ChangeDeBuff(debuff);
                    break;
                case EnemyEffect.BackForce:  
                    if (child.TryGetComponent(out IDamageable enemyDamage))
                    {
                        enemyDamage.TakeDamage(damage, backForce, false);
                    }
                    break;
            }
        }
    } 
}
#endregion
#region 各種持續元素效果
[Serializable] public class ElementEffect
{
    [Header("時間間隔")]public float DelayTime;
    public virtual void Apply(){}
}

[Serializable] public class AddPlayerBuff : ElementEffect
{
    public string effectName = "玩家強化";
    public PlayerBuff buff;
    public float BuffAmount;
    public override void Apply()
    {
        switch (buff)
        {
            case PlayerBuff.Heal:
                GameManager.Instance.GetPlayer().HealthRecover(BuffAmount);
                break;
            case PlayerBuff.SpeedUp:
                GameManager.Instance.GetPlayer().speed += BuffAmount;
                break;
            case PlayerBuff.Small:
                if(GameManager.Instance.GetPlayer().transform.localScale.y <= 0.75f)return;
                GameManager.Instance.GetPlayer().transform.localScale *= 0.75f;
                GameManager.Instance.GetPlayer().GetComponent<Rigidbody2D>().mass *= 0.75f;
                break;
            case PlayerBuff.Heavier:
                GameManager.Instance.GetPlayer().GetComponent<Rigidbody2D>().mass *= 2f;
                break;
            case PlayerBuff.Bigger:
                GameManager.Instance.GetPlayer().GetComponent<Rigidbody2D>().mass *= 4f;
                GameManager.Instance.GetPlayer().transform.localScale *= 1.3f;
                break;
        }
    }
}
[Serializable] public class AddProjectile : ElementEffect
{
    public string effectName = "投射物生成";
    public ProjectileData ProjectileData;
    public override void Apply(){}
}
[Serializable] public class AddSomething : ElementEffect
{
    public string effectName = "道具生成";
    public float distance;
    public ItemDefault something;
    public override void Apply(){}
}
[Serializable] public class AddExp : ElementEffect
{
    public string effectName = "經驗值持續增加";
    public override void Apply()
    {
        ExpManager.Instance.AddExperience(ExpManager.Instance.level * 1);
    }
}
#endregion
#region 各種屬性BuffEnum
public enum ProjectileBuff
{
    DelayTime,
    Attack,
    Speed,
    BackForce,
    StayTime,
    Bigger,
    DeBuffChange,
    CompoundLevelUp
}
public enum PlayerBuff
{
   Heal,
   SpeedUp,
   Small,
   Heavier,
   Bigger,
   ItemRange
}
public enum EnemyEffect
{
   DeBuff,
   BackForce
}
#endregion
#region Editor
    #if UNITY_EDITOR
    [CustomEditor(typeof(ElementData))]
    public class ElementDataEditor : Editor
    {
        private SerializedProperty elementTypeProperty;
        private SerializedProperty compoundMaterialsProperty;

        private void OnEnable()
        {
            elementTypeProperty = serializedObject.FindProperty("elementType");
            compoundMaterialsProperty = serializedObject.FindProperty("_compoundMaterials");
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawPropertiesExcluding(serializedObject, new string[] { "_compoundMaterials" });
            ElementType elementType = (ElementType)elementTypeProperty.enumValueIndex;
            if (elementType == ElementType.Compound)
            {
                EditorGUILayout.PropertyField(compoundMaterialsProperty, true);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
    #endif
#endregion
