using UnityEngine;
using System.Collections;
/// <summary>
/// 军团技能item
/// </summary>
public class LegionSkillItem : MonoBehaviour {
    public UISprite m_Icon;//技能图标
    public UISprite m_IconBorder;//技能边框
    public UILabel m_LabelName;//技能名称
    public UILabel m_LabelLevel;//技能等级
    public UILabel m_LabelProNow;//当前属性
    public UILabel m_LabelProNext;//下级属性
    public UILabel m_LabelWaster;//消耗提示
    public GameObject m_btnOpen;//开启技能按钮

    public UIGrid m_BtnUIGrid;//按钮Grid
    public GameObject m_btnLearn;//学习技能按钮
    public GameObject m_btnUpLevel;//升级技能按钮
    public GameObject m_btnUpgrade;//提升上限按钮
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="skillName">技能名称</param>
    /// <param name="Level">我的技能等级</param>
    /// <param name="addProNow">当前增加的属性</param>
    /// <param name="addProNext">下级增加的属性</param>
    /// <param name="wasterHint">消耗提示</param>
    /// <param name="skillState">技能升级状态</param>
    /// <param name="isDeputy">是否为军团长或副军团长</param>
    public void SetDataInfo(string skillName,int icon,string Level,string addProNow,string addProNext,string wasterHint,EM_LegionSkillState skillState,bool isDeputy,bool skillUpMax)
    {
        EM_Important skillImportant = EM_Important.Red;
        if (icon >= (int)EM_Important.White && icon <= (int)EM_Important.Red)
            skillImportant = (EM_Important)icon;
        m_IconBorder.spriteName = UITool.f_GetImporentColorName((int)skillImportant, ref skillName);

        m_Icon.spriteName = icon.ToString();
        m_LabelName.text = skillName;
        m_LabelLevel.text = Level;
        m_LabelProNow.text = addProNow;
        m_LabelProNext.text = addProNext;
        m_LabelWaster.text = wasterHint;

        m_btnOpen.SetActive(skillState == EM_LegionSkillState.NotOpen);
        m_btnLearn.SetActive(skillState == EM_LegionSkillState.NotLearn);
        m_btnUpLevel.SetActive(skillState == EM_LegionSkillState.UpGrade);
        m_btnUpgrade.SetActive(!skillUpMax && isDeputy && skillState != EM_LegionSkillState.NotOpen);
        m_BtnUIGrid.Reposition();
    }
}
