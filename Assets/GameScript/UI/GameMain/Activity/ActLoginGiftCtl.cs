using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using System;
/// <summary>
/// 登录送礼
/// </summary>
public class ActLoginGiftCtl : UIFramwork
{
    private UIWrapComponent _contentWrapComponet = null;
    private SocketCallbackDT QueryCallback = new SocketCallbackDT();//查询回调
    private SocketCallbackDT RequestGetCallback = new SocketCallbackDT();//领取回调
    private List<BasePoolDT<long>> listContent = new List<BasePoolDT<long>>();
    private List<NBaseSCDT> listLoginGiftDT = new List<NBaseSCDT>();
    ActLoginGiftDT currentSelectLoginGiftDT;//当前选中的dt
    private string strTexAdsRoot = "UI/TextureRemove/Activity/TexLoginGift";
    public void f_DestoryView()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 初始化数据,视图
    /// </summary>
    public void f_ShowView()
    {
        gameObject.SetActive(true);
        RequestGetCallback.m_ccCallbackSuc = OnGetSucCallback;
        RequestGetCallback.m_ccCallbackFail = OnGetFailCallback;

        QueryCallback.m_ccCallbackSuc = OnQuerySucCallback;
        QueryCallback.m_ccCallbackFail = OnQueryFailCallback;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_ActivityCommonData.f_QueryLoginGift(QueryCallback);
        UpdateContent();
        List<NBaseSCDT> listActLoginGiftDT = glo_Main.GetInstance().m_SC_Pool.m_ActLoginGiftSC.f_GetAll();
        for (int i = 0; i < listActLoginGiftDT.Count; i++)
        {
            ActLoginGiftDT actLoginGiftDT = listActLoginGiftDT[i] as ActLoginGiftDT;
            if (actLoginGiftDT.itype == 1 && CommonTools.f_CheckTime(GetTimeByTimeStr(actLoginGiftDT.szStartTime), GetTimeByTimeStr(actLoginGiftDT.szEndTime)))//时间匹配
            {
                string timeEnd = actLoginGiftDT.szEndTime;
                string[] timeEndArry = timeEnd.Split(';');
                DateTime endTime = new DateTime(int.Parse(timeEndArry[0]), int.Parse(timeEndArry[1]), int.Parse(timeEndArry[2]), int.Parse(timeEndArry[3]),
                    int.Parse(timeEndArry[4]), int.Parse(timeEndArry[5]));
                f_GetObject("LabelActTime").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1336), endTime.Year, endTime.Month, endTime.Day, endTime.Hour);
                break;
            }
        }
        f_LoadTexture();
    }
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载广告图
        UITexture TexAds = f_GetObject("TexAds").GetComponent<UITexture>();
		float windowAspect = (float)Screen.width /  (float) Screen.height ;
		MessageBox.ASSERT("" + windowAspect);
		//My Code
		if(windowAspect < 1.6)
		{
			f_GetObject("ScrollView").transform.localPosition = new Vector3(15f, 0f, 0f);
		}
		//
        //TexAds.transform.position = new Vector3(transform.position.x, TexAds.transform.position.y, TexAds.transform.position.z);
        if (TexAds.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexAdsRoot);
            TexAds.mainTexture = tTexture2D;
        }
    }
    /// <summary>
    /// 通过字符串获取日期
    /// </summary>
    /// <param name="StrTime"></param>
    /// <returns></returns>
    private DateTime GetTimeByTimeStr(string StrTime)
    {
        string[] timeArray = StrTime.Split(';');
        DateTime time = new DateTime(int.Parse(timeArray[0]), int.Parse(timeArray[1]), int.Parse(timeArray[2]), int.Parse(timeArray[3]),
            int.Parse(timeArray[4]), int.Parse(timeArray[5]));
        return time;
    }
    /// <summary>
    /// 更新内容
    /// </summary>
    private void UpdateContent()
    {
        listLoginGiftDT = glo_Main.GetInstance().m_SC_Pool.m_ActLoginGiftSC.f_GetAll();
        listContent.Clear();
        
        int loginGiftDay = Data_Pool.m_ActivityCommonData.LoginGiftDay;
        byte loginGiftFlag = Data_Pool.m_ActivityCommonData.LoginGiftFlag;
        int insertIndex = 0;
        for (int i = 0; i < listLoginGiftDT.Count; i++)
        {
            BasePoolDT<long> item = new BasePoolDT<long>();
            ActLoginGiftDT actLoginGiftDT = listLoginGiftDT[i] as ActLoginGiftDT;
            if (actLoginGiftDT.itype == 1 && CommonTools.f_CheckTime(GetTimeByTimeStr(actLoginGiftDT.szStartTime), GetTimeByTimeStr(actLoginGiftDT.szEndTime)))//时间匹配.同时间段才有效
            {
                item.iId = actLoginGiftDT.iId;
                if (item.iId > loginGiftDay || !BitTool.BitTest(loginGiftFlag,(ushort) item.iId))//排序（未领取的插入到前面）
                {
                    listContent.Insert(insertIndex, item);
                    insertIndex++;
                }
                else
                {
                    listContent.Add(item);
                }
            }
        }

        if (_contentWrapComponet == null)
        {
            _contentWrapComponet = new UIWrapComponent(220, 1, 1400, 8, f_GetObject("GridLoginGiftParent"), f_GetObject("LoginGiftItem"), listContent, OnContentItemUpdate, null);
        }
        _contentWrapComponet.f_ResetView();
        _contentWrapComponet.f_UpdateView();
    }

    private void OnContentItemUpdate(Transform item, BasePoolDT<long> data)
    {
        int loginGiftDay = Data_Pool.m_ActivityCommonData.LoginGiftDay;
        ActLoginGiftItem actLoginGiftItem = item.GetComponent<ActLoginGiftItem>();
        ActLoginGiftDT actLoginGiftDT = glo_Main.GetInstance().m_SC_Pool.m_ActLoginGiftSC.f_GetSC((int)data.iId) as ActLoginGiftDT;

        //TsuComment
        //string progress = CommonTools.f_GetTransLanguage(1337) + (loginGiftDay > actLoginGiftDT.iId ? actLoginGiftDT.iId : loginGiftDay) + "/" + actLoginGiftDT.iId;
        //actLoginGiftItem.SetData(actLoginGiftDT.iId.ToString(), actLoginGiftDT.iType1, actLoginGiftDT.iID1, actLoginGiftDT.iCount1, actLoginGiftDT.iType2, actLoginGiftDT.iID2, actLoginGiftDT.iCount2,
        //    actLoginGiftDT.iType3, actLoginGiftDT.iID3, actLoginGiftDT.iCount3, actLoginGiftDT.iType4, actLoginGiftDT.iID4, actLoginGiftDT.iCount4, progress);

        //TsuCode
        string progress = CommonTools.f_GetTransLanguage(1337) + (loginGiftDay > actLoginGiftDT.iCondition ? actLoginGiftDT.iCondition : loginGiftDay) + "/" + actLoginGiftDT.iCondition;
        actLoginGiftItem.SetData(actLoginGiftDT.iCondition.ToString(), actLoginGiftDT.iType1, actLoginGiftDT.iID1, actLoginGiftDT.iCount1, actLoginGiftDT.iType2, actLoginGiftDT.iID2, actLoginGiftDT.iCount2,
           actLoginGiftDT.iType3, actLoginGiftDT.iID3, actLoginGiftDT.iCount3, actLoginGiftDT.iType4, actLoginGiftDT.iID4, actLoginGiftDT.iCount4, progress);
        //--------
        f_RegClickEvent(actLoginGiftItem.f_GetAwardObj(1), OnAwardIconClick, actLoginGiftDT, 1);
        f_RegClickEvent(actLoginGiftItem.f_GetAwardObj(2), OnAwardIconClick, actLoginGiftDT, 2);
        f_RegClickEvent(actLoginGiftItem.f_GetAwardObj(3), OnAwardIconClick, actLoginGiftDT, 3);
        f_RegClickEvent(actLoginGiftItem.f_GetAwardObj(4), OnAwardIconClick, actLoginGiftDT, 4);
        byte loginGiftFlag = Data_Pool.m_ActivityCommonData.LoginGiftFlag;
        item.Find("BtnGet").gameObject.SetActive(false);
        item.Find("BtnHasGet").gameObject.SetActive(false);
        item.Find("BtnWaitGet").gameObject.SetActive(false);

        //if (data.iId > loginGiftDay)//显示领取灰显
        if (actLoginGiftDT.iCondition > loginGiftDay)//显示领取灰显
        {
            item.Find("BtnWaitGet").gameObject.SetActive(true);
        }
        else
        {
            //if (BitTool.BitTest(loginGiftFlag,(ushort) data.iId))
            if (BitTool.BitTest(loginGiftFlag, (ushort)actLoginGiftDT.iCondition))
            {
                //已领取
                item.Find("BtnHasGet").gameObject.SetActive(true);
            }
            else
            {
                //可领取
                item.Find("BtnGet").gameObject.SetActive(true);
                f_RegClickEvent(item.Find("BtnGet").gameObject, OnGetClick, data);
            }
        }
    }
    /// <summary>
    /// 点击奖励icon弹出详细信息
    /// </summary>
    private void OnAwardIconClick(GameObject go, object obj1, object obj2)
    {
        ActLoginGiftDT actLoginGiftDT = obj1 as ActLoginGiftDT;
        ResourceCommonDT commonData = new ResourceCommonDT();
        int type = 0;
        int id = 0;
        int num = 0;
        switch ((int)obj2)
        {
            case 1:
                type = actLoginGiftDT.iType1;
                id = actLoginGiftDT.iID1;
                num = actLoginGiftDT.iCount1;
                break;
            case 2:
                type = actLoginGiftDT.iType2;
                id = actLoginGiftDT.iID2;
                num = actLoginGiftDT.iCount2;
                break;
            case 3:
                type = actLoginGiftDT.iType3;
                id = actLoginGiftDT.iID3;
                num = actLoginGiftDT.iCount3;
                break;
            case 4:
                type = actLoginGiftDT.iType4;
                id = actLoginGiftDT.iID4;
                num = actLoginGiftDT.iCount4;
                break;
        }
        commonData.f_UpdateInfo((byte)type, id, num);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
    }
    /// <summary>
    /// 点击领取按钮事件
    /// </summary>
    private void OnGetClick(GameObject go, object obj1, object obj2)
    {
        BasePoolDT<long> data = obj1 as BasePoolDT<long>;
        ActLoginGiftDT actLoginGiftDT = glo_Main.GetInstance().m_SC_Pool.m_ActLoginGiftSC.f_GetSC((int)data.iId) as ActLoginGiftDT; //TsuCode
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_ActivityCommonData.f_GetLoginGift((byte)actLoginGiftDT.iCondition,RequestGetCallback);
        currentSelectLoginGiftDT = glo_Main.GetInstance().m_SC_Pool.m_ActLoginGiftSC.f_GetSC((int)data.iId) as ActLoginGiftDT;
    }
    #region 领取回调
    /// <summary>
    /// 领取成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetSucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UpdateContent();
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "领取成功！");
        //更新UI显示
        //_contentWrapComponet.f_UpdateView();
        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        AwardPoolDT item1 = new AwardPoolDT();
        item1.f_UpdateByInfo((byte)currentSelectLoginGiftDT.iType1, currentSelectLoginGiftDT.iID1, currentSelectLoginGiftDT.iCount1);
        awardList.Add(item1);
        AwardPoolDT item2 = new AwardPoolDT();
        item2.f_UpdateByInfo((byte)currentSelectLoginGiftDT.iType2, currentSelectLoginGiftDT.iID2, currentSelectLoginGiftDT.iCount2);
        awardList.Add(item2);
        AwardPoolDT item3 = new AwardPoolDT();
        item3.f_UpdateByInfo((byte)currentSelectLoginGiftDT.iType3, currentSelectLoginGiftDT.iID3, currentSelectLoginGiftDT.iCount3);
        awardList.Add(item3);
        AwardPoolDT item4 = new AwardPoolDT();
        item4.f_UpdateByInfo((byte)currentSelectLoginGiftDT.iType4, currentSelectLoginGiftDT.iID4, currentSelectLoginGiftDT.iCount4);
        awardList.Add(item4);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });
    }
    private void OnGetFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1338) + CommonTools.f_GetTransLanguage((int)obj));
    }
    /// <summary>
    /// 查询成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnQuerySucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        //更新UI显示
        UpdateContent();
    }
    private void OnQueryFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1339) + CommonTools.f_GetTransLanguage((int)obj));
    }
    #endregion
}

