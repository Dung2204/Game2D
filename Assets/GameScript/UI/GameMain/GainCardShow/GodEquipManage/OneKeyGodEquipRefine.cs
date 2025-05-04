using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class OneKeyGodEquipRefine : UIFramwork
{
    private OneKeyGodRefineParam _OneKeyRefineParam;
    private GodEquipPoolDT _EquipPoolDt;
    private int _RefineLv;
    private int _MinRefineLv;
    private int _MaxRfineLv;
    private int[] Tempid = new int[4];    //精炼石的ID
    private int[] GoodsNum = new int[4];   //当前拥有的数量
    private int[] Consome = new int[4];
    private GameObject[] GoodsGo = new GameObject[4];

    private UILabel RefineNowLabel;
    private string strRefineNow;

    private UILabel RefineLastLabel;
    private string strRefineLast;


    private SocketCallbackDT OneKeyRefineCallback = new SocketCallbackDT();
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if(!(e is OneKeyGodRefineParam) && e == null)
        {
            MessageBox.ASSERT("Tinh luyện nhanh , thông số đầu vào không thể là Null hoặc EquipPoolDt");
        }
        _OneKeyRefineParam = (OneKeyGodRefineParam)e;
        _EquipPoolDt = _OneKeyRefineParam.m_EquipPoolDT;
        InitGUI();
        int MaxLv = CheckMaxRefine(_EquipPoolDt.m_lvRefine, _EquipPoolDt.m_iexpRefine);
        if(_EquipPoolDt.m_lvRefine == MaxLv)
        {
            UITool.Ui_Trip("Không đủ vật liệu");
            GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Good, 136, _OneKeyRefineParam.m_CallUIBase);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
            Btn_CloseUI(null, null, null);
            return;
        }


        _MaxRfineLv = MaxLv;
        _MinRefineLv = _RefineLv = _EquipPoolDt.m_lvRefine;
        RefineLastLabel.text = string.Format(strRefineLast, _MaxRfineLv);
        UpdateNum();
        UpdateMainUi();
    }
    protected override void f_Create()
    {
        _InitReference();
        base.f_Create();
    }

    private void _InitReference()
    {
        AddGOReference("Panel/Center/Consume/primary");
        AddGOReference("Panel/Center/Consume/middleRank");
        AddGOReference("Panel/Center/Consume/Higt");
        AddGOReference("Panel/Center/Consume/highest");
        AddGOReference("Panel/Center/RefineLv/RefineNow");
        AddGOReference("Panel/Center/RefineLv/RefineLast");
        AddGOReference("Panel/Center/ResourceCommonItem");
        AddGOReference("Panel/Center/Body/NumBg/RefineNum");

        AddGOReference("Panel/Center/AlpheBg");
        AddGOReference("Panel/Center/CloseBtn");
        AddGOReference("Panel/Center/Body/Minus10");
        AddGOReference("Panel/Center/Body/Minus1");
        AddGOReference("Panel/Center/Body/Add1");
        AddGOReference("Panel/Center/Body/Add10");
        AddGOReference("Panel/Center/SucBtn");
    }
    protected override void InitGUI()
    {
        base.InitGUI();
        strRefineNow = "[f1b049]Tinh luyện:[-][fffaec]{0}";
        strRefineLast = "{0}";

        Tempid = UITool.f_GetGoodsForEffect(EM_GoodsEffect.GodEquipRefineExp);
        for(int i = 0; i < GoodsNum.Length; i++)
        {
            GoodsNum[i] = Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(Tempid[i]);
        }
        Consome = new int[4];

        GoodsGo[0] = f_GetObject("primary");
        GoodsGo[1] = f_GetObject("middleRank");
        GoodsGo[2] = f_GetObject("Higt");
        GoodsGo[3] = f_GetObject("highest");

        OneKeyRefineCallback.m_ccCallbackFail = OneKeyRefineFail;
        OneKeyRefineCallback.m_ccCallbackSuc = OneKeyRefineSuc;

        RefineNowLabel = f_GetObject("RefineNow").GetComponent<UILabel>();
        RefineLastLabel = f_GetObject("RefineLast").GetComponent<UILabel>();
    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();

        f_RegClickEvent("AlpheBg", Btn_CloseUI);
        f_RegClickEvent("CloseBtn", Btn_CloseUI);
        f_RegClickEvent("Minus10", Btn_ChangeNum, -10);
        f_RegClickEvent("Minus1", Btn_ChangeNum, -1);
        f_RegClickEvent("Add1", Btn_ChangeNum, 1);
        f_RegClickEvent("Add10", Btn_ChangeNum, 10);
        f_RegClickEvent("SucBtn", Btn_Suc);
    }


    #region  按钮事件

    private void Btn_CloseUI(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.OneKeyGodEquipRefine, UIMessageDef.UI_CLOSE);
    }

    private void Btn_ChangeNum(GameObject go, object obj1, object obj2)
    {
        int num = (int)obj1;
        if(_RefineLv + num > _MaxRfineLv)
        {
            _RefineLv = _MaxRfineLv;
        }
        else if(_RefineLv + num < _MinRefineLv)
        {
            _RefineLv = _MinRefineLv;
        }
        else
        {
            _RefineLv += num;
        }

        Consome = CountRefineConsume(_RefineLv);

        for(int i = 0; i < Consome.Length; i++)
        {
            UpdateGoodsUI(GoodsGo[i], Tempid[i], GoodsNum[i], GoodsNum[i] - Consome[i]);
        }

        UpdateNum();
    }

    private void Btn_Suc(GameObject go, object obj1, object obj2)
    {
        bool isZero = true;
        for(int i = 0; i < Consome.Length; i++)
        {
            if(Consome[i] != 0)
                isZero = false;
        }
        if(isZero)
        {
            UITool.Ui_Trip("Vui lòng chọn mức độ");
            return;
        }

        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_GodEquipPool.f_GodEquipOneKeyRefine(_EquipPoolDt.iId, 4, Tempid, Consome, OneKeyRefineCallback);
    }

    #endregion


    private void UpdateMainUi()
    {
        ResourceCommonItem tItem = f_GetObject("ResourceCommonItem").GetComponent<ResourceCommonItem>();
        tItem.f_UpdateByInfo((int)EM_ResourceType.GodEquip, _EquipPoolDt.m_iTempleteId, 1);

        for(int i = 0; i < GoodsGo.Length; i++)
        {
            UpdateGoodsUI(GoodsGo[i], Tempid[i], GoodsNum[i], GoodsNum[i]);
        }

    }

    private void UpdateGoodsUI(GameObject go, int Id, int MaxGoodsNum, int residueNum)
    {
        ResourceCommonDT tDT = new ResourceCommonDT();
        tDT.f_UpdateInfo((int)EM_ResourceType.Good, Id, MaxGoodsNum);

        go.transform.Find("Icon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(tDT.mIcon);

        go.transform.Find("Label").GetComponent<UILabel>().text = string.Format("{0}/{1}", residueNum, tDT.mResourceNum);
    }
    private void UpdateNum()
    {
        RefineNowLabel.text = string.Format(strRefineNow, _RefineLv);
        f_GetObject("RefineNum").GetComponent<UILabel>().text = _RefineLv.ToString();
    }

    private int CheckMaxRefine(int Lv, int Exp)
    {
        int AddRefineExp = 0;
        int[] tmpNum = new int[4];
        BaseGoodsDT tPoolDT;
        for(int i = 0; i < tmpNum.Length; i++)
        {
            tPoolDT = glo_Main.GetInstance().m_SC_Pool.m_BaseGoodsSC.f_GetSC(Tempid[i]) as BaseGoodsDT;

            AddRefineExp += Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(Tempid[i]) * tPoolDT.iEffectData;
        }
        int tmpRefineLv = Lv;
        int tmpRefineExp = Exp;
        int tmpNeedRefineExp = 0;
        for(; tmpRefineLv < 50; tmpRefineLv++)
        {
            tmpNeedRefineExp = GetRefineNeedExp(_EquipPoolDt.m_EquipDT.iColour, tmpRefineLv) - tmpRefineExp;
            tmpRefineExp = 0;
            if(tmpNeedRefineExp > AddRefineExp)
            {
                break;
            }
            AddRefineExp -= tmpNeedRefineExp;
        }

        return tmpRefineLv;

    }

    private int GetRefineNeedExp(int Imporent, int RefineLv)
    {
        int tmp = 0;
        tmp = UITool.GetEquipRefineId(Imporent) + RefineLv;
        if(glo_Main.GetInstance().m_SC_Pool.m_GodEquipConsumeSC.f_GetSC(tmp + 1) != null)
            return ((GodEquipConsumeDT)glo_Main.GetInstance().m_SC_Pool.m_GodEquipConsumeSC.f_GetSC(tmp + 1)).iRefineExp;
        else
            return 0;
    }

    private int[] CountRefineConsume(int lv)
    {
        int[] tmpConsome = new int[4];
        if(lv == _EquipPoolDt.m_lvRefine)
        {
            return tmpConsome;
        }
        int tmpRefineLv = _EquipPoolDt.m_lvRefine;
        int tmpRefineExp = _EquipPoolDt.m_iexpRefine;
        int tmpNeedRefineExp = 0;
        for(; tmpRefineLv < lv; tmpRefineLv++)
        {
            tmpNeedRefineExp += GetRefineNeedExp(_EquipPoolDt.m_EquipDT.iColour, tmpRefineLv) - tmpRefineExp;
            tmpRefineExp = 0;
        }

        BaseGoodsDT tPoolDT;
        for(int i = 0; i < tmpConsome.Length; i++)
        {
            int index = 4 - i - 1;
            tPoolDT = glo_Main.GetInstance().m_SC_Pool.m_BaseGoodsSC.f_GetSC(Tempid[index]) as BaseGoodsDT;


            if(GoodsNum[index] == 0)
            {
                continue;
            }
            if(tmpNeedRefineExp <= 0)
                break;

            int NeedNum = tPoolDT.iEffectData > tmpNeedRefineExp ? 1 : tmpNeedRefineExp / tPoolDT.iEffectData;

            if(NeedNum == 0)
            {
                continue;
            }

            if(NeedNum >= GoodsNum[index])
            {
                tmpNeedRefineExp -= tPoolDT.iEffectData * GoodsNum[index];
                tmpConsome[index] = GoodsNum[index];
            }
            else
            {
                tmpNeedRefineExp -= tPoolDT.iEffectData * NeedNum;
                tmpConsome[index] = NeedNum;
            }
        }

        return tmpConsome;
    }

    private void OneKeyRefineSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip("Tinh luyện cấp " + _RefineLv + "");
        Btn_CloseUI(null, null, null);
        _OneKeyRefineParam.m_Callback(null);

        //重新计算战斗力
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerFightPowerChange);
    }
    private void OneKeyRefineFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip("Tinh luyện nhanh không thành công " + obj.ToString());
    }
}

public class OneKeyGodRefineParam
{
    public GodEquipPoolDT m_EquipPoolDT;
    public ccCallback m_Callback;
    public ccUIBase m_CallUIBase; //调用的uibase
}
