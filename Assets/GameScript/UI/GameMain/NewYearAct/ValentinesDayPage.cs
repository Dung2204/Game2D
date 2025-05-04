using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class ValentinesDayPage : UIFramwork
{
    private enum em_RoleState
    {
        SelectRole,
        PreviewRole,
    }

    private SocketCallbackDT _InfoSocketCallbackDT = new SocketCallbackDT();
    private SocketCallbackDT _SendRoseCallbackDT = new SocketCallbackDT();
    private SocketCallbackDT _GetBoxCallbackDT = new SocketCallbackDT();

    private List<AwardPoolDT> _ShowAwardList;
    private ResourceCommonItemComponent _ShowAwardComponent;


    private int[] _RoleId = new int[4];
    private GameObject[] _RoleArr = new GameObject[4];
    private GameObject[] _RoleParentArr = new GameObject[4];
    private GameObject[] _GetBox = new GameObject[4];

    private int _SelectRole;
    private int _RoseId = 370;

    private GameParamDT _SendRoseAwardId;
    private GameParamDT _BoxNeedSroce;
    private GameParamDT _BoxAwardId;
    private GameParamDT _ValentinesDayTime;
    private int _MaxSorce;

    private em_RoleState _RolrState;
    private int TiME_UpdateTime;

    private UILabel _OverTime;
    private int _EndTime;
    private int _NowTime;
    public ResourceCommonItemComponent ShowAwardComponent
    {
        get
        {
            if (_ShowAwardComponent == null)
                _ShowAwardComponent = new ResourceCommonItemComponent(f_GetObject("GoodsParent").GetComponent<UIGrid>(), f_GetObject("ResourceCommonItem"));

            return _ShowAwardComponent;
        }

    }
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("CloseBtn", _CloseThis);
        f_RegClickEvent("Role1", _RoleBtn, 0);
        f_RegClickEvent("Role2", _RoleBtn, 1);
        f_RegClickEvent("Role3", _RoleBtn, 2);
        f_RegClickEvent("Role4", _RoleBtn, 3);

        f_RegClickEvent("GiftBox4", _BoxBtn, 0);
        f_RegClickEvent("GiftBox3", _BoxBtn, 1);
        f_RegClickEvent("GiftBox2", _BoxBtn, 2);
        f_RegClickEvent("GiftBox1", _BoxBtn, 3);


        f_RegClickEvent("PresentOne", _PresentBtn, 1);
        f_RegClickEvent("PresentTen", _PresentBtn, 10);
    }


    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        InitUI();
        _EndTime = ccMath.f_Data2Int(_ValentinesDayTime.iParam2);
        UITool.f_OpenOrCloseWaitTip(true, true);
        _InfoSocketCallbackDT.m_ccCallbackSuc = _InfoSuc;
        _InfoSocketCallbackDT.m_ccCallbackFail = _InfoFail;
        Data_Pool.m_ValentinesDayPool.f_ValentinesInfo(_InfoSocketCallbackDT);
        InitTexture();
    }
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        ccTimeEvent.GetInstance().f_UnRegEvent(TiME_UpdateTime);
        for (int i = 0; i < _RoleArr.Length; i++)
        {
            if (_RoleArr[i] != null)
                _RoleInit(_RoleArr[i]);
        }
    }

    private string Bg1Adrees = "UI/TextureRemove/Valentines/ValentinesDayBg";
    private string Bg2Adrees = "UI/TextureRemove/Valentines/ValentinesDayBg2";
    private void InitTexture()
    {
        UITexture Bg1 = f_GetObject("Texture").GetComponent<UITexture>();
        UITexture Bg2 = f_GetObject("MainBg").GetComponent<UITexture>();

        //if (Bg1.mainTexture == null)
        //{
        Texture2D tTexture2D1 = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(Bg1Adrees);
        Bg1.mainTexture = tTexture2D1;
        //}
        //if (Bg2.mainTexture == null)
        //{
        Texture2D tTexture2D2 = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(Bg2Adrees);
        Bg2.mainTexture = tTexture2D2;
        //}
    }
    private void InitUI()
    {
        List<Transform> tTranArr = f_GetObject("Role").GetComponent<UIGrid>().GetChildList();

        for (int i = 0; i < _RoleParentArr.Length; i++)
        {
            if (tTranArr[i] == null)
            {
                break;
            }
            _RoleParentArr[i] = tTranArr[i].gameObject;
        }
        _GetBox[0] = f_GetObject("GiftBox4");
        _GetBox[1] = f_GetObject("GiftBox3");
        _GetBox[2] = f_GetObject("GiftBox2");
        _GetBox[3] = f_GetObject("GiftBox1");
        _OverTime = f_GetObject("OverTime").GetComponent<UILabel>();
        //if (UITool.f_GetGoodNum(EM_ResourceType.Good, _RoseId) > 0)
        //    _RolrState = em_RoleState.PreviewRole;
        //else
        _RolrState = em_RoleState.SelectRole;
        _RoleId[0] = 1119;
        _RoleId[1] = 1217;
        _RoleId[2] = 1302;
        _RoleId[3] = 1409;

        _SelectRole = 0;


        _SendRoseAwardId = glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.ValentinesDayAwardId) as GameParamDT;
        _BoxAwardId = glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.ValentinesDayBoxAwardId) as GameParamDT;
        _BoxNeedSroce = glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.ValentinesDayBoxNeedScore) as GameParamDT;
        _ValentinesDayTime = glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.ValentinesDayOpenTime) as GameParamDT;
        _MaxSorce = _BoxNeedSroce.iParam4;


    }




    private void _UpdateUI()
    {
        _SetBox();
        _SetSlider((float)Data_Pool.m_ValentinesDayPool.m_ValentinesDayPoolDT.m_iGrade / (float)_MaxSorce);
        _SetRoleState();

f_GetObject("RoseNum").GetComponent<UILabel>().text = string.Format("Capital number:{0}", UITool.f_GetGoodsNum(_RoseId));
    }


    private void _SetRoleState()
    {
        f_GetObject("CardRole").SetActive(_RolrState == em_RoleState.PreviewRole);
        f_GetObject("Role").SetActive(_RolrState == em_RoleState.SelectRole);
    }
    private void _SetRole()
    {
        for (int i = 0; i < _RoleId.Length; i++)
        {
            _SetRoleIcon(_RoleId[i], ref _RoleArr[i], _RoleParentArr[i]);

_RoleParentArr[i].transform.Find("RoseNum").GetComponent<UILabel>().text = string.Format("Received [ffe56d]{0}[-] Flower",
                Data_Pool.m_ValentinesDayPool.m_ValentinesDayPoolDT.m_uNum[i]);
        }
    }
    private void _SetRoleIcon(int CardId, ref GameObject go, GameObject Parent)
    {
        UITool.f_CreateRoleByCardId(CardId, ref go, Parent.transform, 2);

        UILabel Name = Parent.transform.Find("RoleName").GetComponent<UILabel>();

        Name.text = ((CardDT)glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(CardId)).szName;

        if (CardId == 1217)
        {
            go.transform.localPosition = new Vector3(0, -194, 0);
            go.transform.localScale = Vector3.one * 135;
            _SetMask(go, 1.53f, 0.78f);
        }
        else if (CardId == 1119)
        {
            go.transform.localPosition = new Vector3(0, -150, 0);
            go.transform.localScale = Vector3.one * 135;
            _SetMask(go, 1.2f, 0.78f);
        }
        else
        {
            go.transform.localPosition = new Vector3(0, -180, 0);
            go.transform.localScale = Vector3.one * 135;
            _SetMask(go, 1.43f, 0.78f);
        }
    }

    private void _SetMask(GameObject go, float x, float y)
    {
        if (go != null)
        {
            Renderer tRenderer = go.GetComponent<Renderer>();
            Shader tShader = Shader.Find("Spine/SkeletonGray");
            if (tShader == null)
                MessageBox.ASSERT("Can't find shader. Name is Spine/SkeletonGray");
            tRenderer.sharedMaterial.shader = tShader;
            tRenderer.sharedMaterial.SetFloat("_GraySwitch", 1.0f);
            tRenderer.sharedMaterial.SetVector("_MaskPos", new Vector4(0, x, y, 0));
        }
    }
    private void _SetSlider(float value)
    {
        f_GetObject("HeartPlan").GetComponent<UISlider>().value = value;
        int Grade = Data_Pool.m_ValentinesDayPool.m_ValentinesDayPoolDT.m_iGrade;
f_GetObject("SroceNum").GetComponent<UILabel>().text = string.Format("Feeling:{0}", Grade >= 2000 ? 2000 : Grade);
    }
    private void _SetBox()
    {
        int[] needParam = new int[4] { _BoxNeedSroce.iParam1, _BoxNeedSroce.iParam2, _BoxNeedSroce.iParam3, _BoxNeedSroce.iParam4 };
        for (int i = 0; i < _GetBox.Length; i++)
        {
            _GetBox[i].transform.Find("Label").GetComponent<UILabel>().text = needParam[i].ToString();
        }

        string flag = Data_Pool.m_ValentinesDayPool.m_ValentinesDayPoolDT.m_iBoxFlag.ToString();
        int Times = 4 - flag.Length;
        for (int i = 0; i < Times; i++)
            flag += "0";

        for (int i = 0; i < flag.Length; i++)
        {
            string SpriteName = string.Empty;
            if (flag[i] == '0')
            {
                if (Data_Pool.m_ValentinesDayPool.m_ValentinesDayPoolDT.m_iGrade >= needParam[i])
                    SpriteName = "Icon_BoxOpen";
                else
                    SpriteName = "Icon_Box";
            }
            else if (flag[i] == '1')
                SpriteName = "Icon_BoxGet";

            _GetBox[i].GetComponent<UISprite>().spriteName = SpriteName;
            _GetBox[i].GetComponent<UISprite>().MakePixelPerfect();
        }

    }


    private void _UpdateTime(object obj)
    {
        _NowTime = GameSocket.GetInstance().f_GetServerTime();

        if (_EndTime - _NowTime <= 0)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ValentinesDayPage, UIMessageDef.UI_CLOSE);
            ccUIHoldPool.GetInstance().f_UnHold();
            return;
        }
