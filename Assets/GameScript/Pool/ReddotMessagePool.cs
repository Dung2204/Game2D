using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;

public class ReddotMessagePool
{
    class ReddotGroup
    {
        List<PromptMsgDT> _aTroupData = new List<PromptMsgDT>();
        private GameObject _Obj = null;
        private ccCallback _ReddotCallback_UpdateShow;

        public void f_Save(GameObject Obj, ccCallback ReddotCallback_UpdateShow, PromptMsgDT tPromptMsgDT)
        {
            _Obj = Obj;
            _ReddotCallback_UpdateShow = ReddotCallback_UpdateShow;
            if (!_aTroupData.Contains(tPromptMsgDT))
            {
                _aTroupData.Add(tPromptMsgDT);
            }
        }

        public void f_Del(PromptMsgDT tPromptMsgDT)
        {
            _aTroupData.Remove(tPromptMsgDT);
        }

        public void f_UpdateUI()
        {
            if (_aTroupData.Count == 0)
            {
                return;
            }
            bool bIsOk = false;
            for (int i = 0; i < _aTroupData.Count; i++)
            {
                if (_aTroupData[i].f_Get() > 0)
                {
                    bIsOk = true;                    
                }
            }
            if (bIsOk)
            {
                _ReddotCallback_UpdateShow(99);
            }
            else
            {
                _ReddotCallback_UpdateShow(0);
            }
        }

        public int f_GetPromptMsgDTNum()
        {
            return _aTroupData.Count;
        }

        public GameObject f_GetGroupObj()
        {
            return _Obj;
        }
    }

    class ReddotCallback
    {
        public GameObject m_Obj;
        public ccCallback m_ReddotCallback_UpdateShow;

        public ReddotCallback(GameObject Obj, ccCallback ReddotCallback_UpdateShow)
        {
            m_Obj = Obj;
            m_ReddotCallback_UpdateShow = ReddotCallback_UpdateShow;
        }
    }

    class PromptMsgDT
    {
        private int _iNum;
        List<ReddotCallback> _aData = new List<ReddotCallback>();
        List<ReddotCallback> _aClearData = new List<ReddotCallback>();
        List<ReddotGroup> _aReddotGroup = null;

        public PromptMsgDT()
        {
            f_Reset();
        }

        public void f_Reset()
        {
            _iNum = 0;
            f_UpdateUI();
        }

        public void f_Add()
        {
            _iNum++;
            f_UpdateUI();
        }

        public void f_Subtract()
        {
            if (_iNum > 0)
            {
                _iNum--;
            }
            f_UpdateUI();
        }

        public int f_Get()
        {
            return _iNum;
        }

        public void f_Reg(GameObject Obj, ccCallback ReddotCallback_UpdateShow)
        {
            ReddotCallback tReddotCallback = new ReddotCallback(Obj, ReddotCallback_UpdateShow);
            _aData.Add(tReddotCallback);
        }

        public void f_SaveGroup(ReddotGroup tReddotGroup)
        {
            if (_aReddotGroup == null)
            {
                _aReddotGroup = new List<ReddotGroup>();
            }
            _aReddotGroup.Add(tReddotGroup);
        }

        public void f_UnReg(GameObject Obj)
        {
            ReddotCallback tReddotCallback = _aData.Find(delegate (ReddotCallback p)
            {
                if (p.m_Obj == Obj)
                {
                    return true;
                }
                return false;
            });
            if (tReddotCallback != null)
            {
                _aData.Remove(tReddotCallback);
            }
        }

        public void f_UpdateUI()
        {
            if (_aReddotGroup != null)
            {
                for (int i = 0; i < _aReddotGroup.Count; i++)
                {
                    _aReddotGroup[i].f_UpdateUI();
                }
            }
            for (int i = 0; i < _aData.Count; i++)
            {
                if (_aData[i].m_Obj != null && _aData[i].m_ReddotCallback_UpdateShow != null)
                {
                    if (!CheckObjIsInGroup(_aData[i].m_Obj))                   
                    {
                        _aData[i].m_ReddotCallback_UpdateShow(_iNum);
                    }
                }
                else
                {
                    _aClearData.Add(_aData[i]);
                }
            }

            if (_aClearData.Count > 0)
            {
                for (int i = 0; i < _aClearData.Count; i++)
                {
                    _aData.Remove(_aClearData[i]);
                }
                _aClearData.Clear();
            }

        }

