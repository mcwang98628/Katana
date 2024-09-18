using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerExperience : MonoBehaviour
{
    // public  PlayerController playerController;
    // public ItemPoolScriptableObject LevelItemPool;
    [Header("每一级所需要的经验")]
    public List<float> LevelExpList;
    protected float maxExperience;
    protected int maxLevel;

    [ShowInInspector]
    public int CurrentLevel => currentLevel;
    private int currentLevel;
    [ShowInInspector]
    public float CurrentExp => currentExp;
    private float currentExp;
    [ShowInInspector]
    public float NextLevelPercent => nextLevelPercent;
    private float nextLevelPercent = 0;

    [Header("升级效果")]
    public ParticleSystem LevelUpPartical;
    public AudioClip LevelUpSound;
    // Start is called before the first frame update
    public void Init()
    {
        // playerController = GetComponent<PlayerController>();
        //设置初始经验
        currentLevel = 1;
        currentExp = 0;

        //初始化最大经验值
        maxLevel = LevelExpList.Count;
        maxExperience = 0;
        for (int i = 0; i < LevelExpList.Count; i++)
        {
            maxExperience += LevelExpList[i];
        }

        UpdateExp();
        //第一次加点
        //StartCoroutine(DelayOpenLevelUpPanel());
    }


    public void AddExp(float exp)
    {
        currentExp += exp;
        UpdateExp();
    }
    public void UpdateExp()
    {
        if (currentLevel >= maxLevel)
        {
            currentLevel = maxLevel;
            nextLevelPercent = 1;
            return;
        }

        float tempExp = currentExp;
        int tempLevel = 0;
        for (int i = 0; i < LevelExpList.Count; i++)
        {
            if (tempExp < LevelExpList[i])
            {
                tempLevel = i + 1;
                nextLevelPercent = tempExp / LevelExpList[i];
                break;
            }
            else
            {
                tempExp -= LevelExpList[i];
            }
        }

        //更新等级，检测是否升级
        if (tempLevel > currentLevel)
        {
            currentLevel = tempLevel;
            LevelUp();
        }


        EventManager.Inst.DistributeEvent(EventName.UpdateExp);
    }

    public void LevelUp()
    {

        if (LevelUpPartical)
        {
            LevelUpPartical.Play();
        }

        if (LevelUpSound)
        {
            AudioManager.Inst.PlaySource(LevelUpSound, 0.5f);
        }

        StartCoroutine(DelayOpenLevelUpPanel());
    }
    IEnumerator DelayOpenLevelUpPanel()
    {
        yield return new WaitForSecondsRealtime(2f);
        UIManager.Inst.Open("ChooseItemPanel");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {

            AddExp(8);
        }   
    }
}
