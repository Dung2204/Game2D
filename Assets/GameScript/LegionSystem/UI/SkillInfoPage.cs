using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class SkillInfoPageParam
{
    public int iconId;
    public string mSkillName;
    public string mCurLevelName;
    public string mContent;
}

public class SkillInfoPage : UIFramwork
{
    /// <summary>
    /// 初始化消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BlackBG", OnBlackBGClick);
        f_RegClickEvent("BtnClose", OnBlackBGClick);
    }
    /// <summary>
    /// 打开界面
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        SkillInfoPageParam param = (SkillInfoPageParam)e;

        EM_Important skillImportant = EM_Important.Red;
        if (param.iconId >= (int)EM_Important.White && param.iconId <= (int)EM_Important.Red)
            skillImportant = (EM_Important)param.iconId;
        f_GetObject("Icon_Border").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName((int)skillImportant, ref param.mSkillName);

        f_GetObject("Icon").GetComponent<UISprite>().spriteName = param.iconId.ToString();
        f_GetObject("LabelName").GetComponent<UILabel>().text = param.mSkillName;
        f_GetObject("LabelLevel").GetComponent<UILabel>().text = param.mCurLevelName;
        f_GetObject("LabelContent").GetComponent<UILabel>().text = param.mContent;
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
    }

    /// <summary>
    /// 点击黑色背景
    /// </summary>
    private void OnBlackBGClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SkillInfoPage, UIMessageDef.UI_CLOSE);
    }

    private void f_ProcessByMsg_ForceClose(object value)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SkillInfoPage, UIMessageDef.UI_CLOSE);
    }
}
