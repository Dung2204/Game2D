using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class CardBattleClothPage : UIFramwork
{
    private GameObject[] mClothDragParents;

    private GameObject[] m_ClothDragRoles;
    private int[] m_ClothTemplateIds;
    private const int RoleLowestOrder = 2;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mClothDragParents = new GameObject[6];
        for (int i = 0; i < mClothDragParents.Length; i++)
        {
            mClothDragParents[i] = f_GetObject(string.Format("DragParent{0}", i + 1));
        }
        m_ClothDragRoles = new GameObject[mClothDragParents.Length];
        m_ClothTemplateIds = new int[mClothDragParents.Length];
        for (int i = 0; i < mClothDragParents.Length; i++)
        {
            f_RegPressEvent(mClothDragParents[i], f_OnClothItemPress, i);
            UIEventListener.Get(mClothDragParents[i]).parameter = i;
            UIEventListener.Get(mClothDragParents[i]).onDrag = f_OnClothItemDrag;
        }
        f_RegClickEvent("BtnClose", f_OnBtnCloseClick);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        f_InitTemplateIdsByDefList();
        UITool.f_OpenOrCloseWaitTip(true, true);
        StartCoroutine(f_InitRoleByTemplateId());
        f_LoadTexture();
    }
    
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
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

    private void f_InitTemplateIdsByDefList()
    {
        List<CardBattleClothPoolDT> atkCloths = Data_Pool.m_CardBattlePool.AtkClothList;
        for (int i = 0; i < atkCloths.Count; i++)
        {
            if (i < m_ClothTemplateIds.Length)
                m_ClothTemplateIds[i] = atkCloths[i].CardTemplateId;
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
                return;
            }
            //不在6个范围了 就不做操作 复原
            f_SetRoleRenderOrder(m_ClothDragRoles[clothIdx], RoleLowestOrder + clothIdx);
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

    /// <summary>
    /// 坐标点是否在布阵Item的范围内
    /// </summary>
    /// <param name="cardTemplateId">卡牌模板Id</param>
    /// <returns></returns>
    private bool f_InClothItemRect(GameObject clothItem, Vector3 pos)
    {
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

    private bool f_ClothHasChanged()
    {
        for (int i = 0; i < Data_Pool.m_CardBattlePool.AtkClothList.Count; i++)
        {
            if (i < m_ClothTemplateIds.Length)
            {
                if (m_ClothTemplateIds[i] != Data_Pool.m_CardBattlePool.AtkClothList[i].CardTemplateId)
                    return true;
            }
        }
        return false;
    }

    private void f_OnBtnCloseClick(GameObject go, object value1, object value2)
    {
        if (!f_ClothHasChanged())
        {
UITool.Ui_Trip("Đội hình không thay đổi");
            ccUIManage.GetInstance().f_SendMsg(UINameConst.CardBattleClothPage, UIMessageDef.UI_CLOSE);
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true, true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_SetAtkClothArray;
        socketCallbackDt.m_ccCallbackFail = f_Callback_SetAtkClothArray;
        Data_Pool.m_CardBattlePool.f_SetAtkClothArray(m_ClothTemplateIds, socketCallbackDt);
    }

    private void f_Callback_SetAtkClothArray(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
UITool.Ui_Trip("Thành công");
        }
        else
        {
UITool.Ui_Trip("Sort failed,code:" + (int)result);
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CardBattleClothPage, UIMessageDef.UI_CLOSE);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_CardBattleClothPage_UpdateCloth);
    }

    private string strTexBgRoot = "UI/TextureRemove/MainMenu/Tex_ClothArrayBg";
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
        UITexture TexBg = f_GetObject("TexBg").GetComponent<UITexture>();
        if (TexBg.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
            TexBg.mainTexture = tTexture2D;
        }
    }
}
