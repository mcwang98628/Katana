using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class ThrowProjectile : MonoBehaviour
{
    [SerializeField][LabelText("可以弹反的时间 (距离爆炸的时间)")]
    private float canBackTime = 1f;
    
    [SerializeField][LabelText("是否显示爆炸范围")]
    private bool ShowDamageArea=true;
    RoleController owner;
    Vector3 desPosition = Vector3.zero;
    float moveTime = 1;
    float maxHeight = 3;
    GameObject spwanOnExplode;
    float damage;
    Vector3 hPos;
    float vPos;
    

    private void Awake()
    {
        BattleManager.Inst.AddBattleThrow(this);
    }

    private void OnDestroy()
    {
        BattleManager.Inst.RemoveBattleThrow(this);
    }
    private void Update()
    {
        if (maxHeight < 0)
        {
            return;
        }
        transform.position = new Vector3(hPos.x, vPos, hPos.z);
        transform.Rotate(new Vector3(0, 0, 500 * Time.deltaTime));
    }

    private float explodeTime;
    public void Back()
    {
        if (explodeTime - Time.time > canBackTime)
        {
            return;
        }
        Init(owner.transform.position,moveTime,maxHeight,spwanOnExplode,spwanOnExplode.GetComponent<DmgBuffOnTouch>().radius,damage,owner);
        BattleManager.Inst.RemoveBattleThrow(this);
    }
    
    void Explode()
    {
        if (spwanOnExplode)
        {
            if (spwanOnExplode.GetComponent<DmgBuffOnTouch>() != null)
            {
                Vector3 pos = transform.position;
                pos.y=0.05f;//这个值需要介于表面和红圈上面
                DmgBuffOnTouch exp = Instantiate(spwanOnExplode, transform.position, Quaternion.identity).GetComponent<DmgBuffOnTouch>();
                exp.Init(owner, damage);
                //exp.Damage.DmgValue = damage;
            }
            //Instantiate(spwanOnExplode, transform.position, Quaternion.identity).GetComponent<DamageOnTouch>().SetDmg(damage);
        }

        Destroy(gameObject);
    }

    private Tweener tweener1;
    private Tweener tweener2;
    private Tweener tweener3;
    private AttackIndeicate_Sector _sector;
    public void Init(Vector3 _desPosition, float _moveTime, float _maxHeight, GameObject _spwanOnExplode, float range, float _damage, RoleController _owner)
    {
        desPosition = _desPosition;
        moveTime = _moveTime;
        maxHeight = _maxHeight;
        spwanOnExplode = _spwanOnExplode;
        damage = _damage;
        owner = _owner;
        explodeTime = Time.time + moveTime;
        DmgBuffOnTouch dmgTouch = spwanOnExplode.GetComponent<DmgBuffOnTouch>();
        dmgTouch.radius=range;
        
        if(ShowDamageArea)
        {
        Color color;
        if (dmgTouch.DamageTarget==DmgTarget.All || dmgTouch.DamageTarget == DmgTarget.Player)
            color = Color.red;
        else
            color = Color.green;
        
        if (_sector!=null)
        {
            _sector.Recover();
        }
        _sector = IndicatorManager.Inst.ShowAttackIndicator();
        _sector.Show(_owner, desPosition, 360, dmgTouch.radius, _moveTime + 0.1f, color);
        }
        


        Vector3 Pos=transform.position;

        hPos = transform.position;
        vPos = transform.position.y;
        if (tweener1!=null)
        {
            tweener1.Kill(false);
            tweener2.Kill(false);
            tweener3.Kill(false);
        }
        tweener1 = DOTween.To(() => hPos, x => hPos = x, desPosition, moveTime).SetEase(Ease.Linear).SetUpdate(true);
        
        tweener2 = DOTween.To(() => vPos, x => vPos = x, maxHeight, moveTime / 2);
        tweener1.SetUpdate(false);
        tweener2.SetUpdate(false);
        tweener2.SetEase(Ease.OutQuad);
        tweener2.OnComplete(() =>
        {
            tweener3 = DOTween.To(() => vPos, x => vPos = x, 0, moveTime / 2);
            tweener3.SetUpdate(false);
            tweener3.SetEase(Ease.InQuad);
            tweener3.OnComplete(() => Explode());
        });
    }
    
}
