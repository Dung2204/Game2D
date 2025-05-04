using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class DungeonFirstWinPage :UIFramwork
{
    private GameObject mMaskClose;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mMaskClose = f_GetObject("MaskClose");
        f_RegClickEvent(mMaskClose, f_MaskClose);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        int tSycee = 0;
        Data_Pool.m_DungeonPool.mDungeonFinishInfo.f_GetInfo(ref tSycee);
        ResourceCommonDT commonDT = new ResourceCommonDT();
        commonDT.f_UpdateInfo((byte)EM_ResourceType.Money, (byte)EM_MoneyType.eUserAttr_Sycee, tSycee);
        f_GetObject("LabelCount").GetComponent<UILabel>().text = commonDT.mResourceNum.ToString();
        f_GetObject("Icon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(commonDT.mIcon);
        string name = commonDT.mName;
        f_GetObject("IconBorder").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(commonDT.mImportant,ref name);
        f_GetObject("LabelName").GetComponent<UILabel>().text = name;
        if (f_GetObject("ModelPoint").transform.Find("Model") != null)//删除旧的卡牌模型
            UITool.f_DestoryStatelObject(f_GetObject("ModelPoint").transform.Find("Model").gameObject);
        DungeonTollgateDT tollgateDT = (DungeonTollgateDT)glo_Main.GetInstance().m_SC_Pool.m_DungeonTollgateSC.f_GetSC(StaticValue.m_CurBattleConfig.m_iTollgateId);
        UITool.f_CreateRoleByModeId(tollgateDT.iModeId, f_GetObject("ModelPoint").transform, new Vector3(0,180,0), Vector3.zero, 400, "Model", 90);
        f_RegClickEvent("Icon", OnAwardIconClick, commonDT);


        if (f_GetObject("SprTitle").transform.GetComponent<TweenScale>() != null)
        {
            Destroy(f_GetObject("SprTitle").transform.GetComponent<TweenScale>());
        }
        TweenScale ts = f_GetObject("SprTitle").AddComponent<TweenScale>();
        ts.from = new Vector3(0, 0, 1);
        ts.to = new Vector3(1, 1, 1);
        ts.animationCurve = AnimationCurve.EaseInOut(0, 0, 0.5f, 1);
    }

    /// <summary>
    /// 点击奖励icon弹出详细信息
    /// </summary>
    private void OnAwardIconClick(GameObject go, object obj1, object obj2)
    {
        SignedDT signedDT = obj1 as SignedDT;
        ResourceCommonDT commonData = (ResourceCommonDT)obj1;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
    }
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_MaskClose(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.DungeonFirstWinPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
    }
}
