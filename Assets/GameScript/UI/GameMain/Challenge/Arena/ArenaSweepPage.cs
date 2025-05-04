using UnityEngine;
using ccU3DEngine;
using System.Collections;

public class ArenaSweepPage : UIFramwork
{
    private ArenaSweepItem[] mArenaSweepItems;
    private GameObject mBtn_SweepClose;
    private UISlider m_ScrollBar;

    private const int SWEEP_MAX_NUM = 5;
    //扫荡显示时间间隔
    private const float SWEEP_TIME_DIS = 0.3f;
    private int mCurSweepNum;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mBtn_SweepClose = f_GetObject("BtnClose");
        mArenaSweepItems = new ArenaSweepItem[SWEEP_MAX_NUM];
        for (int i = 0; i < SWEEP_MAX_NUM; i++)
        {
            mArenaSweepItems[i] = f_GetObject(string.Format("ArenaSweepItem{0}", i)).GetComponent<ArenaSweepItem>();
        }
        f_RegClickEvent(mBtn_SweepClose, f_SweepCloseBtn);
        m_ScrollBar = f_GetObject("ScrollBar").GetComponent<UISlider>();
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        Data_Pool.m_GuidancePool.IsOpenArenaSween = true;
        for (int i = 0; i < mArenaSweepItems.Length; i++)
        {
            mArenaSweepItems[i].f_Disable();
        }
        mBtn_SweepClose.SetActive(false);
        StartCoroutine(f_ShowSweepAni());
    }

    private IEnumerator f_ShowSweepAni()
    {
        curBarValue = 0;
        targetBarValue = 0;
        m_ScrollBar.value = 0;
        for (int i = 0; i < mArenaSweepItems.Length; i++)
        {
            mArenaSweepItems[i].f_UpdateByInfo(i, Data_Pool.m_ArenaPool.m_sArenaSweepRet.nodes[i], this);
            yield return new WaitForSeconds(SWEEP_TIME_DIS);
            if (i >= 3)
            {
                targetBarValue = (i + 1.0f - 3) / (SWEEP_MAX_NUM - 3);
            }
        }
        mBtn_SweepClose.SetActive(true);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        Data_Pool.m_GuidancePool.IsOpenArenaSween = false;
    }

    private void f_SweepCloseBtn(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaSweepPage, UIMessageDef.UI_CLOSE);
    }


    private float curBarValue = 0;
    private float targetBarValue = 0;
    private const float BarSpeedValue = 0.1f;
    protected override void f_Update()
    {
        base.f_Update();
        if (Mathf.Abs(targetBarValue - curBarValue) <= 0.01)
            return;
        curBarValue = Mathf.Lerp(curBarValue, targetBarValue, BarSpeedValue);
        m_ScrollBar.value = curBarValue;
    }
}