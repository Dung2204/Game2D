using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;
using UnityEngine;
public class GuidanceManage : MonoBehaviour
{
    public GameObject Btn_Esc;
    public ccMachineManager tManage = null;
    private Dictionary<int, GuidanceArrTextParam> TextPos;

    private Transform _OffsetVector3;

    public string[] szCheckUi;
    private void Start()
    {
        Btn_Esc.SetActive(false);
        //ccTimeEvent.GetInstance().f_RegEvent(5f, false, null, (object obj1) => { Btn_Esc.SetActive(true); });
        UIEventListener.Get(Btn_Esc).onClick = _BtnEsc;
    }


    /// <summary>ank
    /// 创建一个新手引导的流程    ProcessNum==步骤数   必含Star End    传一个DT
    /// DT :  步骤数  BtnName  iid  指向方向  是否在光环  冒泡文字
    /// </summary>
    public void f_Create()
    {

        szCheckUi = "PopupMenuPage;TopPopupMenuPage;GameNoticePage".Split(';');


        tManage = new ccMachineManager(new GuidanceStar(this));
        tManage.f_RegState(new GuidanceProcess(this));
        tManage.f_RegState(new GuidanceEnd(this));
        tManage.f_RegState(new GuidanceDialogRead());
        tManage.f_RegState(new GuidanceDialogPlay());
        tManage.f_RegState(new GuidanceDialogEnd());
        tManage.f_RegState(new GuidancePass(szCheckUi));

        if(TextPos == null)
        {
            TextPos = new Dictionary<int, GuidanceArrTextParam>();                                    //           1 
            TextPos.Add(1, new GuidanceArrTextParam(new Vector3(0, -47), new Vector3(-595, 0), new Vector3(-297, -86), Vector3.zero));    //       2   l  1
            TextPos.Add(2, new GuidanceArrTextParam(new Vector3(0, -47), new Vector3(599, 0), new Vector3(297, -86), new Vector3(0, 180)));    //      -----+-----
            TextPos.Add(3, new GuidanceArrTextParam(new Vector3(0, 187), new Vector3(599, 0), new Vector3(301, -86), new Vector3(0, 180)));    //       3   l  4
            TextPos.Add(4, new GuidanceArrTextParam(new Vector3(0, 187), new Vector3(-554, 0), new Vector3(-272, -86), Vector3.zero));    //           l
        }
    }



