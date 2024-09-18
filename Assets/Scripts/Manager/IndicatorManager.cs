using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class IndicatorManager : MonoBehaviour
{
    public static IndicatorManager Inst { get; private set; }
    [FormerlySerializedAs("attackIndeicateSector")] [FormerlySerializedAs("rectangleIndicator")] [SerializeField]
    private AttackIndeicate_Rect attackIndeicateRect;
    List<AttackIndeicate_Rect> rectangleIndicatorList = new List<AttackIndeicate_Rect>();
    [FormerlySerializedAs("attackIndicator")] [SerializeField]
    private AttackIndeicate_Sector attackIndeicateSector;
    List<AttackIndeicate_Sector> AttackIndicatorList = new List<AttackIndeicate_Sector>();
    [HideInInspector]
    public Transform CurrentChoosenTarget;
    [SerializeField]
    private TargetIndicator targetIndicator;
    [HideInInspector]
    private FindTarget _findTarget;
    public void Init()
    {
        Inst = this;
    }

    void SetTargetIndicator(Transform target, Color targetColor)//,Vector3 indicatorScale)
    {
        ShowTargetIndicator();
        targetIndicator.SetTarget(target);
        targetIndicator.SetColor(targetColor);
        CurrentChoosenTarget = target;
        //targetIndicator.SetScale(indicatorScale);
    }
    void ShowTargetIndicator()
    {
        targetIndicator.Show();
    }
    void HideTargetIndicator()
    {
        targetIndicator.Hide();
    }

    public AttackIndeicate_Rect ShowRectangleIndicator()
    {
        AttackIndeicate_Rect indicator = null;
        if (rectangleIndicatorList.Count > 0)
        {
            indicator = rectangleIndicatorList[0];
            indicator.transform.SetParent(transform);
            rectangleIndicatorList.RemoveAt(0);
        }

        if (indicator == null)
        {
            indicator = Instantiate(attackIndeicateRect, transform);
        }
        return indicator;
    }

    public void Recover(AttackIndeicate_Rect indicator)
    {
        if (indicator == null || indicator.gameObject == null)
        {
            return;
        }
        indicator.ClearTarget();
        rectangleIndicatorList.Add(indicator);
        indicator.gameObject.SetActive(false);
    }

    public AttackIndeicate_Sector ShowAttackIndicator()
    {
        AttackIndeicate_Sector indeicateSector = null;
        if (AttackIndicatorList.Count > 0)
        {
            indeicateSector = AttackIndicatorList[0];
            //indeicateSector.transform.SetParent(transform);
            AttackIndicatorList.RemoveAt(0);
        }

        if (indeicateSector == null)
        {
            indeicateSector = Instantiate(attackIndeicateSector.gameObject, transform).GetComponent<AttackIndeicate_Sector>();
        }
        return indeicateSector;
    }

    public void Recover(AttackIndeicate_Sector indeicateSector)
    {
        AttackIndicatorList.Add(indeicateSector);
        indeicateSector.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.Inst==null)
        {
            return;
        }
        UpdateTargetIndicate();
    }

    void UpdateTargetIndicate()
    {
        if (_findTarget == null)
        {
            if (BattleManager.Inst == null || BattleManager.Inst.CurrentPlayer == null)
            {
                return;
            }
            else
            {
                _findTarget = BattleManager.Inst.CurrentPlayer.GetComponent<FindTarget>();
            }
        }
        if (_findTarget == null)
        {
            return;
        }

        RoleController enemyTarget = _findTarget.EnemyTarget;
        InteractObj interactTarget = _findTarget.InteractTarget;
        BreakableObj breakableTarget = _findTarget.BreakableTarget;

        if (enemyTarget != null)
        {
            SetTargetIndicator(enemyTarget.transform, Color.red);
        }
        else if (interactTarget != null)
        {
            SetTargetIndicator(interactTarget.transform, Color.green);
        }
        else if (breakableTarget != null)
        {
            SetTargetIndicator(breakableTarget.transform, Color.yellow);
        }
        else
        {
            HideTargetIndicator();
        }
    }
}
