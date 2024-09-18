using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneProcedure : IProcedure
{

    public void Start()
    {
        
        ResourcesManager.Inst.GetAsset<AudioClip>("Assets/AssetsPackage/Music/FightBGM.mp3", delegate(AudioClip clip)
        {
            AudioManager.Inst.PlayBGM(clip,0.7f);
        });
        
    }

    public void End()
    {
        BattleManager.Inst.EndBattle();
    }

    public void Update()
    {
        
    }
}

public class GuideProcedure : IProcedure
{
    public void Start()
    {
    }

    public void End()
    {
        
    }

    public void Update()
    {
        
    }
}