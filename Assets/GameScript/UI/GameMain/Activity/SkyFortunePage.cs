using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System;
/// <summary>
/// 天降横财页面
/// </summary>
public class SkyFortunePage : UIFramwork
{
    private SocketCallbackDT QueryCallback = new SocketCallbackDT();//查询回调
    private SocketCallbackDT RecordCallback = new SocketCallbackDT();//查询回调
    private SocketCallbackDT FortuneCallback = new SocketCallbackDT();//祈福回调
    private int wasterSyceeCount = 0;//消耗元宝数量
    private bool isShowFortune = false;//是否正在抽奖，防止连点
    private bool ActEnd = false;
    private OpenSkyFortuneTimeDT m_OpenSkyFortuneTimeDT;
    /// <summary>
    /// 页面开启
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        m_OpenSkyFortuneTimeDT = (OpenSkyFortuneTimeDT)e;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_SkyFortunePool.f_QueryInfo(QueryCallback);
        Data_Pool.m_SkyFortunePool.f_QueryRecord(RecordCallback);

        DateTime EndTime = CommonTools.f_GetDateTimeByTimeStr(m_OpenSkyFortuneTimeDT.iEndTime.ToString());
        DateTime curDateTime = ccMath.time_t2DateTime(GameSocket.GetInstance().f_GetServerTime());

