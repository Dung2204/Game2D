using System.Collections.Generic;

public class CardArtifactPoolDT
{
    private ArtifactDT m_Template = null;

    /// <summary>
    /// 属性列表
    /// </summary>
    public ArtifactDT Template
    {
        get
        {
            return m_Template;
        }
    }

    private int m_Lv;

    /// <summary>
    /// 神器等级
    /// </summary>
    public int Lv
    {
        get
        {
            return m_Lv;
        }
    } 

    public CardArtifactPoolDT()
    {
        m_Lv = 0;
        m_Template = null;
    }

    public void f_UpdateValue(int lv)
    {
        m_Lv = lv;
        m_Template = (ArtifactDT)glo_Main.GetInstance().m_SC_Pool.m_ArtifactSC.f_GetSC(lv);
        if (lv != 0 && m_Template == null)
        {
            MessageBox.ASSERT(string.Format("ArtifactDT exit null dt. id:{0}", lv));
        }
    }
}
