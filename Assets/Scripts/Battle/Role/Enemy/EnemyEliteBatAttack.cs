using UnityEngine;

public class EnemyEliteBatAttack : EnemyUnicorn_Attacker
{
    // [SerializeField]
    // private DmgBuffOnTouch dmgTouch;
    public DmgBuffOnTouch RotateAttackDmgOnTouch;
    public GameObject RotateAttackParticles;
    public AudioClip RotateAttacksfx;
    public AudioClip DashAttacksfx;
    public float RotateMoveSpeed;
    public float RotateMoveTime;

    private static readonly int Sprint = Animator.StringToHash("Sprint");

    protected override void Start()
    {
        base.Start();
        initDmgTouch();
    }

    public void AttackFunc(Vector3 dir, float dis, float attackMoveWarningTime)
    {
        base.AttackFunc();
        //攻击方式为0
        if (currentAttackStatus == 0)
        {
            if (roleController.IsDie)
            {
                return;
            }
            roleController.Animator.transform.forward = dir;
            var rect = IndicatorManager.Inst.ShowRectangleIndicator();
            rect.Show(roleController, new Vector3(0, 0.1f, 0), dir, new Vector3(1f, 1, 0), new Vector3(0.5f, 1, dis), attackMoveWarningTime);
        }
        //攻击方式为1
        else if(currentAttackStatus==1)
        {

        }
    }
    public void StartRotateAttack()
    {

    }
    protected override void AnimEvent(GameObject go, string eventName)
    {
        base.AnimEvent(go, eventName);
        if(go!= roleController.Animator.gameObject)
        {
            return;
        }
        if (eventName.Contains(AnimatorEventName.DmgStart_))
        {
            if (currentAttackStatus == 1)
            {
                if (RotateAttackParticles != null)
                {
                    //RotateAttackParticles.DuplicateUnderTransform(roleController.Animator.transform);
                    roleController.FastMove(RotateMoveTime,RotateMoveSpeed,roleController.Animator.transform.forward,null);
                    RotateAttackParticles.GetComponent<ParticleSystem>().Play();
                }
                if (RotateAttacksfx != null)
                {
                    AudioManager.Inst.PlaySource(RotateAttacksfx,1);
                }
                RotateAttackDmgOnTouch.gameObject.SetActive(true);
            }
            if(currentAttackStatus==0)
            {
                //DashAttacksfx.Play();
                //StartSprint();
            }
        }
        if(eventName.Contains(AnimatorEventName.DmgEnd_))
        {
            if(currentAttackStatus==1)
            {
                RotateAttackDmgOnTouch.gameObject.SetActive(false);
            }
        }
    }



    private void initDmgTouch()
    {
        dmgTouch.Init(roleController);
        dmgTouch.WZYInit();
        RotateAttackDmgOnTouch.Init(roleController);
        RotateAttackDmgOnTouch.WZYInit();
    }
}
