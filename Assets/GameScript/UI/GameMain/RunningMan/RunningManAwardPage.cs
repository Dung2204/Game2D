using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class RunningManAwardPage : UIFramwork
{
    private UILabel mCurStar;
    private UILabel mDesc;
    private RunningManAwardItem[] mItems;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mCurStar = f_GetObject("CurStar").GetComponent<UILabel>();
        mDesc = f_GetObject("Desc").GetComponent<UILabel>();
        mItems = new RunningManAwardItem[GameParamConst.RMTollgateNumPreChap];
        for (int i = 0; i < mItems.Length; i++)
        {
            mItems[i] = f_GetObject(string.Format("RunningManAwardItem{0}", i)).GetComponent<RunningManAwardItem>();
        }
        f_RegClickEvent("CloseMask", f_CloseMask); 
        f_RegClickEvent("Sprite_Close", f_CloseMask); 
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        RunningManPoolDT poolDT = (RunningManPoolDT)e;
        mCurStar.text = string.Format(CommonTools.f_GetTransLanguage(833),Data_Pool.m_RunningManPool.m_iCurStarNum);
        mDesc.text = string.Format(CommonTools.f_GetTransLanguage(834),
            poolDT.m_TollgatePoolDTs[0].m_TollgateTemplate.iId,poolDT.m_TollgatePoolDTs[poolDT.m_TollgatePoolDTs.Length-1].m_TollgateTemplate.iId);
        mItems[0].f_UpdateByInfo(0, poolDT.m_ChapterTemplate.iBox3);
        mItems[1].f_UpdateByInfo(1, poolDT.m_ChapterTemplate.iBox6);
        mItems[2].f_UpdateByInfo(2, poolDT.m_ChapterTemplate.iBox9);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_CloseMask(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManAwardPage, UIMessageDef.UI_CLOSE);   
    }

}
