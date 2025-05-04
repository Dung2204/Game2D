using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 抽象池类
/// </summary>
public abstract class BasePool : ccBasePool<long>
{
    public BasePool(string strRegDTName, bool bUseDirc = false):base(strRegDTName, bUseDirc)
    {
        f_Init();
        RegSocketMessage();
    }

    /// <summary>
    /// 初始化方法
    /// </summary>
    protected abstract void f_Init();
    protected abstract void RegSocketMessage();

    protected void Callback_SocketData_Update(int iData1, int iData2, int iNum, ArrayList aData)
    {
        foreach (SockBaseDT tData in aData)
        {
            if (iData1 == (int)eUpdateNodeType.node_add)
            {
                f_Socket_AddData(tData, true);
            }
            else if (iData1 == (int)eUpdateNodeType.node_update)
            {
                f_Socket_UpdateData(tData);
            }
            else if (iData1 == (int)eUpdateNodeType.node_default)
            {
                f_Socket_AddData(tData, false);
            }
            //else if (iData1 == (int)eUpdateNodeType.node_delete)
            //{
            //    f_Socket_DelData(tData);
            //}
        }
    }
    protected abstract void f_Socket_AddData(SockBaseDT Obj, bool bNew);
    protected abstract void f_Socket_UpdateData(SockBaseDT Obj);
    //protected abstract void f_Socket_DelData(SockBaseDT Obj);

}