using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

public class AwakenEquipPage : UIFramwork
{


    private GameObject mEquipInduce;
    private GameObject mEquipSythe;
    private GameObject mBtnEquip;
    private GameObject mBtnSythe;
    private GameObject mBtnGetWay;
    private GameObject mSytheUIBtnSythe;
    private GameObject mSythe3;
    private GameObject mSythe4;

    private UILabel mNum;
    private UILabel mDesc;
    private UILabel mName;
    private UILabel mAtk;
    private UILabel mHp;
    private UILabel mDef;

    private Transform Equip;
    private UILabel mSeytheMoney;



    private UI2DSprite mIcon;
    private UISprite mCase;

    private AwakenEquipPageParam Param;
    private AwakenEquipPoolDT mAwakenEquipDT;


    protected override void InitGUI()
    {
        base.InitGUI();
        mEquipInduce = f_GetObject("EquipInduce");
        mEquipSythe = f_GetObject("EquipSythe");
        mBtnEquip = f_GetObject("EquipBtn");
        mBtnSythe = f_GetObject("GoSytheBtn");
        mBtnGetWay = f_GetObject("GetWayBtn");
        mSythe3 = f_GetObject("Sythe3");
        mSythe4 = f_GetObject("Sythe4");
        mSytheUIBtnSythe = f_GetObject("SytheUIBtnSythe");

        mNum = f_GetObject("Num").GetComponent<UILabel>(); ;
        mDesc = f_GetObject("Desc").GetComponent<UILabel>();
        mName = f_GetObject("Name").GetComponent<UILabel>();
        mIcon = f_GetObject("Icon").GetComponent<UI2DSprite>();
        mCase = f_GetObject("Case").GetComponent<UISprite>();

        mAtk = f_GetObject("Atk").GetComponent<UILabel>();
        mHp = f_GetObject("Hp").GetComponent<UILabel>();
        mDef = f_GetObject("Def").GetComponent<UILabel>();

        mSeytheMoney = f_GetObject("MoneyNum").GetComponent<UILabel>();
    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("EquipBtn", f_Equip);
        f_RegClickEvent("GetWayBtn", f_GetWay);
        // f_RegClickEvent("GoSytheBtn", f_Sythe);
        f_RegClickEvent("AwakenEquipInduce_ABg", f_CloseThis);
        f_RegClickEvent("GoSytheBtn", UpdateSytheUI);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (!(e is AwakenEquipPageParam))
        {
MessageBox.DEBUG("The awakening props parameter failed");
        }
        InitGUI();
        Param = e as AwakenEquipPageParam;
        mAwakenEquipDT = UITool.f_GetAwakenEquip(Param.m_ID);
        if (mAwakenEquipDT == null)
        {
            mAwakenEquipDT = new AwakenEquipPoolDT();
            mAwakenEquipDT.m_AwakenEquipDT = glo_Main.GetInstance().m_SC_Pool.m_AwakenEquipSC.f_GetSC(Param.m_ID) as AwakenEquipDT;
        }
        mEquipInduce.SetActive(true);
        mEquipSythe.SetActive(false);
        UpdateInduce();
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    /// <summary>
    /// 刷新信息界面
    /// </summary>
    /// <param name="go"></param>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    private void UpdateInduce()
    {
        mCase.spriteName = UITool.f_GetImporentCase(mAwakenEquipDT.m_AwakenEquipDT.iImportant);
        mName.text = mAwakenEquipDT.m_AwakenEquipDT.szName;
        mIcon.sprite2D = UITool.f_GetIconSprite(mAwakenEquipDT.m_AwakenEquipDT.iIcon);
        mNum.text = string.Format("{0}:{1}", CommonTools.f_GetTransLanguage(151), mAwakenEquipDT.m_num);
        mDesc.text = mAwakenEquipDT.m_AwakenEquipDT.szDesc;
        mAtk.text = string.Format("[f1b049]{0}[-]     [f0eccb]+{1}", UITool.f_GetProName((EM_RoleProperty)mAwakenEquipDT.m_AwakenEquipDT.iAddProId1), mAwakenEquipDT.m_AwakenEquipDT.iAddPro1);
        mHp.text = string.Format("[f1b049]{0}[-]     [f0eccb]+{1}", UITool.f_GetProName((EM_RoleProperty)mAwakenEquipDT.m_AwakenEquipDT.iAddProId2), mAwakenEquipDT.m_AwakenEquipDT.iAddPro2);
        mDef.text = string.Format("[f1b049]{0}[-]     [f0eccb]+{1}", CommonTools.f_GetTransLanguage(152), mAwakenEquipDT.m_AwakenEquipDT.iAddPro3);

        mBtnEquip.SetActive(Param.m_isEquip&&!Param.m_isBagOpen);
        mBtnGetWay.SetActive(!Param.m_isEquip && !Param.m_isBagOpen);
        mBtnSythe.SetActive(!Param.m_isEquip && !Param.m_isBagOpen);
    }

    private void UpdateSytheUI(GameObject go, object obj1, object obj2)
    {
        mEquipInduce.SetActive(false);
        mEquipSythe.SetActive(true);
        bool Sythe3orSythe4 = mAwakenEquipDT.m_AwakenEquipDT.iSynthesisId4 == 0;   //trueSythe4
        mSythe3.SetActive(Sythe3orSythe4);
        mSythe4.SetActive(!Sythe3orSythe4);
        UpdateItem(f_GetObject("SytheEquip").transform, mAwakenEquipDT.m_AwakenEquipDT.iIcon, mAwakenEquipDT.m_num, mAwakenEquipDT.m_AwakenEquipDT.szName);

        if (Sythe3orSythe4)
        {
            UpdateSytheUI(mSythe3.transform, 3);
        }
        else
        {
            UpdateSytheUI(mSythe4.transform, 4);
        }
        if (mAwakenEquipDT.m_AwakenEquipDT.iMoenyNum > Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money))
            mSeytheMoney.text = "[ec2626]" + mAwakenEquipDT.m_AwakenEquipDT.iMoenyNum;
        else
            mSeytheMoney.text = "[24ff00]" + mAwakenEquipDT.m_AwakenEquipDT.iMoenyNum;

        f_RegClickEvent(mSytheUIBtnSythe, f_Sythe, mAwakenEquipDT.m_AwakenEquipDT);
    }
    private void UpdateSytheUI(Transform Tran, int Num)
    {
        int SytheTmpId = 0;
        int NeedNum = 0;
        AwakenEquipDT DogFood;
        for (int i = 0; i < Num; i++)
        {
            if (i == 0)
            {
                SytheTmpId = mAwakenEquipDT.m_AwakenEquipDT.iSynthesisId1;
                NeedNum = mAwakenEquipDT.m_AwakenEquipDT.iSynthesisNum1;
            }
            else if (i == 1)
            {
                SytheTmpId = mAwakenEquipDT.m_AwakenEquipDT.iSynthesisId2;
                NeedNum = mAwakenEquipDT.m_AwakenEquipDT.iSynthesisNum2;
            }
            else if (i == 2)
            {
                SytheTmpId = mAwakenEquipDT.m_AwakenEquipDT.iSynthesisId3;
                NeedNum = mAwakenEquipDT.m_AwakenEquipDT.iSynthesisNum3;
            }
            else if (i == 3)
            {
                SytheTmpId = mAwakenEquipDT.m_AwakenEquipDT.iSynthesisId4;
                NeedNum = mAwakenEquipDT.m_AwakenEquipDT.iSynthesisNum4;
            }
            DogFood = glo_Main.GetInstance().m_SC_Pool.m_AwakenEquipSC.f_GetSC(SytheTmpId) as AwakenEquipDT;
            _SetAwakenShow(Tran.transform.GetChild(i), DogFood, NeedNum);
        }
    }


