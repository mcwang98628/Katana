using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGuide : MonoBehaviour
{
    public static BattleGuide Inst { get; private set; }

    private void Awake()
    {
        Inst = this;
        AddEvent();
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    void AddEvent()
    {
        EventManager.Inst.AddEvent(EventName.OnRoleDead,OnRoleDead);
    }

    void RemoveEvent()
    {
        EventManager.Inst.RemoveEvent(EventName.OnRoleDead,OnRoleDead);
    }

    private void OnRoleDead(string arg1, object arg2)
    {
        //只有章节模式 教程 才执行。
        if (BattleManager.Inst.RuntimeData is ChapterRulesRuntimeData runtimeData && runtimeData.IsTutorial)
        {
            RoleDeadEventData deadData = (RoleDeadEventData) arg2;
            switch (deadData.DeadRole.roleTeamType)
            {
                case RoleTeamType.Player:
                    //TODO 玩家 死亡
                    return;
                case RoleTeamType.EliteEnemy:
                case RoleTeamType.Enemy:
                    if (BattleManager.Inst.CurrentRoom is FightRoom fightRoom && fightRoom.CheckRoomFinished())
                    {
                        //TODO 房间清空
                        int times = runtimeData.GetPassingRoomTimes(fightRoom.RoomType);
                        switch (times)
                        {
                            case 1:
                                
                                break;
                            case 2:
                                
                                break;
                        }
                    }
                    break; 
                case RoleTeamType.Enemy_Boss:
                    //TODO Boss 死亡
                    StartGuide(new List<BattleGuideSequenceData>()
                    {
                        new BattleGuideSequenceData(){ },//TODO
                        new BattleGuideSequenceData(){ },//TODO
                        new BattleGuideSequenceData(){ },//TODO
                    });
                    return; 
            } 
        }
    }

    private int index;
    private List<BattleGuideSequenceData> currentGuideList;
    private BattleGuideSequenceData currentGuideData => currentGuideList[index];
    public void StartRoomGuide(RoomType type, int times)
    {
        if (_showGuideCoroutine!=null)
        {
            BattleManager.Inst.StopCoroutine(_showGuideCoroutine);
        }

        if (_waitNextGuide != null)
        {
            BattleManager.Inst.StopCoroutine(_waitNextGuide);
        }
        
        UIManager.Inst.Close("ToturialPanel");
        
        index = 0;
        currentGuideList = new List<BattleGuideSequenceData>(); 

        if (type == RoomType.StartRoom && times == 1)
        {
            //currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = true, Text = "TutorialDialog_1_1", ShowTime = 2.5f });
            //currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = true, Text = "TutorialDialog_1_2", ShowTime = 2.5f });
            //currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = true, Text = "TutorialDialog_1_3", ShowTime = 2.5f });
            //currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = true, Text = "TutorialDialog_1_4", ShowTime = 2.5f });
            //currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = true, Text = "TutorialDialog_1_5", ShowTime = 2.5f });
            currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = false, Text = "TutorialDialog_1_6", ShowTime = 2.5f });
            currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.ToturialPanel, Force = false, Text = "Move", ShowTime = 4f });
        }
        else if (type == RoomType.StartRoom && times == 2)
        { 
            currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.MoveCamera, Force = true, Text = "CameraPoint", ShowTime = 4f });
            currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = true, Text = "TutorialDialog_2_1", ShowTime = 2.5f });
            currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = true, Text = "TutorialDialog_2_2", ShowTime = 2.5f });
            currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = false, Text = "TutorialDialog_2_3", ShowTime = 2.5f });
            currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.ToturialPanel, Force = false, Text = "Roll", ShowTime = 4f });
        }
        else if (type == RoomType.FightRoom && times == 1)
        { 
            currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.MoveCamera, Force = true, Text = "CameraPoint", ShowTime = 2.5f });
            //currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = false, Text = "TutorialDialog_3_1", ShowTime = 2.5f });
            currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = false, Text = "TutorialDialog_3_2", ShowTime = 2.5f });
            currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.ToturialPanel, Force = false, Text = "Click", ShowTime = 3f });
        }
        else if (type == RoomType.TreasureRoom && times == 1)
        { 
            currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.MoveCamera, Force = true, Text = "CameraPoint", ShowTime = 2.5f });
            //currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = true, Text = "TutorialDialog_4_1", ShowTime = 2.5f });
            currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = false, Text = "TutorialDialog_4_2", ShowTime = 2.5f });
            currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.ToturialPanel, Force = false, Text = "Click", ShowTime = 3f });
        }
        else if (type == RoomType.TreasureRoom && times == 2)
        {
            currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = false, Text = "TutorialDialog_4_Add", ShowTime = 2.5f });
        }
        else if (type == RoomType.FightRoom && times == 2)
        { 
            currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.WaitEnemyCanKill, Force = false, ShowTime = 0f });
            // currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = false, Text = "TutorialDialog_Skill", ShowTime = 2.5f });
            currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.ShowSkillBtn, Force = false, ShowTime = 0f });
            currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Focus, Force = true, Text = "SkillBtn_Joy", FocusTriggerType = FocusGuideData.TriggerType.Button ,ShowTime = 1});
            //currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = false, Text = "Guide_SkillTips", ShowTime = 7f });
            //currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = false, Text = "TutorialDialog_5_2", ShowTime = 2.5f });

        }
        else if (type == RoomType.ShopRoom && times == 1)
        { 
            currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.MoveCamera, Force = true, Text = "CameraPoint", ShowTime = 2.5f });
            //currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = true, Text = "TutorialDialog_6_1", ShowTime = 2.5f });
            //currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = true, Text = "TutorialDialog_6_2", ShowTime = 2.5f });
            currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = true, Text = "TutorialDialog_6_3", ShowTime = 2.5f });
            currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = false, Text = "TutorialDialog_6_4", ShowTime = 2.5f });
        }
        else if(type ==RoomType.BossFightRoom)
        { 
            
            currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = true, Text = "TutorialDialog_7_1", ShowTime = 2f });
            currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.MoveCamera, Force = true, Text = "CameraPoint", ShowTime = 2.5f });
            currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = true, Text = "TutorialDialog_7_2", ShowTime = 2f });
            currentGuideList.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = false, Text = "TutorialDialog_7_3", ShowTime = 2f });
            //TODO：击杀BOSS后的弹字
           
        } 

        if (currentGuideList == null || currentGuideList.Count == 0)
        {
            currentGuideList = null;
            return;
        }

        ShowGuide(currentGuideList[index]);
    }

    public void StartGuide(List<BattleGuideSequenceData> guideList)
    {
        
        if (_showGuideCoroutine!=null)
        {
            BattleManager.Inst.StopCoroutine(_showGuideCoroutine);
        }

        if (_waitNextGuide != null)
        {
            BattleManager.Inst.StopCoroutine(_waitNextGuide);
            _waitNextGuide = null;
        }
        
        index = 0;
        currentGuideList = guideList;
        ShowGuide(currentGuideList[index]);
    }

    public void NextGuide()
    {
        if (_waitNextGuide != null)
        {
            BattleManager.Inst.StopCoroutine(_waitNextGuide);
            _waitNextGuide = null;
        }

        OnNextGuide(currentGuideList[index-1]);
    }

    private Coroutine _showGuideCoroutine;
    private Coroutine _waitNextGuide;
    void ShowGuide(BattleGuideSequenceData sequenceData)
    {
        _showGuideCoroutine = BattleManager.Inst.StartCoroutine(ShowGuideCoroutine(sequenceData));
    }
    IEnumerator ShowGuideCoroutine(BattleGuideSequenceData sequenceData)
    {
        yield return null;
        while (BattleManager.Inst.CurrentPlayer == null)
        {
            yield return null;
        }

        while (!UIManager.Inst.PanelIsOpen("BattlePanel"))
        {
            yield return null;
        }
        
        if (sequenceData.Force)
        {
            BattleManager.Inst.CurrentPlayer.SetAcceptInput(false);
            EventManager.Inst.DistributeEvent(EventName.JoyUp);
            EventManager.Inst.DistributeEvent(EventName.HideJoy, true);
        }
        switch (sequenceData.GuideType)
        {
            case BattleGuideType.Text:
                EventManager.Inst.DistributeEvent(EventName.GuideDialog, sequenceData);
                break;
            case BattleGuideType.MoveCamera:
                var go = GameObject.Find(sequenceData.Text);
                BattleManager.Inst.CurrentPlayer.StopFastMove();
                BattleManager.Inst.CurrentCamera.LookAtPoint(go.transform.position, 1f, 1f,1f, 1f);
                break;
            case BattleGuideType.Focus:
                FocusGuide.Inst.DoGuide(new FocusGuideData(sequenceData.FocusTriggerType, sequenceData.Text));
                break;
            case BattleGuideType.ToturialPanel:
                ToturialGuideType guideType = (ToturialGuideType)Enum.Parse(typeof(ToturialGuideType), sequenceData.Text);
                UIManager.Inst.Open("ToturialPanel", false, guideType);
                break;
            case BattleGuideType.ShowSkillBtn:
                EventManager.Inst.DistributeEvent(EventName.ShowSkillBtn);
                break;
            case BattleGuideType.WaitEnemyCanKill:
                
                bool isCanKill = false;
                while (!isCanKill)
                {
                    foreach (var enemy in BattleManager.Inst.EnemyTeam)
                    {
                        if (!enemy.Value.IsDie &&
                            enemy.Value.CurrentHp <= (enemy.Value.MaxHp / (float) enemy.Value.roleHealth.HpBarCount))
                        {
                            isCanKill = true;
                            break;
                        }
                    }
                    yield return null;
                }
                break;
        }

        index++;

        if (sequenceData.Force && sequenceData.GuideType == BattleGuideType.Text) 
        {
            //强制文字。
        }
        else
        {
            _waitNextGuide = StartCoroutine(waitNextGuide(sequenceData));
        }
    }

    IEnumerator waitNextGuide(BattleGuideSequenceData sequenceData)
    { 
        yield return new WaitForSecondsRealtime(sequenceData.ShowTime);
        OnNextGuide(sequenceData);
    }

    void OnNextGuide(BattleGuideSequenceData sequenceData)
    {
        EventManager.Inst.DistributeEvent(EventName.HideJoy, false);
        if (sequenceData.Force)
        {
            BattleManager.Inst.CurrentPlayer.SetAcceptInput(true);
        }
        if (index < currentGuideList.Count)
        {
            ShowGuide(currentGuideList[index]); 
        }
        else
        {
            //Over
        }
    }

    List<BattleGuideSequenceData> GetTestGuide()
    {
        List<BattleGuideSequenceData> list = new List<BattleGuideSequenceData>();
        list.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = true, Text = "ABCDEF", ShowTime = 2f });

        list.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.MoveCamera, Force = true, Text = "CameraPoint", ShowTime = 2f });

        list.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Text, Force = true, Text = "ABCD12321321312321EF", ShowTime = 2f });

        list.Add(new BattleGuideSequenceData() { GuideType = BattleGuideType.Focus, Force = true, Text = "SkillBtn_Joy", FocusTriggerType = FocusGuideData.TriggerType.Button });

        return list;
    } 

}

public enum BattleGuideType
{
    Text,//文字 对话框
    MoveCamera,//移动相机
    Focus,//UI 焦点强制引导 -》 大黑圈
    ToturialPanel,//旧 教程界面
    ShowSkillBtn,//显示技能按钮
    WaitEnemyCanKill,//等待敌人可以被斩杀

}
public class BattleGuideSequenceData
{
    public BattleGuideType GuideType;
    //持续多久
    public float ShowTime;
    //是否为强制
    public bool Force;

    //Text： 文字内容
    //MoveCamera：对象Object名字
    //Focus：UI组件的Object名字，
    //ToturialPanel：ToturialGuideType文字T
    public string Text;

    public Color TextColor = Color.white;
    //Focus类型专用
    public FocusGuideData.TriggerType FocusTriggerType;
}