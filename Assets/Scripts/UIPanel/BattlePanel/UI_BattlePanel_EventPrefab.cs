using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattlePanel_EventPrefab : MonoBehaviour
{
    [SerializeField]
    private LayoutElement layoutElement;
    [SerializeField]
    private RectTransform eventTransform;
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Image eventBg;
    [SerializeField]
    private Image eventIcon;
    [SerializeField]
    private Text eventName;

    public void Init(Color bgColor,Sprite icon,string eventText)
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        eventBg.color = bgColor;
        eventIcon.sprite = icon;
        eventName.text = eventText;
        GameManager.Inst.StartCoroutine(waitDoTween());
    }

    IEnumerator waitDoTween()
    {
        yield return null; 
        canvasGroup.alpha = 1;
        eventTransform.localPosition = new Vector3(-eventTransform.sizeDelta.x,0,0);
        eventTransform.DOLocalMove(Vector3.zero, 0.3f);
        var tweener = eventTransform.DOLocalMove(new Vector3(-eventTransform.sizeDelta.x, 0, 0), 0.3f);
        tweener.SetDelay(5f).Delay();
        tweener.OnComplete(() =>
        {
            DOTween.To(() => layoutElement.preferredHeight, value => layoutElement.preferredHeight = value, 0, 0.3f)
                .OnComplete(
                    () =>
                    {
                        Destroy(gameObject);
                    });
        });
    }
    
}
