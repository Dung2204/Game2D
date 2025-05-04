using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialTowerPoolDT : BasePoolDT<long>
{

    public bool mPass; //是否通关

    public bool isWin;

    public bool isEnter;

    public int mTempId;
    private TrialTowerDT _Temp;
    public TrialTowerDT mTemp {
        get {
            if (_Temp==null) {
                _Temp= glo_Main.GetInstance().m_SC_Pool.m_TrialTowerSC.f_GetSC(mTempId) as TrialTowerDT;
            }
            return _Temp;
        }
    }
}
