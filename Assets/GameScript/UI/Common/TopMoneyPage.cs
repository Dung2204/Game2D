using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
/// <summary>
/// 右上角金钱界面
/// </summary>
public class TopMoneyPage : UIFramwork {
    private List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
    /// <summary>
    /// 界面打开
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e != null)
        {
            listMoneyType = (List<EM_MoneyType>)e;
            if (listMoneyType.Count > 5)
            {
Debug.LogWarning("Only 5 currency UIs can be displayed");
            }
            CloseAllMoneyUI();
        }
        UpdateUIData(null);
    }
    protected override void On_Destory()
    {
        base.On_Destory();
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_UPDATE_USERINFOR, UpdateUIData, this);
    }
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_UPDATE_USERINFOR, UpdateUIData, this);
    }
    protected override void UI_CLOSE(object e)
    {
        _NeedCloseSound = false;
        base.UI_CLOSE(e);
    }
    /// <summary>
    /// 关闭所有显示
    /// </summary>
    private void CloseAllMoneyUI()
    {
        for (int index = 0; index < 5; index++)
        {
            f_GetObject("SprMoney" + index).SetActive(false);
        }
    }

    /// <summary>
    /// 更新UI信息
    /// </summary>
    /// <param name="obj"></param>
    private void UpdateUIData(object obj)
    {

        //  更新的时候  不能UI_Close
        for (int index = 0; index < listMoneyType.Count; index++)
        {
            EM_MoneyType MoneyType = listMoneyType[index];
            if (index > 4)
                break;
            f_GetObject("SprMoney" + index).SetActive(true);
            int hasCount = 0;
            if (MoneyType == EM_MoneyType.eCardRefineStone || MoneyType == EM_MoneyType.eCardAwakenStone || MoneyType == EM_MoneyType.eCardSkyPill
                || MoneyType == EM_MoneyType.eBattleFormFragment || MoneyType == EM_MoneyType.eRedEquipElite || MoneyType == EM_MoneyType.eRedBattleToken
                || MoneyType == EM_MoneyType.eFreshToken || MoneyType == EM_MoneyType.eTreasureRefinePill || MoneyType == EM_MoneyType.eNorAd
                || MoneyType == EM_MoneyType.eGenAd || MoneyType == EM_MoneyType.eCampAd || MoneyType == EM_MoneyType.eGemCamp || MoneyType == EM_MoneyType.eLimitAd)
            {
                hasCount = UITool.f_GetGoodNum(EM_ResourceType.Good, (int)MoneyType);
            }
            else
            {
                hasCount = Data_Pool.m_UserData.f_GetProperty((int)MoneyType);
            }
            f_GetObject("SprMoney" + index).transform.Find("LabelCount").GetComponent<UILabel>().text = hasCount.ToString();
            f_GetObject("SprMoney" + index).transform.Find("Icon").GetComponent<UISprite>().spriteName = UITool.f_GetMoneySpriteName(MoneyType);
            //f_GetObject("SprMoney" + index).transform.Find("Icon").GetComponent<UISprite>().MakePixelPerfect();
            string moneyText = hasCount.ToString();
            moneyText = UITool.f_CountToChineseStr2(hasCount);
            f_GetObject("SprMoney" + index).transform.Find("LabelCount").GetComponent<UILabel>().text = moneyText;
        }
    }
}
