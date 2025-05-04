using UnityEngine;
using System.Collections;
/// <summary>
/// 用户签到
/// </summary>
public class ActUserSignItemCtl : MonoBehaviour {
    public UILabel LabelUserSignTitle;//标题
    public GameObject BtnGet;//签到/领取按钮
    public UI2DSprite SprAward;//奖励
    public UISprite AwardBorder;//边框
    public UILabel LabelVipHint;//vip提示文字
    public UISprite SprRightDir;//已领取图片
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="day">天数</param>
    public void SetData(int day,int awardtype,int awardID,int awardCount,string vipHintText, EM_AwardGetState awardGetState, bool isCanGetDay)
    {
        //SprAward.depth = SprAward.depth - day;
        LabelUserSignTitle.text = day.ToString();
        UITool.f_SetIconSprite(SprAward, (EM_ResourceType)awardtype, awardID);
        SprAward.transform.Find("LabelCount").GetComponent<UILabel>().text = awardCount.ToString();

        UITool.f_Set2DSpriteGray(SprAward, awardGetState == EM_AwardGetState.AlreadyGet);
        UITool.f_SetSpriteGray(AwardBorder, awardGetState == EM_AwardGetState.AlreadyGet);

        ResourceCommonDT dt = new ResourceCommonDT();
        dt.f_UpdateInfo((byte)awardtype, awardID, awardCount);
        string name = dt.mName;
        AwardBorder.spriteName = UITool.f_GetImporentColorName(dt.mImportant, ref name);
        LabelVipHint.text = vipHintText;

        SprRightDir.gameObject.SetActive(awardGetState == EM_AwardGetState.AlreadyGet);

        BtnGet.GetComponent<UISprite>().spriteName = isCanGetDay ? "mrqd_pic_c" : "mrqd_pic_b";
        BtnGet.GetComponent<UISprite>().depth = BtnGet.GetComponent<UISprite>().depth - day;
        BtnGet.GetComponent<UISprite>().MakePixelPerfect();

        transform.Find("LabelDay/day1").GetComponent<UISprite>().spriteName = isCanGetDay ? "mrqd_font_di_highight" : "mrqd_font_di";
        transform.Find("LabelDay/day2").GetComponent<UISprite>().spriteName = isCanGetDay ? "mrqd_font_tian_highight" : "mrqd_font_tian";
        //LabelUserSignTitle.color = isCanGetDay ? UITool.HexToColor("fffcebff"): UITool.HexToColor("bab09eff");

        transform.Find("Effect").gameObject.SetActive(isCanGetDay);
    }

    private void addTween(Transform target)
    {
        TweenPosition tween = target.GetComponent<TweenPosition>();
        if (tween)
        {
            tween.@from = target.localPosition;
            tween.to = new Vector3(target.localPosition.x+10, target.localPosition.y, target.localPosition.z);
            tween.duration = 0.25f;
            EventDelegate.Callback cb = delegate()
            {
                tween.PlayReverse();
            };
            tween.SetOnFinished(cb);
        }
        else
        {
            tween = target.gameObject.AddComponent<TweenPosition>();
            addTween(target);
        }
    }

    public void playTween()
    {
        TweenWidth tween = transform.GetComponent<TweenWidth>();
        if (!tween)
        {
            tween = transform.gameObject.AddComponent<TweenWidth>();
        }

        tween.@from = 180;
        tween.to = 190;
        tween.duration = 0.25f;
        EventDelegate.Callback cb = delegate() { tween.PlayReverse(); };
        tween.SetOnFinished(cb);
        addTween(SprAward.transform);
        addTween(LabelUserSignTitle.transform);
        addTween(SprRightDir.transform);
        addTween(transform.Find("Effect"));
        addTween(LabelVipHint.transform);
    }
}
