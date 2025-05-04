using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
/// <summary>
/// 夺宝战报界面
/// </summary>
public class GrabSweepResultPage : UIFramwork
{
    List<BasePoolDT<long>> listGrabPoolDt = new List<BasePoolDT<long>>();
    List<BasePoolDT<long>> tempListItem = new List<BasePoolDT<long>>();
    private bool isGetFrag;//是否获取到了碎片
    private bool isShowResultEnd;//是否显示完了动画
    /// <summary>
    /// 打开页面
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        Data_Pool.m_GuidancePool.IsOpponentSween = true;
        StartCoroutine(StartShowItem());
    }
    /// <summary>
    /// 显示夺宝结果
    /// </summary>
    private IEnumerator StartShowItem()
    {
        float curValue = 0.4f;
        isGetFrag = false;
        isShowResultEnd = false;
        f_GetObject("BtnSweepEnd").SetActive(false);
        listGrabPoolDt = Data_Pool.m_GrabTreasurePool.f_GetAll();
        tempListItem.Clear();
        for (int i = 0; i < listGrabPoolDt.Count; i++)
        {
            tempListItem.Add(listGrabPoolDt[i]);
            GridUtil.f_SetGridView<BasePoolDT<long>>(true,f_GetObject("ItemParent"), f_GetObject("Item"), tempListItem, UpdataItem);
            f_GetObject("ProgressBar").GetComponent<UIProgressBar>().value = 1f;
            yield return new WaitForSeconds(curValue);
        }
        if (isGetFrag)
        {
            //如果已经得到了碎片，则关闭选择对手页面
//UITool.Ui_Trip("Receive Magic Piece");
            ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectOpponentPage, UIMessageDef.UI_CLOSE);
        }
        f_GetObject("BtnSweepEnd").SetActive(true);
        isShowResultEnd = true;
    }
    /// <summary>
    /// item更新
    /// </summary>
    /// <param name="go"></param>
    /// <param name="poolDT"></param>
    private void UpdataItem(GameObject go, BasePoolDT<long> poolDT)
    {
        if (tempListItem[tempListItem.Count - 1].iId != poolDT.iId)
            return;
        GrabTreasurePoolDT grabTreasurePoolDT = poolDT as GrabTreasurePoolDT;
string Hint = "The general has failed, please try again~";

        int tLv = StaticValue.m_sLvInfo.m_iAddLv;
        int moneyCount = GameFormula.f_VigorCost2Money(tLv, 2);
        int addExp;
        int exCount = GameFormula.f_VigorCost2Exp(tLv, 2, out addExp);
        string strAddExp = addExp > 0 ? "[FFF700FF]（+" + addExp + "）" : "";
        StaticValue.m_sLvInfo.f_AddExp(exCount + addExp);

        if (grabTreasurePoolDT.treaFragID > 0)
        {
            ResourceCommonDT commonDt = new ResourceCommonDT();
            commonDt.f_UpdateInfo((byte)EM_ResourceType.TreasureFragment, grabTreasurePoolDT.treaFragID, 1);
            string name = commonDt.mName;
            UITool.f_GetImporentColorName(commonDt.mImportant, ref name);
            Hint = Hint = CommonTools.f_GetTransLanguage(787) + name; //+"[b6a791]x"+ commonDt.mResourceNum;
            isGetFrag = true;
        }
        AwardPoolDT awardItem = new AwardPoolDT();
        awardItem.f_UpdateByInfo(grabTreasurePoolDT.resourceType, grabTreasurePoolDT.resourceId, grabTreasurePoolDT.resourceNum);
        go.GetComponent<GrabSweepResultItem>().SetData(listGrabPoolDt.IndexOf(poolDT) + 1, Hint, moneyCount, exCount + strAddExp, (EM_ResourceType)awardItem.mTemplate.mResourceType,
            awardItem.mTemplate.mResourceId, awardItem.mTemplate.mResourceNum, grabTreasurePoolDT.star <= 0 ? true : false);
        f_RegClickEvent(go.GetComponent<GrabSweepResultItem>().SprAwardIcon.gameObject, OnAwardIconClick, awardItem);
        
    }
    /// <summary>
    /// 点击奖励icon弹出详细信息
    /// </summary>
    private void OnAwardIconClick(GameObject go, object obj1, object obj2)
    {
        AwardPoolDT awardItem = obj1 as AwardPoolDT;
        ResourceCommonDT commonData = new ResourceCommonDT();
        commonData.f_UpdateInfo((byte)awardItem.mTemplate.mResourceType, awardItem.mTemplate.mResourceId, awardItem.mTemplate.mResourceNum);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
    }
    /// <summary>
    /// 注册事件
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnBlackBg", OnBtnCloseClick);
        f_RegClickEvent("BtnClose", OnBtnCloseClick);
        f_RegClickEvent("BtnSweepEnd", OnBtnCloseClick);
    }
    #region 按钮事件
    /// <summary>
    /// 点击关闭按钮
    /// </summary>
    private void OnBtnCloseClick(GameObject go, object obj1, object obj2)
    {
        if (!isShowResultEnd)
            return;
        Data_Pool.m_GuidancePool.IsOpponentSween = false;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GrabSweepResultPage, UIMessageDef.UI_CLOSE);
    }
    #endregion
}
