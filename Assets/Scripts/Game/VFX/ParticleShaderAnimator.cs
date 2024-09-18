using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MatFloatAnimation
{
    public string name;
    public AnimationCurve curve;
}
public class ParticleShaderAnimator : MonoBehaviour
{
    //public bool Debug;
    [SerializeField]
    public List<MatFloatAnimation> animations;
    public float duration;
    public Material _mat;
    public float SpeedMul;
    private void OnEnable()
    {
        StartCoroutine(animating());
    }
    public IEnumerator animating()
    {
        float StartTime = Time.time;
        _mat = GetComponent<Renderer>().material;
        while(Time.time<StartTime+duration)
        {
            for(int i=0;i<animations.Count;i++)
            {
                _mat.SetFloat(animations[i].name,animations[i].curve.Evaluate((Time.time-StartTime)*SpeedMul));
            }
            yield return null;
        }
    }


}
