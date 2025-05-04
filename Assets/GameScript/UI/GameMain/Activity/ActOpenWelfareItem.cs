using UnityEngine;
using System.Collections;
/// <summary>
/// 全民福利
/// </summary>
public class ActOpenWelfareItem : UIFramwork {
    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(EM_ResourceType resourceType, int resourceId, int resourceCount, int buyCount)
    {
        string name = resourceCount + UITool.f_GetGoodName(resourceType, resourceId);
        f_GetObject("LabelLevelHint").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1356), buyCount);
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
