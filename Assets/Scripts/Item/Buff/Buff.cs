using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class RoleBuff
{
    public int ID;
    public Sprite sprite;
    public string name;
    public string desc;
    
    public string TemporaryId;//添加到role身上后的临时id
    
    public RoleController roleController;
    public RoleController Adder;//施加者
    private RoleBuffController roleBuffController;
    public BuffOverlayType BuffOverlayType;
    public RoleBuffLifeCycle lifeCycle;
    public BuffTrigger trigger;
    public BuffEffect effect;
    public BuffOverlayEffect overlayEffect;
    public bool IsDestroy;
    
    public virtual void Init(RoleBuffLifeCycle lifecycle,BuffTrigger trig,BuffEffect eff,BuffOverlayEffect overlayEff,ParticleSystem particleSystem,Sprite sprite,string name,string desc,BuffOverlayType buffOverlayType)
    {
        lifeCycle = lifecycle;
        trigger = trig;
        effect = eff;
        overlayEffect = overlayEff;
        if (overlayEffect!=null)
        {
            overlayEffect.Init(this);
        }
        lifecycle.Init(this);
        trigger.Init(this);
        effect.Init(this,particleSystem);
        this.sprite = sprite;
        this.name = name;
        this.desc = desc;
        IsDestroy = false;
        BuffOverlayType = buffOverlayType;
    }

    public void TriggerOverlayEffect(int overlayNumber)
    {
        if (overlayEffect == null)
        {
            return;
        }
        overlayEffect.Trigger(overlayNumber);
    }

    public void TriggerEffect()
    {
        effect.TriggerEffect();
    }
    
    public virtual void Awake(string temporaryId,RoleController roleController,RoleController adder,RoleBuffController roleBuffController)
    {
        TemporaryId = temporaryId;
        this.Adder = adder;
        this.roleController = roleController;
        this.roleBuffController = roleBuffController;
        trigger.Awake();
        lifeCycle.Awake();
    }
    public void IniEffect()
    {
        effect.Awake();
    }


    public virtual void Update()
    {
        if (!IsDestroy)
        {
            trigger.Update();
        }
        if (!IsDestroy)
        {
            effect.Update();
        }
        if (!IsDestroy)
        {
            lifeCycle.Update();
        }
    }

    public virtual void Destroy()
    {
        IsDestroy = true;
        roleBuffController.RemoveBuff(this);
        trigger.Destroy();
        effect.Destroy();
        lifeCycle.Destroy();
        if (overlayEffect!=null)
        {
            overlayEffect.Destroy();
        }
    }
}

public class RoleBuffLifeCycle
{
    protected RoleBuff roleBuff;
    public virtual void Init(RoleBuff rb)
    {
        roleBuff = rb;
    }

    public virtual void ReSet(RoleBuffLifeCycle buffLifeCycle)
    {
    }

    public virtual void Append()
    {
        
    }

    public virtual void Awake(){}
    public virtual void Update(){}
    public virtual void Destroy(){}
}
public class BuffTrigger
{
    protected RoleBuff roleBuff;
    public virtual void Init(RoleBuff rb)
    {
        roleBuff = rb;
    }
    public virtual void Awake(){}
    public virtual void Update(){}
    public virtual void Destroy(){}
}

public class BuffEffect
{
    protected RoleBuff roleBuff;
    private ParticleSystem particle;
    public ParticleSystem Particle=>particle;
    
    //临时特效
    List<GameObject> particleList = new List<GameObject>();
    public virtual void Init(RoleBuff rb,ParticleSystem particle)
    {
        roleBuff = rb;
        this.particle = particle;
    }

    private Color buffColor;
    private Color rimColor;
    private BuffColorType buffColorType;
    public void SetColor(Color color,Color rimCOlor,BuffColorType colorType)
    {
        buffColor = color;
        rimColor = rimCOlor;
        buffColorType = colorType;
    }
    
    public virtual void TriggerEffect()
    {
        if (buffColorType == BuffColorType.Trigger)
        {
            roleBuff.roleController.roleNode.SetColor(
                new RoleColorData(){BuffColor = buffColor,BuffColorType = buffColorType,BuffColorEndTime = Time.time+0.2f}
                );
            roleBuff.roleController.roleNode.Set_RimColor(
                new RoleColorData(){BuffColor = rimColor,BuffColorType = buffColorType,BuffColorEndTime = Time.time+0.2f,Range = 0.5f}
                );
        }
    }

    private RoleColorData continuedRoleColorData;
    public virtual void Awake()
    {
        if (particle!=null)
        {
            ParticleSystem ps = GameObject.Instantiate(particle);
            ps.transform.SetParent(roleBuff.roleController.roleNode.Body);
            ps.transform.localPosition = Vector3.zero; // new Vector3(Random.Range(-0.2f,0.21f),Random.Range(-0.2f,0.21f),Random.Range(-0.2f,0.21f));
            particleList.Add(ps.gameObject);
        }
        
        if (buffColorType == BuffColorType.Continued)
        {
            continuedRoleColorData = new RoleColorData()
                {BuffColor = buffColor, BuffColorType = buffColorType, BuffColorEndTime = float.MaxValue};
            roleBuff.roleController.roleNode.SetColor(continuedRoleColorData);
        }
    }

    public virtual void Update(){}
    

    public virtual void Destroy()
    {
        //Debug.Log(roleBuff.name);
        for (int i = 0; i < particleList.Count; i++)
        {
            if (particleList[i]==null || particleList[i].gameObject == null)
            {
                continue;
            }
            GameObject.Destroy(particleList[i].gameObject);
        }
        particleList.Clear();
        
        
        if (buffColorType == BuffColorType.Continued)
        {
            roleBuff.roleController.roleNode.RemoveColor(continuedRoleColorData);
        }
    }
}

public class BuffOverlayEffect
{
    protected RoleBuff _roleBuff;
    protected List<int> probability = new List<int>();
    public List<int> Probability => probability;
    public BuffOverlayEffect(List<int> probability)
    {
        this.probability = probability;
    }
    
    public void Init(RoleBuff roleBuff)
    {
        _roleBuff = roleBuff;
    }
    public virtual void Trigger(int overlayNumber)
    {
        
    }
    

    public virtual void Destroy()
    {
    }
}
