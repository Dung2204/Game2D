using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using Spine.Unity;
using System.Linq;
/// <summary>
/// 布阵界面
/// 点击阵容然后再点布阵按钮出现的界面
/// </summary>
public class ClothArrayPage : UIFramwork
{
    private static List<EM_FormationPos> listClothArrayTeamPoolID = new List<EM_FormationPos>();//记录卡牌排序序号,从0开始
    private static Dictionary<EM_CloseArrayPos, Vector3> dicCardPos = new Dictionary<EM_CloseArrayPos, Vector3>();//记录坐标位置
    private Dictionary<EM_FormationPos, GameObject> dicObjectModel = new Dictionary<EM_FormationPos, GameObject>();//位置
    private EM_FormationPos moveFormationPosValue;
    private int addSortingLayer = 80;//本页面的层级

    private static Dictionary<EM_CloseArrayPos, long> dicCardID = new Dictionary<EM_CloseArrayPos, long>();
    private List<int> sortingOrderList = new List<int>() 
    { 1, 2, 3, 4, 5,
        6, 9, 12, 15, 18,
        7, 10, 13, 16, 19,
        8, 11, 14, 17, 20 };
    GameObject Arr;
    /// <summary>
    /// UI开启
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        //初始化阵容信息
        InitClothArrayData();
        if (Data_Pool.m_GuidancePool.IsEnter)
        {
            Arr = glo_Main.GetInstance().m_ResourceManager.f_CreateMagic(20000);
            Arr.transform.parent = transform;
            Arr.transform.localPosition = new Vector3(150, -80, 0);
            Arr.transform.localScale = new Vector3(-150, 150, 150);
            Arr.GetComponent<MeshRenderer>().sortingOrder = 320;
            Arr.layer = 5;
            Arr.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "animation", true);
			Data_Pool.m_GuidancePool.m_GuidanceArr.SetActive(false);
        }
        f_LoadTexture();
    }
    private string strTexBgRoot = "UI/TextureRemove/MainMenu/Tex_ClothArrayBg";
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
		string textSeason = "";
        UISprite IconFiveEle = f_GetObject("BtnSeason").GetComponent<UISprite>();
        IconFiveEle.spriteName = "IconEle_" + RolePropertyTools.f_GetElementalSeason();
		UISprite IconFiveEle2 = f_GetObject("BtnSeason2").GetComponent<UISprite>();
        IconFiveEle2.spriteName = "IconEle_" + RolePropertyTools.f_GetElementalSeason();
		UILabel SeasonText = f_GetObject("SeasonText").GetComponent<UILabel>();
		switch(RolePropertyTools.f_GetElementalSeason())
		{
			case 1:
				textSeason = "[FFDD00]Kim";
				break;
			case 2:
				textSeason = "[3ADD00]Mộc";
				break;
			case 3:
				textSeason = "[00ABDD]Thủy";
				break;
			case 4:
				textSeason = "[EC7027]Hỏa";
				break;
			case 5:
				textSeason = "[B47F5E]Thổ";
				break;
		}
        SeasonText.text = textSeason;
		
        UILabel LbNum = f_GetObject("LbNum").GetComponent<UILabel>();
        List<BasePoolDT<long>> TeamBasePoolDT = Data_Pool.m_TeamPool.f_GetAll();
        LbNum.text = TeamBasePoolDT.Count + "/20";
        //加载背景图
        //UITexture TexBg = f_GetObject("TexBg").GetComponent<UITexture>();
        //if (TexBg.mainTexture == null)
        //{
        //    Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
        //    TexBg.mainTexture = tTexture2D;
        //}
        //string iconCamp = "";
        //string lbLevelCamp = "";
        //RolePropertyTools.GetIconAndLevelAuraCamp(ref iconCamp, ref lbLevelCamp);
        //UISprite IconCamp = f_GetObject("BtnAuraCamp").GetComponent<UISprite>();
        //IconCamp.spriteName = iconCamp;//"IconCamp_" + camp;
        //UILabel LevCamp = f_GetObject("LevelCamp").GetComponent<UILabel>();
        //LevCamp.text = lbLevelCamp;

        //string icon = "";
        //string lbLevel = "";
        //RolePropertyTools.GetIconAndLevelAuraType(ref icon, ref lbLevel); 
        //UISprite IconFigthType = f_GetObject("BtnAuraType").GetComponent<UISprite>();
        //IconFigthType.spriteName = icon;
        //UILabel LvFightType = f_GetObject("LevelFightType").GetComponent<UILabel>();
        //LvFightType.text = lbLevel;

        //string icon1 = "";
        //string lbLevel1 = "";
        //RolePropertyTools.GetIconAndLevelAuraFiveEle(ref icon1, ref lbLevel1);
        //UISprite IconFiveEle = f_GetObject("BtnAuraFiveEle").GetComponent<UISprite>();
        //IconFiveEle.spriteName = icon1;
        //UILabel LvFiveEle = f_GetObject("LevelFiveEle").GetComponent<UILabel>();
        //LvFiveEle.text = lbLevel1;
    }
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_ClothArrayPage_CLOSE);
        if (Arr != null)
        {
            glo_Main.GetInstance().m_ResourceManager.f_DestorySD(Arr);
        }
    }
    /// <summary>
    /// 检测上阵完后需不需要更新布阵，如果需要则发送布阵消息
    /// </summary>
    public static void CheckAndCommitClothArray()
    {
        CheckClothArray();
        SendClothToServer();
    }
    /// <summary>
    /// 检测上阵
    /// </summary>
    private static void CheckClothArray()
    {
        // ds của 20 ô chứ 6 tướng "ô rỗng sẽ là EM_FormationPos.eFormationPos_INVALID"
        listClothArrayTeamPoolID.Clear();
        Dictionary<EM_CloseArrayPos, EM_FormationPos> temp = Data_Pool.m_ClosethArrayData.m_dicClothArrayToPos;
        //List<EM_FormationPos> temp = Data_Pool.m_ClosethArrayData.m_aClothArrayTeamPoolID;
        for (int i = 0; i < 20; i++)
        {
            listClothArrayTeamPoolID.Add(EM_FormationPos.eFormationPos_INVALID);
        }
        foreach (KeyValuePair<EM_CloseArrayPos, EM_FormationPos> kvp in temp)
        {
            listClothArrayTeamPoolID[(int)kvp.Key] = kvp.Value;
        }
        //for (int i = 0; i < temp.Count; i++)
        //{
        //    listClothArrayTeamPoolID.Add(temp[i]);
        //}
        //如果编队数量和上阵数量不符，则默认填充在后面
        //int ClosethArrayCount = 0;
        //foreach (EM_FormationPos pos in temp)
        //{
        //    if (pos != EM_FormationPos.eFormationPos_INVALID)
        //        ClosethArrayCount++;
        //}
        List<BasePoolDT<long>> TeamBasePoolDT = Data_Pool.m_TeamPool.f_GetAll();
        for (int i = 0; i < TeamBasePoolDT.Count; i++)
        {
            TeamPoolDT data = (TeamPoolDT)TeamBasePoolDT[i];
            bool isIn = false;//检查所有阵容卡牌都已编队
            foreach (EM_FormationPos pos in listClothArrayTeamPoolID)
            {
                if (pos == data.m_eFormationPos)
                {
                    isIn = true;
                }
            }
            //加入编队前面
            if (!isIn)
            {
                listClothArrayTeamPoolID[(int)data.m_eFormationSlot] = data.m_eFormationPos;
                //for (int j = 0; j <= listClothArrayTeamPoolID.Count - 1; j++)
                //{
                //    if (listClothArrayTeamPoolID[j] == EM_FormationPos.eFormationPos_INVALID)
                //    {
                //        listClothArrayTeamPoolID[j] = data.m_eFormationPos;
                //        break;
                //    }
                //}
            }
        }
    }
    /// <summary>
    /// 初始化阵容信息
    /// </summary>
    private void InitClothArrayData()
    {
        dicObjectModel.Clear();
        CheckClothArray();
        for (int i = 0; i < listClothArrayTeamPoolID.Count; i++)
        {
            string cardName = null;
            EM_FormationPos value = listClothArrayTeamPoolID[i];
            if (value != EM_FormationPos.eFormationPos_INVALID)
            {
                TeamPoolDT teamPoolDt = GetObj(value);
                cardName = teamPoolDt.m_CardPoolDT.m_CardDT.szName + ":" + teamPoolDt.m_eFormationPos.ToString();
            }
            UpdateModel((EM_CloseArrayPos)i, value, cardName);
        }
        f_GetObject("power").GetComponent<UILabel>().text = Data_Pool.m_TeamPool.f_GetTotalBattlePower() + "";
    }
    private TeamPoolDT GetObj(EM_FormationPos pos)
    {
        if (pos != EM_FormationPos.eFormationPos_INVALID)
        {
            List<BasePoolDT<long>> temp = Data_Pool.m_TeamPool.f_GetAll();
            for (int i = 0; i < temp.Count; i++)
            {
                TeamPoolDT data = temp[i] as TeamPoolDT;
                if ((int)data.m_eFormationPos == (int)pos)
                {
                    return data;
                }
            }
        }
        return null;
    }
    /// <summary>
    /// 更新视图
    /// </summary>
    /// <param name="cpos"></param>
    /// <param name="fPos"></param>
    /// <param name="cardName"></param>
    private void UpdateModel(EM_CloseArrayPos cpos, EM_FormationPos fPos, string cardName)
    {

        bool isInvalid = (fPos == EM_FormationPos.eFormationPos_INVALID ? true : false);
        GameObject model = null;
        CardPoolDT cardPoolDT = Data_Pool.m_TeamPool.f_GetFormationPosCardPoolDT(fPos);
        int index = (int)cpos + 1;
        model = f_GetObject("BtnHand"+ index);
        model.transform.localPosition = dicCardPos[cpos];
        model.SetActive(!isInvalid);
        //switch (cpos)
        //{
        //    case EM_CloseArrayPos.eCloseArray_PosOne:
        //        model = f_GetObject("BtnOneHand");
        //        model.transform.localPosition = dicCardPos[EM_CloseArrayPos.eCloseArray_PosOne];
        //        model.SetActive(!isInvalid);
        //        break;
        //    case EM_CloseArrayPos.eCloseArray_PosTwo:
        //        model = f_GetObject("BtnTwoHand");
        //        model.transform.localPosition = dicCardPos[EM_CloseArrayPos.eCloseArray_PosTwo];
        //        model.SetActive(!isInvalid);
        //        break;
        //    case EM_CloseArrayPos.eCloseArray_PosThree:
        //        model = f_GetObject("BtnThreeHand");
        //        model.transform.localPosition = dicCardPos[EM_CloseArrayPos.eCloseArray_PosThree];
        //        model.SetActive(!isInvalid);
        //        break;
        //    case EM_CloseArrayPos.eCloseArray_PosFour:
        //        model = f_GetObject("BtnFourHand");
        //        model.transform.localPosition = dicCardPos[EM_CloseArrayPos.eCloseArray_PosFour];
        //        model.SetActive(!isInvalid);
        //        break;
        //    case EM_CloseArrayPos.eCloseArray_PosFive:
        //        model = f_GetObject("BtnFiveHand");
        //        model.transform.localPosition = dicCardPos[EM_CloseArrayPos.eCloseArray_PosFive];
        //        model.SetActive(!isInvalid);
        //        break;
        //    case EM_CloseArrayPos.eCloseArray_PosSix:
        //        model = f_GetObject("BtnSixHand");
        //        model.transform.localPosition = dicCardPos[EM_CloseArrayPos.eCloseArray_PosSix];
        //        model.SetActive(!isInvalid);
        //        break;
        //}
        if (model.transform.Find("ModelObj") != null)
        {
            UITool.f_DestoryStatelObject(model.transform.Find("ModelObj").gameObject);
        }


        if (!isInvalid && model != null)
        {
            //model.GetComponentInChildren<UILabel>().text = "";// cardName;
            UIEventListener tEvent = UIEventListener.Get(model);
            tEvent.parameter = fPos;
            tEvent.onPressV2 = OnModelItemClick;
            //f_RegPressEvent(model, OnModelItemClick, fPos);
            dicObjectModel.Add(fPos, model);
            int sortingOrder = 1;
            //if (cpos == EM_CloseArrayPos.eCloseArray_PosOne)
            //    sortingOrder = 2;
            //else if (cpos == EM_CloseArrayPos.eCloseArray_PosFour)
            //    sortingOrder = 1;
            //if (cpos == EM_CloseArrayPos.eCloseArray_PosFive)
            //    sortingOrder = 2;
            //else if (cpos == EM_CloseArrayPos.eCloseArray_PosTwo)
            //    sortingOrder = 3;
            //if (cpos == EM_CloseArrayPos.eCloseArray_PosSix)
            //    sortingOrder = 4;
            //else if (cpos == EM_CloseArrayPos.eCloseArray_PosThree)
            //    sortingOrder = 5;
            //sortingOrder += addSortingLayer;
            sortingOrder = (sortingOrderList[(int)cpos]+ addSortingLayer);
            GameObject Role = UITool.f_GetStatelObject(cardPoolDT, model.transform, new Vector3(0, 180, 0), new Vector3(0, -80, 0), sortingOrder, "ModelObj", 60, false);
            if(cpos>= EM_CloseArrayPos.eCloseArray_PosOne && cpos <= EM_CloseArrayPos.eCloseArray_PosFive)
            {
                Role.transform.localScale = new Vector3(-60, 60, 60);
            }
            else {
                Role.transform.localScale = new Vector3(60, 60, 60);
            }
            
            Role.SetActive(false);
            Role.SetActive(true);
        }
    }
    /// <summary>
    /// 初始化，注册ui事件
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        //f_RegClickEvent("BtnBlack", OnBtnBlackClick);
        f_RegClickEvent("BtnClose", OnBtnBlackClick);
        f_RegClickEvent("BtnSave", OnBtnSaveClick);
        f_RegClickEvent("BtnAura", OnBtnAuraClick);
		f_RegClickEvent("BtnSeason", f_OnBtnSeasonClick);
		f_RegClickEvent("CloseSeasonBtn", f_OnBtnSeasonClose);
        //f_RegClickEvent("BtnAuraCamp", OnBtnAuraCampClick);
        //f_RegClickEvent("BtnAuraType", OnBtnAuraTypeClick);
        //f_RegClickEvent("BtnAuraFiveEle", OnBtnAuraFiveEleClick);
        dicCardPos.Clear();
        for(int i=0;i<= (int)EM_CloseArrayPos.eCloseArray_Pos20; i++)
        {
            int index = i + 1;
            dicCardPos.Add((EM_CloseArrayPos)i, f_GetObject("BtnHand"+ index).transform.localPosition);
        }
        //dicCardPos.Add(EM_CloseArrayPos.eCloseArray_PosOne, f_GetObject("BtnOneHand").transform.localPosition);
        //dicCardPos.Add(EM_CloseArrayPos.eCloseArray_PosTwo, f_GetObject("BtnTwoHand").transform.localPosition);
        //dicCardPos.Add(EM_CloseArrayPos.eCloseArray_PosThree, f_GetObject("BtnThreeHand").transform.localPosition);
        //dicCardPos.Add(EM_CloseArrayPos.eCloseArray_PosFour, f_GetObject("BtnFourHand").transform.localPosition);
        //dicCardPos.Add(EM_CloseArrayPos.eCloseArray_PosFive, f_GetObject("BtnFiveHand").transform.localPosition);
        //dicCardPos.Add(EM_CloseArrayPos.eCloseArray_PosSix, f_GetObject("BtnSixHand").transform.localPosition);
    }
    /// <summary>
    /// 点击黑色背景区域，关掉UI
    /// </summary>
	private void f_OnBtnSeasonClick(GameObject go, object value1, object value2)
    {
        f_GetObject("SeasonPanel").SetActive(true);
    }
	private void f_OnBtnSeasonClose(GameObject go, object value1, object value2)
    {
        f_GetObject("SeasonPanel").SetActive(false);
    }
	
    private void OnBtnBlackClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_MODELINFOR);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ClothArrayPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }
    private void OnBtnSaveClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_MODELINFOR);
        SendClothToServer();
    }

    private void OnBtnAuraClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AuraMainPage, UIMessageDef.UI_OPEN);
    }

    //private void OnBtnAuraCampClick(GameObject go, object obj1, object obj2)
    //{
    //    ccUIManage.GetInstance().f_SendMsg(UINameConst.AuraCampPage, UIMessageDef.UI_OPEN);
    //    ccUIManage.GetInstance().f_SendMsg(UINameConst.ClothArrayPage, UIMessageDef.UI_CLOSE);
    //}
    ///// <summary> maco
    ///// Action click Mở UI kích hoạt vòng sáng trận hình 
    ///// </summary>
    //private void OnBtnAuraTypeClick(GameObject go, object obj1, object obj2)
    //{
    //    ccUIManage.GetInstance().f_SendMsg(UINameConst.AuraTypePage, UIMessageDef.UI_OPEN);
    //    ccUIManage.GetInstance().f_SendMsg(UINameConst.ClothArrayPage, UIMessageDef.UI_CLOSE);
    //}
    //private void OnBtnAuraFiveEleClick(GameObject go, object obj1, object obj2)
    //{
    //    ccUIManage.GetInstance().f_SendMsg(UINameConst.AuraFiveElementsPage, UIMessageDef.UI_OPEN);
    //    ccUIManage.GetInstance().f_SendMsg(UINameConst.ClothArrayPage, UIMessageDef.UI_CLOSE);
    //}
    /// <summary>
    /// 发送编队消息，没有改变则不需要发送消息
    /// </summary>
    private static void SendClothToServer()
    {
        //向服务器提交修改
        MessageBox.DEBUG("listClothArrayTeamPoolID  Length:" + listClothArrayTeamPoolID.Count);
        EM_FormationSlot[] posArray = new EM_FormationSlot[GameParamConst.MAX_FIGHT_POS];
        for (int i = 0; i < listClothArrayTeamPoolID.Count; i++)
        {
            if(listClothArrayTeamPoolID[i] != EM_FormationPos.eFormationPos_INVALID)
            {
                posArray[(int)listClothArrayTeamPoolID[i]] = (EM_FormationSlot)i;
            }
        }
        //for (int i = 0; i < posArray.Length; i++)
        //{
        //    posArray[i] = listClothArrayTeamPoolID[i];
        //}
        //检测是否有没有改动，没有改动则不发送编队消息
        //List<EM_FormationPos> temp = Data_Pool.m_ClosethArrayData.m_aClothArrayTeamPoolID;
        //MessageBox.DEBUG("m_aClothArrayTeamPoolID  Length:" + temp.Count);

        Dictionary<EM_CloseArrayPos, EM_FormationPos> temp = Data_Pool.m_ClosethArrayData.m_dicClothArrayToPos;

        for (int i = 0; i < temp.Count; i++)
        {
            if ((int)temp.ElementAt(i).Key != (int)posArray[(int)temp.ElementAt(i).Value])
            {
                Data_Pool.m_ClosethArrayData.CommitData(posArray);
                break;
            }
        }
    }
    private float GetDistance2D(Vector3 pos1, Vector3 pos2)
    {
        return Mathf.Sqrt((pos2.x - pos1.x) * (pos2.x - pos1.x) + (pos2.y - pos1.y) * (pos2.y - pos1.y));
    }
    private bool IsCollider2D(Vector3 pos1, Vector3 pos2)
    {
        return pos1.x <= (pos2.x + 100) && pos1.y <= (pos2.y + 100) && pos2.x <= (pos1.x + 100) && pos2.y <= (pos1.y + 100);
    }
    /// <summary>
    /// 检测物体离哪个位置更近，如果该位置已经有物体，则返回空传递的indexDefault
    /// </summary>
    /// <returns></returns>
    private EM_CloseArrayPos CheckPosNear(GameObject go, EM_CloseArrayPos indexOri)
    {
        Vector3 cardPos = go.transform.localPosition;
        float nearPos = GetDistance2D(cardPos, dicCardPos[EM_CloseArrayPos.eCloseArray_PosOne]);
        EM_CloseArrayPos nearIndex = EM_CloseArrayPos.eCloseArray_PosOne;
        for (int i = (int)(EM_CloseArrayPos.eCloseArray_PosOne + 1); i < dicCardPos.Count; i++)
        {
            float distance = GetDistance2D(cardPos, dicCardPos[(EM_CloseArrayPos)i]);
            if (distance < nearPos)
            {
                nearPos = distance;
                nearIndex = (EM_CloseArrayPos)i;
            }
        }
        if(!IsCollider2D(cardPos, dicCardPos[nearIndex]))
        {
            return indexOri;
        }
        //需要交换位置
        EM_FormationPos nearIndexPos = (listClothArrayTeamPoolID[(int)nearIndex]);
        if (nearIndexPos != EM_FormationPos.eFormationPos_INVALID)
        {
            listClothArrayTeamPoolID[(int)indexOri] = nearIndexPos;
            dicObjectModel[nearIndexPos].transform.localPosition = dicCardPos[(EM_CloseArrayPos)indexOri];
            SetModelSortingOrderLabyer(dicObjectModel[nearIndexPos], indexOri);
        }
        return nearIndex;
    }
    /// <summary>
    /// 点击角色模型布阵，可拖动模型
    /// </summary>
    public void OnModelItemClick(GameObject go, bool obj1, object obj2)
    {

        if (obj1)
        {
            go.transform.Find("ModelObj").GetComponent<Renderer>().sortingOrder = addSortingLayer + 40;
        }
        else
        {
            moveFormationPosValue = (EM_FormationPos)obj2;
            ccTimeEvent.GetInstance().f_RegEvent(0.1f, false, go, OnSetPosition);
            if (Data_Pool.m_GuidancePool.IsEnter)
            {
                Data_Pool.m_GuidancePool.m_GuidanceArr.SetActive(true);
                if (Arr != null)
                {
                    GameObject.Destroy(Arr);
                }
            }
        }

    }
    private void tttttt(GameObject go, object obj1, object obj2)
    {


    }
    /// <summary>
    /// 拖动完成后重设坐标
    /// </summary>
    /// <param name="obj1"></param>
    private void OnSetPosition(object obj1)
    {
        GameObject go = (GameObject)obj1;
        EM_CloseArrayPos neeadPosIndex = EM_CloseArrayPos.eCloseArray_PosOne;
        for (int i = 0; i < listClothArrayTeamPoolID.Count; i++)
        {
            if (listClothArrayTeamPoolID[i] == moveFormationPosValue)
            {
                //listClothArrayTeamPoolID[i] = EM_FormationPos.eFormationPos_INVALID;
                neeadPosIndex = (EM_CloseArrayPos)i;
                break;
            }
        }

        EM_CloseArrayPos nearIndex = CheckPosNear(go, neeadPosIndex);
        if(nearIndex >= EM_CloseArrayPos.eCloseArray_PosOne && nearIndex <= EM_CloseArrayPos.eCloseArray_PosFive)
        {
            CardPoolDT cardPoolDT = Data_Pool.m_TeamPool.f_GetFormationPosCardPoolDT(moveFormationPosValue);
            if(cardPoolDT == null || cardPoolDT.m_CardDT.iCardFightType != (int)EM_CardFightType.eCardKiller)
            {
                UITool.Ui_Trip("Yêu cầu nghề Kích tướng");
                go.transform.localPosition = dicCardPos[neeadPosIndex];
                return;
            }
        }
        
        //Debug.Log("最近的序号:" + nearIndex);

        SetModelSortingOrderLabyer(go, nearIndex);
        EM_FormationPos oldTemp = listClothArrayTeamPoolID[(int)nearIndex];
        listClothArrayTeamPoolID[(int)neeadPosIndex] = oldTemp == EM_FormationPos.eFormationPos_INVALID ? EM_FormationPos.eFormationPos_INVALID : oldTemp;
        listClothArrayTeamPoolID[(int)nearIndex] = moveFormationPosValue;
        
        go.transform.localPosition = dicCardPos[nearIndex];
    }
    /// <summary>
    /// 设置模型渲染顺序
    /// </summary>
    /// <param name="btnHand"></param>
    /// <param name="pos"></param>
    private void SetModelSortingOrderLabyer(GameObject btnHand, EM_CloseArrayPos cpos)
    {
        
        Transform t = btnHand.transform.Find("ModelObj");
        if (t != null)
        {
            int sortingOrder = 1;
            sortingOrder = (sortingOrderList[(int)cpos] + addSortingLayer);
            t.GetComponent<Renderer>().sortingOrder = addSortingLayer + sortingOrder;
            if (cpos >= EM_CloseArrayPos.eCloseArray_PosOne && cpos <= EM_CloseArrayPos.eCloseArray_PosFive)
            {
                t.GetComponent<Renderer>().transform.localScale = new Vector3(-60, 60, 60);
            }
            else
            {
                t.GetComponent<Renderer>().transform.localScale = new Vector3(60, 60, 60);
            }
        }
    }
}
