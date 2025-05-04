using UnityEngine;
using System.Collections;
/// <summary>
/// 登录送礼item
/// </summary>
public class LoginGiftNewServItem : MonoBehaviour {
    public UISprite mSprHasGet;//已领取修改

    public UI2DSprite mAwardIcon;//奖励icon
    public UISprite mIconBorder;//边框
    public UILabel mLabelAwardCount;//奖励数量
    public UILabel mLabelName;//奖励名称

    public UISprite SprBoxBg;//box背景
    public UISprite SprNameBg;//奖励名字背景
    public void SetData(int day, EM_AwardGetState emAwardGetState, int awardType, int awardID, int awardCount)
    {
        mSprHasGet.gameObject.SetActive(emAwardGetState == EM_AwardGetState.AlreadyGet);
        SprBoxBg.spriteName = emAwardGetState == EM_AwardGetState.AlreadyGet ? "Border_NewSevDark" : "Border_NewSevLight";
        SprNameBg.spriteName = emAwardGetState == EM_AwardGetState.AlreadyGet ? "Border_NewSevNameDarkBg" : "Border_NewSevNameLightBg";
		if(day == 7)
		{
			SprBoxBg.spriteName = emAwardGetState == EM_AwardGetState.AlreadyGet ? "Border_NewSevTitleDark" : "Border_NewServTitleLight";
		}
        UITool.f_SetSpriteGray(mIconBorder, emAwardGetState == EM_AwardGetState.AlreadyGet);
        UITool.f_Set2DSpriteGray(mAwardIcon, emAwardGetState == EM_AwardGetState.AlreadyGet);

        UITool.f_SetIconSprite(mAwardIcon, (EM_ResourceType)awardType, awardID);
        ResourceCommonDT dt = new ResourceCommonDT();
        dt.f_UpdateInfo((byte)awardType, awardID, awardCount);
        string name = dt.mName;
        mIconBorder.GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(dt.mImportant, ref name);
        mLabelAwardCount.text = awardCount.ToString();
mLabelName.text = "Celebration" + day;
    }
}
