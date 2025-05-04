using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 军团技能界面
/// </summary>
public class LegionSkillPage : UIFramwork
{
    private List<BasePoolDT<long>> listSkillData = new List<BasePoolDT<long>>();
    private UIWrapComponent _contentComponent = null;//技能

    private UILabel mLegionInfo;

    private SocketCallbackDT InfoCallback = new SocketCallbackDT();//查询回调
    private SocketCallbackDT SkillUpOrOpenCallback = new SocketCallbackDT();//技能开启，提升，学习，升级回调
    private string strUpOrOpenHintText;
    private bool isDeputyOrChief = false;
    /// <summary>
    /// 页面开启
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        LegionInfoPoolDT tLegionInfo = LegionMain.GetInstance().m_LegionInfor.f_getUserLegion();
        mLegionInfo.text = string.Format(CommonTools.f_GetTransLanguage(601),
                                        tLegionInfo.LegionName,
                                        tLegionInfo.f_GetProperty((int)EM_LegionProperty.Lv),
                                        Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_LegionContribution),
                                        tLegionInfo.f_GetProperty((int)EM_LegionProperty.Exp));
        isDeputyOrChief = LegionTool.f_IsEnoughPermission(EM_LegionOperateType.OpenOrUpSkill, false);
        UITool.f_OpenOrCloseWaitTip(true);
        LegionMain.GetInstance().m_LegionSkillPool.f_QueryInfo(InfoCallback);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
    }

    /// <summary>
    /// 注册消息事件
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnBlack", OnBtnBlackClick);
        f_RegClickEvent("BtnClose", OnBtnCloseClick);
        f_RegClickEvent("BtnHelp", OnBtnHelpClick);
        InfoCallback.m_ccCallbackSuc = OnInfoSucCall;
        InfoCallback.m_ccCallbackFail = OnInfoFailCall;
        SkillUpOrOpenCallback.m_ccCallbackSuc = OnUpOrOpenSkillSucCall;
        SkillUpOrOpenCallback.m_ccCallbackFail = OnUpOrOenSkillFailCall;
        mLegionInfo = f_GetObject("LegionInfo").GetComponent<UILabel>();
    }
    /// <summary>
    /// 点击帮助按钮
    /// </summary>
    private void OnBtnHelpClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage, UIMessageDef.UI_OPEN, 10);
    }
    /// <summary>
    /// 初始化UI数据
    /// </summary>
    public void InitUI()
    {
        LegionInfoPoolDT tLegionInfo = LegionMain.GetInstance().m_LegionInfor.f_getUserLegion();
        mLegionInfo.text = string.Format(CommonTools.f_GetTransLanguage(601),
                                        tLegionInfo.LegionName,
                                        tLegionInfo.f_GetProperty((int)EM_LegionProperty.Lv),
                                        Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_LegionContribution),
                                        tLegionInfo.f_GetProperty((int)EM_LegionProperty.Exp));
        listSkillData.Clear();
        listSkillData = CommonTools.f_CopyPoolDTArrayToNewList(LegionMain.GetInstance().m_LegionSkillPool.f_GetAll());
        listSkillData.RemoveAll((BasePoolDT<long> item) => { return item.iId == 9; });
        if (_contentComponent == null)
        {
            _contentComponent = new UIWrapComponent(240, 1, 1200, 5, f_GetObject("LegionSkillParent"), f_GetObject("LegionSkillItem"), listSkillData, OnUpdateSkillItem, null);
            _contentComponent.f_ResetView();
        }
        _contentComponent.f_UpdateList(listSkillData);
        _contentComponent.f_UpdateView();
    }
    #region 服务器回调
    /// <summary>
    /// 查询信息成功
    /// </summary>
    private void OnInfoSucCall(object obj)
    {
        InitUI();
        UITool.f_OpenOrCloseWaitTip(false);
    }
    /// <summary>
    /// 查询失败
    /// </summary>
    private void OnInfoFailCall(object obj)
    {
        //查询失败
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(603));
        UITool.f_OpenOrCloseWaitTip(false);
    }
    /// <summary>
    /// 开启、提升、学习、升级成功回调
    /// </summary>
    private void OnUpOrOpenSkillSucCall(object obj)
    {
        InitUI();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, strUpOrOpenHintText);
        UITool.f_OpenOrCloseWaitTip(false);

        //重新计算战斗力
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerFightPowerChange);
    }
    /// <summary>
    /// 开启、提升、学习、升级失败回调
    /// </summary>
    private void OnUpOrOenSkillFailCall(object obj)
    {
        //升级失败
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(604));
        UITool.f_OpenOrCloseWaitTip(false);
    }
    #endregion
    /// <summary>
    /// 更新技能Item
    /// </summary>
    private void OnUpdateSkillItem(Transform t, BasePoolDT<long> item)
    {
        LegionSkillPoolDT poolDT = item as LegionSkillPoolDT;
        EM_LegionSkillState skillState = EM_LegionSkillState.NotOpen;
        if (poolDT.m_SkillLevelMax <= 0)
            skillState = EM_LegionSkillState.NotOpen;
        else if (poolDT.m_SkillLevel <= 0)
            skillState = EM_LegionSkillState.NotLearn;
        else if (poolDT.m_SkillLevel >= poolDT.m_SkillLevelMax)
            skillState = EM_LegionSkillState.Lock;
        else
            skillState = EM_LegionSkillState.UpGrade;
        LegionSkillDT currentDT = poolDT.m_LegionSkillDT;
        LegionSkillDT nextDT = LegionMain.GetInstance().m_LegionSkillPool.f_GetLegionSkillDT((int)poolDT.iId, poolDT.m_SkillLevel + 1);

        LegionLevelDT currentLevelDT = GetLegion((int)poolDT.iId, poolDT.m_SkillLevelMax);
        LegionLevelDT nextLevelDT = (currentLevelDT == null ? GetLegion(0, 0, true) : GetNextLegion((int)poolDT.iId, poolDT.m_SkillLevelMax) as LegionLevelDT);
        int wasterEx = GetSkillUpgradeWasterValue(nextLevelDT, (int)poolDT.iId);

        string skillName = UITool.f_GetProName((EM_RoleProperty)(currentDT != null ? currentDT.iBuffID : nextDT.iBuffID));
        int lv = LegionMain.GetInstance().m_LegionInfor.f_getUserLegion().f_GetProperty((int)EM_LegionProperty.Lv);
        if (nextLevelDT == null)
        {//上线达最高级
            skillName += CommonTools.f_GetTransLanguage(605);
        }
        else if (currentLevelDT != null)
        {
            if (lv < nextLevelDT.iId)
            {//提升需要军团等级
                skillName += "[ff0000]("+CommonTools.f_GetTransLanguage(606) + nextLevelDT.iId + "）";
            }
        }
        //当前等级
        string Level = CommonTools.f_GetTransLanguage(607) + poolDT.m_SkillLevel + "/" + poolDT.m_SkillLevelMax;
        string addProNow = CommonTools.f_GetTransLanguage(608);//当前属性
        if (currentDT == null)
            //未学习
            addProNow += CommonTools.f_GetTransLanguage(609);
        else
        {
            string addPro = CommonTools.f_GetAddProperty((EM_RoleProperty)currentDT.iBuffID, currentDT.iBuffCount);
            //主角 全队
            string forwardStr = (EM_RoleProperty)currentDT.iBuffID == EM_RoleProperty.ExpR ? CommonTools.f_GetTransLanguage(610) : CommonTools.f_GetTransLanguage(611);
            addProNow += forwardStr + UITool.f_GetProName((EM_RoleProperty)currentDT.iBuffID) + "+" + addPro;
        }
        //下级属性
        string addProNext = CommonTools.f_GetTransLanguage(612);
        if (nextDT == null)
            addProNext += CommonTools.f_GetTransLanguage(613);//已达最高级
        else
        {

            string addPro = CommonTools.f_GetAddProperty((EM_RoleProperty)nextDT.iBuffID, nextDT.iBuffCount);
            string forwardStr = (EM_RoleProperty)nextDT.iBuffID == EM_RoleProperty.ExpR ? CommonTools.f_GetTransLanguage(610) : CommonTools.f_GetTransLanguage(611);//主角 全队
            addProNext += forwardStr + UITool.f_GetProName((EM_RoleProperty)nextDT.iBuffID) + "+" + addPro;
        }
        string wasterHint = CommonTools.f_GetTransLanguage(614) + CommonTools.f_GetTransLanguage(615) + (nextDT == null ? 0 : nextDT.iLevelUpCost);//消耗 军团贡献
        bool isSkillUpMax = (currentLevelDT != null && nextLevelDT == null) ? true : false;//技能是否已经提升到最高级
        LegionSkillItem skillItem = t.GetComponent<LegionSkillItem>();
        skillItem.SetDataInfo(skillName, currentDT != null ? currentDT.iIcon : nextDT.iIcon,
            Level, addProNow, addProNext, wasterHint, skillState, isDeputyOrChief, isSkillUpMax);
        f_RegClickEvent(skillItem.m_btnOpen, OnSkillBtnOpenClick, item, wasterEx);
        f_RegClickEvent(skillItem.m_btnUpgrade, OnSkillBtnUpgradeClick, item, wasterEx);
        f_RegClickEvent(skillItem.m_btnLearn, OnSkillBtnLearnClick, item, nextDT == null ? 0 : nextDT.iLevelUpCost);
        f_RegClickEvent(skillItem.m_btnUpLevel, OnSkillBtnUpLevelClick, item, nextDT == null ? 0 : nextDT.iLevelUpCost);

        SkillInfoPageParam param = new SkillInfoPageParam();
        param.iconId = currentDT != null ? currentDT.iIcon : nextDT.iIcon;
        param.mSkillName = skillName;
        param.mCurLevelName = Level;
        param.mContent = addProNow + "\n" + addProNext + "\n" + wasterHint;
        f_RegClickEvent(skillItem.m_Icon.gameObject, OnSkillIconlick, param);
        if (nextDT == null)//该技能不可再提升，隐藏提升按钮
        {
            skillItem.m_btnUpgrade.SetActive(false);
        }

    }
    /// <summary>
    /// 点击技能icon
    /// </summary>
    private void OnSkillIconlick(GameObject go, object obj1, object obj2)
    {
        SkillInfoPageParam param = (SkillInfoPageParam)obj1;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SkillInfoPage, UIMessageDef.UI_OPEN, param);
    }
    private LegionLevelDT GetLegion(int skillType, int skillLevelMax, bool getFirst = false)
    {
        List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_LegionLevelSC.f_GetAll();
        for (int i = 0; i < listData.Count; i++)
        {
            LegionLevelDT dt = listData[i] as LegionLevelDT;
            if (getFirst)
                return dt;
            if (GetLimitValue(dt, skillType) == skillLevelMax)
                return dt;
        }
        return null;
    }

    private LegionLevelDT GetNextLegion(int skillType, int skillLevelMax)
    {
        List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_LegionLevelSC.f_GetAll();
        for (int i = 0; i < listData.Count; i++)
        {
            LegionLevelDT dt = listData[i] as LegionLevelDT;
            if (GetLimitValue(dt, skillType) > skillLevelMax)
                return dt;
        }
        return null;
    }

    private int GetLimitValue(LegionLevelDT legionLevelDT, int skillType)
    {
        switch (skillType)
        {
            case 1: return legionLevelDT.iSkillLimit1;
            case 2: return legionLevelDT.iSkillLimit2;
            case 3: return legionLevelDT.iSkillLimit3;
            case 4: return legionLevelDT.iSkillLimit4;
            case 5: return legionLevelDT.iSkillLimit5;
            case 6: return legionLevelDT.iSkillLimit6;
            case 7: return legionLevelDT.iSkillLimit7;
            case 8: return legionLevelDT.iSkillLimit8;
            case 9: return legionLevelDT.iSkillLimit9;
        }
        return 0;
    }
    private int GetSkillUpgradeWasterValue(LegionLevelDT legionLevelDT, int skillType)
    {
        if (legionLevelDT == null)
            return 0;
        switch (skillType)
        {
            case 1: return legionLevelDT.iSkillCost1;
            case 2: return legionLevelDT.iSkillCost2;
            case 3: return legionLevelDT.iSkillCost3;
            case 4: return legionLevelDT.iSkillCost4;
            case 5: return legionLevelDT.iSkillCost5;
            case 6: return legionLevelDT.iSkillCost6;
            case 7: return legionLevelDT.iSkillCost7;
            case 8: return legionLevelDT.iSkillCost8;
            case 9: return legionLevelDT.iSkillCost9;
        }
        return 0;
    }
    /// <summary>
    /// 关闭选择界面回调
    /// </summary>
    private void OnSelectCancelCallback(object obj)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 点击技能去开启事件
    /// </summary>
    private void OnSkillBtnOpenClick(GameObject go, object item, object obj2)
    {
        if (LegionTool.f_IsEnoughPermission(EM_LegionOperateType.OpenOrUpSkill))
        {
            if (LegionMain.GetInstance().m_LegionInfor.f_getUserLegion().f_GetProperty((int)EM_LegionProperty.Exp) < (int)obj2)
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(616) + (int)obj2 + CommonTools.f_GetTransLanguage(617));//军团经验不足  开启该技能需要      军团经验值
                return;
            }
            string strContent = CommonTools.f_GetTransLanguage(618);//你确定要开启此技能 ?
            if ((int)obj2 > 0)
                strContent = CommonTools.f_GetTransLanguage(619) + (int)obj2 + CommonTools.f_GetTransLanguage(620);//你确定要花费     军团经验开启此技能?
            PopupMenuParams tParam = new PopupMenuParams(CommonTools.f_GetTransLanguage(621), strContent, CommonTools.f_GetTransLanguage(622), OnConfirmSkillOpen, CommonTools.f_GetTransLanguage(623), OnSelectCancelCallback, item);//系统提示   确定   取消
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);
        }
    }
    private void OnConfirmSkillOpen(object item)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectPage, UIMessageDef.UI_CLOSE);
        LegionSkillPoolDT poolDT = item as LegionSkillPoolDT;
        strUpOrOpenHintText = CommonTools.f_GetTransLanguage(624);//技能开启成功
        UITool.f_OpenOrCloseWaitTip(true);
        LegionMain.GetInstance().m_LegionSkillPool.f_OpenSkill((int)poolDT.iId, SkillUpOrOpenCallback);

    }
    /// <summary>
    /// 点击技能提升上限事件
    /// </summary>
    private void OnSkillBtnUpgradeClick(GameObject go, object item, object obj2)
    {
        if (LegionMain.GetInstance().m_LegionInfor.f_getUserLegion().f_GetProperty((int)EM_LegionProperty.Exp) < (int)obj2)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(616) + (int)obj2 + CommonTools.f_GetTransLanguage(617));//军团经验不足,提升该技能需要     军团经验值
            return;
        }
        LegionSkillPoolDT poolDT = item as LegionSkillPoolDT;
        LegionLevelDT currentLevelDT = GetLegion((int)poolDT.iId, poolDT.m_SkillLevelMax);
        LegionLevelDT nextLevelDT = (currentLevelDT == null ? GetLegion(0, 0, true) : GetNextLegion((int)poolDT.iId, poolDT.m_SkillLevelMax) as LegionLevelDT);
        int lv = LegionMain.GetInstance().m_LegionInfor.f_getUserLegion().f_GetProperty((int)EM_LegionProperty.Lv);

        if (nextLevelDT == null)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(626));//该等级已提升至最高级
            return;
        }
        if (lv < nextLevelDT.iId)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, string.Format(CommonTools.f_GetTransLanguage(627), nextLevelDT.iId));//需要军团等级    级才能提升该等级
            return;
        }
        string strContent = string.Format(CommonTools.f_GetTransLanguage(628), (int)obj2); ;
        PopupMenuParams tParam = new PopupMenuParams(CommonTools.f_GetTransLanguage(621), strContent, CommonTools.f_GetTransLanguage(622), OnConfirmSkillUpgrade, CommonTools.f_GetTransLanguage(623), OnSelectCancelCallback, item);//系统提示  确定  取消
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);

    }
    private void OnConfirmSkillUpgrade(object item)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectPage, UIMessageDef.UI_CLOSE);
        LegionSkillPoolDT poolDT = item as LegionSkillPoolDT;
        LegionLevelDT currentLevelDT = GetLegion((int)poolDT.iId, poolDT.m_SkillLevelMax);
        LegionLevelDT nextLevelDT = (currentLevelDT == null ? GetLegion(0, 0, true) : GetNextLegion((int)poolDT.iId, poolDT.m_SkillLevelMax) as LegionLevelDT);
        LegionLevelDT nextNextLevelDT = glo_Main.GetInstance().m_SC_Pool.m_LegionLevelSC.f_GetSC(nextLevelDT.iId + 1) as LegionLevelDT;
        string AddStr = nextNextLevelDT == null ? CommonTools.f_GetTransLanguage(629) : "！";//已升级至最高级
        strUpOrOpenHintText = string.Format(CommonTools.f_GetTransLanguage(630), GetLimitValue(nextLevelDT, (int)poolDT.iId), AddStr);// "技能提升成功，上限已提升至" + GetLimitValue(nextLevelDT, (int)poolDT.iId) + "级" + AddStr;//技能提升成功,上线已提升至{0}级
        UITool.f_OpenOrCloseWaitTip(true);
        LegionMain.GetInstance().m_LegionSkillPool.f_OpenSkill((int)poolDT.iId, SkillUpOrOpenCallback);
    }
    /// <summary>
    /// 点击技能学习事件
    /// </summary>
    private void OnSkillBtnLearnClick(GameObject go, object item, object obj2)
    {
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_LegionContribution) < (int)obj2)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, string.Format(CommonTools.f_GetTransLanguage(634), (int)obj2));// "军团贡献不足，学习该技能需要" + (int)obj2 + "军团贡献");//军团贡献不足,学习该技能需要   军团贡献
            return;
        }
        string strContent = string.Format(CommonTools.f_GetTransLanguage(632), (int)obj2);//"你确定要花费" + (int)obj2 + "军团贡献来学习此技能？";//你确定要花费    军团贡献来学习此技能?
        PopupMenuParams tParam = new PopupMenuParams(CommonTools.f_GetTransLanguage(621), strContent, CommonTools.f_GetTransLanguage(622), OnConfirmSkillLearn, CommonTools.f_GetTransLanguage(623), OnSelectCancelCallback, item);//系统提示  确定  取消
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);
    }

    private void OnConfirmSkillLearn(object item)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectPage, UIMessageDef.UI_CLOSE);
        LegionSkillPoolDT poolDT = item as LegionSkillPoolDT;
        strUpOrOpenHintText = CommonTools.f_GetTransLanguage(633);//技能学习成功
        UITool.f_OpenOrCloseWaitTip(true);
        LegionMain.GetInstance().m_LegionSkillPool.f_UpSkill((int)poolDT.iId, SkillUpOrOpenCallback);
    }
    /// <summary>
    /// 点击技能升级事件
    /// </summary>
    private void OnSkillBtnUpLevelClick(GameObject go, object item, object obj2)
    {
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_LegionContribution) < (int)obj2)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, string.Format(CommonTools.f_GetTransLanguage(634), (int)obj2));// "军团贡献不足，升级该技能需要" + (int)obj2 + "军团贡献");//军团贡献不足,升级该技能需要    军团贡献
            return;
        }
        string strContent = string.Format(CommonTools.f_GetTransLanguage(635), (int)obj2);//"你确定要花费" + (int)obj2 + "军团贡献来升级此技能？";//你确定要花费{0}军团贡献来升级次技能?
        PopupMenuParams tParam = new PopupMenuParams(CommonTools.f_GetTransLanguage(621), strContent, CommonTools.f_GetTransLanguage(622), OnConfirmSkillUp, CommonTools.f_GetTransLanguage(623), OnSelectCancelCallback, item);//系统提示  确定  取消
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);

    }
    private void OnConfirmSkillUp(object item)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectPage, UIMessageDef.UI_CLOSE);
        LegionSkillPoolDT poolDT = item as LegionSkillPoolDT;
        strUpOrOpenHintText = string.Format(CommonTools.f_GetTransLanguage(636), (poolDT.m_SkillLevel + 1));//"技能升级成功，已升级至" + (poolDT.m_SkillLevel + 1) + "级！";//技能升级成功 已升级至{0}级
        UITool.f_OpenOrCloseWaitTip(true);
        LegionMain.GetInstance().m_LegionSkillPool.f_UpSkill((int)poolDT.iId, SkillUpOrOpenCallback);
    }
    #region 按钮事件相关
    /// <summary>
    /// 点击黑色背景，关闭页面
    /// </summary>
    private void OnBtnBlackClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionSkillPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 点击关闭按钮
    /// </summary>
    private void OnBtnCloseClick(GameObject go, object obj1, object ojb2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionSkillPage, UIMessageDef.UI_CLOSE);
    }
    #endregion

    private void f_ProcessByMsg_ForceClose(object value)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionSkillPage, UIMessageDef.UI_CLOSE);
    }
}

/// <summary>
/// 技能状态
/// </summary>
public enum EM_LegionSkillState
{
    /// <summary>
    /// 未开启状态
    /// </summary>
    NotOpen = 1,
    /// <summary>
    /// 已开启未学习状态
    /// </summary>
    NotLearn = 2,
    /// <summary>
    /// 升级状态
    /// </summary>
    UpGrade = 3,
    /// <summary>
    /// 已升级到最高级，不可点击再升
    /// </summary>
    Lock = 4,
}