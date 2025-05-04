using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
/// <summary>
/// 本地数据保存和获取工具
/// </summary>
public class LocalDataManager
{
    /// <summary>
    /// 本地数据前缀
    /// </summary>
    private static readonly string localDataStr = "localdata";
    #region 设置本地数据 设置值
    /// <summary>
    /// 设置本地数据 设置值
    /// </summary>
    /// <typeparam name="T">数据类型（int,float,string,bool等基本数据类型，或由基本数据类型封装的类）</typeparam>
    /// <param name="dataType">数据名字</param>
    /// <param name="value">数据值</param>
    public static void f_SetLocalData<T>(LocalDataType dataType, T value , string extraParam = "")
    {
        Type type = typeof(T);
        string key = localDataStr + dataType.ToString() + extraParam;
        if (type.Equals(typeof(int)))
        {
            PlayerPrefs.SetInt(key, (int)Convert.ChangeType(value, typeof(int)));
        }
        else if (type.Equals(typeof(float)))
        {
            PlayerPrefs.SetFloat(key, (float)Convert.ChangeType(value, typeof(float)));
        }
        else if (type.Equals(typeof(string)))
        {
            PlayerPrefs.SetString(key, (string)Convert.ChangeType(value, typeof(string)));
        }
        else if (type.Equals(typeof(bool)))
        {
            int data = ((bool)Convert.ChangeType(value, typeof(bool))) ? 1 : 0;
            PlayerPrefs.SetInt(key, (int)Convert.ChangeType(data, typeof(int)));
        }
        else
        {
            try
            {
                string data = (string)Convert.ChangeType(value, typeof(string));
                PlayerPrefs.SetString(key, data);
            }
            catch (Exception ex1)
            {
                try
                {
                    string data = GetEntityToString<T>(ref value);
                    PlayerPrefs.SetString(key, data);
                }
                catch (Exception ex2)
                {
Debug.LogError("Conversion failed，please use type 'int'、 'float'、'string'、'bool'.");
                    Debug.LogError(ex1.Message);
                    Debug.LogError(ex2.Message);
                }
            }
        }
        PlayerPrefs.Save();
    }
    #endregion
    #region  如果本地数据中不存在该数据，则设置该数据
    /// <summary>
    /// 如果本地数据中不存在该数据，则设置该数据。
    /// 返回是否设置了该数据
    /// </summary>
    /// <typeparam name="T">数据类型（int,float,string,bool等基本数据类型，或由基本数据类型封装的类）</typeparam>
    /// <param name="dataType">数据名字</param>
    /// <param name="value">数据值</param>
    /// <returns>是否设置了该数据</returns>
    public static bool f_SetLocalDataIfDataNotExist<T>(LocalDataType dataType, T value)
    {
        if (!f_HasLocalData(dataType))
        {
            f_SetLocalData<T>(dataType, value);
            return true;
        }
        return false;
    }
    #endregion
    #region 检查本地数据中是否已经有该数据
    /// <summary>
    /// 检查本地数据中是否已经有该数据
    /// </summary>
    /// <param name="dataType">数据名字</param>
    /// <returns>是否已经有该数据</returns>
    public static bool f_HasLocalData(LocalDataType dataType)
    {
        string key = localDataStr + dataType.ToString();
        return PlayerPrefs.HasKey(key);
    }
    #endregion
    #region 从本地数据中删除该数据值
    /// <summary>
    /// 从本地数据中删除该数据值
    /// </summary>
    /// <param name="dataType">数据名字</param>
    public static void f_DeleteLocalData(LocalDataType dataType)
    {
        string key = localDataStr + dataType.ToString();
        PlayerPrefs.DeleteKey(key);
    }
    #endregion
    #region 删除所有本地数据
    /// <summary>
    /// 删除所有本地数据
    /// </summary>
    public static void f_DeleteAllLocalData()
    {
        PlayerPrefs.DeleteAll();
    }
    #endregion
    #region 获取本地数据中某个数据值 如果没有则设置并返回该值
    /// <summary>
    /// 获取本地数据中某个数据值
    /// 如果没有则设置并返回该值
    /// </summary>
    /// <typeparam name="T">数据类型（int,float,string,bool等基本数据类型，或由基本数据类型封装的类）</typeparam>
    /// <param name="LocalDataType">数据名字</param>
    /// <param name="defaultValue">如果没有key则设置并返回该值</param>
    /// <returns>数据中该数据值</returns>
    public static T f_GetLocalDataIfNotExitSetData<T>(LocalDataType dataType, T defaultValue)
    {
        T returnValue = defaultValue;
        string key = localDataStr + dataType.ToString();
        if (PlayerPrefs.HasKey(key))
        {
            Type type = typeof(T);
            if (type.Equals(typeof(int)))
            {
                returnValue = (T)Convert.ChangeType(PlayerPrefs.GetInt(key), type);
            }
            else if (type.Equals(typeof(float)))
            {
                returnValue = (T)Convert.ChangeType(PlayerPrefs.GetFloat(key), type);
            }
            else if (type.Equals(typeof(string)))
            {
                returnValue = (T)Convert.ChangeType(PlayerPrefs.GetString(key), type);
            }
            else if (type.Equals(typeof(bool)))
            {
                bool value = PlayerPrefs.GetInt(key) == 1 ? true : false;
                returnValue = (T)Convert.ChangeType(value, type);
            }
            else
            {
                try
                {
                    returnValue = (T)Convert.ChangeType(PlayerPrefs.GetString(key), type);
                }
                catch (Exception ex1)
                {
                    try
                    {
                        returnValue = GetEntityStringToEntity<T>(PlayerPrefs.GetString(key));
                    }
                    catch (Exception ex2)
                    {
Debug.LogError("Conversion failed，please use type 'int'、 'float'、'string'、'bool'.");
Debug.LogError("The variable name cannot match its class.");
                        Debug.LogError(ex1.Message);
                        Debug.LogError(ex2.Message);
                    }
                }
            }
        }
        else//没有则设置该值
        {
            f_SetLocalData<T>(dataType, defaultValue);
        }
        return returnValue;
    }
    #endregion
    #region 获取本地数据中某个数据值
    /// <summary>
    /// 获取本地数据中某个数据值
    /// </summary>
    /// <typeparam name="T">数据类型（int,float,string,bool等基本数据类型，或由基本数据类型封装的类）</typeparam>
    /// <param name="dataType">数据名字</param>
    /// <returns>数据中该数据值</returns>
    public static T f_GetLocalData<T>(LocalDataType dataType, string extraParam ="")
    {
        T returnValue = default(T);
        string key = localDataStr + dataType.ToString()+ extraParam;
        if (PlayerPrefs.HasKey(key))
        {
            Type type = typeof(T);
            if (type.Equals(typeof(int)))
            {
                returnValue = (T)Convert.ChangeType(PlayerPrefs.GetInt(key), type);
            }
            else if (type.Equals(typeof(float)))
            {
                returnValue = (T)Convert.ChangeType(PlayerPrefs.GetFloat(key), type);
            }
            else if (type.Equals(typeof(string)))
            {
                returnValue = (T)Convert.ChangeType(PlayerPrefs.GetString(key), type);
            }
            else if (type.Equals(typeof(bool)))
            {
                bool value = PlayerPrefs.GetInt(key) == 1 ? true : false;
                returnValue = (T)Convert.ChangeType(value, type);
            }
            else
            {
                try
                {
                    returnValue = (T)Convert.ChangeType(PlayerPrefs.GetString(key), type);
                }
                catch (Exception ex1)
                {
                    try
                    {
                        returnValue = GetEntityStringToEntity<T>(PlayerPrefs.GetString(key));
                    }
                    catch (Exception ex2)
                    {
Debug.LogError("Conversion failed，please use type 'int'、 'float'、'string'、'bool'等基本数据类型保存本地数据.");
Debug.LogError("The variable name cannot match its class.");
                        Debug.LogError(ex1.Message);
                        Debug.LogError(ex2.Message);
                    }
                }
            }
        }
        else
        {
Debug.LogError("Getting data failed，Please use 'HasLocalData' to check the data and use 'SetLocalData' to initialize the data!");
Debug.LogError("Use 'SetLocalDataIfDataNotExist' to check if data exists and initialize quickly!");
        }
        return returnValue;
    }
    #endregion
    #region 将实体类通过反射组装成字符串
    /// <summary>
    /// 将实体类通过反射组装成字符串
    /// </summary>
    /// <param name="t">实体类</param>
    /// <returns>组装的字符串</returns>
    private static string GetEntityToString<T>(ref T t)
    {
        System.Text.StringBuilder sb = new StringBuilder();
        Type type = t.GetType();
        System.Reflection.PropertyInfo[] propertyInfos = type.GetProperties();
        for (int i = 0; i < propertyInfos.Length; i++)
        {
            sb.Append(propertyInfos[i].Name + ":" + propertyInfos[i].GetValue(t, null) + ",");
        }
        System.Reflection.FieldInfo[] fieldInfos = type.GetFields();
        for (int j = 0; j < fieldInfos.Length; j++)
        {
            sb.Append(fieldInfos[j].Name + ":" + fieldInfos[j].GetValue(t) + ",");
        }
        return sb.ToString().TrimEnd(new char[] { ',' });
    }
    #endregion
    #region 将反射得到字符串转换为对象
    /// <summary>
    /// 将反射得到字符串转换为对象
    /// </summary>
    /// <param name="str">反射得到的字符串</param>
    /// <returns>实体类</returns>
    private static T GetEntityStringToEntity<T>(string str)
    {
        string[] array = str.Split(',');
        string[] temp = null;
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        foreach (string s in array)
        {
            temp = s.Split(':');
            dictionary.Add(temp[0], temp[1]);
        }
        System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(T));
        T entry = (T)assembly.CreateInstance(typeof(T).FullName);
        //System.Text.StringBuilder sb = new StringBuilder();
        Type type = entry.GetType();
        System.Reflection.PropertyInfo[] propertyInfos = type.GetProperties();
        for (int i = 0; i < propertyInfos.Length; i++)
        {
            foreach (string key in dictionary.Keys)
            {
                if (propertyInfos[i].Name == key.ToString())
                {
                    propertyInfos[i].SetValue(entry, GetObject(propertyInfos[i].PropertyType, dictionary[key]), null);
                    break;
                }
            }
        }
        System.Reflection.FieldInfo[] fieldInfos = type.GetFields();
        for (int j = 0; j < fieldInfos.Length; j++)
        {
            foreach (string key in dictionary.Keys)
            {
                if (fieldInfos[j].Name == key.ToString())
                {
                    fieldInfos[j].SetValue(entry, GetObject(fieldInfos[j].FieldType, dictionary[key]));
                    break;
                }
            }
        }
        return entry;
    }
    /// <summary>
    /// 将字符串转换值的类型
    /// </summary>
    /// <param name="p"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private static object GetObject(Type type, string value)
    {
        try
        {
            return Convert.ChangeType(value, type);
        }
        catch (Exception ex)
        {
Debug.LogError("Data conversion failed，Please use type 'int'、 'float'、'string'、'bool'.");
            Debug.LogError(ex.Message);
            return default(Type);
        }
    }
    #endregion
}
