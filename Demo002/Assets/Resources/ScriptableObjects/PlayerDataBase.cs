using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "ScriptableObject/Player", order = 0)]
public class PlayerDataBase : PlayerData
{
    public GameObject prefab;
    public Gender gender;
    public Sprite HeadIcon;
    public ElementData[] PlayerElementList;
}
[Serializable] public enum Gender
{
    Boy = 0,
    Girl = 1
};