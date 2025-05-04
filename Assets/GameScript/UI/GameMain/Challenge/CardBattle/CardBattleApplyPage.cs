using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class CardBattleApplyPage : SubPanelBase
{
    public GameObject mTeamItemParent;
    public GameObject mTeamItem;
    private List<BasePoolDT<long>> teamList;
    private UIWrapComponent _teamListWrapComponent;
    private UIWrapComponent mTeamListWrapComponent
    {
        get
        {
            if (_teamListWrapComponent == null)
            {
                teamList = Data_Pool.m_CardBattlePool.f_GetAll();
                //需要保证滚动Item不能少于展示Item，不然拖拽时有可能改变了Item的数据
                _teamListWrapComponent = new UIWrapComponent(160, 1, 160, 10, mTeamItemParent, mTeamItem, teamList, f_OnTeamItemUpdate, null);
            }
            return _teamListWrapComponent;
        }
    }

    public UILabel mTeamRefreshTimes;
    public GameObject mBtnRefresh;
    public GameObject mBtnRefreshGray;
    public UILabel mBtnRefreshGrayLabel;
    public GameObject mBtnSureCloth;
    public GameObject mBtnSureClothGray;
    public UILabel mBtnSureClothGrayLabel;
    public GameObject mBtnHelp;
    public UILabel mChallengeTime;

    public GameObject[] mClothDragParents;

    private GameObject[] m_ClothDragRoles;
    private int[] m_ClothTemplateIds;
    private const int RoleLowestOrder = 2;

    public override void f_Init(UIFramwork parentUI)
    {
        base.f_Init(parentUI);
        m_ClothDragRoles = new GameObject[mClothDragParents.Length];
        m_ClothTemplateIds = new int[mClothDragParents.Length];
        for (int i = 0; i < mClothDragParents.Length; i++)
        {
            f_RegPressEvent(mClothDragParents[i], f_OnClothItemPress, i);
            UIEventListener.Get(mClothDragParents[i]).parameter = i;
            UIEventListener.Get(mClothDragParents[i]).onDrag = f_OnClothItemDrag;
        }
        f_RegClickEvent(mBtnRefresh, f_OnBtnRefreshClick);
        f_RegClickEvent(mBtnSureCloth, f_OnBtnSureClothClick);
        f_RegClickEvent(mBtnHelp, f_OnBtnHlepClick);
    }

    public override void f_Open()
    {
        base.f_Open();
        for (int i = 0; i < mClothDragParents.Length; i++)
        {
            mClothDragParents[i].transform.localPosition = Vector3.zero;
        }
        f_InitTemplateIdsByDefList();
        mTeamListWrapComponent.f_ResetView();
mTeamRefreshTimes.text = string.Format("[F6F3F0]Làm mới：[FF0B43] {0} [-]lần[-]", Data_Pool.m_CardBattlePool.LeftRefreshTeamTimes);
mChallengeTime.text = string.Format("[ff8439]Thời gian hoạt động: [72ff00]{0:d2}:{1:d2}:{2:d2}[-][-]", Data_Pool.m_CardBattlePool.BattleTime [0], Data_Pool.m_CardBattlePool.BattleTime[1], 0);
        f_UpdateBySecond();
        UITool.f_OpenOrCloseWaitTip(true, true);
        StartCoroutine(f_InitRoleByTemplateId());
    }

    private void f_InitTemplateIdsByDefList()
    {
        List<CardBattleClothPoolDT> defCloths = Data_Pool.m_CardBattlePool.DefClothList;
        for (int i = 0; i < defCloths.Count; i++)
        {
            if (i < m_ClothTemplateIds.Length)
                m_ClothTemplateIds[i] = defCloths[i].CardTemplateId;
        }
    }

    private System.Collections.IEnumerator f_InitRoleByTemplateId()
    {
        for (int i = 0; i < m_ClothTemplateIds.Length; i++)
        {
            if (m_ClothTemplateIds[i] != 0)
            {
                UITool.f_CreateRoleByCardId(m_ClothTemplateIds[i], ref m_ClothDragRoles[i], mClothDragParents[i].transform.Find("RoleParent"), RoleLowestOrder + i, 1, false, false);
                yield return new WaitForSeconds(0.2f);
            }
            else if (m_ClothDragRoles[i] != null)
            {
                UITool.f_DestoryStatelObject(m_ClothDragRoles[i]);
                m_ClothDragRoles[i] = null;
            }
        }
        UITool.f_OpenOrCloseWaitTip(false);
    }

    public override void f_Close()
    {
        base.f_Close();
        StopAllCoroutines();
        for (int i = 0; i < m_ClothDragRoles.Length; i++)
        {
            if (m_ClothDragRoles[i] != null)
            {
                UITool.f_DestoryStatelObject(m_ClothDragRoles[i]);
                m_ClothDragRoles[i] = null;
            }
            m_ClothTemplateIds[i] = 0;
        }
    }

    private void f_OnTeamItemUpdate(Transform tf, BasePoolDT<long> node)
    {
        CardBattleTeamPoolDT tInfo = (CardBattleTeamPoolDT)node;
        CardBattleTeamItem tItem = tf.GetComponent<CardBattleTeamItem>();
        tItem.f_UpdateByInfo(tInfo, f_InCloth(tInfo.CardTemplateId));
        tItem.DragParent.transform.localPosition = Vector3.zero;
        f_RegClickEvent(tItem.DragParent, f_OnTeamItemClick, tInfo);
        f_RegPressEvent(tItem.DragParent, f_OnTeamItemPress, tInfo);
        UIEventListener.Get(tItem.DragParent).onDrag = f_OnTeamItemDrag;
    }

    private void f_OnTeamItemClick(GameObject go, object value1, object value2)
    {
        CardBattleTeamPoolDT teamInfo = (CardBattleTeamPoolDT)value1;
        if (teamInfo.CardTemplateId == 0 || teamInfo.CardTemplate == null)
            return;
        CardPoolDT cardPoolDT = new CardPoolDT();
        cardPoolDT.m_CardDT = teamInfo.CardTemplate as CardDT;
        CardBox cardBox = new CardBox();
        cardBox.m_Card = cardPoolDT;
        cardBox.m_bType = CardBox.BoxType.Intro;
        cardBox.m_oType = CardBox.OpenType.CardBattlePage;
        ccUIHoldPool.GetInstance().f_Hold(mParentUI);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CardProperty, UIMessageDef.UI_OPEN, cardBox);
    }

    private bool m_TeamItemPress = false;
    private int m_TeamItemTouchId = 0;
    private void f_OnTeamItemPress(GameObject go, object value1, object value2)
    {
        bool isPress = (bool)value1;
        CardBattleTeamPoolDT tInfo = (CardBattleTeamPoolDT)value2;
        CardBattleTeamItem tItem = go.GetComponentInParent<CardBattleTeamItem>();
        if (isPress)
        {
            if (!f_InCloth(tInfo.CardTemplateId))
            {
                UITool.f_CreateRoleByCardId(tInfo.CardTemplateId, ref tItem.role, tItem.RoleParent.transform, RoleLowestOrder + 7, 1 , false, false);
                m_TeamItemPress = true;
            }
            else
            {
                m_TeamItemPress = false;
            }

        }
        else
        {
            if (!m_TeamItemPress)
                return;
            m_TeamItemPress = false;
            tItem.DragParent.transform.localPosition = Vector3.zero;
            Vector3 worldPos = UICamera.currentCamera.ScreenToWorldPoint(new Vector3(UICamera.currentTouch.pos.x, UICamera.currentTouch.pos.y));
            for (int i = 0; i < mClothDragParents.Length; i++)
            {
                if (f_InClothItemRect(mClothDragParents[i], worldPos))
                {
                    if (m_ClothTemplateIds[i] != 0 && m_ClothDragRoles[i] != null)
                    {
                        UITool.f_DestoryStatelObject(m_ClothDragRoles[i]);
                        m_ClothDragRoles[i] = null;
                    }
                    m_ClothTemplateIds[i] = tInfo.CardTemplateId;
                    m_ClothDragRoles[i] = tItem.role;
                    f_SetRoleParent(m_ClothDragRoles[i], mClothDragParents[i].transform.Find("RoleParent"));
                    f_SetRoleRenderOrder(m_ClothDragRoles[i], RoleLowestOrder + i);
                    tItem.role = null;
                }
            }
            if (tItem.role != null)
            {
                UITool.f_DestoryStatelObject(tItem.role);
                tItem.role = null;
            }
            mTeamListWrapComponent.f_UpdateView();
        }
    }

    private void f_OnTeamItemDrag(GameObject go, Vector2 delta)
    {
        if (!m_TeamItemPress)
            return;
        if (go == null || go.transform == null)
            return;
        Vector2 pos = UICamera.currentCamera.ScreenToWorldPoint(new Vector3(UICamera.currentTouch.pos.x, UICamera.currentTouch.pos.y));
        go.transform.position = pos;
    }

    private bool m_ClothPress = false;
    private int m_ClothTouchId = 0;
    private void f_OnClothItemPress(GameObject go, object value1, object value2)
    {
        bool isPress = (bool)value1;
        int clothIdx = (int)value2;
        if (isPress)
        {
            if (m_ClothDragRoles[clothIdx] != null)
            {
                m_ClothPress = true;
                f_SetRoleRenderOrder(m_ClothDragRoles[clothIdx], RoleLowestOrder + m_ClothDragRoles.Length);
            }
            else
            {
                m_ClothPress = false;
            }
        }
        else
        {
            if (!m_ClothPress)
                return;
            m_ClothPress = false;
            go.transform.localPosition = Vector3.zero;
            Vector3 worldPos = UICamera.currentCamera.ScreenToWorldPoint(new Vector3(UICamera.currentTouch.pos.x, UICamera.currentTouch.pos.y));
            for (int i = 0; i < mClothDragParents.Length; i++)
            {
                if (!f_InClothItemRect(mClothDragParents[i], worldPos))
                    continue;
                if (clothIdx == i)
                    return;
                if (m_ClothDragRoles[i] != null)
                {
                    GameObject role = m_ClothDragRoles[i];
                    int templateId = m_ClothTemplateIds[i];
                    m_ClothDragRoles[i] = m_ClothDragRoles[clothIdx];
                    m_ClothTemplateIds[i] = m_ClothTemplateIds[clothIdx];
                    f_SetRoleParent(m_ClothDragRoles[i], mClothDragParents[i].transform.Find("RoleParent"));
                    f_SetRoleRenderOrder(m_ClothDragRoles[i], RoleLowestOrder + i);
                    m_ClothDragRoles[clothIdx] = role;
                    m_ClothTemplateIds[clothIdx] = templateId;
                    f_SetRoleParent(m_ClothDragRoles[clothIdx], mClothDragParents[clothIdx].transform.Find("RoleParent"));
                    f_SetRoleRenderOrder(m_ClothDragRoles[clothIdx], RoleLowestOrder + clothIdx);
                }
                else
                {
                    m_ClothDragRoles[i] = m_ClothDragRoles[clothIdx];
                    m_ClothTemplateIds[i] = m_ClothTemplateIds[clothIdx];
                    f_SetRoleParent(m_ClothDragRoles[i], mClothDragParents[i].transform.Find("RoleParent"));
                    f_SetRoleRenderOrder(m_ClothDragRoles[i], RoleLowestOrder + i);
                    m_ClothDragRoles[clothIdx] = null;
                    m_ClothTemplateIds[clothIdx] = 0;
                }
                mTeamListWrapComponent.f_UpdateView();
                return;
            }
            UITool.f_DestoryStatelObject(m_ClothDragRoles[clothIdx]);
            m_ClothDragRoles[clothIdx] = null;
            m_ClothTemplateIds[clothIdx] = 0;
            mTeamListWrapComponent.f_UpdateView();
        }
    }

    private void f_OnClothItemDrag(GameObject go, Vector2 delta)
    {
        if (!m_ClothPress)
            return;
        if (go == null || go.transform == null)
            return;
        Vector2 pos = UICamera.currentCamera.ScreenToWorldPoint(new Vector3(UICamera.currentTouch.pos.x, UICamera.currentTouch.pos.y));
        go.transform.position = pos;
    }

    /// <summary>
    /// 卡牌是否在布阵中
    /// </summary>
    /// <param name="cardTemplateId">卡牌模板Id</param>
    /// <returns></returns>
    private bool f_InCloth(int cardTemplateId)
    {
        for (int i = 0; i < m_ClothTemplateIds.Length; i++)
        {
            if (m_ClothTemplateIds[i] == cardTemplateId)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 坐标点是否在布阵Item的范围内
    /// </summary>
    /// <param name="cardTemplateId">卡牌模板Id</param>
    /// <returns></returns>
    private bool f_InClothItemRect(GameObject clothItem, Vector3 pos)
    {
        //界面关闭了则全部都不判断范围，直接返回false
        if (!mPanel.activeSelf)
            return false;
        BoxCollider boxCollider = clothItem.GetComponent<BoxCollider>();
        if (boxCollider == null)
            return false;
        Vector3 closePos = boxCollider.ClosestPointOnBounds(pos);
        if (closePos == pos)
        {
            return true;
        }
        return false;
    }

    private bool f_ClothInvaild()
    {
        for (int i = 0; i < m_ClothTemplateIds.Length; i++)
        {
            if (m_ClothTemplateIds[i] != 0)
                return false;
        }
        return true;
    }

    private bool f_ClothHasChanged()
    {
        for (int i = 0; i < Data_Pool.m_CardBattlePool.DefClothList.Count; i++)
        {
            if (i < m_ClothTemplateIds.Length)
            {
                if (m_ClothTemplateIds[i] != Data_Pool.m_CardBattlePool.DefClothList[i].CardTemplateId)
                    return true;
            }
        }
        return false;
    }

    private void f_SetRoleParent(GameObject role, Transform parent)
    {
        role.transform.parent = parent;
        role.transform.localPosition = Vector3.zero;
        role.transform.localRotation = Quaternion.identity;
        role.transform.localScale = Vector3.one;
    }

    private void f_SetRoleRenderOrder(GameObject role, int order)
    {
        Renderer renderer = role.GetComponent<Renderer>();
        if (renderer != null)
            renderer.sortingOrder = order;
    }

    private void f_OnBtnRefreshClick(GameObject go, object value1, object value2)
    {
        if (Data_Pool.m_CardBattlePool.LeftRefreshTeamTimes <= 0)
        {
UITool.Ui_Trip("Đã hết số lần làm mới");
            return;
        }
        else if (GameSocket.GetInstance().f_GetServerTime() - Data_Pool.m_CardBattlePool.RefreshTeamTime < Data_Pool.m_CardBattlePool.RefreshTeamTimeCD)
        {
            int cdTime = Data_Pool.m_CardBattlePool.RefreshTeamTimeCD - GameSocket.GetInstance().f_GetServerTime() + Data_Pool.m_CardBattlePool.RefreshTeamTime;
UITool.Ui_Trip(string.Format("Thử lại sau {0} giây", cdTime));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_RefreshCardTeam;
        socketCallbackDt.m_ccCallbackFail = f_Callback_RefreshCardTeam;
        Data_Pool.m_CardBattlePool.f_RefreshCardTeam(socketCallbackDt);
    }

    private void f_Callback_RefreshCardTeam(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
UITool.Ui_Trip("Làm mới thành công");
            f_InitTemplateIdsByDefList();
            mTeamListWrapComponent.f_ResetView();
            UITool.f_OpenOrCloseWaitTip(true, true);
            StartCoroutine(f_InitRoleByTemplateId());
mTeamRefreshTimes.text = string.Format("[F6F3F0]Làm mới：[FF0B43] {0} [-]lần[-]", Data_Pool.m_CardBattlePool.LeftRefreshTeamTimes);
            f_UpdateBySecond();
        }
        else
        {
UITool.Ui_Trip("Làm mới thất bại,code:" + (int)result);
        }
    }

    private void f_OnBtnSureClothClick(GameObject go, object value1, object value2)
    {
        if (f_ClothInvaild())
        {
UITool.Ui_Trip("Đội hình trống");
			return;
        }
        else if (!f_ClothHasChanged())
        {
UITool.Ui_Trip("Đội hình không thay đổi");
            return;
        }
        else if (GameSocket.GetInstance().f_GetServerTime() - Data_Pool.m_CardBattlePool.SetDefClothTime < Data_Pool.m_CardBattlePool.SetDefClothTimeCD)
        {
            int cdTime = Data_Pool.m_CardBattlePool.SetDefClothTimeCD - GameSocket.GetInstance().f_GetServerTime() + Data_Pool.m_CardBattlePool.SetDefClothTime;
UITool.Ui_Trip(string.Format("Thử lại sau {0} giây", cdTime));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_SetDefClothArray;
        socketCallbackDt.m_ccCallbackFail = f_Callback_SetDefClothArray;
        Data_Pool.m_CardBattlePool.f_SetDefClothArray(m_ClothTemplateIds, socketCallbackDt);
    }

    private void f_Callback_SetDefClothArray(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
UITool.Ui_Trip("Thành công");
            f_UpdateBySecond();
        }
        else
        {
UITool.Ui_Trip("Thất bại,code:" + (int)result);
        }
    }



    public void f_OnBtnHlepClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage, UIMessageDef.UI_OPEN, 6);
    }

    //public UIGrid btnGrid;
    public void f_UpdateBySecond()
    {
        if (!mPanel.activeSelf)
            return;
        int now = GameSocket.GetInstance().f_GetServerTime();
        int refreshTeamTime = Data_Pool.m_CardBattlePool.RefreshTeamTime;
        int setDefClothTime = Data_Pool.m_CardBattlePool.SetDefClothTime;
        if (now - refreshTeamTime < Data_Pool.m_CardBattlePool.RefreshTeamTimeCD)
        {
            if (mBtnRefresh.activeSelf)
            {
                mBtnRefresh.SetActive(false);
                //btnGrid.Reposition();
            }

            if (!mBtnRefreshGray.activeSelf)
            {
                mBtnRefreshGray.SetActive(true);
                //btnGrid.Reposition();
            }
mBtnRefreshGrayLabel.text = string.Format("Làm mới（{0}）", Data_Pool.m_CardBattlePool.RefreshTeamTimeCD - (now - refreshTeamTime));
        }
        else
        {
            if (!mBtnRefresh.activeSelf)
            {
                mBtnRefresh.SetActive(true);
                //btnGrid.Reposition();
            }
       
            if (mBtnRefreshGray.activeSelf)
            {
                mBtnRefreshGray.SetActive(false);
                //btnGrid.Reposition();
            }
        
        }
        if (now - setDefClothTime < Data_Pool.m_CardBattlePool.SetDefClothTimeCD)
        {
            if (mBtnSureCloth.activeSelf)
            {
                mBtnSureCloth.SetActive(false);
                //btnGrid.Reposition();
            }
            if (!mBtnSureClothGray.activeSelf)
            {
                mBtnSureClothGray.SetActive(true);
                //btnGrid.Reposition();
            }
mBtnSureClothGrayLabel.text = string.Format("Xác nhận（{0}）", Data_Pool.m_CardBattlePool.SetDefClothTimeCD - (now - setDefClothTime));
        }
        else
        {
            if (!mBtnSureCloth.activeSelf)
            {
                mBtnSureCloth.SetActive(true);
                //btnGrid.Reposition();
            }
            if (mBtnSureClothGray.activeSelf)
            {
                mBtnSureClothGray.SetActive(false);
                //btnGrid.Reposition();
            }

        }

    }
}
