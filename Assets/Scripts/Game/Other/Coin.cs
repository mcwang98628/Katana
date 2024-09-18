using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Coin : MonoBehaviour
{
    enum CoinStatus
    {
        Dispersion,
        Aggregation
    }
    public FeedBackObject PickCoinFeedback;
    [SerializeField]
    private AudioClip ac;
    private CoinStatus currentStatus;
    Transform model;
    bool isInCollect=false;
    Vector3 moveDirection;
    [SerializeField]
    private float rotateTime = 1f;
    [SerializeField]
    private AnimationCurve rotateCurve;
    [SerializeField]
    private float dispersionTime = 0.1f;
    [SerializeField]
    private AnimationCurve dispersionCurve;
    private float timer = 0f;
    private float speed = 0;
    void Start()
    {
        model = transform.GetChild(0);
        moveDirection = new Vector3(Random.Range(-1f,1.1f),Random.Range(0f,1.1f),Random.Range(-1f,1.1f));
        timer = 0f;
        speed = 14f;
        currentStatus = CoinStatus.Dispersion;
    }

    private void Update()
    {
        model.Rotate(Vector3.up * Time.deltaTime*400);
        if (isInCollect)
        {
            updateStatus();
        }
    }

    public void Collect()
    {
        isInCollect = true;
        moveDirection = new Vector3(Random.value, 0, Random.value).normalized;
    }

    void updateStatus()
    {
        float value;
        float speedMagnification;
        switch (currentStatus)
        {
            case CoinStatus.Dispersion:                          
                timer += Time.deltaTime;
                value = timer / dispersionTime;
                if (value>1f)
                {
                    value = 1f;
                }

                speedMagnification = dispersionCurve.Evaluate(value);
                if (timer>dispersionTime && currentStatus == CoinStatus.Dispersion)
                {
                    timer = 0;
                    currentStatus = CoinStatus.Aggregation;
                }
                transform.position += speed * speedMagnification * Time.deltaTime * moveDirection;
                
                break;
            case CoinStatus.Aggregation:                                
                timer += Time.deltaTime;
                value = timer / rotateTime;
                if (value>1f)
                {
                    value = 1f;
                }
                speedMagnification = rotateCurve.Evaluate(value);
                var targetPosition = BattleManager.Inst.CurrentPlayer.transform.position + new Vector3(0,1f,0);
                var position = transform.position;
                float dis = Vector3.Distance(targetPosition, position);
                var v3 = (targetPosition - position).normalized;
                moveDirection = Vector3.Lerp(moveDirection, v3, value).normalized;
                if (dis < 0.22f)
                {
//                    BattleManager.Inst.CurrentPlayer.GetComponent<CoinCollect>().AddCoin(1);
                    BattleManager.Inst.AddGold(1);
//TODO 硬币
                    if (ac!=null)
                    {
                        AudioManager.Inst.PlaySource(ac,0.5f);
                    }
                    Destroy(gameObject);
                    if(PickCoinFeedback!=null)
                    FeedbackManager.Inst.UseFeedBack(BattleManager.Inst.CurrentPlayer.GetComponent<RoleController>(),PickCoinFeedback);
                }
                transform.position += speed * speedMagnification * Time.deltaTime * moveDirection;
                
                break;
        }
    }

    
}
