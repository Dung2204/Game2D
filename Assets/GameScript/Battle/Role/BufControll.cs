using UnityEngine;
using System.Collections;
using Spine.Unity;

public class BufControll
{
    private GameObject _ParentObj = null;

    MeshRenderer BufEffectType6 = null;
    MeshRenderer BufEffectType5 = null;
    MeshRenderer BufEffectType4 = null;
    MeshRenderer BufEffectType3 = null;
    MeshRenderer BufEffectType2 = null;
    MeshRenderer BufEffectType15 = null;
    MeshRenderer BufEffectType16 = null;
    MeshRenderer BufEffectType17 = null;
    MeshRenderer BufEffectType18 = null;
    MeshRenderer BufEffectType19 = null;
    MeshRenderer BufEffectType20 = null;
    MeshRenderer BufEffectType21 = null;

    private RoleControl _RoleControl;
    public BufControll(RoleControl tRoleControl)
    {
        _RoleControl = tRoleControl;
    }

    public void f_ShowBuf()
    {
        if (_ParentObj != null)
        {
            _ParentObj.SetActive(true);
        }
    }

    public void f_UnShowBuf()
    {
        if (_ParentObj != null)
        {
            _ParentObj.SetActive(false);
        }
    }

    public void f_UpdateBuf(int iBufEffect)
    {
        if (_RoleControl.f_CheckIsDie())
        {
            return;
        }
        if (iBufEffect == -99)
        {
            BufShow(BufEffectType2, false);
            BufShow(BufEffectType3, false);
            BufShow(BufEffectType4, false);
            BufShow(BufEffectType5, false);
            BufShow(BufEffectType6, false);
            BufShow(BufEffectType15, false);
            BufShow(BufEffectType16, false);
            BufShow(BufEffectType17, false);
            BufShow(BufEffectType18, false);
            BufShow(BufEffectType19, false);
            BufShow(BufEffectType20, false);
            BufShow(BufEffectType21, false);
            return;
        }
        //                        pos   type
        //防护罩20001              5      6
        //眩晕20002                4      5
        //治疗20003                3      4
        //中毒20004                2      3
        //灼烧20005	              1       2

        // Trầm mặt 19995	      6     15
        //Mê hoặc 19996	          7     16
        //Lãnh tĩnh 19997	      8     17
        //Giải giới 19998	      9     18
        //Mờ mịt 19999	          10    19
        //Đả kích 19994	          11    20
        //Chảy máu 19993	      12    21
        bool bBufEffectType6 = false;//6
        bool bBufEffectType5 = false;//5
        bool bBufEffectType4 = false;//4
        bool bBufEffectType3 = false; //3
        bool bBufEffectType2 = false; // 2
        bool bBufEffectType15 = false; // 15
        bool bBufEffectType16 = false; // 16
        bool bBufEffectType17 = false; //17
        bool bBufEffectType18 = false; //18
        bool bBufEffectType19 = false; //19
        bool bBufEffectType20 = false; //20
        bool bBufEffectType21 = false; //21
                                           
        MessageBox.DEBUG("Buf " + iBufEffect);
        if (BitTool.BitTest(iBufEffect, 1))
        {
            bBufEffectType2 = true;
            BufEffectType2 = GetBufSpine(20005);
        }
        if (BitTool.BitTest(iBufEffect, 2))
        {
            bBufEffectType3 = true;
            BufEffectType3 = GetBufSpine(20004);
        }
        if (BitTool.BitTest(iBufEffect, 3))
        {
            bBufEffectType4 = true;
            BufEffectType4 = GetBufSpine(20003);
        }
        if (BitTool.BitTest(iBufEffect, 4))
        {
            bBufEffectType5 = true;
            BufEffectType5 = GetBufSpine(20002);
        }
        if (BitTool.BitTest(iBufEffect, 5))
        {
            bBufEffectType6 = true;
            BufEffectType6 = GetBufSpine(20001);
        }

        if (BitTool.BitTest(iBufEffect, 6))
        {
            bBufEffectType15 = true;
            BufEffectType15 = GetBufSpine(19995);
        }
        if (BitTool.BitTest(iBufEffect, 7))
        {
            bBufEffectType16 = true;
            BufEffectType16 = GetBufSpine(19996);
        }
        if (BitTool.BitTest(iBufEffect, 8))
        {
            bBufEffectType17 = true;
            BufEffectType17 = GetBufSpine(19997);
        }
        if (BitTool.BitTest(iBufEffect, 9))
        {
            bBufEffectType18 = true;
            BufEffectType18 = GetBufSpine(19998);
        }
        if (BitTool.BitTest(iBufEffect, 10))
        {
            bBufEffectType19 = true;
            BufEffectType19 = GetBufSpine(19999);
        }
        if (BitTool.BitTest(iBufEffect, 11))
        {
            bBufEffectType20 = true;
            BufEffectType20 = GetBufSpine(19994);
        }
        if (BitTool.BitTest(iBufEffect, 12))
        {
            bBufEffectType21 = true;
            BufEffectType21 = GetBufSpine(19993);
        }

        BufShow(BufEffectType21, bBufEffectType21);//21
        BufShow(BufEffectType20, bBufEffectType20);//20
        BufShow(BufEffectType19, bBufEffectType19);//19
        BufShow(BufEffectType18, bBufEffectType18);//18
        BufShow(BufEffectType17, bBufEffectType17);//17
        BufShow(BufEffectType16, bBufEffectType16);//16
        BufShow(BufEffectType15, bBufEffectType15);//15
        BufShow(BufEffectType6, bBufEffectType6); // 6
        BufShow(BufEffectType5, bBufEffectType5);//5
        BufShow(BufEffectType4, bBufEffectType4);//4
        BufShow(BufEffectType3, bBufEffectType3);//3
        BufShow(BufEffectType2, bBufEffectType2);//2

            //_ParentObj.SetActive(bShow);
        //}
        
    }
    