_OverTime.text = string.Format("Event duration:{0}", ccMath.f_Time_Int2String(_EndTime - _NowTime));
    }

    private void _InfoSuc(object obj)
    {
        _UpdateUI();
        _SetRole();
        TiME_UpdateTime = ccTimeEvent.GetInstance().f_RegEvent(1f, true, null, _UpdateTime);
        UITool.f_OpenOrCloseWaitTip(false);
        if (_SendRoseAwardId == null)
        {
UITool.Ui_Trip("Không có phần thưởng");
            return;
        }
        _ShowAwardList = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(_SendRoseAwardId.iParam1);
        if (_ShowAwardList.Count > 9)
            ShowAwardComponent.f_Show(_ShowAwardList.GetRange(0, 9));
        else
            ShowAwardComponent.f_Show(_ShowAwardList);
    }

    private void _InfoFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
UITool.Ui_Trip("Sự kiện tải không thành công" + obj.ToString());
    }
    private void _SendRoseSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
f_GetObject("RoseNum").GetComponent<UILabel>().text = string.Format("Số lượng:{0}", UITool.f_GetGoodsNum(_RoseId));
        _SetSlider((float)Data_Pool.m_ValentinesDayPool.m_ValentinesDayPoolDT.m_iGrade / (float)_MaxSorce);
        _UpdateUI();
        if (Data_Pool.m_ValentinesDayPool.m_ValentinesDayAwardList.Count > 0)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { Data_Pool.m_ValentinesDayPool.m_ValentinesDayAwardList });
        }
        _RolrState = em_RoleState.PreviewRole;
        _SetRoleState();
    }
    private void _SendRoseFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
