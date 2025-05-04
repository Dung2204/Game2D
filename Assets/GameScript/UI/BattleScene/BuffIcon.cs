using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffIcon : MonoBehaviour
{
    // Start is called before the first frame update
    public UISprite icon;
    public UISprite ipara;
    public UILabel iLive;
    void Start()
    {

    }

    public void SetUI(int _id, int _iLive)
    {
        string spriteName = "";
        string iParabName = "";
        BufDT _BufDT = (BufDT)glo_Main.GetInstance().m_SC_Pool.m_BufSC.f_GetSC(_id);
        if (_BufDT == null)
        {
            MessageBox.ASSERT("Buff is not exits by id=" + _id);
            return;
        }

        switch (_BufDT.iType)
        {
            case 1:
                spriteName = _BufDT.iType + "_" + _BufDT.iPara;
                if (_BufDT.iParaY > 0)//% buff số âm là debuff
                {
                    iParabName = "_up";
                }
                else
                {
                    iParabName = "_down";
                }
                break;
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
                spriteName = _BufDT.iType.ToString();
                break;
            case 7:
                spriteName = _BufDT.iType.ToString();
                if (_BufDT.iParaY > 10000)//% buff lấy mốc 10000 là 100% 
                {
                    iParabName = "_up";
                }
                else
                {
                    iParabName = "_down";
                }
                break;
            case 8: // hồi phục mỗi lượt
                spriteName = _BufDT.iType.ToString();
                break;
            case 9: // tăng máu max
                spriteName = _BufDT.iType.ToString();
                break;
            default:
                spriteName = _BufDT.iType.ToString();
                break;
        }

        icon.spriteName = spriteName + iParabName;
        //ipara.spriteName = iParabName;
        iLive.text = _iLive.ToString();
    }
}
