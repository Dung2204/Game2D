using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 休眠提示
/// </summary>
public class SleepTimePage : UIFramwork
{
    public static bool m_ShowPage = false;
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BlackBG", OnCloseClick);
    }
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        m_ShowPage = true;
        int energyValue = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Energy);
        int vigorValue = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vigor);
        int crusadeTokenValue = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_CrusadeToken);
        f_GetObject("EnergyValue").GetComponent<UILabel>().text = energyValue + "/100";
        f_GetObject("VigorValue").GetComponent<UILabel>().text = vigorValue + "/30";
        f_GetObject("ChallengeValue").GetComponent<UILabel>().text = crusadeTokenValue + "/10";
        if (energyValue > 0 || vigorValue > 0 || crusadeTokenValue > 0)
        {
f_GetObject("LabelContent").GetComponent<UILabel>().text = "The General did nothing for 10 minutes although there is still work to do！";
        }
        else
        {
f_GetObject("LabelContent").GetComponent<UILabel>().text = "General did nothing for 10 minutes！";
        }
    }
    protected override void On_Destory()
    {
        base.On_Destory();
        m_ShowPage = false;
    }
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        m_ShowPage = false;
    }
    private void OnCloseClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SleepTimePage, UIMessageDef.UI_CLOSE);
    }
}