    /// <summary>
    /// 创建箭头
    /// </summary>
    /// <param name="go">箭头的父级</param>
    /// <param name="ArrDirection">箭头指向</param>
    /// <param name="IsAura">是否显示光环</param>
    public GameObject f_CreateArr(GameObject go, Vector2 Pos, int ArrDirection, bool ArrShow, bool IsAura, ref UIScrollView Scrollview, string Text = "")
    {
        GameObject tArr = Data_Pool.m_GuidancePool.m_GuidanceArr;

        //if (!ArrShow)
        //{
        //    return tArr;
        //}
        int quadrant = 0;
        Vector3 tScreenPoint = UICamera.mainCamera.WorldToScreenPoint(go.transform.position);
        _OffsetVector3 = go.transform;

        Transform Parent = go.transform;
        for(int i = 0; i < 5; i++)
        {
            Scrollview = Parent.GetComponent<UIScrollView>();
            if(Scrollview == null)
            {
                Parent = Parent.parent;
            }
            else
            {
                break;
            }
        }
        if(Scrollview != null)
            Scrollview.enabled = false;
        float Vector3X = tScreenPoint.x - Screen.width / 2;
        float Vector3Y = tScreenPoint.y - Screen.height / 2;
        if(Vector3X > 0 && Vector3Y > 0)
            quadrant = 1;
        else if(Vector3X < 0 && Vector3Y > 0)
            quadrant = 2;
        else if(Vector3X < 0 && Vector3Y < 0)
            quadrant = 3;
        else if(Vector3X > 0 && Vector3Y < 0)
            quadrant = 4;
        //tArr.transform.position = UICamera.mainCamera.ScreenToWorldPoint(tScreenPoint);
        //tArr.transform.SetParent(go.transform);
        switch(ArrDirection)
        {
            case 1:
                tArr.transform.localRotation = Quaternion.Euler(Vector3.zero);
                break;
            case 2:
                tArr.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
                break;
        }
        Transform TextObject = tArr.transform.Find("Text");
        TextObject.gameObject.SetActive(Text != "");
        if(Text != "")
        {
            tArr.transform.Find("Text/Texture/text").GetComponent<UILabel>().text = Text;
            if(quadrant != 0)
            {
                MessageBox.DEBUG("  Cung phần tư      " + quadrant);
                GuidanceArrTextParam tTextPos = TextPos[quadrant];

                TextObject.localPosition = tTextPos.MainPos;
                TextObject.Find("Texture").localPosition = tTextPos.TextPos;
                TextObject.Find("Girl").localPosition = tTextPos.GirlPos;
                TextObject.Find("Girl").localEulerAngles = tTextPos.GirlAngle;
            }
        }
        tArr.transform.localScale = Vector3.one;
        Transform arrTf = tArr.transform.Find("Arr").transform;
        Vector3 arr = new Vector3(Pos.x, Pos.y, 0);
        arrTf.localPosition = arr;

        tArr.transform.Find("BoxUp").localPosition = new Vector3(arr.x, arr.y + 590, 0); 
        tArr.transform.Find("BoxDown").localPosition = new Vector3(arr.x, arr.y - 694, 0);
        tArr.transform.Find("BoxLeft").localPosition = new Vector3(arr.x - 1010, arr.y, 0);
        tArr.transform.Find("BoxRight").localPosition = new Vector3(arr.x + 1098, arr.y, 0);

        Transform Particle = tArr.transform.Find("Arr/Particle");
        //Particle.localPosition = new Vector3(9 + Pos.x, -7 + Pos.y, 0);
        Particle.gameObject.SetActive(true);
        Data_Pool.m_GuidancePool.m_GuidanceArr.transform.position = go.transform.position;
        tArr.SetActive(ArrShow);
        //Vector3 tScreenPoint = UICamera.mainCamera.WorldToScreenPoint(_OffsetVector3.position);

        return tArr;
    }
    private float time = 0;
    private void FixedUpdate()
    {
        if(Data_Pool.m_GuidancePool.IsEnter)
        {
            tManage.f_Update();
            //if(_OffsetVector3 != null)
            //{
            //    if(_OffsetVector3.position != Data_Pool.m_GuidancePool.m_GuidanceArr.transform.position)
            //    {
            //        Vector3 tScreenPoint = UICamera.mainCamera.WorldToScreenPoint(_OffsetVector3.position);
            //        Data_Pool.m_GuidancePool.m_GuidanceArr.transform.position = UICamera.mainCamera.ScreenToWorldPoint(tScreenPoint);
            //    }
            //}
            if(time > 10)
            {
                Btn_Esc.SetActive(true);
            }
            else
            {
                time += Time.deltaTime;
            }
        }
    }
    /// <summary>
    /// 跳过当前
    /// </summary>
    private void _BtnEsc(GameObject go)
    {
        tManage.f_ChangeState((int)EM_Guidance.GuidancePass);
        PopupMenuParams tPopupMenuParams = new PopupMenuParams(CommonTools.f_GetTransLanguage(2119), CommonTools.f_GetTransLanguage(2120), CommonTools.f_GetTransLanguage(2121), _BtnEscSuc, CommonTools.f_GetTransLanguage(2122));

        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tPopupMenuParams);
    }

    private void _BtnEscSuc(object obj)
    {
        tManage.f_ChangeState((int)EM_Guidance.GuidanceEnd);
        //int GuidanceID = Data_Pool.m_GuidancePool.m_GuidanceDT.iId;
        //while(glo_Main.GetInstance().m_SC_Pool.m_GuidanceSC.f_GetSC(GuidanceID + 1) != null)
        //{
        //    GuidanceID++;
        //}
        Data_Pool.m_GuidancePool.IGuidanceID = 30000;

        //Data_Pool.m_GuidancePool.f_SaveNewbie(Data_Pool.m_GuidancePool.m_GuidanceDT);
        Data_Pool.m_GuidancePool.f_Skip_GuidanceDtiTeam();
        //if(Data_Pool.m_GuidancePool.m_GuidanceDT.iSave != 0)
        //{
        //    Data_Pool.m_GuidancePool.IGuidanceID = Data_Pool.m_GuidancePool.m_GuidanceDT.iSave;
        //}
        //if(Data_Pool.m_GuidancePool.m_GuidanceType == EM_GuidanceType.FirstLogin)
        //    Data_Pool.m_GuidancePool.IGuidanceID = 30000;



        Data_Pool.m_GuidancePool.IsEnter = false;
    }
}

public class GuidanceArrTextParam
{
    public Vector3 MainPos;
    public Vector3 TextPos;
    public Vector3 GirlPos;
    public Vector3 GirlAngle;

    public GuidanceArrTextParam(Vector3 MainPos, Vector3 TextPos, Vector3 GirlPos, Vector3 GirlAngle)
    {
        this.MainPos = MainPos;
        this.TextPos = TextPos;
        this.GirlPos = GirlPos;
        this.GirlAngle = GirlAngle;
    }

}

