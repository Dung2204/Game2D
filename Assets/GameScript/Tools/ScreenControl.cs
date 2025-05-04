using UnityEngine;
using System.Collections;

public class ScreenControl : Singleton<ScreenControl>
{
    private float m_scaleRatio;
    private float m_functionRatio;

    private float m_backGroundRotio;
    private float m_heghtRatio;
    private float m_widthRatio;
    /// <summary>
    /// 获取背景宽高比
    /// </summary>
    public float mScaleRatio
    {
        get { return m_backGroundRotio; }
    }

    /// <summary>
    /// 功能界面缩放比例,以宽高比较小那一个为准
    /// </summary>
    public float mFunctionRatio
    {
        get { return m_functionRatio; }
    }

    public float mWidthRatio
    {
        get { return m_widthRatio; }
    }

    public float mHeghtRatio
    {
        get { return m_heghtRatio; }
    }

    public void f_InitScreenDesign(float DesignScreenWidth, float DesignScreenHeight)
    {
MessageBox.DEBUG("Set screen size：" + Screen.width + ":" + Screen.height);
        float m_screenAspectRatio = (float)Screen.width / (float)Screen.height;
        float designAspect = DesignScreenWidth / DesignScreenHeight;

        m_scaleRatio = m_screenAspectRatio / designAspect;

        if (m_screenAspectRatio < designAspect)
        {
            m_functionRatio = m_scaleRatio;
            //m_functionRatio = Mathf.Lerp(m_functionRatio, 1f, 0.2f);
            m_backGroundRotio = 1;
        }
        else
        {
            m_functionRatio = 1;
            m_backGroundRotio = m_scaleRatio;
            //m_backGroundRotio = Mathf.Lerp(m_backGroundRotio, 1f, 0.2f);
        }
        m_widthRatio = (float)Screen.width / DesignScreenWidth;
        m_heghtRatio = (float)Screen.height / DesignScreenHeight;

MessageBox.DEBUG("Set functionRatio：" + mFunctionRatio);
    }


}
