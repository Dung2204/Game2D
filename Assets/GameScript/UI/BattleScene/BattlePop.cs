using UnityEngine;
using System.Collections;

public class BattlePop
{
    private TweenPosition _BattlePopW;
    private TweenPosition _BattlePopH;


    public BattlePop(TweenPosition tTweenPositionW, TweenPosition tTweenPositionH)
    {
        _BattlePopW = tTweenPositionW;
        _BattlePopH = tTweenPositionH;

        InitMessage();
    }


    private void InitMessage()
    {
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_BattlePopOpenW, On_UI_BattlePopOpenW);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_BattlePopCloseW, On_UI_BattlePopCloseW);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_BattlePopOpenH, On_UI_BattlePopOpenH);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_BattlePopCloseH, On_UI_BattlePopCloseH);
    }

    private void On_UI_BattlePopOpenW(object Obj)
    {
        // _BattlePopW.enabled = true;
		_BattlePopW.enabled = false;
    }

    private void On_UI_BattlePopCloseW(object Obj)
    {
        _BattlePopW.enabled = false;
        _BattlePopW.transform.localPosition = Vector3.zero;
    }

    private void On_UI_BattlePopOpenH(object Obj)
    {
        // _BattlePopH.enabled = true;
		_BattlePopH.enabled = false;
    }

    private void On_UI_BattlePopCloseH(object Obj)
    {
        _BattlePopH.enabled = false;
        _BattlePopH.transform.localPosition = Vector3.zero;
    }

    public void f_Destory()
    {
        On_UI_BattlePopCloseH(null);
        On_UI_BattlePopCloseW(null);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_BattlePopOpenW, On_UI_BattlePopOpenW);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_BattlePopCloseW, On_UI_BattlePopCloseW);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_BattlePopOpenH, On_UI_BattlePopOpenH);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_BattlePopCloseH, On_UI_BattlePopCloseH);

    }

}