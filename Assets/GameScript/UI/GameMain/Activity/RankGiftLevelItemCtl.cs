using UnityEngine;
using System.Collections;
/// <summary>
/// 滑动界面item控制
/// </summary>
public class RankGiftLevelItemCtl : MonoBehaviour {
    public UILabel LabelLevel;
    public UILabel LabelFunc;//提示功能信息
    public GameObject m_BtnItem;
    public GameObject m_BtnGoto;//前往
    public UILabel LabelTitle;//标题
    public UISprite SprIcon;//图标
    public UILabel LabelLvLess;//等级不足
    public UISprite SprReddot;//红点提示
    public GameObject m_ObjLvMask;
    public GameObject mObjSelectedFlag;
    public void SetData(int Level,string szFunc,string szTitle,string iconSpriteName,bool isGetLv,bool isRedPoint)
    {
        LabelLevel.text = string.Format(CommonTools.f_GetTransLanguage(1470), Level);
        LabelFunc.text = szFunc;
        LabelTitle.text = szTitle;
        SprIcon.spriteName = iconSpriteName;
        LabelLvLess.gameObject.SetActive(!isGetLv);
        m_BtnGoto.SetActive(isGetLv);
        SprReddot.gameObject.SetActive(isRedPoint);
    }
    public void ShowRedPoint(bool isRedPoint)
    {
        SprReddot.gameObject.SetActive(isRedPoint);
    }
}
