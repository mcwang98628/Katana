//
//
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// public partial class ItemEffectEffectData
// {
//     [ShowIf("EffectType", EffectType.FeverEffect)]
//     [LabelText("最大能量")]
//     public float FeverMaxPower;
//     [ShowIf("EffectType", EffectType.FeverEffect)]
//     [LabelText("每次恢复能量数")]
//     public float FeverRecoveryPower;
//     [ShowIf("EffectType", EffectType.FeverEffect)]
//     [LabelText("每秒消耗能量")]
//     public float FeverConsumePower;
// }
// public class FeverEffect:ItemEffect
// {
//     public float MaxPower => _maxPower;
//     public float CurrentPower => _currentPower;
//
//     private float _maxPower;
//     private float _recoveryPower;
//     private float _feverConsumePower;//每秒消耗
//
//     private float _currentPower;
//     private bool _fevering;
//     
//     public FeverEffect(float maxPower,float recoveryPower,float feverConsumePower)
//     {
//         _maxPower = maxPower;
//         _recoveryPower = recoveryPower;
//         _feverConsumePower = feverConsumePower;
//         _fevering = false;
//         _currentPower = 0;
//     }
//
//     public override void TriggerEffect(ItemEffectTriggerValue? value)
//     {
//         if (_fevering)
//             return;
//         
//         base.TriggerEffect(value);
//         _currentPower += _recoveryPower;
//         if (_currentPower >= _maxPower)
//         {
//             _fevering = true;
//             _currentPower = _maxPower;
//             roleController.SetFever(true);
//         }
//     }
//
//     public override void Update(RoleItemController rpe)
//     {
//         base.Update(rpe);
//         if (_fevering)
//         {
//             _currentPower -= _feverConsumePower * Time.deltaTime;
//             if (_currentPower<=0)
//             {
//                 _fevering = false;
//                 _currentPower = 0;
//                 roleController.SetFever(false);
//             }
//         }
//     }
// }