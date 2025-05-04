using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System.Threading;
using Spine.Unity;

public class GodEquipSkillEff : MonoBehaviour
{
    public TextControl m_GodEquipSkillName;
    private List<TextControl> normalGodEquipSkillNameControlCacheList;
    MeshRenderer BufEffect1 = null;
    public Transform IconGodEquip;
    private void Awake()
    {
        normalGodEquipSkillNameControlCacheList = new List<TextControl>();
        normalGodEquipSkillNameControlCacheList.Add(m_GodEquipSkillName);
    }

    public void ClearFlyTxt()
    {
        for (int i = 0; i < normalGodEquipSkillNameControlCacheList.Count; i++)
        {
            normalGodEquipSkillNameControlCacheList[i].gameObject.SetActive(false);
        }
    }

    GameObject tSpine;
    public void f_ShowGodEquipSkillName(MagicDT MagicDT)
    {
        TextControl skillName = f_GetTextControlByCache(normalGodEquipSkillNameControlCacheList);
        if (null == skillName)
            return;
        skillName.f_Show(MagicDT.szName);
        if(GetBufSpine(MagicDT.iId)!= null)
        {
            tSpine = GetBufSpine(MagicDT.iId).gameObject;
            ccTimeEvent.GetInstance().f_RegEvent(0.5f, false, null, AutoDestory);
        }
    }

    public TextControl f_GetTextControlByCache(List<TextControl> textControlList)
    {
        //先从缓存中获取
        for (int i = 0; i < textControlList.Count; i++)
        {
            if (!textControlList[i].IsPlay)
            {
                textControlList[i].IsPlay = true;
                return textControlList[i];
            }
        }

        //不够则新增一个
        GameObject hpObj = NGUITools.AddChild(gameObject, textControlList[0].gameObject);
        TextControl hpTextControl = hpObj.GetComponent<TextControl>();
        hpTextControl.IsPlay = true;
        textControlList.Add(hpTextControl);
        return hpTextControl;
    }

    private GameObject _ParentObj = null;
    private RoleControl _RoleControl;
    public void f_SetGodEquipUse(int id, RoleControl tRoleControl)
    {
        _RoleControl = tRoleControl;
        //int iColour = 0;
        if(id != 0)
        {
            //id la config skill than binh moc nguoc lai id model
            GodEquipDT godEquipDT = glo_Main.GetInstance().m_SC_Pool.m_GodEquipSC.f_GetSCByidSkillGod(id);
            BufEffect1 = GetBufSpine(godEquipDT.iId);
        }
           

        //if (godEquipDT != null)
        //{
        //    iColour = godEquipDT.iColour;
        //}
        //switch (iColour)
        //{
        //    case 5:
        //        BufEffect1 = GetBufSpine(75005);
        //        BufShow(BufEffect1, true);
        //        break;
        //    case 6:
        //        BufEffect1 = GetBufSpine(75006);
        //        BufShow(BufEffect1, true);
        //        break;

        //    case 7:
        //        BufEffect1 = GetBufSpine(75007);
        //        BufShow(BufEffect1, true);
        //        break;
        //}
    }

    private void BufShow(MeshRenderer tMeshRenderer, bool bShow)
    {
        if (tMeshRenderer == null)
        {
            return;
        }
        if (bShow)
        {
            tMeshRenderer.gameObject.SetActive(true);
        }
        else
        {
            if (tMeshRenderer != null)
            {
                tMeshRenderer.gameObject.SetActive(false);
            }
        }
        tMeshRenderer.transform.localPosition = new Vector3(-.5f, 2, 0);
    }

    private MeshRenderer GetBufSpine(int iBufName)
    {
        if (_ParentObj == null)
        {
            _ParentObj = new GameObject();
            _ParentObj.transform.parent = _RoleControl.transform;
            _ParentObj.transform.localScale = Vector3.one;
            _ParentObj.transform.localPosition = Vector3.zero;
        }
        MeshRenderer tMeshRenderer = null;
        Transform oBufSpine = _ParentObj.transform.Find(iBufName.ToString());
        if (oBufSpine == null)
        {
            GameObject oModel = glo_Main.GetInstance().m_ResourceManager.f_CreateMagic(iBufName);
            if (oModel == null)
            {
                oModel = glo_Main.GetInstance().m_ResourceManager.f_CreateMagic(1201112);
                //return null; lấy mặc định
            }
            oBufSpine = oModel.transform;
            oBufSpine.parent = _ParentObj.transform;
            oBufSpine.localPosition = new Vector3(-1, 3, 0);
            oBufSpine.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            oBufSpine.gameObject.layer = 5;
            oBufSpine.name = iBufName.ToString();
            oBufSpine.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "animation", true);

        }

        tMeshRenderer = oBufSpine.GetComponent<MeshRenderer>();
        UpdateDepth(tMeshRenderer, _RoleControl.f_GetDepth());

        return tMeshRenderer;
    }

    private void UpdateDepth(MeshRenderer tMeshRenderer, int iDepth)
    {
        tMeshRenderer.sortingOrder = iDepth;
    }

    private void AutoDestory(object Obj)
    {
        if (tSpine != null)
        {
            glo_Main.GetInstance().m_ResourceManager.f_DestorySD(tSpine);
            tSpine = null;
        }
    }
}