    private void f_Equip(GameObject go, object obj1, object obj2)
    {
        if (UITool.f_GetAwakenCardDT(Param.m_Card.m_iLvAwaken).iCardNeedLv > Param.m_Card.m_iLv)
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(148), UITool.f_GetAwakenCardDT(Param.m_Card.m_iLvAwaken).iCardNeedLv));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT CallBack = new SocketCallbackDT();
        CallBack.m_ccCallbackSuc = EquipSuc;
        CallBack.m_ccCallbackFail = EquipFill;
        Data_Pool.m_AwakenEquipPool.f_Equip(Param.m_Card.iId, Param.m_Index, CallBack);
        //f_GetObject("AwakenEquipInduce").SetActive(false);
    }
    private void f_Sythe(GameObject go, object obj1, object obj2)
    {
        AwakenEquipDT tmpEquip = (AwakenEquipDT)obj1;
        if (tmpEquip.iSynthesisNum1 > UITool.f_GetAwakenEquipPoolDT(tmpEquip.iSynthesisId1).m_num ||
            tmpEquip.iSynthesisNum2 > UITool.f_GetAwakenEquipPoolDT(tmpEquip.iSynthesisId2).m_num ||
            tmpEquip.iSynthesisNum3 > UITool.f_GetAwakenEquipPoolDT(tmpEquip.iSynthesisId3).m_num ||
            tmpEquip.iSynthesisNum4 > UITool.f_GetAwakenEquipPoolDT(tmpEquip.iSynthesisId4).m_num)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(153));
            return;
        }
        if (tmpEquip.iMoenyNum > Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(154));
            ccUIBase cardProperty = ccUIManage.GetInstance().f_GetUIHandler(UINameConst.CardProperty);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.AwakenEquipPage, UIMessageDef.UI_CLOSE);
            GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Money, cardProperty);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT tmpCallBack = new SocketCallbackDT();
        tmpCallBack.m_ccCallbackFail = SytheFail;
        tmpCallBack.m_ccCallbackSuc = SytheSuc;
        Data_Pool.m_AwakenEquipPool.f_Sythe(tmpEquip.iId, 1, tmpCallBack);
    }
    private void f_GetWay(GameObject go, object obj1, object obj2)
    {
        ccUIBase cardProperty = ccUIManage.GetInstance().f_GetUIHandler(UINameConst.CardProperty);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AwakenEquipPage, UIMessageDef.UI_CLOSE);
        GetWayPageParam param = new GetWayPageParam(EM_ResourceType.AwakenEquip, mAwakenEquipDT.m_AwakenEquipDT.iId, cardProperty);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, param);
    }

    private void f_CloseThis(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AwakenEquipPage, UIMessageDef.UI_CLOSE);
    }

    private void EquipSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        f_CloseThis(null, null, null);
        if (Param.EquipSuc != null) Param.EquipSuc(mAwakenEquipDT);
    }

    private void EquipFill(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        f_CloseThis(null, null, null);
        if (Param.EquipFail != null) Param.EquipFail(mAwakenEquipDT);
    }

    private void SytheSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(156));
        f_CloseThis(null, null, null);
        if (Param.SytheSuc != null) Param.SytheSuc(mAwakenEquipDT);
    }

    private void SytheFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);

        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(155));
        f_CloseThis(null, null, null);
        if (Param.SytheFail != null) Param.SytheFail(mAwakenEquipDT);
    }
    void UpdateItem(Transform tran, int Icon, int Num, string Name, int NeedNum = 0)
    {
        UI2DSprite icon = tran.Find("Icon").GetComponent<UI2DSprite>();
        UILabel num = tran.Find("Num").GetComponent<UILabel>();
        UILabel name = tran.Find("Name").GetComponent<UILabel>();
        if (NeedNum != 0)
            num.text = NeedNum > Num ? "[ec2626]" + Num + "/" + NeedNum : "[24ff00]" + Num + "/" + NeedNum;
        else
            num.text = Num.ToString();
        name.text = Name;
        icon.sprite2D = UITool.f_GetIconSprite(Icon);
    }

    void _SetAwakenShow(Transform tTran, AwakenEquipDT tAwakenEquip, int num)
    {
        AwakenEquipDT tAwaken = tAwakenEquip;
        AwakenEquipPoolDT AwakenPoolDT = UITool.f_GetAwakenEquipPoolDT(tAwaken.iId);
        UI2DSprite tmp2DSprite = tTran.GetComponent<UI2DSprite>();
        UILabel tmpLbale = tTran.Find("Label").GetComponent<UILabel>();
        UISprite tmpSprite = tTran.Find("Case").GetComponent<UISprite>();
        UILabel name = tTran.Find("Name").GetComponent<UILabel>();

        name.text = UITool.f_GetImporentForName(tAwaken.iImportant, tAwaken.szName);
        tmp2DSprite.sprite2D = UITool.f_GetIconSprite(tAwaken.iIcon);
        tmpSprite.spriteName = UITool.f_GetImporentCase(tAwaken.iImportant);
        if (AwakenPoolDT.iId != 0)
        {

            tmpLbale.text = AwakenPoolDT.m_num >= num ? "[24ff00]" + AwakenPoolDT.m_num + "/" + num : "[ec2626]" + AwakenPoolDT.m_num + "/" + num;
           

            //if (AwakenPoolDT.m_num >= num)
            //{
            //    f_RegClickEvent(tTran.gameObject, UI_OpenNeedSytheAwakenEquipTrip, tAwaken);
            //}
            //else
            //{
            //    // f_RegClickEvent(tTran.gameObject, UI_OpenNeedSytheAwakenEquip, tAwaken);
            //}
        }
        else
        {
            tmpLbale.text = "[ec2626]0/" + num;
            // f_RegClickEvent(tTran.gameObject, UI_OpenNeedSytheAwakenEquip, tAwaken);
        }
        f_RegClickEvent(tTran.gameObject, UI_OpenNeedSytheAwakenEquipTrip, tAwaken);
    }
    void UI_OpenNeedSytheAwakenEquipTrip(GameObject go, object obj1, object obj2)
    {
        AwakenEquipDT tAwaken = (AwakenEquipDT)obj1;
        ResourceCommonDT tResourceCommonDT = new ResourceCommonDT();
        tResourceCommonDT.f_UpdateInfo((byte)EM_ResourceType.AwakenEquip, tAwaken.iId, UITool.f_GetAwakenEquipPoolDT(tAwaken.iId) == null ? 0 : UITool.f_GetAwakenEquipPoolDT(tAwaken.iId).m_num);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, tResourceCommonDT);
    }
}


public class AwakenEquipPageParam
{
    public enum em_State
    {
        Sythe,
        Induce,
    }
    public AwakenEquipPageParam(int id, byte index, bool isEquip, CardPoolDT card, em_State state)
    {
        m_ID = id;
        m_State = state;
        m_Index = index;
        m_isEquip = isEquip;
        m_Card = card;
    }

    public AwakenEquipPageParam(int id) {
        m_ID = id;
    }

    public int m_ID { private set; get; }

    public em_State m_State { private set; get; }

    public byte m_Index { private set; get; }

    public bool m_isEquip;

    public CardPoolDT m_Card { private set; get; }


    public ccCallback EquipSuc;
    public ccCallback EquipFail;


    public ccCallback SytheSuc;
    public ccCallback SytheFail;

    public bool m_isBagOpen=false;
}
