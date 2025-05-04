using UnityEngine;
using System.Collections;

public class ActBtnItemCtl : MonoBehaviour {
    public UILabel m_LabelActName;//活动名称
    public void SetData(string actName)
    {
        m_LabelActName.text = actName;
    }
}
