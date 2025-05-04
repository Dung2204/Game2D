using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class LegionCreatePage : UIFramwork
{
    private GameObject mCancelCreateBtn;
    private GameObject mCreateBtn;
    private UIInput mLegionNameInput;
    private UILabel mCostLabel;

    private UI2DSprite[] mFlags;
    private UIScrollView mFlagScrollView;
    private UIGrid mFlagGrid;


    private byte _iIconId;
    private byte _iFrameId;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mCancelCreateBtn = f_GetObject("CancelCreateBtn");
        mCreateBtn = f_GetObject("CreateBtn");
        mLegionNameInput = f_GetObject("LegionNameInput").GetComponent<UIInput>();
        mCostLabel = f_GetObject("CostLabel").GetComponent<UILabel>();
        f_RegClickEvent(mCancelCreateBtn, f_CancelCreateBtn);
        f_RegClickEvent("MaskClose", f_CancelCreateBtn);
        f_RegClickEvent(mCreateBtn, f_CreateBtn);
        mFlagScrollView = f_GetObject("FlagScrollView").GetComponent<UIScrollView>();
        mFlagGrid = f_GetObject("FlagGrid").GetComponent<UIGrid>();
        mFlags = new UI2DSprite[LegionConst.LEGION_ICON_LIST.Length];
        GameObject tSourceItem = f_GetObject("FlagItem");
        for (int i = 0; i < mFlags.Length; i++)
        {
            GameObject go = NGUITools.AddChild(mFlagGrid.gameObject, tSourceItem);
            go.SetActive(true);
            go.name = string.Format("Flage{0}", i);
            mFlags[i] = go.GetComponent<UI2DSprite>();
            f_RegClickEvent(mFlags[i].gameObject, f_FlagClick, i);
            mFlags[i].sprite2D = UITool.f_GetIconSprite(LegionConst.LEGION_ICON_LIST[i]);
        }
        mFlagGrid.repositionNow = true;
        mFlagGrid.Reposition();
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        mCostLabel.text = LegionConst.LEGION_CREATE_COST.ToString();
        mLegionNameInput.defaultText = CommonTools.f_GetTransLanguage(453);
        mLegionNameInput.value = string.Empty; 
        f_SelectFlagIdx(0);
        _iFrameId = 0;
        mFlagScrollView.ResetPosition();
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_CancelCreateBtn(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionCreatePage, UIMessageDef.UI_CLOSE);
    }

    private void f_CreateBtn(GameObject go, object value1, object value2)
    {
        string legionName = mLegionNameInput.value.Trim();
        int byteNum = ccMath.f_GetStringBytesLength(legionName);
        int syceeNum = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee);
        int vipLv = UITool.f_GetNowVipLv();
        int tNow = GameSocket.GetInstance().f_GetServerTime();
        if (tNow - LegionMain.GetInstance().m_LegionInfor.m_iIOTime <= LegionConst.LEGION_JOIN_AGAIN_TIME_DIS)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(454));
            return;
        }
        else if (!Data_Pool.m_BlockWordPool.f_CheckValidity(ref legionName))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(455));
            mLegionNameInput.value = legionName;
            return;
        }
        else if (vipLv < LegionConst.LEGION_CREATE_VIPLV)
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(456), LegionConst.LEGION_CREATE_VIPLV));
            return;
        }
        else if (!UITool.f_IsEnoughMoney(EM_MoneyType.eUserAttr_Sycee, LegionConst.LEGION_CREATE_COST,true, true, this))
        {
            return;
        }
        else if (string.IsNullOrEmpty(legionName))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(457));
            return;
        }
        else if (byteNum < LegionConst.LEGION_NAME_BYTE_MIN_NUM)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(458));
            return;
        }
        else if (byteNum > LegionConst.LEGION_NAME_BYTE_MAX_NUM)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(459));
            return;
        }

        string tShowStr = string.Format(CommonTools.f_GetTransLanguage(460), LegionConst.LEGION_CREATE_COST);
        PopupMenuParams tParam = new PopupMenuParams(CommonTools.f_GetTransLanguage(461), tShowStr, CommonTools.f_GetTransLanguage(462), f_SureCreateLegion, CommonTools.f_GetTransLanguage(463));
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);
    }

    private void f_SureCreateLegion(object result)
    {
        string legionName = mLegionNameInput.value.Trim();
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_CreateResult;
        socketCallbackDt.m_ccCallbackFail = f_CreateResult;
        LegionMain.GetInstance().m_LegionInfor.f_LegionFound(_iIconId, _iFrameId, legionName, socketCallbackDt);
    }

    private void f_CreateResult(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(464));
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionCreatePage, UIMessageDef.UI_CLOSE);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionListPage, UIMessageDef.UI_CLOSE);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionMenuPage, UIMessageDef.UI_OPEN, true);
        }
        else if ((int)result == (int)eMsgOperateResult.eOR_LegionDuplicateName)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(465));
        }
        else
        {
            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(466) + result);
        }
    }

    private void f_FlagClick(GameObject go, object value1, object value2)
    {
        int idx = (int)value1;
        f_SelectFlagIdx(idx);
    }

    private void f_SelectFlagIdx(int idx)
    {
        for (int i = 0; i < mFlags.Length; i++)
        {
            mFlags[i].transform.Find("FlagSelect").gameObject.SetActive(false);
        }
        mFlags[idx].transform.Find("FlagSelect").gameObject.SetActive(true);
        _iIconId = LegionConst.LEGION_ICON_LIST[idx];
    }
}
