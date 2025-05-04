using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 批量操作页面
/// </summary>
public class MutiOperatePage : UIFramwork
{
    private MutiOperateParam param;//参数
    private int userCount;//使用数量 
    /// <summary>
    /// 页面开启
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        param = (MutiOperateParam)e;
        userCount = param.canUserTimes;
        InitUI();
    }
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }
    /// <summary>
    /// 初始化UI
    /// </summary>
    private void InitUI()
    {
        ResourceCommonDT dt = new ResourceCommonDT();
        dt.f_UpdateInfo((byte)param.resourceType, param.resourceID, param.resourceCount);
        UITool.f_SetIconSprite(f_GetObject("SpriteIcon").GetComponent<UI2DSprite>(), param.resourceType, param.resourceID);
        string itemName = dt.mName;
        string borderName = UITool.f_GetImporentColorName(dt.mImportant, ref itemName);
        f_GetObject("IconBorder").GetComponent<UISprite>().spriteName = borderName;
        f_GetObject("LabelTitle").GetComponent<UISprite>().spriteName = param.title;
        f_GetObject("LabelTitle").GetComponent<UISprite>().MakePixelPerfect();
        f_GetObject("GoodNum").GetComponent<UILabel>().text = param.resourceCount.ToString();
        f_GetObject("LabelName").GetComponent<UILabel>().text = itemName;
        f_GetObject("LabelHasCount").GetComponent<UILabel>().text = UITool.f_GetGoodNum(param.resourceType, param.resourceID).ToString();
        f_GetObject("LabelHint").GetComponent<UILabel>().text = param.userHint;
        UpdateUI();
        f_RegClickEvent("SpriteIcon", OnIconClick);
    }
    /// <summary>
    /// 点击图标
    /// </summary>
    private void OnIconClick(GameObject go, object obj1, object obj2)
    {
        ResourceCommonDT commonData = new ResourceCommonDT();
        commonData.f_UpdateInfo((byte)param.resourceType, param.resourceID, param.resourceCount);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
    }
    /// <summary>
    /// 更新UI
    /// </summary>
    private void UpdateUI()
    {
        f_GetObject("InputUseCount").GetComponent<UILabel>().text = userCount.ToString();
    }
    /// <summary>
    /// 初始化
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnBlack", OnBtnBlackClick);
        f_RegClickEvent("BtnConfirm", OnConfirmClick);
        f_RegClickEvent("BtnCancel", OnCancelClick);
        f_RegClickEvent("BtnAddOne", OnBuyAddOneClick);
        f_RegClickEvent("BtnAddTen", OnBuyAddTenClick);
        f_RegClickEvent("BtnReduceOne", OnBuyReduceOneClick);
        f_RegClickEvent("BtnReduceTen", OnBuyReduceTenClick);
    }
    #region 按钮事件相关
    /// <summary>
    /// 点击黑色背景关闭按钮
    /// </summary>
    private void OnBtnBlackClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.MutiOperatePage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 点击确定按钮
    /// </summary>
    private void OnConfirmClick(GameObject go, object obj1, object obj2)
    {
        CheckBuyCount(false);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.MutiOperatePage, UIMessageDef.UI_CLOSE);
        if (param.onConfirmOperateCallback != null)
        {
            param.onConfirmOperateCallback(param.iId, param.resourceType, param.resourceID, param.resourceCount, userCount);
        }
    }
    /// <summary>
    /// 点击取消按钮
    /// </summary>
    private void OnCancelClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.MutiOperatePage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 点击购买道具减10
    /// </summary>
    private void OnBuyReduceTenClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        userCount -= 10;
        userCount = userCount < 1 ? 1 : userCount;
        UpdateUI();
    }
    /// <summary>
    /// 点击购买道具减1
    /// </summary>
    private void OnBuyReduceOneClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        userCount -= 1;
        userCount = userCount < 1 ? 1 : userCount;
        UpdateUI();
    }
    /// <summary>
    /// 点击购买道具加10
    /// </summary>
    private void OnBuyAddTenClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        userCount += 10;
        CheckBuyCount(true);
        UpdateUI();
    }
    /// <summary>
    /// 点击购买道具加1
    /// </summary>
    private void OnBuyAddOneClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        userCount += 1;
        CheckBuyCount(true);
        UpdateUI();
    }
    /// <summary>
    /// 检测购买数量是否合法，不合法则纠正购买数量并显示提示
    /// </summary>
    private void CheckBuyCount(bool isTips)
    {
        //体力丹和精力丹还要判断体力或精力是否达上限
        if (param.resourceType == EM_ResourceType.Good && (param.resourceID == GameParamConst.VigorGoodId || param.resourceID == GameParamConst.EnergyGoodId))
        {
            bool isEnergy = param.resourceID == GameParamConst.EnergyGoodId;
            int haveNum = isEnergy ? Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Energy) :
                Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vigor);
            int mEnergyLimit = UITool.f_GetNowVipPrivilege((int)EM_VipPrivilege.eVip_EnergyLimit);
            int maxNum = isEnergy ? mEnergyLimit : GameParamConst.VigorMax;
            BaseGoodsSC baseGoodsSC = glo_Main.GetInstance().m_SC_Pool.m_BaseGoodsSC;
            if (null == baseGoodsSC)
            {
                MessageBox.ASSERT("MutiOperatePage:CheckBuyCount,glo_Main.GetInstance().m_SC_Pool.m_BaseGoodsSC NULL!");
                return;
            }
            BaseGoodsDT baseGoodsDt = (BaseGoodsDT)baseGoodsSC.f_GetSC(param.resourceID);
            if (haveNum + baseGoodsDt.iEffectData * userCount > maxNum)
            {
                userCount = (maxNum - haveNum) / baseGoodsDt.iEffectData;
                if(isTips)
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Số lượng tối đã: " + userCount);
            }

        }
        else if (param.canUserTimes != 0 && userCount > param.canUserTimes)
        {
            userCount = param.canUserTimes;
            if(isTips)
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Số lượng tối đã: " + param.canUserTimes);
        }
    }
    #endregion
}
/// <summary>
/// 确认操作委托
/// </summary>
/// <param name="UseCount">使用数量</param>
public delegate void ConfirmOperateCallback(long iId, EM_ResourceType type, int resourceId, int resourceCount, int UseCount);
/// <summary>
/// 参数
/// </summary>
public class MutiOperateParam
{
    public long iId;
    /// <summary>
    /// 标题
    /// </summary>
public string title = "Use a lot";
    /// <summary>
    /// 资源类型
    /// </summary>
    public EM_ResourceType resourceType;
    /// <summary>
    /// 资源id
    /// </summary>
    public int resourceID;
    /// <summary>
    /// 资源数量
    /// </summary>
    public int resourceCount = 1;
    /// <summary>
    /// 使用限制次数
    /// </summary>
    public int canUserTimes = 0;
    /// <summary>
    /// 提示消息
    /// </summary>
    public string userHint = "";
    /// <summary>
    /// 确认操作回调
    /// </summary>
    public ConfirmOperateCallback onConfirmOperateCallback;
    public MutiOperateParam() { }

    public MutiOperateParam(string title, EM_ResourceType resourceType, int resourceID, int resourceCount, int canUserTimes, string userHint, ConfirmOperateCallback onConfirmOperateCallback)
    {
        this.title = title;
        this.resourceType = resourceType;
        this.resourceID = resourceID;
        this.resourceCount = resourceCount;
        this.canUserTimes = canUserTimes;
        this.userHint = userHint;
        this.onConfirmOperateCallback = onConfirmOperateCallback;
    }
}