        private bool CheckObjIsInGroup(GameObject Obj)
        {
            if (_aReddotGroup != null)
            {
                for (int i = 0; i < _aReddotGroup.Count; i++)
                {
                    if (_aReddotGroup[i].f_GetGroupObj() == Obj)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
    
    Dictionary<EM_ReddotMsgType, PromptMsgDT> _aData = new Dictionary<EM_ReddotMsgType, PromptMsgDT>();
    Dictionary<GameObject, ReddotGroup> _aGroupData = new Dictionary<GameObject, ReddotGroup>();

    private PromptMsgDT CreateAndGetMsgType(EM_ReddotMsgType tEM_ReddotMsgType)
    {
        PromptMsgDT tPromptMsgDT = null;
        if (!_aData.TryGetValue(tEM_ReddotMsgType, out tPromptMsgDT))
        {
            tPromptMsgDT = new PromptMsgDT();
            _aData.Add(tEM_ReddotMsgType, tPromptMsgDT);
        }
        return tPromptMsgDT;
    }
    /// <summary>
    /// 清除所有红点注册消息（如切换场景）
    /// </summary>
    public void f_Clear()
    {
        _aData.Clear();
        _aGroupData.Clear();
    }
    /// <summary>
    /// 增加红点提示数据
    /// </summary>
    /// <param name="tEM_ReddotMsgType"></param>
    public void f_MsgAdd(EM_ReddotMsgType tEM_ReddotMsgType)
    {
        PromptMsgDT tPromptMsgDT = CreateAndGetMsgType(tEM_ReddotMsgType);
        tPromptMsgDT.f_Add();
    }

    /// <summary>
    /// 减少红点提示数据
    /// </summary>
    /// <param name="tEM_ReddotMsgType"></param>
    public void f_MsgSubtract(EM_ReddotMsgType tEM_ReddotMsgType)
    {
        PromptMsgDT tPromptMsgDT = CreateAndGetMsgType(tEM_ReddotMsgType);
        tPromptMsgDT.f_Subtract();
    }

    /// <summary>
    /// 重置红点提示数据
    /// </summary>
    /// <param name="tEM_ReddotMsgType"></param>
    public void f_MsgReset(EM_ReddotMsgType tEM_ReddotMsgType)
    {
        PromptMsgDT tPromptMsgDT = CreateAndGetMsgType(tEM_ReddotMsgType);
        tPromptMsgDT.f_Reset();
    }
    
    /// <summary>
    /// 注册红点提示信息及UI
    /// </summary>
    /// <param name="tEM_ReddotMsgType">红点提示类型</param>
    /// <param name="Obj">显示红点提示对象</param>
    /// <param name="ReddotCallback_UpdateShow">红点显示更新回调</param>
    /// <param name="bIsGroup">true 存在Group情况 false唯一情况</param>
    public void f_Reg(EM_ReddotMsgType tEM_ReddotMsgType, GameObject Obj, ccCallback ReddotCallback_UpdateShow, bool bIsGroup = false)
    {
        PromptMsgDT tPromptMsgDT = CreateAndGetMsgType(tEM_ReddotMsgType);
        tPromptMsgDT.f_Reg(Obj, ReddotCallback_UpdateShow);

        if (bIsGroup)
        {
            ReddotGroup tReddotGroup = null;
            if (_aGroupData.TryGetValue(Obj, out tReddotGroup))
            {
                tReddotGroup.f_Save(Obj, ReddotCallback_UpdateShow, tPromptMsgDT);
                tPromptMsgDT.f_SaveGroup(tReddotGroup);
            }
            else
            {
                tReddotGroup = new ReddotGroup();
                tReddotGroup.f_Save(Obj, ReddotCallback_UpdateShow, tPromptMsgDT);
                tPromptMsgDT.f_SaveGroup(tReddotGroup);
                _aGroupData.Add(Obj, tReddotGroup);                
            }
        }
    }

    /// <summary>
    /// 注销红点提示信息
    /// </summary>
    /// <param name="tEM_ReddotMsgType"></param>
    /// <param name="Obj"></param>
    /// <param name="bIsGroup"></param>
    public void f_UnReg(EM_ReddotMsgType tEM_ReddotMsgType, GameObject Obj, bool bIsGroup = false)
    {
        PromptMsgDT tPromptMsgDT = null;
        if (_aData.TryGetValue(tEM_ReddotMsgType, out tPromptMsgDT))
        {
            tPromptMsgDT.f_UnReg(Obj);
        }
        if (bIsGroup)
        {//回收Group资源
            ReddotGroup tReddotGroup = null;
            if (_aGroupData.TryGetValue(Obj, out tReddotGroup))
            {
                tReddotGroup.f_Del(tPromptMsgDT);
            }
            if (tReddotGroup.f_GetPromptMsgDTNum() == 0)
            {
                _aGroupData.Remove(Obj);
            }
        }
    }

    /// <summary>
    /// 强制更新某个EM_ReddotMsgType显示
    /// </summary>
    /// <param name="tEM_ReddotMsgType"></param>
    public void f_ForceUpdateUI(EM_ReddotMsgType tEM_ReddotMsgType)
    {
        PromptMsgDT tPromptMsgDT = null;
        if (_aData.TryGetValue(tEM_ReddotMsgType, out tPromptMsgDT))
        {
            tPromptMsgDT.f_UpdateUI();
        }
    }
}