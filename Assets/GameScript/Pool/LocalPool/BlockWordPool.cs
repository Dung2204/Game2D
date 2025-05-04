using UnityEngine;
using System.Collections.Generic;

public class BlockWordPool
{
    private BetterList<string> m_List;

    public BlockWordPool()
    {
        f_Init();
    }

    private void f_Init()
    {
        byte[] _aBuffer = (Resources.Load("Pingbici", typeof(TextAsset)) as TextAsset).bytes;
        string content = System.Text.Encoding.UTF8.GetString(_aBuffer);
        string[] contentArr = ccMath.f_String2ArrayString(content, ",");
        if (m_List == null)
        {
            m_List = new BetterList<string>();
        }
        for (int i = 0; i < contentArr.Length; i++)
        {
			contentArr[i] = contentArr[i].ToUpper();
            m_List.Add(contentArr[i]);
        }
    }

    /// <summary>
    /// 检查是否敏感内容
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public bool f_CheckValidity(ref string str)
    {
        bool result = true;
        while (!f_CheckStep(ref str))
        {
            result = false;
        }
        return result;
    }

    private bool f_CheckStep(ref string str)
    {
		string newstr = str.ToUpper();
        for (int i = 0; i < m_List.size; i++)
        {
            if (string.IsNullOrEmpty(m_List[i])) continue;
            if (newstr.Contains(m_List[i]) && !m_List[i].Equals("N") && !m_List[i].Equals("T") && !m_List[i].Equals("D"))
            {
                str = "********";
                return false;
            }
        }
        return true;
    }

}
