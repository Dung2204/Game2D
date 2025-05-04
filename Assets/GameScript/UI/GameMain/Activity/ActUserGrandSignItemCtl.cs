using UnityEngine;
using System.Collections;
/// <summary>
/// 豪华签到item控制
/// </summary>
public class ActUserGrandSignItemCtl : MonoBehaviour {
    public UILabel LabelUserSignTitle;//标题
    public UILabel LabelRechargeCount;//充值数量
	public UILabel LabelRechargeCount2;
    public GameObject BtnGet;//领取按钮
    public GameObject BtnRecharge;//充值按钮
    public UI2DSprite SprAward1;//奖励1
    public UI2DSprite SprAward2;//奖励2
    public UI2DSprite SprAward3;//奖励3
    public UI2DSprite SprAward4;//奖励4
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="day">天数</param>
    /// <param name="awardID1">奖励1</param>
    /// <param name="awardCount1">奖励1数量</param>
    /// <param name="awardID2">奖励2</param>
    /// <param name="awardCount2">奖励2数量</param>
    /// <param name="awardID3">奖励3</param>
    /// <param name="awardCount3">奖励3数量</param>
    /// <param name="awardID4">奖励4</param>
    /// <param name="awardCount4">奖励4数量</param>
    public void f_SetData(int rechargeCount,int day, int awardType1, int awardID1, int awardCount1, int awardType2, int awardID2,int awardCount2,
        int awardType3, int awardID3,int awardCount3, int awardType4, int awardID4,int awardCount4)
    {
        LabelUserSignTitle.text = day.ToString();
        // LabelRechargeCount.text = string.Format(CommonTools.f_GetTransLanguage(1303), rechargeCount);
		LabelRechargeCount.text = CommonTools.f_GetTransLanguage(2269) + "";
		LabelRechargeCount2.text = CommonTools.f_GetTransLanguage(1303) + "";
        UITool.f_SetIconSprite(SprAward1, (EM_ResourceType)awardType1,awardID1);
        SprAward1.transform.Find("LabelCount").GetComponent<UILabel>().text = awardCount1.ToString();
        SetBorder(SprAward1.transform.Find("IconBorder").gameObject, awardType1, awardID1, awardCount1);
        UITool.f_SetIconSprite(SprAward2,(EM_ResourceType)awardType2, awardID2);
        SprAward2.transform.Find("LabelCount").GetComponent<UILabel>().text = awardCount2.ToString();
        SetBorder(SprAward2.transform.Find("IconBorder").gameObject, awardType2, awardID2, awardCount2);
        UITool.f_SetIconSprite(SprAward3, (EM_ResourceType)awardType3, awardID3);
        SprAward3.transform.Find("LabelCount").GetComponent<UILabel>().text = awardCount3.ToString();
        SetBorder(SprAward3.transform.Find("IconBorder").gameObject, awardType3, awardID3, awardCount3);
        if (awardType4 > 0)
        {
            UITool.f_SetIconSprite(SprAward4, (EM_ResourceType)awardType4, awardID4);
            SprAward4.transform.Find("LabelCount").GetComponent<UILabel>().text = awardCount4.ToString();
            SetBorder(SprAward4.transform.Find("IconBorder").gameObject, awardType4, awardID4, awardCount4);
        }
        SprAward4.gameObject.SetActive(awardType4 > 0);
    }
    /// <summary>
    /// 设置边框
    /// </summary>
    private void SetBorder(GameObject border,int type,int id,int count)
    {
        ResourceCommonDT dt = new ResourceCommonDT();
        dt.f_UpdateInfo((byte)type, id, count);
        string name = dt.mName;
        border.GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(dt.mImportant, ref name);

        if (border.transform.Find("effect") != null)
            GameObject.DestroyImmediate(border.transform.Find("effect").gameObject);
        if (type == (int)EM_ResourceType.Good)
        {
            BaseGoodsDT baseGoodDT = glo_Main.GetInstance().m_SC_Pool.m_BaseGoodsSC.f_GetSC(id) as BaseGoodsDT;
            if ((baseGoodDT.iImportant >= (int)EM_Important.Red) && baseGoodDT.iEffect == (int)EM_GoodsEffect.OptionalReward)
            {
                UITool.f_CreateEquipEffect(border.transform, "effect", (EM_Important)dt.mImportant, Vector3.zero, new Vector3(160, 160, 160));
            }
        }
    }
}
