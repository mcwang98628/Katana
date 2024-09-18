using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainPanel_ChapterInfoPanel : MonoBehaviour
{
    [SerializeField]
    private Image chapterIcon;
    [SerializeField]
    private UIText chapterName;
    [SerializeField]
    private UIText descText;
    [SerializeField]
    private Text soulText;
    [SerializeField]
    private Button soulBtn;

    private int cpid;
    
    private string chapterIconPath = "Assets/Arts/Textures/UISprites/ChapterCovers/{0}.png";
    public void Init(int chapterId)
    {
        cpid = chapterId;
        var tableData = DataManager.Inst.ChapterTableDatas[chapterId];
        chapterName.text = tableData.ChapterName;
        descText.text = tableData.Desc;
        ResourcesManager.Inst.GetAsset<Sprite>(string.Format(chapterIconPath,tableData.IconName),
            delegate(Sprite sprite)
            {
                chapterIcon.sprite = sprite;
            });
        soulText.text = "X " + DataManager.Inst.ChapterTableDatas[chapterId].SoulCount;
        soulBtn.gameObject.SetActive(!ArchiveManager.Inst.ReceivedChapterFreeSoul(cpid));
        EventManager.Inst.DistributeEvent(TGANames.MainPanelPeekChapterInfo,chapterId);
        gameObject.SetActive(true);
    }

    public void OnCloseBtnClick()
    {
        gameObject.SetActive(false);
    }

    public void OnSoulBtnClick()
    {
        ArchiveManager.Inst.ReceiveChapterFreeSoul(cpid);
        soulBtn.gameObject.SetActive(false);
        EventManager.Inst.DistributeEvent(TGANames.MainPanelGetChapterInfoSoul,cpid);
    }

}
