using UnityEngine;

public class GameSet
{

    public static void f_ResetALL()
    {
        PlayerPrefs.SetString("ResMD5", "");
        PlayerPrefs.SetString("Ver", "");
        PlayerPrefs.SetInt("iPriority", -99);
        //f_Clear();
    }

    public static void f_Clear()
    {
        PlayerPrefs.SetString("ccName", "");
        PlayerPrefs.SetString("ccPwd", "");
    }

    public static void f_SaveAccountInfor()
    {
        PlayerPrefs.SetString("ccName", StaticValue.m_LoginName);
        PlayerPrefs.SetString("ccPwd", StaticValue.m_LoginPwd);
    }

    public static void f_ReadAccountInfor()
    {
        StaticValue.m_LoginName = PlayerPrefs.GetString("ccName");
        StaticValue.m_LoginPwd = PlayerPrefs.GetString("ccPwd");
    }


    public static void f_LoadGameSet()
    {
        f_ReadAccountInfor();

    }


    public static void f_SaveGameSet()
    {
       

    }





}