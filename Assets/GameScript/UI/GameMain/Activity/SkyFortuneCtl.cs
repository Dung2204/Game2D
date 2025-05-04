using UnityEngine;
using System.Collections;
using System;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 天降横财
/// </summary>
public class SkyFortuneCtl : UIFramwork
{
    private bool ActEnd = false;
    private string strTexManRoot = "UI/TextureRemove/Activity/Tex_FortuneMan";
    private string strTexFortuneRoot = "UI/TextureRemove/Activity/Tex_Fortune";

    private OpenSkyFortuneTimeDT m_OpenSkyFortuneTimeDT;
    /// <summary>
    /// 关闭UI
    /// </summary>
    public void f_DestoryView()
    {
        gameObject.SetActive(false);
        CancelInvoke("TimeControl");
    }
    /// <summary>
    /// 初始化数据,视图
    /// </summary>
    public void f_ShowView(OpenSkyFortuneTimeDT m_OpenSkyFortuneTimeDT)
    {
        this.m_OpenSkyFortuneTimeDT = m_OpenSkyFortuneTimeDT;
        gameObject.SetActive(true);
        DateTime curDateTime = ccMath.time_t2DateTime(GameSocket.GetInstance().f_GetServerTime());
        DateTime EndTime = CommonTools.f_GetDateTimeByTimeStr(m_OpenSkyFortuneTimeDT.iEndTime.ToString());
        //timesLeft = - curDateTime.TimeOfDay.TotalSeconds;
        timesLeft = (int)(ccMath.DateTime2time_t(EndTime) - ccMath.DateTime2time_t(curDateTime));
        if(ActEnd)
            timesLeft = 0;
        InvokeRepeating("TimeControl", 0f, 1f);
        f_LoadTexture();
    }
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        UITexture TexMan = f_GetObject("TexMan").GetComponent<UITexture>();
        UITexture TexFortune = f_GetObject("TexFortune").GetComponent<UITexture>();
        if(TexMan.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexManRoot);
            TexMan.mainTexture = tTexture2D;

            Texture2D tTexFortune = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexFortuneRoot);
            TexFortune.mainTexture = tTexFortune;
        }
    }
    /// <summary>
    /// 注册消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnSeeAct", OnBtnSeeActClick);
    }
    private int timesLeft = 0;
    private void TimeControl()
    {
        DateTime tTime = new DateTime().AddSeconds(timesLeft);//ccMath.time_t2DateTime(timesLeft);
        //f_GetObject("LabelTimeLeft").GetComponent<UILabel>().text = "活动结束倒计时：[FDBD1DFF]" + CommonTools.f_GetStringBySecond((int)timesLeft);

        f_GetObject("LabelTimeLeft").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1366), tTime.Day - 1, tTime.Hour, tTime.Minute, tTime.Second);
        timesLeft--;
        if(timesLeft <= 0)
        {
            timesLeft = 0;
            ActEnd = true;
        }
    }
    #region 按钮事件
    /// <summary>
    /// 点击查看活动
    /// </summary>
    private void OnBtnSeeActClick(GameObject go, object obj1, object obj2)
    {
        if(ActEnd)
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1367));
        else
            ccUIManage.GetInstance().f_SendMsg(UINameConst.SkyFortunePage, UIMessageDef.UI_OPEN, m_OpenSkyFortuneTimeDT);
    }
    #endregion
}
