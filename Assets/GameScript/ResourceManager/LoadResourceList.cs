using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class LoadResourceList
{

    List<ResourceDT> _aList = new List<ResourceDT>();

    public void f_Add(ResourceDT tResourceDT)
    {
        //if (!CheckIsHave(tResourceDT))
        //{
        //    _aList.Add(tResourceDT);
        //}
    }

    public void f_Clear()
    {
        _aList.Clear();
    }

    public void f_Export()
    {
        string _strLogPath = Application.dataPath + "/GuidanceResourceList.txt";
        if (System.IO.File.Exists(_strLogPath))
        {
            File.Delete(_strLogPath);
        }
        foreach (ResourceDT t in _aList)
        {
            using (StreamWriter writer = new StreamWriter(_strLogPath, true, Encoding.UTF8))
            {
                writer.WriteLine(_aList.IndexOf(t) + ": iType = " + ",strPath = " + t.strPath);
            }
        }
MessageBox.DEBUG("ResourceList exported。");
        _aList.Clear();
    }

    private bool CheckIsHave(ResourceDT tResourceDT)
    {
        ResourceDT __ResourceDT = _aList.Find(delegate (ResourceDT tItem)
                                                {
                                                    return tItem == tResourceDT ? true : false;
                                                }
        
                );
        if (__ResourceDT == null)
        {
            return false;
        }
        return true;
    }

}
