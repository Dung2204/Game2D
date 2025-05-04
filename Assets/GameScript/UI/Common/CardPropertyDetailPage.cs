using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 武将详细属性加成界面
/// </summary>
public class CardPropertyDetailPage : UIFramwork {
    /// <summary>
    /// 页面开启
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        RoleProperty roleProperty = e as RoleProperty;
        if (roleProperty == null)
MessageBox.ASSERT("Parameter error：RoleProperty");
        InitData(roleProperty);
    }
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="rolePropperTy"></param>
    private void InitData(RoleProperty rolePropperTy)
    {
        for (int i = 1; i < (int)EM_RoleProperty.IgDefCR; i++)//暂时只显示到20
        {
            if(i == (int)(EM_RoleProperty.Atk)|| i == (int)(EM_RoleProperty.Def) || i == (int)(EM_RoleProperty.MDef)|| i == (int)(EM_RoleProperty.Anger))
            {
                f_GetObject("Proper" + i).transform.Find("Value").GetComponent<UILabel>().text = rolePropperTy.f_GetProperty(i).ToString();
                f_GetObject("Proper" + i).transform.GetComponent<UILabel>().text = UITool.f_GetProName((EM_RoleProperty)i);
            }

            else if(i == (int)EM_RoleProperty.Hp)
            {
                f_GetObject("Proper" + i).transform.Find("Value").GetComponent<UILabel>().text = rolePropperTy.f_GetHp().ToString();
                f_GetObject("Proper" + i).transform.GetComponent<UILabel>().text = UITool.f_GetProName((EM_RoleProperty)i);
            }
            else
            {
                f_GetObject("Proper" + i).transform.Find("Value").GetComponent<UILabel>().text = CommonTools.f_GetPercentValueTenThousandStr(rolePropperTy.f_GetProperty(i));
                f_GetObject("Proper" + i).transform.GetComponent<UILabel>().text = UITool.f_GetProName((EM_RoleProperty)i);
            }
        }
    }
    /// <summary>
    /// 初始化消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("CloseMask", OnBlackClick);
    }
    #region 按钮事件
    /// <summary>
    /// 点击黑色背景关闭页面
    /// </summary>
    /// <param name="go"></param>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    private void OnBlackClick(GameObject go,object obj1,object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CardPropertyDetailPage, UIMessageDef.UI_CLOSE);
    }
    #endregion
}
