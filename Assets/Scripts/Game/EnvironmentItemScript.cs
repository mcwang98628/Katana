
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Environment Data Asset")]
public class EnvironmentItemScript : ScriptableObject
{
    [Header("主光（方向光）")]
    [FoldoutGroup("Main Light")] public Color MainLightColor;
    [FoldoutGroup("Main Light")] public float MainLightIntensity;
    [FoldoutGroup("Main Light")] public Vector3 MainLightEulerAngle;
    [Header("角色（点光）")]
    [FoldoutGroup("Character Light")] public Color PointLightColor = Color.white;
    [FoldoutGroup("Character Light")] public float PointLightIntensity = 10;
    [FoldoutGroup("Character Light")] public float PointLightRange = 65;
    [FoldoutGroup("Character Light")] public Vector3 PointLightPos = new Vector3(0, 6, 0);

    [Header("环境光")]
    [FoldoutGroup("Environment Light")] [ColorUsage(false, true)] public Color SkyColor;
    [FoldoutGroup("Environment Light")] [ColorUsage(false, true)] public Color EquatorColor;
    [FoldoutGroup("Environment Light")] [ColorUsage(false, true)] public Color GroundColor;


    [Header("雾效")]
    [InfoBox("从上到下越来越小，世界空间")]
    [FoldoutGroup("Fog")] public float FogEndY1 = 1f;
    [FoldoutGroup("Fog")] public float FogStartY1 = 2f;

    [FoldoutGroup("Fog")] public float FogStartY2 = -1f;
    [FoldoutGroup("Fog")] public float FogEndY2 = -2f;

    [FoldoutGroup("Fog")] public float FogStartZ = 60f;
    [FoldoutGroup("Fog")] public float FogEndZ = -30f;
    [InfoBox("雾效平滑参数，也决定浓淡")]
    [FoldoutGroup("Fog")] public float FogPow = 1f;

    [Header("Fog范围")]
    [InfoBox("分为上下颜色和光源颜色，屏幕空间（0-1）")]
    [LabelText("上半部颜色")]
    [FoldoutGroup("Fog")] [ColorUsage(false, true)] public Color UpFogColor;
    [LabelText("渐变起点")]
    [FoldoutGroup("Fog")] public float UpColorEdgeScrren = 1f;
    [LabelText("下半部颜色")]
    [FoldoutGroup("Fog")] [ColorUsage(false, true)] public Color DownFogColor;
    [LabelText("渐变终点")]
    [FoldoutGroup("Fog")] public float DownColorEdgeScrren = 2f;
    [LabelText("光源颜色")]
    [FoldoutGroup("Fog")] [ColorUsage(false, true)] public Color LightColor;
    [LabelText("光源位置")]
    [FoldoutGroup("Fog")] public Vector2 LightPosScrren;
    [LabelText("光源范围")]
    [FoldoutGroup("Fog")] public float LightRange = 5;
    [LabelText("光源Gloss")]
    [FoldoutGroup("Fog")] public float LightGloss = 2;

    [Header("Fog Noise")]
    [FoldoutGroup("Fog")] public Texture2D NoiseMap;
    [FoldoutGroup("Fog")] public float NoiseIntensity;
    [FoldoutGroup("Fog")] public float NoiseSpeedX;
    [FoldoutGroup("Fog")] public float NoiseSpeedY;

    public void ApplyEnvironment(float time = 0)
    {
        SetMainLight(time);
        SetCharacterLight(time);
        SetAmbientColor(time);
        SetShaderproperties(time);
    }

