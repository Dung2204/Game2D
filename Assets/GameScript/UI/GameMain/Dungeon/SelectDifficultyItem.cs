using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
/// <summary>
/// 日常副本选择难度item
/// </summary>
public class SelectDifficultyItem : UIFramwork {
    public GameObject BtnCharllenge;//挑战
    public UILabel LabelBattlePower;//推荐战力
    public UILabel LabelOpenLevel;//开启等级
    public GameObject AwardParent;//奖励父物体
    public GameObject AwardItem;//奖励item
    public UISprite DiffType;
    /// <summary>
    /// 设置数据
    /// </summary>
    public void InitData(DailyPveGateDT dailyPveGateDT)
    {
        int PlayerLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        bool isLevelLimit = dailyPveGateDT.iLevelLimit1 > PlayerLevel;
        LabelOpenLevel.gameObject.SetActive(isLevelLimit);
        BtnCharllenge.gameObject.SetActive(!isLevelLimit);
        LabelBattlePower.gameObject.SetActive(!isLevelLimit);
        DiffType.spriteName = dailyPveGateDT.szLevelIcon1;

        LabelBattlePower.text = string.Format("[cbc4b1]{0}[fed323] {1}[-][-]",CommonTools.f_GetTransLanguage(2177), dailyPveGateDT.szZhanLi1);
LabelOpenLevel.text = "Cấp " + dailyPveGateDT.iLevelLimit1 + " mở";

        string StrAward = dailyPveGateDT.szAward;
        string[] ArrayStrAward = StrAward.Split(';');
        List<AwardPoolDT> listAwardData = new List<AwardPoolDT>();
        AwardPoolDT awardPoolDT = new AwardPoolDT();
        awardPoolDT.f_UpdateByInfo(byte.Parse(ArrayStrAward[0]), int.Parse(ArrayStrAward[1]), int.Parse(ArrayStrAward[2]));
        listAwardData.Add(awardPoolDT);

        GridUtil.f_SetGridView<AwardPoolDT>(AwardParent, AwardItem, listAwardData, OnShowItemCallback);
        AwardParent.transform.parent.GetComponent<UIScrollView>().ResetPosition();
        AwardParent.transform.parent.parent.GetComponent<UIDragScrollView>().enabled = (listAwardData.Count > 2);//当奖励item大于2个时才开启左右拖拽
    }
    /// <summary>
    /// 显示内容
    /// </summary>
    /// <param name="item"></param>
    /// <param name="awardPoolDt"></param>
    private void OnShowItemCallback(GameObject item, AwardPoolDT awardPoolDt)
    {
        item.GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(awardPoolDt.mTemplate.mIcon);
        item.transform.Find("LabelCount").GetComponent<UILabel>().text = UITool.f_CountToChineseStr(awardPoolDt.mTemplate.mResourceNum);
        string name = awardPoolDt.mTemplate.mName;
        item.transform.Find("IconBorder").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(awardPoolDt.mTemplate.mImportant, ref name);
        item.transform.Find("Case").GetComponent<UISprite>().spriteName = UITool.f_GetImporentCase(awardPoolDt.mTemplate.mImportant);
        f_RegClickEvent(item, OnAwardIconClick, awardPoolDt);
    }
    /// <summary>
    /// 点击奖励icon弹出详细信息
    /// </summary>
    private void OnAwardIconClick(GameObject go, object obj1, object obj2)
    {
        AwardPoolDT awardPoolDt = (AwardPoolDT)obj1;
        ResourceCommonDT commonData = new ResourceCommonDT();
        commonData.f_UpdateInfo((byte)awardPoolDt.mTemplate.mResourceType, awardPoolDt.mTemplate.mResourceId, awardPoolDt.mTemplate.mResourceNum);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
    }
}
