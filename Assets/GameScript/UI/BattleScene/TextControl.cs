using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System;

public class TextControl : MonoBehaviour
{
    public UILabel m_AddHpText;
    public TweenColor m_TextTweenColor;
    public TweenPosition m_TextTweenPosition;
    private bool _IsRole = true;
    private void Awake()
    {
        _IsRole = true;
    }
    public bool IsRole
    {
        set
        {
            _IsRole = value;
        }
    }

    public bool IsPlay
    {
        get;set;
    }

    public int HarmNum { get; set; }

    public void f_Show(string szText, GameObject go = null)
    {
        //显示飘字
        ccCallback showHp = (object obj) =>
        {
            // if (null == gameObject) return;
			try
			{
				if (gameObject == null)
				{
					return;
				}
			}
			catch (Exception e)
			{
				MessageBox.ASSERT(e.Message);
				return;
			}
            gameObject.SetActive(true);
            m_TextTweenColor.ResetToBeginning();
            m_TextTweenColor.PlayForward();
            float remainTime = 1.5f - HarmNum * 0.2f;
            float duration = remainTime > 0.15f ? remainTime : 0.15f;
            m_TextTweenColor.duration = duration;
            if (null != m_TextTweenPosition)
            {
                m_TextTweenPosition.ResetToBeginning();
                m_TextTweenPosition.PlayForward();
                m_TextTweenPosition.duration = duration;
                EventDelegate.Add(m_TextTweenPosition.onFinished, OnFinish);
            }
            else
            {
                EventDelegate.Add(m_TextTweenColor.onFinished, OnFinish);
            }

            if (m_AddHpText != null)
            {
                m_AddHpText.text = szText;
            }
        };

        //延迟显示飘字
        if (HarmNum > 0)
        {
            ccTimeEvent.GetInstance().f_RegEvent(HarmNum * 0.2f, false, null, showHp);
        }
        else
        {
            showHp(null);
        }
    }

    private void OnFinish()
    {
        IsPlay = false;
        gameObject.SetActive(false);
    }
}