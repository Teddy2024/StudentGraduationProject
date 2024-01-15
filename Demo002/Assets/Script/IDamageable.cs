using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public interface IDamageable 
{
    void TakeDamage(float damage, float knockbackForce, bool isDeBuff);
}

public interface IPickupable 
{
    void OnPickUp();
    void GetItem();
}

public interface IDebuffable 
{
    void ChangeDeBuff(DeBuff debuff);
}

public static class Extensions
{
    public static Dictionary<ElementData, int> ToElementDic(this List<ElementData> ElementList)
    {
        return ElementList.GroupBy(element => element)
        .ToDictionary(group => group.Key, group => group.Count());
    }
}
