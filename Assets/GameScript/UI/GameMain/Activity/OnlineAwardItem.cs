using UnityEngine;
using System.Collections;
using ccU3DEngine;

/// <summary>
/// 全民福利
/// </summary>
public class OnlineAwardItem : UIFramwork
{
    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(EM_ResourceType resourceType, int resourceId, int resourceCount, int time)
    {
        ResourceCommonDT dt = new ResourceCommonDT();
        dt.f_UpdateInfo((byte)resourceType, resourceId, resourceCount);
        string name = dt.mName + "×" + resourceCount;
        f_GetObject("LabelCount").GetComponent<UILabel>().text = resourceCount.ToString();
        f_GetObject("IconBorder").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(dt.mImportant, ref name);
        f_GetObject("Hint").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1395), time);
        UITool.f_SetIconSprite(f_GetObject("Icon").GetComponent<UI2DSprite>(), resourceType, resourceId);

        f_RegClickEvent(this.gameObject, delegate(GameObject go, object v1, object v2)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, v1);
        }, dt, null);
    }
}

