using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 全民福利页面
/// </summary>
public class ActOpenWelfareCtl : UIFramwork {
    private UIWrapComponent _contentWrapComponet = null;
    private SocketCallbackDT QueryCallback = new SocketCallbackDT();//查询回调
    private SocketCallbackDT RequestGetCallback = new SocketCallbackDT();//领取回调 (添加购买回调)
    private List<BasePoolDT<long>> listOpenServFundPoolDT = new List<BasePoolDT<long>>();
    OpenServFundDT currentSelectDT;//当前选中的dt
    private ccCallback onBtnGotoCallback;
    private string strTexAdsRoot = "UI/TextureRemove/Activity/TexOpenServFund";
    public void f_DestoryView()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 初始化数据,视图
    /// </summary>
    public void f_ShowView(ccCallback OnBtnGotoClickCallback)
    {
        gameObject.SetActive(true);
        RequestGetCallback.m_ccCallbackSuc = OnGetSucCallback;
        RequestGetCallback.m_ccCallbackFail = OnGetFailCallback;

        QueryCallback.m_ccCallbackSuc = OnQuerySucCallback;
        QueryCallback.m_ccCallbackFail = OnQueryFailCallback;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_OpenServFundPool.f_QueryBuyInfo(QueryCallback);
        Data_Pool.m_OpenServFundPool.f_QueryOpenServFundInfo(QueryCallback);
        this.onBtnGotoCallback = OnBtnGotoClickCallback;
        f_RegClickEvent("BtnGoto", OnBtnGotoClick);
        f_LoadTexture();
    }
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载广告图
        UITexture TexAds = f_GetObject("TexAds").GetComponent<UITexture>();
        //TexAds.transform.position = new Vector3(transform.position.x, TexAds.transform.position.y, TexAds.transform.position.z);
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
        listOpenServFundPoolDT = CommonTools.f_CopyPoolDTArrayToNewList(Data_Pool.m_OpenServFundPool.f_GetAll());
        for (int i = listOpenServFundPoolDT.Count - 1; i >= 0; i--)
        {
            OpenServFundPoolDT openServFundPoolDT = listOpenServFundPoolDT[i] as OpenServFundPoolDT;
            if (openServFundPoolDT.eOpenServFundType != EM_OpenServFundType.OpenWelfare)
            {
                listOpenServFundPoolDT.RemoveAt(i);
            }
        }
        //排序，已领取的下沉
        for (int i = 0; i < listOpenServFundPoolDT.Count - 1; i++)
        {
            for (int j = i + 1; j < listOpenServFundPoolDT.Count; j++)
            {
                OpenServFundPoolDT temp = listOpenServFundPoolDT[i] as OpenServFundPoolDT;
                OpenServFundPoolDT data = listOpenServFundPoolDT[j] as OpenServFundPoolDT;
                if (temp.m_buyTimes > data.m_buyTimes)
                {
                    listOpenServFundPoolDT[i] = data;
                    listOpenServFundPoolDT[j] = temp;
                }
            }
        }

        f_GetObject("GridParent").SetActive(true);
        if (_contentWrapComponet == null)
        {
            _contentWrapComponet = new UIWrapComponent(230, 1, 1400, 8, f_GetObject("GridParent"), f_GetObject("OpenWelfareItem"), listOpenServFundPoolDT, OnContentItemUpdate, null);
        }
        _contentWrapComponet.f_ResetView();
        _contentWrapComponet.f_UpdateList(listOpenServFundPoolDT);
        _contentWrapComponet.f_UpdateView();
        f_GetObject("LabelHasGetCount").GetComponent<UILabel>().text = Data_Pool.m_OpenServFundPool.m_buyFundCount.ToString();
    }
    private void OnContentItemUpdate(Transform item, BasePoolDT<long> data)
    {
        ActOpenWelfareItem openServFundItem = item.GetComponent<ActOpenWelfareItem>();
        OpenServFundPoolDT openServFundPoolDT = data as OpenServFundPoolDT;
        OpenServFundDT openServFundDT = openServFundPoolDT.m_OpenServFundDT;
        openServFundItem.SetData((EM_ResourceType)openServFundDT.iGiftTabID, openServFundDT.iGiftID, openServFundDT.iGiftCount, openServFundDT.iCondiction);
        f_RegClickEvent(openServFundItem.GetAwardObj(), OnAwardIconClick, openServFundDT);
        int buyTimes = openServFundPoolDT.m_buyTimes;
        item.Find("BtnGet").gameObject.SetActive(false);
        item.Find("BtnHasGet").gameObject.SetActive(false);
        item.Find("BtnWaitGet").gameObject.SetActive(false);
        if (Data_Pool.m_OpenServFundPool.m_buyFundCount >= openServFundDT.iCondiction)
        {
            if (buyTimes > 0)
            {
                item.Find("BtnHasGet").gameObject.SetActive(true);
            }
            else
            {
                item.Find("BtnGet").gameObject.SetActive(true);
                f_RegClickEvent(openServFundItem.transform.Find("BtnGet").gameObject, OnGetClick, data);
            }
        }
        else
        {
            item.Find("BtnWaitGet").gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 点击前往
    /// </summary>
    private void OnBtnGotoClick(GameObject go,object obj1,object obj2)
    {
        if (onBtnGotoCallback != null)
            onBtnGotoCallback(null);
    }
    /// <summary>
    /// 点击奖励icon弹出详细信息
    /// </summary>
    private void OnAwardIconClick(GameObject go, object obj1, object obj2)
    {
        OpenServFundDT openServFundDT = obj1 as OpenServFundDT;
        ResourceCommonDT commonData = new ResourceCommonDT();
        commonData.f_UpdateInfo((byte)openServFundDT.iGiftTabID, openServFundDT.iGiftID, openServFundDT.iGiftCount);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
    }
    /// <summary>
    /// 点击购买基金
    /// </summary>
    private void OnBuyClick(GameObject go, object obj1, object ob2)
    {
        Data_Pool.m_OpenServFundPool.m_HasBuyOpenServFund = true;
        UpdateContent();
    }
    /// <summary>
    /// 点击领取按钮事件
    /// </summary>
    private void OnGetClick(GameObject go, object obj1, object obj2)
    {
        OpenServFundPoolDT openServFundPoolDT = obj1 as OpenServFundPoolDT;
        currentSelectDT = openServFundPoolDT.m_OpenServFundDT;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_OpenServFundPool.f_Get(EM_OpenServFundType.OpenWelfare, (int)openServFundPoolDT.iId, RequestGetCallback);
    }
    #region 领取回调
    /// <summary>
    /// 领取成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetSucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        //更新UI显示
        _contentWrapComponet.f_UpdateView();
        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        AwardPoolDT item1 = new AwardPoolDT();
        item1.f_UpdateByInfo((byte)currentSelectDT.iGiftTabID, currentSelectDT.iGiftID, currentSelectDT.iGiftCount);
        awardList.Add(item1);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });
        Data_Pool.m_OpenServFundPool.QueryOpenServFundInfoSucRedPoint(null);
    }
    private void OnGetFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1354) + CommonTools.f_GetTransLanguage((int)obj));
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
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1355) + CommonTools.f_GetTransLanguage((int)obj));
    }
    #endregion
}
