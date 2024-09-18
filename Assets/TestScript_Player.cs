using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XftWeapon;
public class TestScript_Player : MonoBehaviour
{
    public bool EmmitSwordBeam;
    public XWeaponTrail trail;
    public RoleController roleController;
    public List<ParticleSystem> FeverParticles;
    public GameObject SwordTrail;
    public Transform StartPoint;
    private void Start()
    {
        roleController = GetComponent<RoleController>();
        FeverSwitch(true);
        // roleController.FeverChangeEvent += FeverSwitch;
    }
    private void OnDestroy()
    {
        // roleController.FeverChangeEvent -= FeverSwitch;
    }
    public void FeverSwitch(bool isfever)
    {
        //释放粒子。
        if(isfever)
        {
            for(int i=0;i<FeverParticles.Count;i++)
            {
                FeverParticles[i].Play();
            }
        }
    }

    private void OnEnable()
    {
        EventManager.Inst.AddAnimatorEvent(AnimEvent);
    }
    private void OnDisable()
    {
        EventManager.Inst.RemoveAnimatorEvent(AnimEvent);
    }
    protected  void AnimEvent(GameObject go, string eventName)
    {
        if (go != roleController.Animator.gameObject)
        {
            return;
        }
        if (eventName.Contains(AnimatorEventName.StartAttack_))
        {
            //trail.StopSmoothly(0f);
        }
        if (eventName.Contains(AnimatorEventName.EndAttack_))
        {
            //trail.Activate();
        }
        if (eventName.Contains(AnimatorEventName.DmgStart_))
        {
            if (EmmitSwordBeam)
            {
                GameObject SwordBeam = Instantiate(SwordTrail, StartPoint.transform.position, Quaternion.identity);
                SwordBeam.transform.forward = roleController.Animator.transform.forward;
            }
        
        }
    }
}
