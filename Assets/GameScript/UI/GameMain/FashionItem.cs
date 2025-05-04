using UnityEngine;
using System.Collections;

public class FashionItem : MonoBehaviour {

    public GameObject mBtnEquip;//装备
    public GameObject mBtnUnload;//卸下
    public UI2DSprite mIcon;//图标
    public UISprite mSprBorder;//边框
    public UILabel mLabelHasEquip;//已装备
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="tFanshionableDressPoolDT">时装poolDT</param>

    public void SetData(FanshionableDressPoolDT tFanshionableDressPoolDT)
    {
        mIcon.sprite2D = UITool.f_GetIconSprite(tFanshionableDressPoolDT.m_FashionableDressDT.iIcon);
        mSprBorder.spriteName = UITool.f_GetImporentCase(tFanshionableDressPoolDT.m_FashionableDressDT.iImportant);
        TeamPoolDT teamPoolDT = Data_Pool.m_TeamPool.f_GetForId((long)EM_FormationPos.eFormationPos_Main) as TeamPoolDT;
        mLabelHasEquip.gameObject.SetActive(teamPoolDT.m_CardPoolDT.iId == tFanshionableDressPoolDT.m_iCaridId);
    }
}
