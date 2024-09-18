using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideTextTrigger : MonoBehaviour
{
    [SerializeField]
    private string textKey;
    [SerializeField]
    private bool isForce;
    public void OnTriggerEnter(Collider other)
    {
        var layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Player") || layer == LayerMask.NameToLayer("PlayerRoll"))
        {
            this.gameObject.SetActive(false);
            
            BattleGuide.Inst.StartGuide(new List<BattleGuideSequenceData>(){new BattleGuideSequenceData()
            {
                GuideType = BattleGuideType.Text,
                Force = isForce, 
                Text = textKey, 
                ShowTime = 1.5f 
            }});
            
        }
    }
}
