using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainPanel_HeroHaloBreath : MonoBehaviour
{
    public float MinAlpha;

    public float MaxAlpha;

    public float BreathRhythm;

    private Image haloImg;

    private Color haloColor;
    // Start is called before the first frame update
    void Start()
    {
        haloImg = GetComponent<Image>();
        if(!haloImg)
            Destroy(this);
        
    }

    // Update is called once per frame
    void Update()
    {
        haloColor = haloImg.color;
        haloColor.a = (Mathf.Sin(Time.time*BreathRhythm)*(MaxAlpha-MinAlpha)+(MaxAlpha+MinAlpha))*0.5f;
        haloImg.color = haloColor;
    }
}
