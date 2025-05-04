using UnityEngine;
using System.Collections;
/// <summary>
/// 阵图系统item控制
/// </summary>
public class BattleFormItemCtl : UIFramwork {
    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(BattleFormationsDT DataDt,bool isSelect,bool isGet)
    {
        f_GetObject("LabelName").GetComponent<UILabel>().text = DataDt.szTypeName;
        transform.Find("BgSelectEffect").gameObject.SetActive(isSelect);
        f_GetObject("isSelect").SetActive(isSelect);
        //gameObject.GetComponent<UI2DSprite>().color = isGet ? Color.white : new Color(0, 0.5f, 0.5f);

        //gameObject.GetComponent<UI2DSprite>().sprite2D=UITool.f_GetIconSprite(int.Parse(DataDt.szIconID));
        gameObject.GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(DataDt.szIconID);
        gameObject.GetComponent<UITexture>().color = isGet ? Color.white : new Color(0, 0.5f, 0.5f);


        //f_GetObject("Bg").GetComponent<UISprite>().spriteName = isSelect ? "jx_sheet_press" : "jx_sheet_normal";

        //if (isGet && transform.Find("Particle").childCount == 0)
        //{
        //    UITool.f_CreateEffect_Old(UIEffectName.ztdl_guangquan_01, transform.Find("Particle"), Vector3.zero, 0.2f, 0f, UIEffectName.UIEffectAddress2);
        //}
    }
}
