using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;
using System;

public class SevenStarAwardList : UIFramwork  {

    private SocketCallbackDT Info=new SocketCallbackDT();

    public void OpenUI() {
        gameObject.SetActive(true);
        InitMessage();
        UITool.f_OpenOrCloseWaitTip(true);
        Info.m_ccCallbackFail = InfoFail;
        Info.m_ccCallbackSuc = InfoSuc;

        Data_Pool.m_SevenStarPool.f_GetAwardList(Info);
    }

    private void InitMessage() {
        f_RegClickEvent("Mask",Close);

    }

    private void Close(GameObject go,object obj1,object obj2) {
        gameObject.SetActive(false);

    }
    private void InfoSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UpdateITem();
    }
    private void InfoFail(object obj) {
        UITool.f_OpenOrCloseWaitTip(false);
    }

    private void UpdateITem() {
        //Data_Pool.m_SevenStarPool.mRecord;
        GridUtil.f_SetGridView<SC_SevenStarInfoNode>(f_GetObject("LabelParent"),f_GetObject("Label"), Data_Pool.m_SevenStarPool.mRecord, ITem);
    }
    private void ITem(GameObject go, SC_SevenStarInfoNode node) {
        DateTime t= ccMath.time_t2DateTime(node.iTime);
        ResourceCommonDT ResCommon = new ResourceCommonDT();
        ResCommon.f_UpdateInfo((byte)node.utype,node.uId,node.uNum);
        string Desc = string.Format("[-][4B2828]{0}/{1}/{6}[-] [00FF20]{2}:{3}:{7}[-], [-][ff8534]{4}[-] [5C3716]receive :[-][FF8258]{5}[-]", t.Day, t.Month, t.Hour, t.Minute, node.szRoleName, ResCommon.mName, t.Year, t.Second);
        go.GetComponent<UILabel>().text=Desc;
        //Desc += node.szRoleName;



    }
}
