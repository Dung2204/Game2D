using System.Collections;
using ccU3DEngine;
using UnityEngine;
/// <summary>
/// 通用的帮助界面
/// </summary>
public class SkillIntroDetailPage : UIFramwork
{
    /// <summary>
    /// 初始化消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BlackBG", OnBlackBGClick);
    }
    /// <summary>
    /// 打开界面
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        object[] infoMagic = (object[])e;
        MagicDT magicDT = (MagicDT)infoMagic[0];
        f_GetObject("LabelName").GetComponent<UILabel>().text = "[FAE96E]" + magicDT.szName;
        f_GetObject("LabelContent").GetComponent<UILabel>().text = magicDT.szReadme;
        //string icon = (string)infoMagic[1];
        string icon = magicDT.iId.ToString();

        f_GetObject("Icon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetSkillIcon(icon);
    }
    /// <summary>
    /// 点击黑色背景
    /// </summary>
    private void OnBlackBGClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SkillIntroDetailPage, UIMessageDef.UI_CLOSE);
    }
}
