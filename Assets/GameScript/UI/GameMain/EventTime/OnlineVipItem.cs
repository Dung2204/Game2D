using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using System;
using Spine;
using Spine.Unity;
/// <summary>
/// 活动首充界面
/// </summary>
public class OnlineVipItem : UIFramwork
{
    public OnlineVipAwardDT OnlineVipAwardDT = null;
    private int timeLeft;
    public UILabel m_TimeLeft;//下次吃大餐的剩余时间
    public void SetData(OnlineVipAwardDT Data)
    {
        OnlineVipAwardDT = Data;
        ResourceCommonDT _CommonDT = CommonTools.f_GetCommonResourceByResourceStr(Data.szAward);

        UISprite Case = transform.Find("Case").GetComponent<UISprite>();
        UI2DSprite Icon = transform.Find("Icon").GetComponent<UI2DSprite>();
        UILabel Num = transform.Find("Num").GetComponent<UILabel>();
        UILabel Name = transform.Find("Name").GetComponent<UILabel>();

        m_TimeLeft = transform.Find("Time").GetComponent<UILabel>();

        string name = _CommonDT.mName;
        Case.spriteName = UITool.f_GetImporentColorName(_CommonDT.mImportant, ref name);
        Icon.sprite2D = UITool.f_GetIconSprite(_CommonDT.mIcon);
        Num.text = _CommonDT.mResourceNum.ToString();
        //Name.text = name;

 

        ItemChange(null);

        f_RegClickEvent(Icon.gameObject, OnClickItem, _CommonDT);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_ONLINE_VIP_AWARD, ItemChange, this);
    }
    private void OnClickItem(GameObject go, object obj1, object obj2)
    {
        ResourceCommonDT commonData = (ResourceCommonDT)obj1;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
    }

    private void TimerControl(bool isStart)
    {
        CancelInvoke("ReduceTime");
        if (isStart)
        {
            InvokeRepeating("ReduceTime", 0f, 1f);
        }
    }
    private void ReduceTime()
    {
        timeLeft--;
        if (timeLeft <= 0)
        {
            timeLeft = 0;
            TimerControl(false);
        }
        m_TimeLeft.text = CommonTools.f_GetStringBySecond(timeLeft);
    }

    private void ItemChange(object value)
    {
        Debug.Log("1111");
        bool check = Data_Pool.m_OnlineVipAwardPool.CheckGetAward(OnlineVipAwardDT.iId);
        if (check)
        {
            m_TimeLeft.text = CommonTools.f_GetTransLanguage(1274);
            //TimerControl(false);
        }
        else
        {
            int timeSec = Data_Pool.m_OnlineAwardPool.m_timeSecondToday;

            int timeEvent = OnlineVipAwardDT.iTime * 60;

            timeLeft = timeEvent - timeSec;
            TimerControl(true);
        }
    }

    public void f_DestoryView()
    {
        TimerControl(false);
    }

}

