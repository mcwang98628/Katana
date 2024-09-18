using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PickAbleObj : MonoBehaviour
{
    float DropRange = 1f;
    public float RotateSpeed = 8;
    Vector3 dropPoint;
    public AudioClip DropSound;
    public AudioClip PickSound;
    public ParticleSystem PickParticle;
    public FeedBackObject PickFeedback;
    public bool CollectOnTouch = false;
    PlayerController player;
    bool canCollect = false;
    bool isCollecting = false;
    protected virtual void  Start()
    {
        Vector3 randOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * DropRange;
        dropPoint = transform.position + randOffset;
        dropPoint.y = 0f;
        transform.position = transform.position + randOffset * 0.2f;
        transform.eulerAngles = new Vector3(0, 1, 0) * Random.Range(0, 360);
        EventManager.Inst.AddEvent(EventName.CollectRoomProps, Collect);
        player = BattleManager.Inst.CurrentPlayer;
        Drop();

    }
    void FixedUpdate()
    {
        
        if (canCollect && CollectOnTouch)
        {
            if (player != null && Vector3.Distance(player.transform.position, transform.position) < 1f)
            {
                Collect();
            }
        }
        
        if(RotateSpeed!=0)
            transform.Rotate(new Vector3(0, RotateSpeed, 0));
    }

    void Drop()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveY(2f, 0.3f).SetEase(Ease.OutQuad));
        sequence.Append(transform.DOMoveY(0, 0.3f).SetEase(Ease.InQuad));
        sequence.Insert(0, transform.DOMoveX(dropPoint.x, 0.6f).SetEase(Ease.Linear));
        sequence.Insert(0, transform.DOMoveZ(dropPoint.z, 0.6f).SetEase(Ease.Linear));
        sequence.OnComplete(() =>
        {
            if (DropSound)
                AudioManager.Inst.PlaySource(DropSound, 1);
        });

        /*
        transform.DOMoveY(2f, 0.1f).SetEase(Ease.OutQuad).OnComplete(() =>
           {
               transform.DOMove(dropPoint, 0.2f).SetEase(Ease.InQuad).OnComplete(() =>
                 {
                     if (DropSound)
                         AudioManager.Inst.PlaySource(DropSound, 1);
                     
                 });
           });
           */
        StartCoroutine(waitSetCanColect(1f));
    }
    IEnumerator waitSetCanColect(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canCollect = true;

    }
    void Collect(string eventName, object obj)
    {
        Collect();
    }

    public void Collect()
    {
        EventManager.Inst.RemoveEvent(EventName.CollectRoomProps, Collect);
        float randomMoveTime = Random.Range(0.02f, 1f);
        transform.DOMoveY(0.7f, randomMoveTime).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            //isCollecting = true;
            Vector3 des = BattleManager.Inst.CurrentPlayer.transform.position + new Vector3(0, 1, 0);
            transform.DOMove(des, 0.12f).SetEase(Ease.Linear).OnComplete(() =>
            {
                TakeEffect();
            });
        });
    }
    
    protected virtual void TakeEffect()
    {
        if (PickFeedback)
            FeedbackManager.Inst.UseFeedBack(BattleManager.Inst.CurrentPlayer.GetComponent<RoleController>(), PickFeedback);
        if (PickSound)
            AudioManager.Inst.PlaySource(PickSound, 1);
        if (PickParticle)
        {
            GameObject PickPar = Instantiate(PickParticle.gameObject, BattleManager.Inst.CurrentPlayer.transform.position, PickParticle.transform.rotation);
            PickPar.transform.SetParent(BattleManager.Inst.CurrentPlayer.transform);
        }
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.CollectRoomProps, Collect);
    }
}
