using UnityEngine;
using System.Collections;
using System;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 铜雀台（已改名宫宴）
/// </summary>
public class ActBanQuetCtl : UIFramwork {
    public GameObject m_BtnEat;//吃大餐按钮 12-
    public GameObject m_BtnEatUnAct;//吃大餐未激活
    public UILabel m_TimeLeft;//下次吃大餐的剩余时间
    public SocketCallbackDT m_eatCallback = new SocketCallbackDT();//吃大餐回调
    private int timeLeft;//剩余时间
    private string strTexBgRoot = "UI/TextureRemove/Activity/Tex_EnergyBg";
    public void f_DestoryView()
    {
        gameObject.SetActive(false);
        TimerControl(false);
    }
    /// <summary>
    /// 初始化数据,视图
    /// </summary>
    public void f_ShowView()
    {
        gameObject.SetActive(true);
        f_RegClickEvent(m_BtnEat.gameObject, OnBtnEatClick);
        m_TimeLeft.text = CommonTools.f_GetTransLanguage(1304);
        int timeNow = GameSocket.GetInstance().f_GetServerTime();
        DateTime dateTime = ccMath.time_t2DateTime(timeNow);
        m_eatCallback.m_ccCallbackSuc = OnCallSuc;
        m_eatCallback.m_ccCallbackFail = OnCallFail;
        Debug.Log(CommonTools.f_GetTransLanguage(1305) + dateTime+";"+ dateTime.Hour);
        if (dateTime.Hour < 12)
        {
            timeLeft = 12 * 60 * 60 - dateTime.Hour * 60 * 60 - dateTime.Minute * 60 - dateTime.Second;
            TimerControl(true);
        }
        else if (dateTime.Hour < 14)
        {
            if (Data_Pool.m_ActivityCommonData.isEatNoon)
            {
                timeLeft = 18 * 60 * 60 - dateTime.Hour * 60 * 60 - dateTime.Minute * 60 - dateTime.Second;
                TimerControl(true);
            }
            else
            {
                UITool.f_OpenOrCloseWaitTip(true);
                Data_Pool.m_ActivityCommonData.f_GetPowerInfo(OnGetPowerInfoCallback);
            }
        }
        else if (dateTime.Hour < 18)
        {
            timeLeft = 18 * 60 * 60 - dateTime.Hour * 60 * 60 - dateTime.Minute * 60 - dateTime.Second;
            TimerControl(true);
        }
        else if (dateTime.Hour < 20)
        {
            if (Data_Pool.m_ActivityCommonData.isEatNight)
            {
                timeLeft = 24 * 60 * 60 - dateTime.Hour * 60 * 60 - dateTime.Minute * 60 - dateTime.Second;
                timeLeft += 12 * 60 * 60;
                TimerControl(true);
            }
            else
            {
                UITool.f_OpenOrCloseWaitTip(true);
                Data_Pool.m_ActivityCommonData.f_GetPowerInfo(OnGetPowerInfoCallback);
            }
        }
        else
        {
            timeLeft = 24 * 60 * 60 - dateTime.Hour * 60 * 60 - dateTime.Minute * 60 - dateTime.Second;
            timeLeft += 12 * 60 * 60;
            TimerControl(true);
        }
        f_LoadTexture();
    }
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
        UITexture TexBg = f_GetObject("TexBg").GetComponent<UITexture>();
        if (TexBg.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
            TexBg.mainTexture = tTexture2D;
        }
    }
    /// <summary>
    /// 成功查询信息
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetPowerInfoCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        int timeNow = GameSocket.GetInstance().f_GetServerTime();
        DateTime dateTime = ccMath.time_t2DateTime(timeNow);
        if (dateTime.Hour >= 12 && dateTime.Hour < 14)
        {
            if (Data_Pool.m_ActivityCommonData.isEatNoon)
            {
                timeLeft = 18 * 60 * 60 - dateTime.Hour * 60 * 60 - dateTime.Minute * 60 - dateTime.Second;
                TimerControl(true);
            }
            else
            {
                TimerControl(false);
            }
        }
        else if (dateTime.Hour >= 18 && dateTime.Hour < 20)
        {
            if (Data_Pool.m_ActivityCommonData.isEatNight)
            {
                timeLeft = 24 * 60 * 60 - dateTime.Hour * 60 * 60 - dateTime.Minute * 60 - dateTime.Second;
                timeLeft += 12 * 60 * 60;
                TimerControl(true);
            }
            else
            {
                TimerControl(false);
            }
        }
        else
        {
            Debug.LogError(CommonTools.f_GetTransLanguage(1306));
        }
    }
    /// <summary>
    /// 是否开启定时
    /// </summary>
    /// <param name="isStart"></param>
    private void TimerControl(bool isStart)
    {
        CancelInvoke("ReduceTime");
        m_BtnEat.gameObject.SetActive(!isStart);
        m_BtnEatUnAct.gameObject.SetActive(isStart);
        m_TimeLeft.gameObject.SetActive(isStart);
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
        m_TimeLeft.text = "[f1b049]" + CommonTools.f_GetStringBySecond(timeLeft);
    }

    private void CheckRed() {
        int timeNow = GameSocket.GetInstance().f_GetServerTime();
        DateTime dateTime = ccMath.time_t2DateTime(timeNow);


    }

    /// <summary>
    /// 查询成功
    /// </summary>
    /// <param name="obj"></param>
    private void OnInfoCallSuc(object obj)
    {
        Debug.Log(CommonTools.f_GetTransLanguage(1307));
    }

    /// <summary>
    /// 查询失败
    /// </summary>
    /// <param name="obj"></param>
    private void OnInfoCallFail(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        Debug.Log(CommonTools.f_GetTransLanguage(1308) + CommonTools.f_GetTransLanguage((int)obj));
    }
    /// <summary>
    /// 点击吃大餐按钮（铜雀台活动）
    /// </summary>
    private void OnBtnEatClick(GameObject go,object obj1,object obj2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_ActivityCommonData.f_Eat(m_eatCallback, OnGetPowerInfoCallback);
    }
    /// <summary>
    /// 领取体力成功
    /// </summary>
    /// <param name="obj"></param>
    private void OnCallSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        AwardPoolDT item1 = new AwardPoolDT();
        item1.f_UpdateByInfo((byte)EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Energy, 50);
        awardList.Add(item1);

        if (Data_Pool.m_ActivityCommonData.isGetSycee)
        {
            AwardPoolDT item2 = new AwardPoolDT();
            item2.f_UpdateByInfo((byte)EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Sycee, 50);
            awardList.Add(item2);
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });
    }
    /// <summary>
    /// 领取体力失败
    /// </summary>
    /// <param name="obj"></param>
    private void OnCallFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1339) + CommonTools.f_GetTransLanguage((int)obj));
    }
}
