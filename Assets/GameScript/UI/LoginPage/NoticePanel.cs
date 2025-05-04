using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 公告item
/// </summary>
public class NoticeItem
{
    public string tapTitle;
    public string title;
    public string content;
    public NoticeItem(string tapTitle, string title, string content)
    {
        this.tapTitle = tapTitle;
        this.title = title;
        this.content = content;
    }
}
/// <summary>
/// 公告面板
/// </summary>
public class NoticePanel : UIFramwork {
    private GameObject btnKnow;
    private GameObject btnBlack;
    private string[] titleNanes;
    List<NoticeItem> listCount = new List<NoticeItem>();
    /// <summary>
    /// 注册消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        BindUI();
        f_RegClickEvent(btnKnow, OnBtnKnowClick);
        f_RegClickEvent(btnBlack, OnBtnBlackClick);
    }

    

    /// <summary>
    /// 页面开启
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        _NeedCloseSound = false;
        base.UI_OPEN(e);
        string msg = (string)e;
        listCount.Clear();
        string[] listMsg = ccMath.f_String2ArrayString(msg, "AAABBB");
        titleNanes = new string[3];
        for (int i = 0; i < listMsg.Length; i++)
        {
            string splistMsg = listMsg[i];
            if(splistMsg.Length>=3)
            {
                string tapTitle = splistMsg.Split('#')[0];
                string title = splistMsg.Split('#')[1];
                string content = splistMsg.Split('#')[2];
                listCount.Add(new NoticeItem(tapTitle, title, content));
            }
            
        }

        titleNanes[0] = "title_whgg";
        titleNanes[1] = "title_yxhd";
        titleNanes[2] = "title_yxgg";

        var selectIndex = 0;

        for (int i = 2; i >=0; i--)//固定公告三个
        {
            GameObject ObjTapItem = f_GetObject("BtnTap" + i);
            if ((i + 1) > listCount.Count)
            {
                ObjTapItem.SetActive(false);
                continue;
            }
            else
            if(string.IsNullOrEmpty(listCount[i].content))
                ObjTapItem.SetActive(false);
            else
            {
                selectIndex = i;
                ObjTapItem.SetActive(true);
                f_RegClickEvent(ObjTapItem, OnBtnTapItemClick, listCount[i], i);
            }
        }
        //默认点击第一个公告
        OnBtnTapItemClick(null, listCount[selectIndex], selectIndex);
    }
    /// <summary>
    /// 点击分页
    /// </summary>
    private void OnBtnTapItemClick(GameObject go, object obj1, object obj2)
    {
        NoticeItem noticeItem = (NoticeItem)obj1;
        if (string.IsNullOrEmpty(noticeItem.content)) return;
        int index = (int)obj2;
        f_GetObject("BtnTap0").transform.Find("Press").gameObject.SetActive(index == 0);
        f_GetObject("BtnTap1").transform.Find("Press").gameObject.SetActive(index == 1);
        f_GetObject("BtnTap2").transform.Find("Press").gameObject.SetActive(index == 2);
     
        f_GetObject("LabelContent").GetComponent<UILabel>().text = noticeItem.content;
        f_GetObject("ScrollView").GetComponent<UIScrollView>().ResetPosition();

        //设置标题
        if (index >= titleNanes.Length) {
            return;
        }
        f_GetObject("Title").GetComponent<UISprite>().spriteName = titleNanes[index];
    }

    /// <summary>
    /// 绑定UI
    /// </summary>
    private void BindUI()
    {
        btnKnow = f_GetObject("BtnKnow");
        btnBlack = f_GetObject("BtnBlack");
    }
    /// <summary>
    /// 点击知道了按钮事件
    /// </summary>
    void OnBtnKnowClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);//播放按钮声音
        ccUIManage.GetInstance().f_SendMsg(UINameConst.NoticePanel, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 点击公告黑色背景关闭公告
    /// </summary>
    void OnBtnBlackClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.NoticePanel, UIMessageDef.UI_CLOSE);
    }
}
