using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Action = System.Action;

public class UIManager : MonoBehaviour
{
    public static UIManager Inst { get; private set; }
    private const string OnOpenStr = "OnOpen";
    private const string OnCloseStr = "OnClose";
    private const string OnHideStr = "OnHide";
    private const string OnReShowStr = "OnReShow";

    public CanvasGroup AssetsBar;
    public UI_Tips Tips;
    public CanvasGroup MaskImg;
    public CanvasGroup LogoMaskImg;

    public UIFocusMask FocusMask;
    //UI prefab的路径
    private const string Path = "Assets/AssetsPackage/UIPrefabs/{0}.prefab";

    [SerializeField]
    private Transform panelGroup;
    
    /// <summary>
    /// 当前已经Open的Panel栈,str = path
    /// </summary>
    private List<string> panelStack = new List<string>();
    /// <summary>
    /// 已经打开的panel
    /// </summary>
    private Dictionary<string,PanelBase> panelDic = new Dictionary<string, PanelBase>();

    public void Init()
    {
        Inst = this;
    }

    public bool PanelIsOpen(string panelName)
    {
        return panelStack.Contains(string.Format(Path,panelName));
    }

    private Tweener assetBarTweener;
    public void ShowAssetsBar(bool isShow)
    {
        if (assetBarTweener!=null)
        {
            assetBarTweener.Kill();
        }
        assetBarTweener = AssetsBar.DOFade(isShow?1:0, 0.3f);
    }
    
    private Tweener maskTweener;
    private int maskNumber;
    public bool MaskIsShowing => maskNumber > 0;
    public void ShowMask(Action callBack)
    {
        maskNumber++;
        if (maskNumber-1 > 0)
        {
            callBack?.Invoke();
            return;
        }
        MaskImg.gameObject.SetActive(true);
        if (maskTweener!=null)
        {
            maskTweener.Kill(true);
        }
        maskTweener = MaskImg.DOFade(1, 0.3f).OnComplete(() =>
        {
            if (callBack != null)
            {
                try
                {
                    callBack();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    throw;
                }
            }
        });
    }

    public void HideMask(Action callBack)
    {
        maskNumber--;
        if (maskNumber>0)
        {
            callBack?.Invoke();
            return;
        }
        
        if (maskTweener!=null)
        {
            maskTweener.Kill(true);
        }
        maskTweener = MaskImg.DOFade(0, 0.3f).OnComplete(() =>
        {
            if (callBack != null)
            {
                callBack();
            }
            MaskImg.gameObject.SetActive(false);
        });
    }

    
    private Tweener logoMaskTweener;
    private int logoMaskNumber;
    public bool LogoMaskIsShowing => logoMaskNumber > 0;
    public void ShowLogoMask(Action callBack)
    {
        logoMaskNumber++;
        if (logoMaskNumber-1 > 0)
        {
            callBack?.Invoke();
            return;
        }
        // LogoMaskImg.gameObject.SetActive(true);
        if (logoMaskTweener!=null)
        {
            logoMaskTweener.Kill(true);
        }
        logoMaskTweener = LogoMaskImg.DOFade(1, 0.3f).OnComplete(() =>
        {
            if (callBack != null)
            {
                try
                {
                    callBack();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    throw;
                }
            }
        });
    }

    public void HideLogoMask(Action callBack)
    {
        logoMaskNumber--;
        if (logoMaskNumber>0)
        {
            callBack?.Invoke();
            return;
        }
        
        if (logoMaskTweener!=null)
        {
            logoMaskTweener.Kill(true);
        }
        logoMaskTweener = LogoMaskImg.DOFade(0, 0.3f).OnComplete(() =>
        {
            if (callBack != null)
            {
                callBack();
            }
            LogoMaskImg.gameObject.SetActive(false);
        });
    }

    
    
    public void Open(string path,bool hideOldPanel=true, params object[] values)
    {
        openPanel(string.Format(Path, path),hideOldPanel,values);
    }

    public void Close()
    {
        if (panelStack.Count == 0)
        {
            return;
        }
        string topPanelName = panelStack[panelStack.Count-1];
    
        closePanel(topPanelName);
    }

    public void CloseAll()
    {
        while (panelStack.Count > 0)
        {
            string topPanelName = panelStack[panelStack.Count-1];

            closePanel(topPanelName);
        }
    }

    public void Close(string panelName)
    {
        closePanel(string.Format(Path, panelName));
    }


    void openPanel(string path,bool hideOldPanel, params object[] args)
    {
        ResourcesManager.Inst.GetAsset<GameObject>(path, delegate(GameObject go)
        {
            if (panelDic.ContainsKey(path))
            {
                return;
            }
            var panelGo = Instantiate(go, panelGroup);
            PanelBase panelBase = panelGo.GetComponent<PanelBase>();

            while (true)
            {
                if (panelStack.Count == 0)
                {
                    break;
                }
                var top = panelStack[panelStack.Count - 1];
                if (panelDic.ContainsKey(top))
                {
                    if (hideOldPanel)
                    {
                        panelDic[top].Hide(delegate {  }); 
                        GameTools.CallFunc(panelDic[top], OnHideStr); 
                    }
                    break;
                }
                else
                {
                    panelStack.RemoveAt(panelStack.Count - 1);
                }
            }
        
            panelStack.Add(path);
            panelDic.Add(path,panelBase);
            panelBase.Show();
            GameTools.CallFunc(panelBase,OnOpenStr,args);
        });
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    /// <param name="path">panel 名字</param>
    void closePanel(string path)
    {
        if (!panelDic.ContainsKey(path))
        {
            return;
        }

        var panel = panelDic[path];
        panel.Hide(() =>
        {
            Destroy(panel.gameObject);
        });
        GameTools.CallFunc(panel, OnCloseStr);
        
        panelStack.Remove(path);
        panelDic.Remove(path);

        if (panelStack.Count>0)
        {
            var currentTop = panelStack[panelStack.Count-1];
            if (panelDic.ContainsKey(currentTop))
            {
                if (!panelDic[currentTop].IsShow)
                {
                    panelDic[currentTop].Show();
                    GameTools.CallFunc(panelDic[currentTop], OnReShowStr);
                }
            }
        }
    }
    
}
