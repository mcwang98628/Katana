using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AnimatorTools
{
    [CustomEditor(typeof(AnimatorEvent))]
    public class AnimatorEventEditor : UnityEditor.Editor
    {
        public AnimatorEvent animatorEvent;
        public GUISkin skin;
        public GUIStyle timeLineStyle;

        private void OnEnable()
        {
            animatorEvent = target as AnimatorEvent;
        }

        public override void OnInspectorGUI()
        {
            if (!skin) skin = Resources.Load("skin") as GUISkin;

            if (timeLineStyle == null) timeLineStyle = new GUIStyle(EditorStyles.inspectorFullWidthMargins);
            serializedObject.Update();
            EditorGUILayout.BeginVertical(skin.box);
            EditorGUILayout.HelpBox("事件时间是从0-1的标准化时间\n\n确保事件的时间在动画播放的时间里(注意动画切换直接的融合时间点).", MessageType.Info);
            var events = serializedObject.FindProperty("eventTriggers");
            if (GUILayout.Button("Add Event", skin.button, GUILayout.ExpandWidth(true)))
            {
                events.arraySize++;
                events.GetArrayElementAtIndex(events.arraySize - 1).FindPropertyRelative("eventName").stringValue = "New Event";
            }
            for (int i = 0; i < events.arraySize; i++)
            {
                if (!DraEvent(events, i)) break;
            }
            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }

        public bool DraEvent(SerializedProperty list, int index)
        {
            EditorGUILayout.BeginVertical(skin.box, GUILayout.ExpandWidth(true));
            {
                var eventName = list.GetArrayElementAtIndex(index).FindPropertyRelative("eventName");
                var eventTriggerType = list.GetArrayElementAtIndex(index).FindPropertyRelative("eventTriggerType");
               
                var normalizedTime = list.GetArrayElementAtIndex(index).FindPropertyRelative("normalizedTime");
                var width = 30;
                EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
                {
                    eventName.stringValue = EditorGUILayout.TextField(GUIContent.none, eventName.stringValue, GUILayout.ExpandWidth(true));
                    if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(width)))
                    {
                        list.DeleteArrayElementAtIndex(index);

                        return false;
                    }
                }
                EditorGUILayout.EndHorizontal(); 
                eventTriggerType.enumValueIndex = (int)(AnimatorEvent.AnimatorEventTrigger.AnimatorEventTriggerType)EditorGUILayout.EnumPopup("Type",(AnimatorEvent.AnimatorEventTrigger.AnimatorEventTriggerType)eventTriggerType.enumValueIndex);
                var valueSelected = (AnimatorEvent.AnimatorEventTrigger.AnimatorEventTriggerType)eventTriggerType.enumValueIndex;
                switch (valueSelected)
                {
                    case AnimatorEvent.AnimatorEventTrigger.AnimatorEventTriggerType.NormalizedTime:
                    case AnimatorEvent.AnimatorEventTrigger.AnimatorEventTriggerType.NormalizedTimeOrExitState:
                        Rect rect = GUILayoutUtility.GetLastRect();
                        GUILayout.Space(20);
                        var color = GUI.color;

                        rect.y += 20;
                        rect.width -= width;
                        GUI.Box(rect, "", skin.GetStyle("AnimatorEventBar"));
                        int number = 0;
                        for (int i = 0; i < 101; i++)
                        {
                            timeLineStyle.alignment = TextAnchor.UpperCenter;
                            var rectAdjust = new Rect(rect.position.x + ((rect.width - 5) * (0.01f * i)), rect.position.y, 2.5f, 15);
                            var par = i % 10f;
                            timeLineStyle.fontSize = par == 0 ? 15 : par == 5 ? (rect.width * 0.01f > 3f) ? 10 : 5 : (rect.width * 0.01f > 4f) ? 5 : 1;
                            rectAdjust.y += par == 0 ? -2 : par == 5 ? (rect.width * 0.01f > 3f) ? -1 : 0 : 0;
                            timeLineStyle.normal.textColor = Color.grey;
                            GUI.Box(rectAdjust, "|", timeLineStyle);
                            rectAdjust.y = rect.position.y + 5;

                            if (par == 0)
                            {
                                timeLineStyle.normal.textColor = Color.black;
                                timeLineStyle.alignment = TextAnchor.MiddleLeft;
                                timeLineStyle.fontSize = Mathf.Clamp((int)(rect.width * 0.02f), 6, 10);
                                rectAdjust.width = 25;
                                rectAdjust.height = 15;
                                rectAdjust.x += number < 10 ? -1 : -7;
                                GUI.Label(rectAdjust, (number * 0.1f).ToString(), timeLineStyle);
                                number++;
                            }
                        }
                        GUI.color = color;
                        rect.x -= 5;
                        rect.width += 10;
                        normalizedTime.floatValue = GUI.HorizontalSlider(rect, normalizedTime.floatValue, 0f, 1f, GUIStyle.none, skin.GetStyle("AnimatorEventThumb"));
                        rect.x = rect.x + rect.width - 3;
                        rect.width = width - 2;
                        normalizedTime.floatValue = EditorGUI.FloatField(rect, (float)System.Math.Round(normalizedTime.floatValue, 2));
                        break;
                }
               
            }
            EditorGUILayout.EndVertical();
            return true;
        }
        protected void DrawNormalizeEventStyle()
        {

        }
        public override bool UseDefaultMargins()
        {
            return false;
        }
    }
}