    public void f_UpdateDepth()
    {
        int iDepth = _RoleControl.f_GetDepth();
        if (BufEffectType21 != null)
        {
            UpdateDepth(BufEffectType21, iDepth);
        }
        if (BufEffectType20 != null)
        {
            UpdateDepth(BufEffectType20, iDepth);
        }
        if (BufEffectType19 != null)
        {
            UpdateDepth(BufEffectType19, iDepth);
        }
        if (BufEffectType18 != null)
        {
            UpdateDepth(BufEffectType18, iDepth);
        }
        if (BufEffectType17 != null)
        {
            UpdateDepth(BufEffectType17, iDepth);
        }
        if (BufEffectType16 != null)
        {
            UpdateDepth(BufEffectType16, iDepth);
        }
        if (BufEffectType15 != null)
        {
            UpdateDepth(BufEffectType15, iDepth);
        }
        if (BufEffectType6 != null)
        {
            UpdateDepth(BufEffectType6, iDepth);
        }
        if (BufEffectType5 != null)
        {
            UpdateDepth(BufEffectType5, iDepth);
        }
        if (BufEffectType4 != null)
        {
            UpdateDepth(BufEffectType4, iDepth);
        }
        if (BufEffectType3 != null)
        {
            UpdateDepth(BufEffectType3, iDepth);
        }
        if (BufEffectType2 != null)
        {
            UpdateDepth(BufEffectType2, iDepth);
        }
    }

    private void UpdateDepth(MeshRenderer tMeshRenderer, int iDepth)
    {
        tMeshRenderer.sortingOrder = iDepth;
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
            oBufSpine = glo_Main.GetInstance().m_ResourceManager.f_CreateMagic(iBufName).transform;
            oBufSpine.parent = _ParentObj.transform;
            oBufSpine.localPosition = new Vector3(0, 0, 0);
            oBufSpine.localScale = Vector3.one;
            oBufSpine.gameObject.layer = 5;
            oBufSpine.name = iBufName.ToString();
            oBufSpine.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "animation", true);
        }
        tMeshRenderer = oBufSpine.GetComponent<MeshRenderer>();
        UpdateDepth(tMeshRenderer, _RoleControl.f_GetDepth());
        
        return tMeshRenderer;
    }
}