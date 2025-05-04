using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using System;
/// <summary>
/// 十万元宝活动
/// </summary>
public class TenSyceeCtl : UIFramwork
{
    private UIWrapComponent _contentWrapComponet = null;
    private SocketCallbackDT InfoCallback = new SocketCallbackDT();//十万元宝信息回调
    private SocketCallbackDT RequestUserGetCallback = new SocketCallbackDT();//领取回调
    private TenSyceePoolDT currentTenPoolDT;
    private string strTexAdsRoot = "UI/TextureRemove/Activity/TexOpenServFund";
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
        InfoCallback.m_ccCallbackSuc = OnInfoSucCallback;
        InfoCallback.m_ccCallbackFail = OnInfoFailCallback;
        RequestUserGetCallback.m_ccCallbackSuc = OnGetSucCallback;
        RequestUserGetCallback.m_ccCallbackFail = OnGetFailCallback;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_TenSyceePool.f_RequestSyceeInfo(InfoCallback);
        InitUI();
        f_LoadTexture();
    }
    /// <summary>
    /// 初始化UI
    /// </summary>
    private void InitUI()
    {
        int vipExp = (int)Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vip);
        int tVipLv = 0;
        int tNeedExp = 0;
        UITool.f_GetVipLvAndNeedExp(vipExp , ref tVipLv , ref tNeedExp);

        f_GetObject("VipLevel").GetComponent<UILabel>().text = tVipLv.ToString();
        f_GetObject("SliderVipExp").GetComponent<UISlider>().value = (float)Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vip) / (float)tNeedExp;
    }
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载广告图
        UITexture TexAds = f_GetObject("TexAds").GetComponent<UITexture>();
        //TexAds.transform.position = new Vector3(transform.position.x , TexAds.transform.position.y , TexAds.transform.position.z);
        if (TexAds.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexAdsRoot);
            TexAds.mainTexture = tTexture2D;
        }
    }
    /// <summary>
    /// 更新内容
    /// </summary>
    private void UpdateContent()
    {
        Data_Pool.m_TenSyceePool.CheckRedPoint();
        Data_Pool.m_TenSyceePool.f_SortPoolData();
        List<BasePoolDT<long>> listData = CommonTools.f_CopyPoolDTArrayToNewList(Data_Pool.m_TenSyceePool.f_GetAll());
        //排序(已领取的在后面)



        //for (int i = 0 ; i < (listData.Count - 1) ; i++)
        //{
        //    for (int j = i + 1 ; j < listData.Count ; j++)
        //    {
        //        TenSyceePoolDT poolDTI = listData [i] as TenSyceePoolDT;
        //        TenSyceePoolDT poolDTJ = listData [j] as TenSyceePoolDT;
        //        if (poolDTI.mIsGet && !poolDTJ.mIsGet)
        //        {
        //            BasePoolDT<long> temp = listData [i];
        //            listData [i] = listData [j];
        //            listData [j] = temp;
        //        }
        //    }
        //}

        if (_contentWrapComponet == null)
        {
            _contentWrapComponet = new UIWrapComponent(230 , 1 , 1400 , 8 , f_GetObject("GridContentParent") , f_GetObject("ContentItem") , listData , OnContentItemUpdate , null);
        }
        _contentWrapComponet.f_ResetView();
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnContentItemUpdate(Transform item , BasePoolDT<long> data)
    {
        TenSyceeItemCtl tenSyceeItemCtl = item.GetComponent<TenSyceeItemCtl>();

        TenSyceePoolDT poolDT = data as TenSyceePoolDT;

        int createRoleTime = Data_Pool.m_UserData.m_CreateTime;
        //int serverTime = GameSocket.GetInstance().f_GetServerTime();

        //DateTime dataTime1 = ccMath.time_t2DateTime(createRoleTime);
        //long dataTimeStart = ccMath.DateTime2time_t(new DateTime(dataTime1.Year , dataTime1.Month , dataTime1.Day));

        bool isGetCondition = false;
        int dayCount = Data_Pool.m_UserData.m_LoginDays;   //(int)(serverTime - dataTimeStart) / (24 * 60 * 60) + 1;
        tenSyceeItemCtl.InitData(dayCount , poolDT);
        if (dayCount >= poolDT.mSyceeAwardDT.iCondition)
        {
            isGetCondition = true;
        }

        tenSyceeItemCtl.mBtnHasGet.SetActive(poolDT.mIsGet);
        tenSyceeItemCtl.mBtnGet.SetActive(!poolDT.mIsGet && isGetCondition);
        tenSyceeItemCtl.mBtnGetGay.SetActive(!poolDT.mIsGet && !isGetCondition);
        f_RegClickEvent(tenSyceeItemCtl.mBtnGet , OnGetClick , poolDT);
    }
    /// <summary>
    /// 点击奖励icon弹出详细信息
    /// </summary>
    private void OnAwardIconClick(GameObject go , object obj1 , object obj2)
    {
        ResourceCommonDT commonData = (ResourceCommonDT)obj1;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage , UIMessageDef.UI_OPEN , commonData);
    }

    /// <summary>
    /// 点击领取按钮事件
    /// </summary>
    private void OnGetClick(GameObject go , object obj1 , object obj2)
    {
        currentTenPoolDT = obj1 as TenSyceePoolDT;
        Data_Pool.m_TenSyceePool.f_GetSyceeAward((short)currentTenPoolDT.mSyceeAwardDT.iId , RequestUserGetCallback);
        UITool.f_OpenOrCloseWaitTip(true);
    }
    #region 服务器回调
    /// <summary>
    /// 信息成功回调
    /// </summary>
    private void OnInfoSucCallback(object obj)
    {
        UpdateContent();
        UITool.f_OpenOrCloseWaitTip(false);
    }
    /// <summary>
    /// 信息失败回调
    /// </summary>
    private void OnInfoFailCallback(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage , UIMessageDef.UI_OPEN , CommonTools.f_GetTransLanguage(1375) + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }
    /// <summary>
    /// 领取成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetSucCallback(object obj)
    {
        InitUI();
        //更新UI显示
        Data_Pool.m_TenSyceePool.f_SortPoolData();
        _contentWrapComponet.f_UpdateList(Data_Pool.m_TenSyceePool.f_GetAll());
        _contentWrapComponet.f_UpdateView();

        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage , UIMessageDef.UI_OPEN ,
            new object [] { CommonTools.f_GetListAwardPoolDT(currentTenPoolDT.mSyceeAwardDT.szAward) });
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnGetFailCallback(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage , UIMessageDef.UI_OPEN , CommonTools.f_GetTransLanguage(1376) + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }
    #endregion
}