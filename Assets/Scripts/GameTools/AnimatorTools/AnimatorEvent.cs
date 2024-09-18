using System.Collections.Generic;
using UnityEngine;

namespace AnimatorTools
{
    public class AnimatorEvent : StateMachineBehaviour
    {
        [System.Serializable]
        public class AnimatorEventTrigger
        {
            public enum AnimatorEventTriggerType
            {
                NormalizedTime,
                EnterState,
                ExitState,
                NormalizedTimeOrExitState,
            }

            public GameObject _gameObject;
            public string eventName = "Event Name";
            public AnimatorEventTriggerType eventTriggerType = AnimatorEventTriggerType.NormalizedTime;
            public float normalizedTime;
            private int _loopCount;

            public int TriggerEventTimes => triggerEventTimes;

            private int triggerEventTimes = 0;

            public void ClearTriggerTimes()
            {
                _loopCount = 0;
                triggerEventTimes = 0;
            }
            public void UpdateEventTrigger(float normalizedTime)
            {
                var normalizedTimeClamped = Mathf.Clamp(normalizedTime, 0, _loopCount + 1f);
                if (normalizedTimeClamped >= _loopCount + this.normalizedTime)
                {
                    TriggerEvent();
                    _loopCount++;
                }
            }
            public void TriggerEvent()
            {
                EventManager.Inst.DistributeAnimatorEvent(_gameObject,eventName);
                triggerEventTimes++;
            }
            public void Init()
            {
                _loopCount = 0;
            }
        }
        
        public List<AnimatorEventTrigger> eventTriggers;


        protected bool HasNormalizedEvents;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            for (int i = 0; i < eventTriggers.Count; i++)
            {
                eventTriggers[i]._gameObject = animator.gameObject;
                if (eventTriggers[i].eventTriggerType == AnimatorEventTrigger.AnimatorEventTriggerType.EnterState)
                {
                    eventTriggers[i].TriggerEvent();
                }
                else if (eventTriggers[i].eventTriggerType == AnimatorEventTrigger.AnimatorEventTriggerType.NormalizedTime)
                {
                    HasNormalizedEvents = true;
                    eventTriggers[i].Init();
                    eventTriggers[i].UpdateEventTrigger(stateInfo.normalizedTime);
                }
                else if (eventTriggers[i].eventTriggerType ==
                         AnimatorEventTrigger.AnimatorEventTriggerType.NormalizedTimeOrExitState)
                {
                    eventTriggers[i].ClearTriggerTimes();
                }
            }
        }
  
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!stateInfo.loop && stateInfo.normalizedTime > 1 || !HasNormalizedEvents) return;
            for (int i = 0; i < eventTriggers.Count; i++)
            {
                if (eventTriggers[i].eventTriggerType == AnimatorEventTrigger.AnimatorEventTriggerType.NormalizedTime)
                {
                    eventTriggers[i].UpdateEventTrigger(stateInfo.normalizedTime);
                }
                else if (eventTriggers[i].eventTriggerType ==
                                         AnimatorEventTrigger.AnimatorEventTriggerType.NormalizedTimeOrExitState && 
                                         eventTriggers[i].TriggerEventTimes <= 0)
                {
                    eventTriggers[i].UpdateEventTrigger(stateInfo.normalizedTime);
                }
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            for (int i = 0; i < eventTriggers.Count; i++)
            {
                if (eventTriggers[i].eventTriggerType == AnimatorEventTrigger.AnimatorEventTriggerType.ExitState)
                    eventTriggers[i].TriggerEvent();
                if (eventTriggers[i].eventTriggerType ==
                    AnimatorEventTrigger.AnimatorEventTriggerType.NormalizedTimeOrExitState && 
                    eventTriggers[i].TriggerEventTimes <= 0)
                {
                    eventTriggers[i].TriggerEvent();
                }
                // else if (eventTriggers[i].eventTriggerType == AnimatorEventTrigger.AnimatorEventTriggerType.NormalizedTime)
                //     eventTriggers[i].UpdateEventTrigger(stateInfo.normalizedTime);
            }
        }
    }
}