        //timesLeft = 24 * 60 * 60 - curDateTime.TimeOfDay.TotalSeconds;
        timesLeft = (int)(ccMath.DateTime2time_t(EndTime) - ccMath.DateTime2time_t(curDateTime));
        if (ActEnd)
            timesLeft = 0;
        InvokeRepeating("TimeControl", 0f, 1f);
        f_LoadTexture();
    }
    private string strTexBgRoot = "UI/TextureRemove/Activity/Tex_SkyFortuneBg";
    private string strTexRotateDirRoot = "UI/TextureRemove/Activity/Tex_SkyFortuneDir";
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
        UITexture TexBg = f_GetObject("TexBg").GetComponent<UITexture>();
        UITexture TexRotateDir = f_GetObject("TexRotateDir").GetComponent<UITexture>();
        if (TexBg.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
            TexBg.mainTexture = tTexture2D;

            Texture2D tTexRotateDir = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexRotateDirRoot);
            TexRotateDir.mainTexture = tTexRotateDir;
        }
    }
    private double timesLeft = 0;
    private void TimeControl()
    {
        DateTime tTime = new DateTime().AddSeconds(timesLeft);
f_GetObject("LabelTimeLeft").GetComponent<UILabel>().text = string.Format("[5AFF00] {0} date {1}:{2}:{3}", tTime.Day - 1, tTime.Hour , tTime.Minute, tTime.Second);
        //f_GetObject("LabelTimeLeft").GetComponent<UILabel>().text = "[5AFF00]" + CommonTools.f_GetStringBySecond((int)timesLeft);
        timesLeft--;
        if (timesLeft <= 0)
        {
            timesLeft = 0;
            ActEnd = true;
        }
    }
    /// <summary>
    /// 页面关闭
    /// </summary>
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        CancelInvoke("TimeControl");
    }
    /// <summary>
    /// 初始化消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnBlack", OnBtnBlackClick);
        f_RegClickEvent("BtnClose", OnBtnBlackClick);
        f_RegClickEvent("BtnSkyFortune", OnBtnSkyFortuneClick);

        FortuneCallback.m_ccCallbackSuc = OnFortuneSucCallback;
        FortuneCallback.m_ccCallbackFail = OnFortuneFailCallback;
        QueryCallback.m_ccCallbackSuc = OnQuerySucCallback;
        QueryCallback.m_ccCallbackFail = OnQueryFailCallback;
        RecordCallback.m_ccCallbackSuc = OnRecordSucCallback;
        RecordCallback.m_ccCallbackFail = OnRecordFailCallback;
    }
    /// <summary>
    /// 更新UI
    /// </summary>
    private void InitUI()
    {
        int vipLevel = UITool.f_GetNowVipLv();
        int times = Data_Pool.m_SkyFortunePool.mTimes;//已经祈福的次数
        int CanFortuneTimes = 0;//当前vip可祈福总次数
        List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_DescendFortuneSC.f_GetAll();
        int curIndex = 0;
        for (int i = 0; i < listData.Count; i++)
        {
            DescendFortuneDT dt = listData[i] as DescendFortuneDT;
            if (i == times)
                curIndex = dt.iId;
            if (dt.iVip <= vipLevel)
                CanFortuneTimes++;
        }
        if (times <= CanFortuneTimes)
        {
            DescendFortuneDT descendFortuneDT = glo_Main.GetInstance().m_SC_Pool.m_DescendFortuneSC.f_GetSC(curIndex) as DescendFortuneDT;
            if (descendFortuneDT != null)
            {
                f_GetObject("LabelFortunePrice").GetComponent<UILabel>().text = UITool.f_CountToChineseStr(descendFortuneDT.iParam);
                f_GetObject("LabelFortunePrice").transform.GetComponentInParent<UITable>().Reposition();
                string[] sztimes = descendFortuneDT.szTimes.Split(';');
                for (int i = 1; i <= 8; i++)
                {
f_GetObject("Item" + i).GetComponent<UILabel>().text = (int.Parse(sztimes[i - 1]) * 1.0f / 100) + "times";
                }
            }
        }
f_GetObject("LabelTimes").GetComponent<UILabel>().text = "[F5C040FF]Remaining：[F24B69FF]" + (CanFortuneTimes - times);
    }
    /// <summary>
    /// 显示记录
    /// </summary>
    private void ShowRecord()
    {
        GameObject RecordParent = f_GetObject("RecordParent");
        for (int i = RecordParent.transform.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(RecordParent.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < Data_Pool.m_SkyFortunePool.listRecordFortune.Count; i++)
        {
            GameObject RecordItem = GameObject.Instantiate(f_GetObject("RecordItem")) as GameObject;
            if (RecordItem != null)
            {
                RecordItem.GetComponent<UILabel>().text = Data_Pool.m_SkyFortunePool.listRecordFortune[i];
                RecordItem.gameObject.SetActive(true);
                RecordItem.transform.SetParent(RecordParent.transform);
                RecordItem.transform.localEulerAngles = Vector3.zero;
                RecordItem.transform.localScale = Vector3.one;
            }
        }
        RecordParent.GetComponent<UIGrid>().Reposition();
        f_GetObject("RecordScrollView").GetComponent<UIScrollView>().ResetPosition();
    }
    #region 按钮事件相关
    /// <summary>
    /// 点击黑色背景，关闭页面
    /// </summary>
    private void OnBtnBlackClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SkyFortunePage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 点击祈福
    /// </summary>
    private void OnBtnSkyFortuneClick(GameObject go, object obj1, object obj2)
    {
        if (ActEnd)
        {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Sự kiện đã kết thúc");
            return;
        }
        if (isShowFortune)
            return;
        int vipLevel = UITool.f_GetNowVipLv();
        int times = Data_Pool.m_SkyFortunePool.mTimes;//已经祈福的次数
        int CanFortuneTimes = 0;//当前vip可祈福总次数
        int curIndex = 0;
        List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_DescendFortuneSC.f_GetAll();
        for (int i = 0; i < listData.Count; i++)
        {
            DescendFortuneDT dt = listData[i] as DescendFortuneDT;
            if (i == times)
                curIndex = dt.iId;
            if (dt.iVip <= vipLevel)
                CanFortuneTimes++;
        }
        if (times >= CanFortuneTimes)//可祈福
        {
            if (CanFortuneTimes < glo_Main.GetInstance().m_SC_Pool.m_DescendFortuneSC.f_GetAll().Count)
UITool.Ui_Trip("Đã hết lượt, nâng VIP để có thêm");
            else
UITool.Ui_Trip("Đã hết lượt");
            return;
        }
        DescendFortuneDT descendFortuneDT = glo_Main.GetInstance().m_SC_Pool.m_DescendFortuneSC.f_GetSC(curIndex) as DescendFortuneDT;
        wasterSyceeCount = descendFortuneDT.iParam;
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee) < wasterSyceeCount)
        {
UITool.Ui_Trip("Không đủ KNB");
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        isShowFortune = true;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
        Data_Pool.m_SkyFortunePool.f_Fortune(FortuneCallback);
    }
    /// <summary>
    /// 显示祈福倍数结果
    /// </summary>
    private void ShowResult()
    {
string StrHint = "Chúc mừng bạn đã nhận được x" + (Data_Pool.m_SkyFortunePool.m_GetSyceeTime * 1.0f / 100) + " KNB";
        UITool.Ui_Trip(StrHint);
        string syceeCount = Mathf.Floor(Data_Pool.m_SkyFortunePool.m_GetSyceeTime * 1.0f / 100 * wasterSyceeCount).ToString("0"); 
        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        AwardPoolDT item1 = new AwardPoolDT();
        item1.f_UpdateByInfo((byte)1, 7, int.Parse(syceeCount));
        awardList.Add(item1);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });
        Data_Pool.m_SkyFortunePool.mTimes ++;
        InitUI();
        string userName = Data_Pool.m_UserData.m_szRoleName;
		Data_Pool.m_SkyFortunePool.listRecordFortune.Insert(0, "Chúc mừng [6FFF48FF][" + userName + "][FFFFFFFF]đã nhận được x[F44B6AFF]" + (Data_Pool.m_SkyFortunePool.m_GetSyceeTime * 1.0f / 100) + "[FFFFFFFF] KNB！");

        isShowFortune = false;
        ShowRecord();
        Data_Pool.m_SkyFortunePool.CheckRedPoint();

        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN);
    }
    #endregion
    #region 祈福回调
    /// <summary>
    /// 祈福成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnFortuneSucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);

        GameObject RotateDir = f_GetObject("RotateDir");

        if (RotateDir.GetComponent<TweenRotation>() != null)
        {
            Destroy(RotateDir.GetComponent<TweenRotation>());
        }
        //限定z于-360到0内
        float curAngleZ = RotateDir.transform.localEulerAngles.z;
        while (curAngleZ > 0)
        {
            curAngleZ -= 360;
        }
        while (curAngleZ <= -360)
        {
            curAngleZ += 360;
        }
        TweenRotation tr = RotateDir.AddComponent<TweenRotation>();

        float OneRotateTime = 0.25f;
        //0, -270   -270， -180
        float tempAngle = curAngleZ - GetIndexAngle(GetIndexByTimes(Data_Pool.m_SkyFortunePool.m_GetSyceeTime));
        if (tempAngle < 0)
        {
            tempAngle = 360 + tempAngle;
        }
        float totalAngle = 360 * 7 + tempAngle + UnityEngine.Random.Range(-22, 23);
        float totalRotateTime = (totalAngle * 1.0f / 360) * OneRotateTime;

        tr.from = new Vector3(0, 0, curAngleZ);
        tr.to = new Vector3(0, 0, curAngleZ - totalAngle);
        tr.duration = totalRotateTime;
        tr.animationCurve = f_GetObject("AnimTemp").GetComponent<TweenRotation>().animationCurve;
        tr.AddOnFinished(ShowResult);
    }
    private int GetIndexByTimes(int timescur)
    {
        int vipLevel = UITool.f_GetNowVipLv();
        int times = Data_Pool.m_SkyFortunePool.mTimes;//已经祈福的次数
        List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_DescendFortuneSC.f_GetAll();
        int curIndex = 0;
        for (int i = 0; i < listData.Count; i++)
        {
            DescendFortuneDT dt = listData[i] as DescendFortuneDT;
            if (i == times)
                curIndex = dt.iId;
        }
        DescendFortuneDT descendFortuneDT = glo_Main.GetInstance().m_SC_Pool.m_DescendFortuneSC.f_GetSC(curIndex) as DescendFortuneDT;

        string[] sztimes = descendFortuneDT.szTimes.Split(';');
        for (int i = 0; i < sztimes.Length; i++)
        {
            if (timescur == int.Parse(sztimes[i]))
            {
                return i;
            }
        }
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Máy chủ trả về nhiều lỗi sai");
        return 0;
    }
    private int GetIndexAngle(int index)
    {
        switch (index)
        {
            case 0:return 0;
            case 1:return -45;
            case 2:return -90;
            case 3:return -135;
            case 4:return -180;
            case 5:return -225;
            case 6:return -270;
            case 7:return -315;
            default:
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Máy chủ trả về nhiều lỗi sai");
                return 0;
        }
    }
    /// <summary>
    /// 祈福失败
    /// </summary>
    /// <param name="obj"></param>
    private void OnFortuneFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Không thành công! " + CommonTools.f_GetTransLanguage((int)obj));
        isShowFortune = false;
    }

    /// <summary>
    /// 查询成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnQuerySucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        //更新UI显示
        InitUI();
    }
    private void OnQueryFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Truy vấn không thành công" + CommonTools.f_GetTransLanguage((int)obj));
    }
    /// <summary>
    /// 记录成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnRecordSucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        ShowRecord();
        InitUI();
    }
    private void OnRecordFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Truy vấn không thành công" + CommonTools.f_GetTransLanguage((int)obj));
    }
    #endregion
}
