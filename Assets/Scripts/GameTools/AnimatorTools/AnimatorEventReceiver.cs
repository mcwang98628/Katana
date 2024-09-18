using System;
using System.Collections.Generic;
using UnityEngine;

namespace AnimatorTools
{
    public class AnimatorEventReceiver : MonoBehaviour
    {
        [SerializeField]
        private List<AnimatorEventReceiverData> events = new List<AnimatorEventReceiverData>();

        private bool AddRemoveInActive = true;

        private void OnEnable()
        {
            if (AddRemoveInActive)
            {
                foreach (var data in events)
                {
                    EventManager.Inst.AddEvent(data.eventName, eventFunc);
                }
            }
        }

        private void OnDisable()
        {
            if (AddRemoveInActive)
            {
                foreach (var data in events)
                {
                    EventManager.Inst.RemoveEvent(data.eventName, eventFunc);
                }
            }
        }

        void eventFunc(string _eventName,object value)
        {
            foreach (var eventRec in events)
            {
                if (eventRec.eventName == _eventName)
                {
                    eventRec.onTriggerEvent.Invoke();
                }
            }
        }

    }


    [Serializable]
    public class AnimatorEventReceiverData
    {
        public string eventName;
        public UnityEngine.Events.UnityEvent onTriggerEvent;
    }
}