using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 流程管理器
/// </summary>
public class ProcedureManager : MonoBehaviour
{
    public static ProcedureManager Inst { get; private set; }
    /// <summary>
    /// 当前流程
    /// </summary>
    public IProcedure CurrentProcedure { get; private set; }

    public void Init()
    {
        Inst = this;
    }

    public void StartProcedure(IProcedure procedure)
    {
        if (CurrentProcedure != null)
        {
            if (procedure.GetType() == CurrentProcedure.GetType())
            {
                Debug.LogError("Error：相同流程转跳--》" + procedure.GetType());
                return;
            }

            CurrentProcedure.End();
        }
        CurrentProcedure = procedure;
        CurrentProcedure.Start();
    }

    private void Update()
    {
        if (CurrentProcedure != null)
        {
            CurrentProcedure.Update();
        }
    }
}
