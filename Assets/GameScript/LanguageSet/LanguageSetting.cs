using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
/// <summary>
/// 语言设置
/// </summary>
public class LanguageSetting : MonoBehaviour
{
    public int _TextId;
    public bool replacecode = false;
    public bool replacespace = false;
	public bool capFirst = false;
    private UILabel _Text;
    private static Dictionary<int, LanguageSetting> mTotaltext = new Dictionary<int, LanguageSetting>();
    public enum LanguageType
    {
        Chinese,    //简体
        Traditional,//繁体
    }

    void Start()
    {
        _Text = transform.GetComponent<UILabel>();
        if (_Text == null)
        {
Debug.LogError("Object has no UILabel element");
            return;
        }
        SetText();
        if (!mTotaltext.ContainsKey(_TextId))
        {
            mTotaltext.Add(_TextId, this);
        }
    }

    //根据语言设置文本
    void SetText()
    {
        //通过_TextId 获取文本消息
        if (_TextId <= 0)
        {
Debug.LogError("TextId is not normal");
            return;
        }
        TranslateLanguageDT _TranslateDt = (TranslateLanguageDT)glo_Main.GetInstance().m_SC_Pool.m_TranslateLanguageSC.f_GetSC(_TextId);
        if (_TranslateDt == null)
        {
Debug.LogError("Data null,id:"+ _TextId);
            return;
        }
        //获取文本字符串
        if (glo_Main._Language == 0)//默认中文
        {
            _Text.text = _TranslateDt.szVietnamese;
        }
        else if(glo_Main._Language == 1)//繁体
        {
            _Text.text = _TranslateDt.szEnglish;
        }
        else if (glo_Main._Language == 2)//繁体
        {
            _Text.text = _TranslateDt.szThailand;
        }

        if (replacecode)
        {
            _Text.text = UITool.f_ReplaceName(_Text.text, "#", "\n");
        }
        if (replacespace)
        {
            _Text.text = UITool.f_ReplaceName(_Text.text, " ", "\n");
        }
		if(capFirst)
		{
			_Text.text = char.ToUpper(_Text.text[0]) + _Text.text.Substring(1);
		}
    }
}