UITool.Ui_Trip("không thành công" + obj.ToString());
    }


    private void _GetBoxSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        _UpdateUI();
        if (Data_Pool.m_AwardPool.m_GetLoginAward.Count > 0)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { Data_Pool.m_AwardPool.m_GetLoginAward });
        }
    }

    private void _GetBoxFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
UITool.Ui_Trip("Nhận không thành công" + obj.ToString());

    }
    #region   按钮事件


    private void _CloseThis(GameObject go, object obj1, object obj2)
    {
        ccUIHoldPool.GetInstance().f_UnHold();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ValentinesDayPage, UIMessageDef.UI_CLOSE);
    }


    private void _PresentBtn(GameObject go, object obj1, object obj2)
    {
        int Times = (int)obj1;
        //if (Data_Pool.m_ValentinesDayPool.m_ValentinesDayPoolDT.m_iGrade >= Times * 10)
        //{
        //    UITool.Ui_Trip("积分已达上限");
        //    return;
        //}
        if (UITool.f_GetGoodsNum(_RoseId) < Times)
        {
UITool.Ui_Trip("Không đủ vật phẩm");
            return;
        }



        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_ValentinesDayPool.m_ValentinesDayAwardList.Clear();
        _SendRoseCallbackDT.m_ccCallbackFail = _SendRoseFail;
        _SendRoseCallbackDT.m_ccCallbackSuc = _SendRoseSuc;
        Data_Pool.m_ValentinesDayPool.f_ValentinesSendRose((byte)_SelectRole, Times, _SendRoseCallbackDT);
    }

    private void _BoxBtn(GameObject go, object obj1, object obj2)
    {
        int idx = (int)obj1;
        int NeedScore = 0;
        int tBoxAwardId = 0;

        if (_BoxNeedSroce == null || _BoxAwardId == null)
        {
UITool.Ui_Trip("Không có phần thưởng");
            return;
        }

        if (idx == 0)
        {
            NeedScore = _BoxNeedSroce.iParam1;
            tBoxAwardId = _BoxAwardId.iParam1;
        }
        else if (idx == 1)
        {
            NeedScore = _BoxNeedSroce.iParam2;
            tBoxAwardId = _BoxAwardId.iParam2;
        }
        else if (idx == 2)
        {
            NeedScore = _BoxNeedSroce.iParam3;
            tBoxAwardId = _BoxAwardId.iParam3;
        }
        else if (idx == 3)
        {
            NeedScore = _BoxNeedSroce.iParam4;
            tBoxAwardId = _BoxAwardId.iParam4;
        }
        string flag = Data_Pool.m_ValentinesDayPool.m_ValentinesDayPoolDT.m_iBoxFlag.ToString();
        int Times = 4 - flag.Length;
        for (int i = 0; i < Times; i++)
            flag += "0";

        if (flag[idx] == '1')
        {
PreviewBoxPageParam tPreviewBoxPageParam = new PreviewBoxPageParam(tBoxAwardId, idx, "View Reward", "", "Get", EM_BoxGetState.AlreadyGet, null);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonBoxPage, UIMessageDef.UI_OPEN, tPreviewBoxPageParam);
            return;
        }
        else if (flag[idx] == '0')
        {
            if (Data_Pool.m_ValentinesDayPool.m_ValentinesDayPoolDT.m_iGrade < NeedScore)
            {
                //打开宝箱预览
PreviewBoxPageParam tPreviewBoxPageParam = new PreviewBoxPageParam(tBoxAwardId, idx, "View Reward", "", "Get", EM_BoxGetState.Lock, null);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonBoxPage, UIMessageDef.UI_OPEN, tPreviewBoxPageParam);
            }
            else
            {
                Data_Pool.m_AwardPool.m_GetLoginAward.Clear();
                UITool.f_OpenOrCloseWaitTip(true);
                _GetBoxCallbackDT.m_ccCallbackSuc = _GetBoxSuc;
                _GetBoxCallbackDT.m_ccCallbackFail = _GetBoxFail;
                Data_Pool.m_ValentinesDayPool.f_ValentinesGetBox((byte)idx, _GetBoxCallbackDT);
            }
        }
    }


    private void _RoleBtn(GameObject go, object obj1, object obj2)
    {
        int idx = (int)obj1;
        _SelectRole = idx;
        _RolrState = em_RoleState.PreviewRole;
        _SetRoleState();
        GameObject Role = _RoleArr[idx];
        _RoleInit(Role);
        Role.transform.SetParent(f_GetObject("CardRole").transform);
        Role.transform.localPosition = new Vector3(300, -265);
        Role.transform.localScale = Vector3.one * 250;
    }
    #endregion


    private void _RoleInit(GameObject go)
    {
        glo_Main.GetInstance().m_ResourceManager.ResetShader(go);
    }
}
