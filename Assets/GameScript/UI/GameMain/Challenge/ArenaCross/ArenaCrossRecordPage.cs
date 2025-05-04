using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class ArenaCrossRecordPage : UIFramwork
{
    private GameObject mRecordItemParent;
    private GameObject mrecorditem;
    private UIWrapComponent _recordWrapComponet;
    public UIWrapComponent mRecordWrapComponet
    {
        get
        {
            if (_recordWrapComponet == null)
            {
                mArenaRecordList = Data_Pool.m_CrossArenaPool.f_GetRecordList();
                _recordWrapComponet = new UIWrapComponent(100, 1, 100, 6, mRecordItemParent, mrecorditem, mArenaRecordList, f_RecordItemUpdateByInfo, null);
            }
            return _recordWrapComponet;
        }
    }

    private List<BasePoolDT<long>> mArenaRecordList;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mrecorditem = f_GetObject("RecordItem");
        mRecordItemParent = f_GetObject("ArenaRecordItemParent");

        f_RegClickEvent("BtnClose2", f_CloseBtnHandle);
        f_RegClickEvent("MaskClose", f_CloseBtnHandle);
    }
    protected override void f_Create()
    {
        _InitReference();
        base.f_Create();
    }
    private void _InitReference()
    {
        AddGOReference("Panel/Anchor-Center/BG/Scroll View/ArenaRecordItemParent");
        AddGOReference("Panel/Anchor-Center/RecordItem");
        AddGOReference("Panel/Anchor-Center/BtnClose2");
        AddGOReference("Panel/MaskClose");
    }
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        mRecordWrapComponet.f_ResetView();
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        //glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_ArenaRankPage_CLOSE);
    }

    private void f_RecordItemUpdateByInfo(Transform tf, BasePoolDT<long> dt)
    {
        UILabel text = tf.GetComponent<UILabel>();
        ArenaCrossRecordPoolDT record = (ArenaCrossRecordPoolDT)dt;
        DateTime time = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Unspecified).AddSeconds(record.m_ArenaCrossRecord.recordTime + 7 * 60 * 60);
        string sztime = string.Format("{0}/{1} {2}:{3}", time.Month, time.Day, time.Hour, time.Minute);
        string mess = string.Format(CommonTools.f_GetTransLanguage(2306), record.ServerName, record.m_ArenaCrossRecord.userName, record.ServerNameEnemy, record.m_ArenaCrossRecord.enemyName);
        string win = (record.m_ArenaCrossRecord.battleRes != 0 ? (record.m_ArenaCrossRecord.myChange != 0 ? string.Format(CommonTools.f_GetTransLanguage(2307),record.m_ArenaCrossRecord.enemyChange, record.m_ArenaCrossRecord.myChange): CommonTools.f_GetTransLanguage(2309)) : CommonTools.f_GetTransLanguage(2308));
        text.text = "[564E2E]" + sztime +"[-]"+ mess +"\n"+ win;
    }

    private void f_CloseBtnHandle(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaCrossRecordPage, UIMessageDef.UI_CLOSE);
    }
}