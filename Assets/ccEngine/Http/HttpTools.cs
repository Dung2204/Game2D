using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// http下载工具
/// </summary>
public class HttpTools
{

    /// <summary>
    /// 通过http加载资源
    /// </summary>
    /// <param name="strUrl">资源url</param>
    /// <param name="CallBack_LoadSuc">加载成功函数回调</param>
    public static void f_HttpLoad(string strUrl, ccCallback CallBack_LoadSuc)
    {
        ccTaskManager.GetInstance().f_Add(HttpLoading(strUrl, CallBack_LoadSuc), true);
    }

    /// <summary>
    /// 异步访问服务器
    /// </summary>
    /// <param name="strUrl">资源url</param>
    /// <param name="CallBack_LoadSuc">加载成功函数回调</param>
    /// <returns></returns>
    static IEnumerator HttpLoading(string strUrl, ccCallback CallBack_LoadSuc)
    {
        WWW w = new WWW(strUrl);

        while (!w.isDone)
            yield return 1;

        ///HttpDataDT eHttpDataDT = new HttpDataDT();
        byte[] bData = null;
        if (w.error != null)
        {
            MessageBox.DEBUG("网络错误" + w.error.ToString());
        }
        else if (w.text.Length < 4)
        {
            MessageBox.DEBUG("网络错误2 ERO Data Format Ero");
        }
        else
        {
            bData = w.bytes;
        }
        w.Dispose();

        CallBack_LoadSuc(bData);
    }



}