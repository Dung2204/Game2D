using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;
public class ShopCardList : UIFramwork
{


    private SocketCallbackDT InfoCallBack=new SocketCallbackDT();
    private SocketCallbackDT GetCardCallBack=new SocketCallbackDT();

    private Transform selectTran;
    private ShopCardParam selectParam;
    public void OpenUI()
    {
        InfoCallBack.m_ccCallbackFail = InfoFail;
        InfoCallBack.m_ccCallbackSuc = InfoSuc;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_SevenStarPool.f_AwardList(InfoCallBack);
        gameObject.SetActive(true);

    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("Mask", close);

    }
    private void InfoSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UpdateUI();
    }

    private void InfoFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
UITool.Ui_Trip("Không thành công "+obj.ToString());
    }

    private void GetCardSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        GameObject Show = selectTran.Find("Show").gameObject;
        GameObject Hide = selectTran.Find("Hide").gameObject;
        Show.SetActive(false);
        Hide.SetActive(true);
        UIInput Input = selectTran.Find("Key").GetComponent<UIInput>();
        Input.value = selectParam.Key;
    }

    private void GetCardFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
UITool.Ui_Trip("Không thành công" + obj.ToString());
    }

    private void UpdateUI()
    {
        GridUtil.f_SetGridView<ShopCardParam>(f_GetObject("ItemParent"), f_GetObject("Item"),
            Data_Pool.m_SevenStarPool.mShopCardList, UpdateItem);
    }
    private void UpdateItem(GameObject go, ShopCardParam Node)
    {
        System.DateTime Time = ccMath.time_t2DateTime(Node.Time);
        Transform parent = go.transform;
        UI2DSprite Icon = parent.Find("Card").GetComponent<UI2DSprite>();
        UILabel TimeLabel = parent.Find("Time").GetComponent<UILabel>();
        TimeLabel.text = string.Format("{0}-{1}-{2}  {3}:{4}:{5}", Time.Year, Time.Month, Time.Day, Time.Hour, Time.Minute, Time.Second);
        UIInput Input = parent.Find("Key").GetComponent<UIInput>();
        GameObject Show = parent.Find("Show").gameObject;
        GameObject Hide = parent.Find("Hide").gameObject;
        Input.value = "**************";
        Icon.sprite2D = UITool.f_GetIconSprite(307);
        Icon.MakePixelPerfect();
        Show.SetActive(true);
        Hide.SetActive(false);
        f_RegClickEvent(Show,ShowKey, Node, parent);
        f_RegClickEvent(Hide, HideKey, Input, parent);
    }

    private void ShowKey(GameObject go, object obj1, object obj2)
    {
        selectParam = (ShopCardParam)obj1;
        selectTran = (Transform)obj2;  // input = selectInput = (UIInput)obj2;
        GetCardCallBack.m_ccCallbackSuc = GetCardSuc;
        GetCardCallBack.m_ccCallbackFail = GetCardFail;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_SevenStarPool.f_GetShopCardKey(selectParam.ID, GetCardCallBack);
    }

    private void HideKey(GameObject go, object obj1, object obj2) {
        UIInput password = (UIInput)obj1;
        password.value= "**************";
        Transform tran = (Transform)obj2;
        GameObject Show = tran.Find("Show").gameObject;
        GameObject Hide = tran.Find("Hide").gameObject;
        Show.SetActive(true);
        Hide.SetActive(false);
    }

    private void close(GameObject go, object obj1, object obj2) {
        gameObject.SetActive(false);
    }
}


public class ShopCardParam
{
    public int Time;
    public string Key;
    public int ID;
    public ShopCardParam()
    {
        Time = 0;
        Key = string.Empty;
        ID = 0;
    }
}
