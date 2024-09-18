using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleOverPanel_SoundManager : MonoBehaviour
{
   public List<AudioClip> SoundList=new List<AudioClip>();
   
   public void PlaySound(int index)
   {
      if (index>=SoundList.Count)
      {
         Debug.LogError("音频数组越界！");
         return;
      }

      AudioManager.Inst.PlaySource(SoundList[index]);
   }
   
}
