
using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace UnityEngine.Rendering.Universal
{
    [System.Serializable, VolumeComponentMenu("Jeff/SimpleFogEffect")]
    public sealed class SimpleFog : VolumeComponent, IPostProcessComponent
    {
        [Header("Basic")]
        public BoolParameter isActive = new BoolParameter(false);
        public MinFloatParameter endDistance = new MinFloatParameter(500f, 0f);
        public MinFloatParameter startDistance = new MinFloatParameter(45f, 0f);


        [Header("HeightFog")]
        public ClampedFloatParameter heightFollowCamera = new ClampedFloatParameter(0f,0f,1f);
        public FloatParameter heightFogDensity = new FloatParameter(0.005f);
        public FloatParameter fogHeight = new FloatParameter(0f);
        public MinFloatParameter fogHeightFade = new MinFloatParameter(65f, 0f);
        public MinFloatParameter fogHeightPower = new MinFloatParameter(1f, 0f);


        [Header("Sun")]
        public Vector3Parameter SunDirection = new Vector3Parameter(new Vector3(90, 0, 0));
        public ColorParameter sunLightColor = new ColorParameter(Color.white, true, false, true);

        [Header("SunFog")]
        public ColorParameter sunColor = new ColorParameter(Color.white, true, false, true);
        public MinFloatParameter sunPower = new MinFloatParameter(1f, 0f);

        [Header("Sky")]
        public ColorParameter topColor = new ColorParameter(Color.white, true, false, true);
        public MinFloatParameter topPower = new MinFloatParameter(1f, 0f);
        public ColorParameter skyColor = new ColorParameter(Color.white, true, false, true);
        public ColorParameter groundColor = new ColorParameter(Color.white, true, false, true);
        public MinFloatParameter groundPower = new MinFloatParameter(1f, 0f);

        [Header("Ambient")]
        public ColorParameter ambientSkyColor = new ColorParameter(Color.white, false);
        public ColorParameter ambientEquatorColor = new ColorParameter(Color.white, false);
        public ColorParameter ambientGroundColor = new ColorParameter(Color.white, false);

        public bool IsActive()
        {
            return isActive.value;
        }

        public bool IsTileCompatible()
        {
            return false;
        }
        public void SetMaterialAndAmbientValues(Material material)
        {
            material.SetFloat("_StartDistance", startDistance.value);
            material.SetFloat("_EndDistance", endDistance.value);
            if (RenderSettings.sun != null)
            {
                RenderSettings.sun.transform.rotation = Quaternion.Euler(SunDirection.value);
                RenderSettings.sun.color = sunLightColor.value;
                RenderSettings.sun.intensity = 1;
                material.SetVector("_SunDirection", RenderSettings.sun.transform.forward);
            }
            material.SetFloat("_FogHeight",fogHeight.value);
            material.SetFloat("_FogHeightPower", fogHeightPower.value);
            material.SetFloat("_FogHeightFade", fogHeightFade.value);
            material.SetFloat("_HeightFogDensity", heightFogDensity.value);
            material.SetFloat("_HeightFollowCamera", heightFollowCamera.value);
            material.SetColor("_SunColor", sunColor.value);
            material.SetFloat("_SunPower", sunPower.value);

            material.SetColor("_TopColor", topColor.value);
            material.SetFloat("_TopPower", topPower.value);

            material.SetColor("_SkyColor", skyColor.value);

            material.SetColor("_GroundColor", groundColor.value);
            material.SetFloat("_GroundPower", groundPower.value);

            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = ambientSkyColor.value;
            RenderSettings.ambientEquatorColor = ambientEquatorColor.value;
            RenderSettings.ambientGroundColor = ambientGroundColor.value;
        }




    }
}