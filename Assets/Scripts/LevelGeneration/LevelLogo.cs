using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Level Theme Asset ")]
public class LevelLogo : ScriptableObject
{
    [Space]
    [Space]
    [Space]
    [Header("Logo")]
    public GameObject StartRoom;
    public GameObject FightRoom;
    public GameObject EliteFightRoom;
    public GameObject SpecialFightRoom;
    public GameObject BossFightRoom;
    public GameObject AwardRoom;
    public GameObject EventRoom;
    public GameObject ShopRoom;

    [Space]
    public GameObject Attribute_Attack;
    public GameObject Attribute_MaxHp;
    public GameObject Attribute_Speed;
    public GameObject Money;
    public GameObject Potion;
}
