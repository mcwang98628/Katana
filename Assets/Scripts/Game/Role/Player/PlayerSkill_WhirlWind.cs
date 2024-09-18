using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_WhirlWind : PlayerSkill
{
    [Header("攻击风刃")]
    public DmgBuffOnTouch WaveObj;
    public float WaveDamageMutiply = 0.5f;
    [Header("剑刃风暴风刃")]
    public DmgBuffOnTouch BladeStormWaveObj;
    public float BladeStormWaveDamageMutiply = 0.5f;
    
    void SpwanWind()
    {
        if (!WaveObj)
            return;
        
        var Instance=Instantiate(WaveObj,WaveObj.transform.position,WaveObj.transform.rotation);
        Instance.transform.SetParent(roleController.transform);
        Instance.gameObject.SetActive(true);
        Instance.Init(roleController,roleController.AttackPower*WaveDamageMutiply);
    }

    private DmgBuffOnTouch bladeStormWaveObjInstance;
    void StartBladeSormVortex()
    {
        if (!BladeStormWaveObj)
            return;
        
        bladeStormWaveObjInstance=Instantiate(BladeStormWaveObj,BladeStormWaveObj.transform.position,BladeStormWaveObj.transform.rotation);
        bladeStormWaveObjInstance.gameObject.SetActive(true);
        bladeStormWaveObjInstance.transform.SetParent(roleController.transform);
        bladeStormWaveObjInstance.Init(roleController,roleController.AttackPower*BladeStormWaveDamageMutiply);
    }
    void EndBladeSormVortex()
    {
        if (!bladeStormWaveObjInstance)
            return;
        bladeStormWaveObjInstance.DestroyObj();
    }

    protected override void AnimEvent(GameObject go, string eventName)
    {
        if (go != roleController.Animator.gameObject)
        {
            return;
        }

        if (eventName.Contains("Katana_SpawnWindWave_AtkAfterRoll"))
        {
            SpwanWind();
        }
         else if (eventName.Contains("Katana_SpawnWindWave_Atk3"))
        {                    
             if(roleColorLevel>=2)
                SpwanWind();
        }
        else if (eventName.Contains("Katana_StartBladeStromVortex"))
        {
            if(roleColorLevel>=3)
                StartBladeSormVortex();
        }
        else if (eventName.Contains("Katana_EndBladeStromVortex"))
        {
            if(roleColorLevel>=3)
                EndBladeSormVortex();
        }
    }
}