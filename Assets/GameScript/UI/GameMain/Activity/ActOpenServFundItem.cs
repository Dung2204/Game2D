using UnityEngine;
using System.Collections;
/// <summary>
/// 开服送礼item
/// </summary>
public class ActOpenServFundItem : UIFramwork {
    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(EM_ResourceType resourceType, int resourceId,int resourceCount,int openLevel)
    {
        string name = resourceCount + UITool.f_GetGoodName(resourceType, resourceId);
f_GetObject("LabelLevelHint").GetComponent<UILabel>().text = "Pass " + openLevel + " get";
        UITool.f_SetIconSprite(f_GetObject("Award").GetComponent<UI2DSprite>(), resourceType, resourceId);
        f_GetObject("Award").transform.Find("LabelCount").GetComponent<UILabel>().text = resourceCount.ToString();
        ResourceCommonDT dt = new ResourceCommonDT();
        dt.f_UpdateInfo((byte)resourceType, resourceId, resourceCount);
        f_GetObject("Award").transform.Find("IconBorder").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(dt.mImportant, ref name);
        f_GetObject("LabelAwardHint").GetComponent<UILabel>().text = name;
    }
    public GameObject GetAwardObj()
    {
        return f_GetObject("Award");
    }
}
