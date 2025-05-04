using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class ArenaBreakAwardPage : UIFramwork
{
    private UILabel mCurRankLabel;
    private UILabel mBreakRankLabel;
    private UILabel mBreakSyceeLabel;

    private GameObject mMaskClose;

    private TweenScale m_TitleAmi;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mCurRankLabel = f_GetObject("CurRankLabel").GetComponent<UILabel>();
        mBreakRankLabel = f_GetObject("BreakRankLabel").GetComponent<UILabel>();
        mBreakSyceeLabel = f_GetObject("BreakSyceeLabel").GetComponent<UILabel>();
        mMaskClose = f_GetObject("MaskClose");
        f_RegClickEvent(mMaskClose, f_MaskClose);
        m_TitleAmi = f_GetObject("Title").GetComponent<TweenScale>();
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        int curRank = 0;
        int rankBreakNum = 0;
        int breakSyceeNum = 0;
        Data_Pool.m_ArenaPool.mBreakRankInfo.f_GetInfo(ref curRank, ref rankBreakNum, ref breakSyceeNum);
        mCurRankLabel.text = curRank.ToString();
        mBreakRankLabel.text = rankBreakNum.ToString();
        mBreakSyceeLabel.text = breakSyceeNum.ToString();
        m_TitleAmi.ResetToBeginning();
        m_TitleAmi.Play(true);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_MaskClose(GameObject go,object value1,object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaBreakAwardPage, UIMessageDef.UI_CLOSE);
    }
}
