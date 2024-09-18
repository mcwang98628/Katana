using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EquipmentObject : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem light;
    [SerializeField]
    private ParticleSystem glow;
    [SerializeField]
    private GameObject getFx;
    [SerializeField]
    private Text3D equipName;
    [SerializeField]
    private SpriteRenderer EquipmentIcon;
    [SerializeField]
    private SpriteRenderer BackGround;

    private Equipment _equipment;

    private float setTime;
    //设置装备
    public void SetEquipment(Equipment equipment)
    {
        _equipment = equipment;
        equipment.LoadIcon(delegate (Sprite sprite)
        {
            EquipmentIcon.sprite = sprite;
            BackGround.color = GetColor();
            //icon.color = Color.white;
        }); ;
        setTime = Time.time;
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.4f);
        light.startColor = GetColor();
        glow.startColor = GetColor();
        equipName.text = equipment.Name;
        BattleManager.Inst.RuntimeData.GetEquipmentList.Add(_equipment);
    }
    //测试。
    //private void Start()
    //{
    //    var equipment = EquipmentTool.RandomScoreEquipmentById(1);
    //    //var equipGo = GameObject.Instantiate(prefab).GetComponent<EquipmentObject>();
    //    SetEquipment(equipment);
    //}
    private float moveTimer = 0;
    private void Update()
    {
        if (_equipment == null || Time.time - setTime < 2.5f)
            return;
        moveTimer += Time.deltaTime;
        if (moveTimer>0.6f)
            moveTimer = 0.6f;
        
        var playerPos = BattleManager.Inst.CurrentPlayer.transform.position;
        var pos = Vector3.Lerp(transform.position, playerPos, moveTimer / 1.6f);
        transform.position = pos;

        if ((playerPos - transform.position).magnitude < 0.5f)
        {
            var fx = GameObject.Instantiate(getFx,transform.position+new Vector3(0,0.5f,0),transform.rotation);
            fx.gameObject.SetActive(true);
            var fxs = fx.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem particleSystem in fxs)
            {
                particleSystem.startColor = GetColor();
                particleSystem.Play();
            }
            Destroy(this.gameObject);
        }
    }

    Color GetColor()
    {
        switch (_equipment.Quality)
        {
            case EquipmentQuality.Lv1:
                return EquipmentColor.Lv1Color ;
                break;
            case EquipmentQuality.Lv2:
                return EquipmentColor.Lv2Color ;
                break;
            case EquipmentQuality.Lv3:
                return EquipmentColor.Lv3Color ;
                break;
            case EquipmentQuality.Lv4:
                return EquipmentColor.Lv4Color ;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
