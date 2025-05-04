using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class RunningManElitePage :UIFramwork
{
    private GameObject mEliteItemParent;
    private GameObject mEliteItem;

    private UILabel mPrestigeLabel;
    private UILabel mHistoryStarLabel;
    private UILabel mCurStarLabel;
    private UILabel mLeftStarLabel;
    private UILabel mResetTimesLabel;

    private List<BasePoolDT<long>> _eliteList;
    private UIWrapComponent _eliteListWrapComponent;
    private UIWrapComponent mEliteListWrapComponent
    {
        get
        {
            if (_eliteListWrapComponent == null)
            {
                _eliteList = Data_Pool.m_RunningManPool.m_EliteList;
                //_eliteList.Reverse();
                _eliteListWrapComponent = new UIWrapComponent(220, 1, 800, 10, mEliteItemParent, mEliteItem, _eliteList,f_EliteItemUpdateByInfo , f_EliteItemClickHandle);
            }
            return _eliteListWrapComponent;
        }
    }


    private int eliteLeftTimes;
    private RunningManElitePoolDT mData;

    private GameObject mNullProperty;
    private GameObject mLabel_MoreAttr;
    private GameObject mShowItem;
    private UIGrid mItemGrid;
    private UIScrollView mScrollView;
    private List<UILabel> mShowItems = new List<UILabel>();

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mEliteItemParent = f_GetObject("EliteItemParent");
        mEliteItem = f_GetObject("TollgateItem");
        //f_RegClickEvent("MaskClose", f_MaskClose);
        f_RegClickEvent("BtnBack", f_MaskClose);
        f_RegClickEvent("BtnClothArray", f_BtnClothArray);
        f_RegClickEvent("BtnChallenge", f_BtnChallenge);

        mPrestigeLabel = f_GetObject("PrestigeLabel").GetComponent<UILabel>();
        mHistoryStarLabel = f_GetObject("HistoryStarLabel").GetComponent<UILabel>();
        mCurStarLabel = f_GetObject("CurStarLabel").GetComponent<UILabel>();
        mLeftStarLabel = f_GetObject("LeftStarLabel").GetComponent<UILabel>();
        mResetTimesLabel = f_GetObject("ResetTimesLabel").GetComponent<UILabel>();

        mNullProperty = f_GetObject("NullProperty");
        mShowItem = f_GetObject("ShowItem");
        mItemGrid = f_GetObject("ItemGrid").GetComponent<UIGrid>();
        mScrollView = f_GetObject("ScrollView").GetComponent<UIScrollView>();

        f_RegClickEvent("BtnEliteChallenge", f_BtnEliteChallenge);
        f_RegClickEvent("BtnRMShop", f_BtnRMShop);
        f_RegClickEvent("BtnNorChallenge", f_BtnNorChallenge);


    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);

        RunningManElitePoolDT tPoolDt = Data_Pool.m_RunningManPool.f_GetCurElitePoolDt();
        f_UpdateByInfo(tPoolDt);
        mEliteListWrapComponent.f_ResetView();
        //mEliteListWrapComponent.f_ViewGotoRealIdx((int)tPoolDt.iId-1, 3);
        f_LoadTexture();
    }
    private string strTexBgRoot = "UI/TextureRemove/Challenge/Texture_RunningManElite";
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        return;
        //加载背景图
        UITexture Texture_BG = f_GetObject("Texture_BG").GetComponent<UITexture>();
        if (Texture_BG.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
            Texture_BG.mainTexture = tTexture2D;
        }
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_UpdateLabelInfo()
    {
        //更新文字信息
        mPrestigeLabel.text = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Prestige).ToString();
        mHistoryStarLabel.text = Data_Pool.m_RunningManPool.m_iHistoryStarNum.ToString();
        mCurStarLabel.text = Data_Pool.m_RunningManPool.m_iCurStarNum.ToString();
        mLeftStarLabel.text = Data_Pool.m_RunningManPool.m_iLeftStarNum.ToString();
        //mHistoryStarTable.repositionNow = true;
        //mCurStarTable.repositionNow = true;
        //mLeftStarTable.repositionNow = true;
        mResetTimesLabel.text = (Data_Pool.m_RunningManPool.m_iResetTimesLimit - Data_Pool.m_RunningManPool.m_iResetTimes).ToString();
    }

    private void f_ShowReferrerProperty(List<RunningManBuffProperty> dataList)
    {
        mNullProperty.SetActive(dataList.Count == 0);
        //mLabel_MoreAttr.SetActive(dataList.Count > 2);
        for (int i = 0; i < dataList.Count; i++)
        {
            if (i < mShowItems.Count)
            {
                mShowItems[i].gameObject.SetActive(true);
                mShowItems[i].text = UITool.f_GetProName((EM_RoleProperty)dataList[i].m_iPropertyType) + ":";
                mShowItems[i].transform.Find("NumProperty").GetComponent<UILabel>().text = "[00FF00FF]+" + dataList[i].m_iPropertyValue / 10000.0f * 100 + "%";
                continue;
            }
            GameObject tItem = NGUITools.AddChild(mItemGrid.gameObject, mShowItem.gameObject);
            tItem.SetActive(true);
            UILabel tLabel = tItem.GetComponent<UILabel>();
            tLabel.text = UITool.f_GetProName((EM_RoleProperty)dataList[i].m_iPropertyType) + ":";
            UILabel nLabel = tItem.transform.Find("NumProperty").GetComponent<UILabel>();
            nLabel.text = "[00FF00FF]+" + dataList[i].m_iPropertyValue / 10000.0f * 100 + "%";
            mShowItems.Add(tLabel);
        }
        if (mShowItems.Count > dataList.Count)
        {
            for (int i = dataList.Count; i < mShowItems.Count; i++)
            {
                mShowItems[i].gameObject.SetActive(false);
            }
        }
        mItemGrid.repositionNow = true;
        mItemGrid.Reposition();
        mScrollView.ResetPosition();
    }

    private void f_BtnEliteChallenge(GameObject go, object value1, object value2)
    {
        //精英挑战
        if (Data_Pool.m_RunningManPool.m_iHistoryChapter <= 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(816));
            return;
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManElitePage, UIMessageDef.UI_OPEN);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManPage, UIMessageDef.UI_CLOSE);

    }

    private void f_BtnRMShop(GameObject go, object value1, object value2)
    {
        //过关斩将商店
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopMutiCommonPage, UIMessageDef.UI_OPEN, EM_ShopMutiType.RunningMan);
    }

    private void f_BtnNorChallenge(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManElitePage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManPage, UIMessageDef.UI_OPEN);
    }

    private void f_UpdateByInfo(RunningManElitePoolDT poolDt)
    {
        mData = poolDt;
        f_UpdateLabelInfo();
        f_ShowReferrerProperty(Data_Pool.m_RunningManPool.m_BuffPropertyList);
    }

    private void f_EliteItemUpdateByInfo(Transform tf, BasePoolDT<long> dt)
    {
        RunningManEliteItem tItem = tf.GetComponent<RunningManEliteItem>();
        tItem.f_UpdateByInfo((RunningManElitePoolDT)dt, mData.m_Template.iId);
    }

    private void f_EliteItemClickHandle(Transform tf, BasePoolDT<long> dt)
    {
        RunningManElitePoolDT tItem = (RunningManElitePoolDT)dt;
        if (tItem.iId == mData.iId)
            return;
        if (dt.iId > Data_Pool.m_RunningManPool.m_iHistoryChapter)
        {
UITool.Ui_Trip(string.Format("Đã vượt qua {0}", dt.iId * GameParamConst.RMTollgateNumPreChap));
            return;
        }
        else if (dt.iId > Data_Pool.m_RunningManPool.m_iEliteFirstProg + 1)
        {
UITool.Ui_Trip(string.Format("Đã vượt qua {0}", tItem.m_szPreTollgateName));
            return;
        }
        f_UpdateByInfo(tItem);
        mEliteListWrapComponent.f_UpdateView();
    }
    

    private void f_BtnChallenge(GameObject go, object value1, object value2)
    {
        //if (eliteLeftTimes <= 0)
        //{
        //    UITool.Ui_Trip("Đã hết lượt khiêu chiến hôm nay！");
        //    return;
        //}
        //UITool.f_OpenOrCloseWaitTip(true);
        //SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        //socketCallbackDt.m_ccCallbackSuc = f_CallbackChallenge;
        //socketCallbackDt.m_ccCallbackFail = f_CallbackChallenge;
        //Data_Pool.m_RunningManPool.f_RunningManElite((ushort)mData.m_Template.iId, mData.m_Template.iSceneId, socketCallbackDt);

        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManEliteChallengePage, UIMessageDef.UI_OPEN, mData);
    }

    private void f_CallbackChallenge(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            //挑战
            ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManPage, UIMessageDef.UI_CLOSE);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManElitePage, UIMessageDef.UI_CLOSE);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.BattleMain);
        }
        else
        {
MessageBox.ASSERT("Passed and killed the general Tinh Anh，code：" + result);
        }
    }

    private void f_MaskClose(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManElitePage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }

    private void f_BtnClothArray(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ClothArrayPage, UIMessageDef.UI_OPEN);
    }

   
}
