using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class MagicControl : MonoBehaviour
{

    public CharActionController m_CharActionController;
    private ccCallback _CallBack_MagicHarm;
    private object _CallBackObj;
    private bool _bIsDestory = false;

    public void f_Create(Vector3 Pos, GameObject oRoleSpine, ccCallback CallBack_MagicHarm, object Obj)
    {
        _CallBack_MagicHarm = CallBack_MagicHarm;
        _CallBackObj = Obj;
        oRoleSpine.transform.parent = BattleManager.GetInstance().transform;
        oRoleSpine.transform.position = Pos;
        oRoleSpine.transform.localScale = new Vector3(1, 1, 1);

        m_CharActionController = oRoleSpine.GetComponent<CharActionController>();
        if (m_CharActionController == null)
        {
            m_CharActionController = oRoleSpine.AddComponent<CharActionController>();
        }
        m_CharActionController.f_Init(oRoleSpine, Callback_Event);
        m_CharActionController.f_PlayMagic();
        f_SetDepthForAttack();

        _bIsDestory = false;
    }


    private void Callback_Event(object Obj)
    {
        string ppSQL = (string)Obj;
        if (ppSQL == "OnCreateMagicHarm")
        {
            if (_CallBack_MagicHarm != null)
            {
                _CallBack_MagicHarm(_CallBackObj);
                //_CallBack_MagicHarm = null;
            }

        }
        else if (ppSQL == "Skill_Complete")
        {
            _CallBack_MagicHarm = null;
            if (_bIsDestory)
            {
MessageBox.DEBUG("Skill_Complete command duplicated, " + gameObject.name);
            }
            else
            {
                ccTimeEvent.GetInstance().f_RegEvent(0.5f, false, null, AutoDestory);
                _bIsDestory = true;
            }
        }


    }

    private void AutoDestory(object Obj)
    {
        if (this != null) 
        {
            if (gameObject != null)
            {
                // m_CharActionController.f_SetSkillDepth(false);
                m_CharActionController.SetUnScaledTime(false);
                glo_Main.GetInstance().m_ResourceManager.f_DestorySD(gameObject);
            }
        }
    }

    private void f_SetDepthForAttack()
    {
        m_CharActionController.f_SetDepthForAttack();
    }

}
