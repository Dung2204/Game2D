using UnityEngine;
using System.Collections;
/// <summary>
/// 扫荡item
/// </summary>
public class GrabSweepResultItem : MonoBehaviour
{
    public UILabel LabelTimes;//次数
    public UILabel LabelHint;//提示，有没有获得碎片
    public UILabel LabelMoneyCount;//获得的银币数量
    public UILabel LabelEx;//获得的经验值
    public UI2DSprite SprAwardIcon;//奖励的icon
    public UISprite SprBorder;//奖励物品的边框
    public UILabel LabelCount;//奖励物品的数量
    public UILabel LabelName;//奖励名称
    public GameObject WarFail;
    public GameObject WarSuc;
    public GameObject AwardGoods;
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="times"></param>
    /// <param name="hint"></param>
    /// <param name="moneyCount"></param>
    /// <param name="ex"></param>
    /// <param name="awardType"></param>
    /// <param name="awardID"></param>
    /// <param name="awardCount"></param>
    public void SetData(int times, string hint, int moneyCount, string ex, EM_ResourceType awardType, int awardID, int awardCount, bool isFail, bool isOneKey = false)
    {
        if(!isOneKey)
LabelTimes.text = "Trận chiến " + times + "";
        else
LabelTimes.text = "Trận chiến " + times + "";
        LabelHint.text = hint;
        LabelMoneyCount.text = moneyCount.ToString();
        LabelEx.text = ex.ToString();
        if(AwardGoods != null)
        {
            AwardGoods.SetActive(awardID != 0);
        }
        if(WarFail != null)
        {
            WarSuc.SetActive(!isFail);
            WarFail.SetActive(isFail);
        }
        if(!isFail)
        {
            ResourceCommonDT comonDT = new ResourceCommonDT();
            comonDT.f_UpdateInfo((byte)awardType, awardID, awardCount);
            if(SprAwardIcon != null)
                SprAwardIcon.sprite2D = UITool.f_GetIconSprite(comonDT.mIcon);
            string name = comonDT.mName;

            if(SprBorder != null)
                SprBorder.spriteName = UITool.f_GetImporentColorName(comonDT.mImportant, ref name);
            if(LabelCount != null)
                LabelCount.text = "";
           // if(!isOneKey)
           //     LabelName.text = name;
           // else
                LabelName.text = name + "X" + awardCount;
        }
    }
    /// <summary>
    /// 数字转中文
    /// </summary>
    /// <param name="number">0-5</param>
    /// <returns></returns>
    public string NumberToChinese(int number)
    {
        string res = string.Empty;
        switch(number.ToString())
        {
            case "1":
                res = "1";
                break;
            case "2":
                res = "2";
                break;
            case "3":
                res = "3";
                break;
            case "4":
                res = "4";
                break;
            case "5":
                res = "5";
                break;
        }
        return res;
    }
}
