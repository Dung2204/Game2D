using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class TacticalPage : UIFramwork
{

    enum SkillPos
    {
        pos1,
        pos2,
        pos3,
        pos4,
        pos5,
        pos6,
        Invalid,
    }


    private Transform CardParent;

    private GameObject SkillItem;
    private GameObject SkillItemParent;

    private SkillPos _SkillPos;
    private Dictionary<SkillPos, Transform> _PosDictionary;
    private Dictionary<Transform, Transform> CriclePoint;


    private bool _IsTweenPlay;

    private TweenRotation _CricleOut;

    private Transform CricleOut;

    private int Time_CriclePoint;

    private List<GameObject> _SkillItemList;

    private TacticalDT _TacticalNow;
    private byte[] _CardSkillLoca;

    private GameObject Effect_CricleOut;
    private GameObject Effect_StudyGoodsEffect;
    private GameObject Effect_StudySuc;
    private GameObject Effect_Skill;

    private float EffectX = -275;
    private float EffectY = 25;

    private bool IsStudyBtn = false;

    private string _BgAdress = "UI/TextureRemove/Tarining/Texture_TacticalBg";
    private string _CricleBg = "UI/TextureRemove/Tarining/Texture_TacticalOut";
    private string _CricalInBg = "UI/TextureRemove/Tarining/Texture_TacticalIn";
    private string _CardSelectBg = "UI/TextureRemove/Tarining/Border_CardSelectSkill";

    #region 红点
    protected override void InitRaddot()
    {
        base.InitRaddot();
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.TacticalStdySkill, f_GetObject("Btn_Study"), Btn_TacticalRed);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.TacticalStdySkill);
    }

    private void Btn_TacticalRed(object obj)
    {
        int num = (int)obj;
        GameObject BtnActivity = f_GetObject("Btn_Study");
        UITool.f_UpdateReddot(BtnActivity, num, new Vector3(90, 20, 0), 102);
    }
    #endregion
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        InitGUI();
        InitBg();
        UpdateMain();
    }

    private void InitBg()
    {
        f_GetObject("Bg").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(_BgAdress);
        f_GetObject("In").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(_CricalInBg);
        f_GetObject("CricleOut").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(_CricleBg);
        f_GetObject("SkillBg").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(_CardSelectBg);
    }
    protected override void InitGUI()
    {
        base.InitGUI();
        SkillItem = f_GetObject("SkillItem");
        SkillItemParent = f_GetObject("SkillItemParent");
        CardParent = f_GetObject("CardParent").transform;
        _PosDictionary = new Dictionary<SkillPos, Transform>();
        CriclePoint = new Dictionary<Transform, Transform>();
        _SkillItemList = new List<GameObject>();
        _IsTweenPlay = false;
        IsStudyBtn = false;
        CricleOut = f_GetObject("CricleOut").transform;
        _CricleOut = f_GetObject("CricleOut").GetComponent<TweenRotation>();

        _CardSkillLoca = new byte[6];

        for(int i = 0; i < _CardSkillLoca.Length; i++)
        {
            _CardSkillLoca[i] = Data_Pool.m_TariningAndTacticalPool.m_TacticalInfo.formatInfo[i];
        }
        if(Effect_CricleOut == null)
            Effect_CricleOut = UITool.f_CreateEffect(UIEffectName.UIEffectAddress4, UIEffectName.fazhen_guangquan_01, f_GetObject("CricleEffectParent").transform);
    }




    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("Btn_Close", Btn_ClosePage);
        f_RegClickEvent("Btn_Study", Btn_Study);

        f_RegPressEvent("SkillItem", Btn_Drag);
    }

    private void UpdateMain()
    {
        UpdateCardSkill();
        UpdateGoods();
        UpdateCricleOut();
    }
    /// <summary>
    /// 刷新卡牌的技能 
    /// </summary>
    private void UpdateCardSkill()
    {
        for(int i = 0; i < CardParent.childCount; i++)
        {
            GameObject Card = CardParent.GetChild(i).Find("Card").gameObject;
            GameObject NotCard = CardParent.GetChild(i).Find("NoCard").gameObject;
            Transform SkillBg = CardParent.GetChild(i).Find("SkillBg");


            ResourceCommonItem tItem = Card.GetComponent<ResourceCommonItem>();

            //CardPoolDT tCardPoolDT = Data_Pool.m_TeamPool.f_GetFormationPosCardPoolDT(Data_Pool.m_ClosethArrayData.m_dicClothArrayToPos[(EM_CloseArrayPos)i]);
            CardPoolDT tCardPoolDT = Data_Pool.m_TeamPool.f_GetFormationPosCardPoolDT((EM_FormationPos)i);



            Card.SetActive(tCardPoolDT != null);
            NotCard.SetActive(tCardPoolDT == null);
            if(tCardPoolDT != null)
            {
                tItem.f_UpdateByInfo((int)EM_ResourceType.Card, tCardPoolDT.m_CardDT.iId, 1);
                f_RegClickEvent(Card, Btn_OpenTeam, i);
                _PosDictionary.Add((SkillPos)i, SkillBg);

                if(_CardSkillLoca != null)
                {
                    TacticalDT tTacticalDT = glo_Main.GetInstance().m_SC_Pool.m_TacticalSC.f_GetSC(_CardSkillLoca[i]) as TacticalDT;

                    SetCardSkill(tTacticalDT, SkillBg);

                }
            }
            else
            {
                SetCardSkill(null, SkillBg);
            }
        }
    }

    private void SetCardSkill(TacticalDT tTacticalDT, Transform pos)
    {
        pos.Find("SkillName").gameObject.SetActive(tTacticalDT != null);
        pos.Find("SkillIcon").gameObject.SetActive(tTacticalDT != null);

        if(tTacticalDT != null)
        {
            pos.Find("SkillIcon").GetComponent<UISprite>().spriteName = "Tactical_" + tTacticalDT.iSkillType;
            pos.Find("SkillName").GetComponent<UILabel>().text = tTacticalDT.szSkillName;
        }
    }
    private void UpdateGoods()
    {
        int GoodsId = Data_Pool.m_TariningAndTacticalPool.TacticalGoodsId;
        f_GetObject("TacticalGoods").GetComponent<ResourceCommonItem>().f_UpdateByInfo((int)EM_ResourceType.Good, GoodsId, Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(GoodsId));

        _TacticalNow = glo_Main.GetInstance().m_SC_Pool.m_TacticalSC.f_GetSC(Data_Pool.m_TariningAndTacticalPool.m_TacticalInfo.maxFormatId + 1) as TacticalDT;

        if(_TacticalNow != null)
        {
            string NeedConsume = "";
            if(_TacticalNow.iNeedConsume > Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(GoodsId))
            {
                NeedConsume = string.Format("[FF0000]{0}/{1}", Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(GoodsId), _TacticalNow.iNeedConsume);
            }
            else
            {
                NeedConsume = string.Format("{0}/{1}", Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(GoodsId), _TacticalNow.iNeedConsume);
            }

            f_GetObject("GoodsNum").GetComponent<UILabel>().text = NeedConsume;
        }
        else
        {
            f_GetObject("GoodsNum").GetComponent<UILabel>().text = "0";
        }


    }
    private void UpdateCricleOut()
    {
        int SkillNum = Data_Pool.m_TariningAndTacticalPool.m_TacticalInfo.maxFormatId;
        int SkillTime = SkillNum > 10 ? 10 : SkillNum;

        CricleOut.localEulerAngles = new Vector3(0, 0, SkillNum == 0 ? SkillNum * -36 : (SkillNum - 1) * -36);

        for(int i = 1; i <= SkillTime; i++)
        {
            Transform tChilder = NGUITools.AddChild(SkillItemParent, SkillItem).transform;
            tChilder.position = CricleOut.GetChild(i - 1).position;
            tChilder.name = i + "";

            CriclePoint.Add(tChilder, CricleOut.GetChild(i - 1));
            _SkillItemList.Add(tChilder.gameObject);
            tChilder.gameObject.SetActive(true);
            TacticalDT tTacticalDT = glo_Main.GetInstance().m_SC_Pool.m_TacticalSC.f_GetSC(i) as TacticalDT;
            if(tTacticalDT != null)
            {
                int Skillindex = SkillNum % 10;
                int SkillLv = i <= Skillindex ? SkillNum / 10 + 1 : SkillNum / 10;

                UpdateItem(tChilder, tTacticalDT.szSkillName, i, SkillLv);
                f_RegPressEvent(tChilder.gameObject, Btn_Drag, tTacticalDT.iId);
            }
        }
    }

    private void UpdateItem(Transform Childer, string SkillName, int SkillIdx, int Lv)
    {
        Childer.Find("SkillIcon").GetComponent<UISprite>().spriteName = "Tactical_" + SkillIdx;
        Childer.Find("SkillName").GetComponent<UILabel>().text = string.Format("{0}\nLv.{1}", SkillName, Lv);
    }

    private void UpdateSkill()
    {
        int SkillNum = Data_Pool.m_TariningAndTacticalPool.m_TacticalInfo.maxFormatId;
        int SkillTime = SkillNum > 10 ? 10 : SkillNum;


        for(int i = 1; i <= SkillTime; i++)
        {


            TacticalDT tTacticalDT = glo_Main.GetInstance().m_SC_Pool.m_TacticalSC.f_GetSC(i) as TacticalDT;


            if(SkillItemParent.transform.Find(i.ToString()) != null)
            {
                int Skillindex = SkillNum % 10;
                int SkillLv = i <= Skillindex ? SkillNum / 10 + 1 : SkillNum / 10;
                UpdateItem(SkillItemParent.transform.Find(i.ToString()), tTacticalDT.szSkillName, i, SkillLv);
                continue;
            }
            Transform tChilder = NGUITools.AddChild(SkillItemParent, SkillItem).transform;
            tChilder.position = CricleOut.GetChild(i - 1).position;
            tChilder.name = i + "";

            CriclePoint.Add(tChilder, CricleOut.GetChild(i - 1));
            _SkillItemList.Add(tChilder.gameObject);
            tChilder.gameObject.SetActive(true);

            if(tTacticalDT != null)
            {
                int Skillindex = SkillNum % 10;
                int SkillLv = i <= Skillindex ? SkillNum / 10 + 1 : SkillNum / 10;

                UpdateItem(tChilder, tTacticalDT.szSkillName, i, SkillLv);
                f_RegPressEvent(tChilder.gameObject, Btn_Drag, tTacticalDT.iId);
            }

        }
        CreateSkillEffect(SkillNum % 10 == 0 ? 10 : SkillNum % 10);

    }

    private void CreateSkillEffect(int i)
    {
        Transform parent = SkillItemParent.transform.Find(i.ToString());

        Effect_Skill = UITool.f_CreateEffect(UIEffectName.UIEffectAddress4, UIEffectName.fazhen_yuankuang_01, parent);

        Destroy(Effect_Skill, 2f);
    }
    #region 按钮事件


    private void Btn_Drag(GameObject go, object obj1, object obj2)
    {
        if(_IsTweenPlay)
            return;
        int tmpSkillID = (int)obj2;
        if((bool)obj1)
        {
            if(!go.transform.Find("SkillDesc").gameObject.activeSelf)
            {
                int MaxId = Data_Pool.m_TariningAndTacticalPool.m_TacticalInfo.maxFormatId;
                int SkillID = MaxId % 10;

                if(MaxId > 10)
                {
                    if(SkillID >= tmpSkillID)
                        tmpSkillID = (MaxId / 10) * 10 + tmpSkillID;
                    else
                        tmpSkillID = ((MaxId / 10) - 1) * 10 + tmpSkillID;
                }

                TacticalDT tTacticalDT = glo_Main.GetInstance().m_SC_Pool.m_TacticalSC.f_GetSC(tmpSkillID) as TacticalDT;
                go.transform.Find("SkillDesc").gameObject.SetActive(true);
                go.transform.Find("SkillDesc/SkillLabel").GetComponent<UILabel>().text = string.Format("{0}+{1}", UITool.f_GetProName((EM_RoleProperty)tTacticalDT.iProId), UITool.f_GetPercentagePro(tTacticalDT.iProId, tTacticalDT.iProNum));
                ccTimeEvent.GetInstance().f_RegEvent(1f, false, go.transform.Find("SkillDesc").gameObject, GameObjtectTimeClose);
            }
        }
        else
        {
            TacticalDT tTacticalDT = glo_Main.GetInstance().m_SC_Pool.m_TacticalSC.f_GetSC((int)obj2) as TacticalDT;
            ccTimeEvent.GetInstance().f_UnRegEvent(Time_CriclePoint);
            Time_CriclePoint = ccTimeEvent.GetInstance().f_RegEvent(0.2f, true, go.transform, _CheckPoint);
            SkillPos tPos = CheckNearPos(go);
            if(tPos != SkillPos.Invalid)
            {
                //TacticalDT tTacticalDT = glo_Main.GetInstance().m_SC_Pool.m_TacticalSC.f_GetSC((int)obj2) as TacticalDT;
                //TacticalDT tTacticalDT = new TacticalDT();
                //tTacticalDT.szSkillName = "测试";
                //tTacticalDT.iId = 1;
                SetCardSkill(tTacticalDT, _PosDictionary[tPos]);
                for(int i = 0; i < _CardSkillLoca.Length; i++)
                {
                    if((int)tPos == i || !_PosDictionary.ContainsKey((SkillPos)i))
                    {
                        continue;
                    }

                    if(_CardSkillLoca[i] == tTacticalDT.iId)
                    {
                        SetCardSkill(null, _PosDictionary[(SkillPos)i]);
                        _CardSkillLoca[i] = 0;
                        break;
                    }
                }
                _CardSkillLoca[(int)tPos] = (byte)tTacticalDT.iId;
                Data_Pool.m_TeamPool.f_GetFormationPosCardPoolDT(Data_Pool.m_ClosethArrayData.m_dicClothArrayToPos[(EM_CloseArrayPos)tPos]).m_TacticalId = tTacticalDT.iId;
                int MaxId = Data_Pool.m_TariningAndTacticalPool.m_TacticalInfo.maxFormatId;
                int SkillID = MaxId % 10;

                if(MaxId > 10)
                {
                    if(SkillID >= tmpSkillID)
                        tmpSkillID = (MaxId / 10) * 10 + tmpSkillID;
                    else
                        tmpSkillID = ((MaxId / 10) - 1) * 10 + tmpSkillID;
                }
                TacticalDT tTacticalDT1 = glo_Main.GetInstance().m_SC_Pool.m_TacticalSC.f_GetSC(tmpSkillID) as TacticalDT;
                UITool.Ui_Trip(string.Format("{0}+{1}", UITool.f_GetProName((EM_RoleProperty)tTacticalDT1.iProId), UITool.f_GetPercentagePro(tTacticalDT1.iProId, tTacticalDT1.iProNum)));
            }
            //MessageBox.DEBUG(CheckNearPos(go).ToString());
        }
    }


    private void Btn_ClosePage(GameObject go, object obj1, object obj2)
    {
        if(_IsTweenPlay)
            return;
        for(int i = 0; i < _SkillItemList.Count; i++)
        {
            Destroy(_SkillItemList[i]);
        }
        ccUIHoldPool.GetInstance().f_UnHold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TacticalPage, UIMessageDef.UI_CLOSE);
        _SendCardPoint();



    }


    private void Btn_Study(GameObject go, object obj1, object obj2)
    {
        if(IsStudyBtn)
        {
            return;
        }
        if(_IsTweenPlay)
            return;

        if(_TacticalNow == null)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1634));
            return;
        }

        if(_TacticalNow.iNeedConsume > Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(Data_Pool.m_TariningAndTacticalPool.TacticalGoodsId))
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1635)));
            return;
        }
        IsStudyBtn = true;
        SocketCallbackDT tSocketCallbackDT = new SocketCallbackDT();
        tSocketCallbackDT.m_ccCallbackSuc = StudySuc;
        tSocketCallbackDT.m_ccCallbackFail = StudtFail;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_TariningAndTacticalPool.f_TacticalStudy(tSocketCallbackDT);
        //StudySuc(null);
    }

    private void Btn_OpenTeam(GameObject go, object obj, object ob2)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.LineUpPage, UIMessageDef.UI_OPEN, Data_Pool.m_ClosethArrayData.m_dicClothArrayToPos[(EM_CloseArrayPos)obj]);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LineUpPage, UIMessageDef.UI_OPEN, (EM_FormationPos)obj);
    }
    #endregion


    private SkillPos CheckNearPos(GameObject go)
    {
        float MinSpace = 0.15f;
        Vector3 GoPosition = go.transform.position;

        Vector3 ParentPosition;

        List<SkillPos> tPos = new List<SkillPos>(_PosDictionary.Keys);
        for(int i = 0; i < _PosDictionary.Count; i++)
        {
            ParentPosition = _PosDictionary[tPos[i]].position;
            float Space = CountPosSpace(GoPosition, ParentPosition);
            //MessageBox.DEBUG("距离" + Space);
            if(Space < MinSpace)
            {
                return tPos[i];
            }
        }
        return SkillPos.Invalid;
    }

    private float CountPosSpace(Vector3 pos1, Vector3 pos2)
    {
        return Mathf.Sqrt(Mathf.Pow((pos2.x - pos1.x), 2) + Mathf.Pow(pos2.y - pos1.y, 2));
    }

    private void StudySuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);

        Effect_StudySuc = UITool.f_CreateEffect(UIEffectName.UIEffectAddress4, UIEffectName.zhenfa_xuexi_01, f_GetObject("Effect").transform, EffectX, EffectY);
        if(Effect_StudySuc != null)
            Destroy(Effect_StudySuc, 3f);
        ccTimeEvent.GetInstance().f_RegEvent(0.96f, false, null, CreateBoxEffect);
        Data_Pool.m_ReddotMessagePool.f_MsgSubtract(EM_ReddotMsgType.TacticalStdySkill);
    }

    private void CreateBoxEffect(object obj)
    {
        if(Effect_StudyGoodsEffect == null)
            Effect_StudyGoodsEffect = UITool.f_CreateEffect(UIEffectName.UIEffectAddress4, UIEffectName.fazhen_fangkuang_01, f_GetObject("Effect").transform, EffectX, EffectY);
        if(Effect_StudyGoodsEffect != null)
            Destroy(Effect_StudyGoodsEffect, 1f);

        UpdateGoods();
        if(Data_Pool.m_TariningAndTacticalPool.m_TacticalInfo.maxFormatId == 1)
        {
            TweenPlayEnd();
            return;
        }


        _CricleOut.enabled = true;
        _CricleOut.onFinished.Clear();
        _CricleOut.onFinished.Add(new EventDelegate(TweenPlayEnd));
        _IsTweenPlay = true;
        _CricleOut.from = _CricleOut.transform.localEulerAngles;
        _CricleOut.to = _CricleOut.transform.localEulerAngles + new Vector3(0, 0, -36);

        _CricleOut.ResetToBeginning();
        _CricleOut.Play(true);
    }

    private void TweenPlayEnd()
    {
        _IsTweenPlay = false;
        IsStudyBtn = false;
        UpdateSkill();
    }

    private void StudtFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1636) + obj.ToString());
    }
    protected override void f_Update()
    {
        base.f_Update();
        if(_IsTweenPlay)
        {
            for(int i = 0; i < SkillItemParent.transform.childCount; i++)
            {
                SkillItemParent.transform.GetChild(i).position = CricleOut.GetChild(i).position;
            }
        }

    }
    private void _CheckPoint(object obj)
    {
        Transform tTran = (Transform)obj;
        if(tTran.position != CriclePoint[tTran].position)
        {
            tTran.position = CriclePoint[tTran].position;
            ccTimeEvent.GetInstance().f_UnRegEvent(Time_CriclePoint);
        }
    }

    private void _SendCardPoint()
    {
        //for(int i = 0; i < _CardSkillLoca.Length; i++)
        //{
        //    if(_CardSkillLoca[i] != Data_Pool.m_TariningAndTacticalPool.m_TacticalInfo.formatInfo[i])
        //    {
        //        //if(_CardSkillLoca[i] == 0)
        //        //    continue;
        //        SocketCallbackDT tSocketCallbackDT = new SocketCallbackDT();
        //        tSocketCallbackDT.m_ccCallbackSuc = AppCardSkillSuc;
        //        tSocketCallbackDT.m_ccCallbackFail = AppCardSkillFail;
        //        Data_Pool.m_TariningAndTacticalPool.f_TacticalAppCard((byte)i, (byte)_CardSkillLoca[i], tSocketCallbackDT);
        //    }
        //}
        bool IsSend = false;

        for(int i = 0; i < _CardSkillLoca.Length; i++)
        {
            if(_CardSkillLoca[i] != Data_Pool.m_TariningAndTacticalPool.m_TacticalInfo.formatInfo[i])
            {
                IsSend = true;
                break;
            }
        }

        if(!IsSend)
            return;

        SocketCallbackDT tSocketCallbackDT = new SocketCallbackDT();
        tSocketCallbackDT.m_ccCallbackSuc = AppCardSkillSuc;
        tSocketCallbackDT.m_ccCallbackFail = AppCardSkillFail;
        Data_Pool.m_TariningAndTacticalPool.f_TacticalAppCard(_CardSkillLoca, tSocketCallbackDT);
    }

    private void AppCardSkillSuc(object obj)
    {
        //MessageBox.DEBUG("技能附加成功");
    }

    private void AppCardSkillFail(object obj)
    {
        //MessageBox.DEBUG("技能附加失败");
    }


    private void GameObjtectTimeClose(object obj)
    {
        GameObject go = (GameObject)obj;

        go.SetActive(false);
    }

}