    private void SetMainLight(float time)
    {
        var go = GameObject.FindWithTag("MainLight");
        if (go == null)
        {
            go = new GameObject("MainLight");
            go.tag = "MainLight";
        }
        Light mainLight = go.GetComponent<Light>();
        if (mainLight == null)
            mainLight = go.AddComponent<Light>();
        DOTween.To(() => mainLight.color, value =>
        {
            mainLight.color = value;
        }, MainLightColor, time);
        
        DOTween.To(() => mainLight.intensity, value =>
        {
            mainLight.intensity = value;
        }, MainLightIntensity, time);
        
        // DOTween.To(() => mainLight.transform.eulerAngles, value =>
        // {
        //     mainLight.transform.eulerAngles = value;
        // }, MainLightEulerAngle, time);
        
        // mainLight.color = MainLightColor;
        // mainLight.intensity = MainLightIntensity;
        // mainLight.transform.eulerAngles = MainLightEulerAngle;
    }
    public void SetCharacterLight(float time=0)
    {
        if (BattleManager.Inst.CurrentPlayer == null)
            return;
        var go = GameObject.FindWithTag("CharacterLight");
        if (go == null)
        {
            go = new GameObject("CharacterLight");
            go.tag = "CharacterLight";
        }

        Light charcterLight = go.GetComponent<Light>();
        if (charcterLight == null)
            charcterLight = go.AddComponent<Light>();
        
        DOTween.To(() => charcterLight.color, value =>
        {
            charcterLight.color = value;
        }, PointLightColor, time);
        
        DOTween.To(() => charcterLight.intensity, value =>
        {
            charcterLight.intensity = value;
        }, PointLightIntensity, time);
        
        DOTween.To(() => charcterLight.range, value =>
        {
            charcterLight.range = value;
        }, PointLightRange, time);
        
        
        // charcterLight.color = PointLightColor;
        // charcterLight.intensity = PointLightIntensity;
        // charcterLight.range = PointLightRange;
        go.transform.SetParent(BattleManager.Inst.CurrentPlayer.transform);
        go.transform.localPosition = PointLightPos;
    }
    private void SetAmbientColor(float time)
    {
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        
        DOTween.To(() => RenderSettings.ambientSkyColor, value =>
        {
            RenderSettings.ambientSkyColor = value;
        }, SkyColor, time);
        
        DOTween.To(() => RenderSettings.ambientEquatorColor, value =>
        {
            RenderSettings.ambientEquatorColor = value;
        }, EquatorColor, time);
        
        DOTween.To(() => RenderSettings.ambientGroundColor, value =>
        {
            RenderSettings.ambientGroundColor = value;
        }, GroundColor, time);
        
        // RenderSettings.ambientSkyColor = SkyColor;
        // RenderSettings.ambientEquatorColor = EquatorColor;
        // RenderSettings.ambientGroundColor = GroundColor;
    }
    private void SetShaderproperties(float time)
    {
        Shader.SetGlobalTexture("_NoiseMap", NoiseMap);


        DOTween.To(() => Shader.GetGlobalFloat("_FogEndY1"), value =>
        {
            Shader.SetGlobalFloat("_FogEndY1", value);
        },FogEndY1,time);
        
        DOTween.To(() => Shader.GetGlobalFloat("_FogStartY1"), value =>
        {
            Shader.SetGlobalFloat("_FogStartY1", value);
        },FogStartY1,time);
        
        DOTween.To(() => Shader.GetGlobalFloat("_FogEndY2"), value =>
        {
            Shader.SetGlobalFloat("_FogEndY2", value);
        },FogEndY2,time);
        
        DOTween.To(() => Shader.GetGlobalFloat("_FogStartY2"), value =>
        {
            Shader.SetGlobalFloat("_FogStartY2", value);
        },FogStartY2,time);
        
        DOTween.To(() => Shader.GetGlobalFloat("_FogStartZ"), value =>
        {
            Shader.SetGlobalFloat("_FogStartZ", value);
        },FogStartZ,time);
        
        DOTween.To(() => Shader.GetGlobalFloat("_FogEndZ"), value =>
        {
            Shader.SetGlobalFloat("_FogEndZ", value);
        },FogEndZ,time);
        
        DOTween.To(() => Shader.GetGlobalFloat("_FogPow"), value =>
        {
            Shader.SetGlobalFloat("_FogPow", value);
        },FogPow,time);
        
        DOTween.To(() => Shader.GetGlobalFloat("_UpColorEdgeScrren"), value =>
        {
            Shader.SetGlobalFloat("_UpColorEdgeScrren", value);
        },UpColorEdgeScrren,time);
        
        DOTween.To(() => Shader.GetGlobalFloat("_DownColorEdgeScrren"), value =>
        {
            Shader.SetGlobalFloat("_DownColorEdgeScrren", value);
        },DownColorEdgeScrren,time);
        
        
        DOTween.To(() => Shader.GetGlobalFloat("_FogLightGloss"), value =>
        {
            Shader.SetGlobalFloat("_FogLightGloss", value);
        },LightGloss,time);
        
        DOTween.To(() => Shader.GetGlobalFloat("_FogLightRange"), value =>
        {
            Shader.SetGlobalFloat("_FogLightRange", value);
        },LightRange,time);
        
        DOTween.To(() => Shader.GetGlobalFloat("_NoiseIntensity"), value =>
        {
            Shader.SetGlobalFloat("_NoiseIntensity", value);
        },NoiseIntensity,time);
        
        DOTween.To(() => Shader.GetGlobalFloat("_NoiseSpeedX"), value =>
        {
            Shader.SetGlobalFloat("_NoiseSpeedX", value);
        },NoiseSpeedX,time);
        
        DOTween.To(() => Shader.GetGlobalFloat("_NoiseSpeedY"), value =>
        {
            Shader.SetGlobalFloat("_NoiseSpeedY", value);
        },NoiseSpeedY,time);
        
        

        
        DOTween.To(() => Shader.GetGlobalColor("_UpFogColor"), value =>
        {
            Shader.SetGlobalColor("_UpFogColor", value);
        },UpFogColor,time);
        DOTween.To(() => Shader.GetGlobalColor("_DownFogColor"), value =>
        {
            Shader.SetGlobalColor("_DownFogColor", value);
        },DownFogColor,time);
        DOTween.To(() => Shader.GetGlobalColor("_FogLightColor"), value =>
        {
            Shader.SetGlobalColor("_FogLightColor", value);
        },LightColor,time);
        DOTween.To(() => (Vector2)Shader.GetGlobalVector("_FogLightPos"), value =>
        {
            Shader.SetGlobalVector("_FogLightPos", value);
        },LightPosScrren,time);
        
        


    }
}
