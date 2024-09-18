
using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.Rendering;

namespace UnityEngine.Rendering.Universal
{
    [VolumeComponentEditor(typeof(SimpleFog))]
    sealed class SimpleFogEditor : VolumeComponentEditor
    {
        public List<SerializedDataParameter> SerializedDataParameters = new List<SerializedDataParameter>();
        
        public override void OnEnable()
        {
            SerializedDataParameters.Clear();
            var o = new PropertyFetcher<SimpleFog>(serializedObject);
            SerializedDataParameters.Add(Unpack(o.Find(x => x.isActive)));
            SerializedDataParameters.Add(Unpack(o.Find(x => x.endDistance)));
            SerializedDataParameters.Add(Unpack(o.Find(x => x.startDistance)));
            SerializedDataParameters.Add(Unpack(o.Find(x => x.heightFollowCamera)));
            SerializedDataParameters.Add( Unpack(o.Find(x => x.heightFogDensity)));
            SerializedDataParameters.Add(Unpack(o.Find(x => x.fogHeight)));
            SerializedDataParameters.Add(Unpack(o.Find(x => x.fogHeightFade)));
            SerializedDataParameters.Add(Unpack(o.Find(x => x.fogHeightPower)));
            SerializedDataParameters.Add(Unpack(o.Find(x => x.SunDirection)));
            SerializedDataParameters.Add(Unpack(o.Find(x => x.sunLightColor)));
            SerializedDataParameters.Add(Unpack(o.Find(x => x.sunColor)));
            SerializedDataParameters.Add(Unpack(o.Find(x => x.sunPower)));
            SerializedDataParameters.Add(Unpack(o.Find(x => x.topColor)));
            SerializedDataParameters.Add(Unpack(o.Find(x => x.topPower)));
            SerializedDataParameters.Add(Unpack(o.Find(x => x.skyColor)));
            SerializedDataParameters.Add(Unpack(o.Find(x => x.groundColor)));
            SerializedDataParameters.Add(Unpack(o.Find(x => x.groundPower)));
            SerializedDataParameters.Add(Unpack(o.Find(x => x.ambientSkyColor)));
            SerializedDataParameters.Add(Unpack(o.Find(x => x.ambientEquatorColor)));
            SerializedDataParameters.Add(Unpack(o.Find(x => x.ambientGroundColor)));
        }
        
        
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Simple Fog", EditorStyles.miniLabel);
            for (int i = 0; i < SerializedDataParameters.Count; i++)
            {
                PropertyField(SerializedDataParameters[i]);
            }
        }
    }
}