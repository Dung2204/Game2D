using UnityEngine;
using System.Collections;

public class ActLoginGiftItem : UIFramwork {

    

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="awardID1">奖励1</param>
    /// <param name="awardCount1">奖励1数量</param>
    /// <param name="awardID2">奖励2</param>
    /// <param name="awardCount2">奖励2数量</param>
    /// <param name="awardID3">奖励3</param>
    /// <param name="awardCount3">奖励3数量</param>
    /// <param name="awardID4">奖励4</param>
    /// <param name="awardCount4">奖励4数量</param>
    /// <param name="progress">进度</param>
    public void SetData(string title,int awardType1, int awardID1, int awardCount1,int awardType2, int awardID2, int awardCount2,
        int awardType3, int awardID3, int awardCount3,int awardType4, int awardID4, int awardCount4,string progress)
    {
        f_GetObject("LabelDay").GetComponent<UILabel>().text = title;
        UITool.f_SetIconSprite(f_GetObject("Award1").GetComponent<UI2DSprite>(), (EM_ResourceType)awardType1, awardID1);
        f_GetObject("Award1").transform.Find("LabelCount").GetComponent<UILabel>().text = awardCount1.ToString();
        SetBorder(f_GetObject("Award1").transform.Find("IconBorder").gameObject, awardType1, awardID1, awardCount1);
        UITool.f_SetIconSprite(f_GetObject("Award2").GetComponent<UI2DSprite>(), (EM_ResourceType)awardType2, awardID2);
        f_GetObject("Award2").transform.Find("LabelCount").GetComponent<UILabel>().text = awardCount2.ToString();
        SetBorder(f_GetObject("Award2").transform.Find("IconBorder").gameObject, awardType2, awardID2, awardCount2);

        UITool.f_SetIconSprite(f_GetObject("Award3").GetComponent<UI2DSprite>(), (EM_ResourceType)awardType3, awardID3);
        f_GetObject("Award3").transform.Find("LabelCount").GetComponent<UILabel>().text = awardCount3.ToString();
        SetBorder(f_GetObject("Award3").transform.Find("IconBorder").gameObject, awardType3, awardID3, awardCount3);
        UITool.f_SetIconSprite(f_GetObject("Award4").GetComponent<UI2DSprite>(), (EM_ResourceType)awardType4, awardID4);
        f_GetObject("Award4").transform.Find("LabelCount").GetComponent<UILabel>().text = awardCount4.ToString();
        SetBorder(f_GetObject("Award4").transform.Find("IconBorder").gameObject, awardType4, awardID4, awardCount4);

        f_GetObject("LabelProgress").GetComponent<UILabel>().text = progress;
    }
    public GameObject f_GetAwardObj(int index)
    {
        return f_GetObject("Award" + index.ToString());
    }
    /// <summary>
    /// 设置边框
    /// </summary>
    private void SetBorder(GameObject border, int type, int id, int count)
    {
        ResourceCommonDT dt = new ResourceCommonDT();
        dt.f_UpdateInfo((byte)type, id, count);
        string name = dt.mName;
        border.GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(dt.mImportant, ref name);
    }
}
