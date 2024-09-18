
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class ItemEffectEffectData
{
    [ShowIf("EffectType", EffectType.KnifeWing)]
    [LabelText("KnifeWing物体")]
    public Bullet KnifeWingObject;
    [ShowIf("EffectType", EffectType.KnifeWing)]
    [LabelText("数量")]
    public int KnifeWingCount;
    [ShowIf("EffectType", EffectType.KnifeWing)]
    [LabelText("Offset")]
    public float KnifeWingOffset;
}
//刀翼
public class KnifeWing:ItemEffect
{
    private Bullet _bullet;
    private int _count;
    private float _offset;

    public KnifeWing(Bullet bullet,int count,float offset)
    {
        this._bullet = bullet;
        this._count = count;
        this._offset = offset;
    }
    
    public override void TriggerEffect(ItemEffectTriggerValue? value)
    {
        base.TriggerEffect(value);
        roleController.StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        List<Bullet> bullets = new List<Bullet>();
        for (int i = 0; i < _count; i++)
        {
            float xOffset = 0.5f - i/(float)_count * 3f;
            var bulletGo = GameObject.Instantiate(_bullet);
            var forward = roleController.Animator.transform.forward;
            bulletGo.Init(roleController,forward);
            var transform = bulletGo.transform;
            transform.position = 
                roleController.transform.position + 
                -forward * _offset + 
                Vector3.up + 
                roleController.Animator.transform.right * xOffset;
            
            transform.forward = forward;
            bulletGo.IsFlying = false;
            bullets.Add(bulletGo);
        }

        // float timer = 0;
        // while (timer < 0.3f)
        // {
            for (int i = 0; i < bullets.Count; i++)
            {
                float xOffset = (0.5f - i / (float) (bullets.Count-1)) * 3f;
                var transform = roleController.Animator.transform;
                bullets[i].transform.position = 
                    roleController.transform.position + 
                    -transform.forward * _offset * (1+(1f-Mathf.Abs(xOffset)))  + 
                    Vector3.up + 
                    transform.right * xOffset;
                
                bullets[i].transform.forward = transform.forward;
            }
            
        yield return null;
        //     timer += Time.deltaTime;
        // }
        
        foreach (Bullet bullet in bullets)
            bullet.IsFlying = true;
    }
    
    
    
}