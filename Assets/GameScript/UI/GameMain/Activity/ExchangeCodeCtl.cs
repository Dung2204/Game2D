using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 兑换码界面
/// </summary>
public class ExchangeCodeCtl : UIFramwork
{

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnConfirm", SendCode);
    }

    private SocketCallbackDT RequestExchangeCodeCallback = new SocketCallbackDT();//请求礼品码回调
    UIInput tUIInput;
    public void f_DestoryView()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 初始化数据,视图
    /// </summary>
    public void f_ShowView()
    {
        gameObject.SetActive(true);
        tUIInput = f_GetObject("InputValue").GetComponent<UIInput>();
        InputValueInit();
        RequestExchangeCodeCallback.m_ccCallbackSuc = OnUserSignSucCallback;
        RequestExchangeCodeCallback.m_ccCallbackFail = OnUserSignFailCallback;
        //Data_Pool.m_AwardPool.f_ExchangeCode();
        //UITool.f_OpenOrCloseWaitTip(true);
		float windowAspect = (float)Screen.width /  (float) Screen.height ;
		MessageBox.ASSERT("" + windowAspect);
		if(windowAspect < 1.7)
		{
			f_GetObject("Bg").transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
		}
    }
    #region 每日签到回调
    /// <summary>
    /// 每日签到成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnUserSignSucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        System.Collections.Generic.List<AwardPoolDT> tListAward = Data_Pool.m_AwardPool.f_GetAwardPoolDTByType(EM_AwardSource.eAward_Cdkey);

        if (tListAward.Count >= 1)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { tListAward });
        }

        InputValueInit();
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnUserSignFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        switch ((eMsgOperateResult)obj)
        {
            case eMsgOperateResult.eOR_CdkeyInvalid:
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1368));
                break;
            case eMsgOperateResult.eOR_CdkeyTimesLimit:
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1369));
                break;
            case eMsgOperateResult.eOR_CdkeyUserLimit:
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1370));
                break;
            case eMsgOperateResult.eOR_CdkeyTimeout:
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1371));
                break;
            case eMsgOperateResult.eOR_CdkeyNotOpen:
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1372));
                break;
            default:
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1373) + obj.ToString());
                break;
        }


        InputValueInit();
        UITool.f_OpenOrCloseWaitTip(false);
    }
    #endregion


    private void SendCode(GameObject go, object obj1, object obj2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        string tCode = tUIInput.value.Trim();
        Data_Pool.m_AwardPool.f_ExchangeCode(tCode, RequestExchangeCodeCallback);
    }

    private void InputValueInit()
    {
        if (tUIInput != null)
        {
            // tUIInput.value = CommonTools.f_GetTransLanguage(1374);
			tUIInput.value = "";
        }
    }
}
