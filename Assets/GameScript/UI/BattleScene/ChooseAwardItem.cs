using UnityEngine;
using System.Collections;
/// <summary>
/// 抽牌界面选择item脚本
/// </summary>
public class ChooseAwardItem : MonoBehaviour {
    public UI2DSprite mIcon;//图标
    public UILabel mName;//名称
    public UILabel mNum;//数量
    public UISprite mBorder;//边框
    public GameObject mBtnBox;//box
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="resourceCommonDT">资源通用dt</param>
    public void SetData(ResourceCommonDT resourceCommonDT)
    {
        mIcon.sprite2D = UITool.f_GetIconSprite(resourceCommonDT.mIcon);
        mNum.text = resourceCommonDT.mResourceNum.ToString();
        string name = resourceCommonDT.mName;
        mBorder.spriteName = UITool.f_GetImporentColorName(resourceCommonDT.mImportant, ref name);
        mName.text = name;
    }
}
