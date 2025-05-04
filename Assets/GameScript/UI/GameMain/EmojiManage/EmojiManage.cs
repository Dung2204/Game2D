using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System;

public class EmojiManage : UIFramwork
{

    // Use this for initialization
    //GameObject ExitEmoji;
    //void Start()
    //{
    //    UIEventListener.Get(gameObject).onClick = Open;
    //    ExitEmoji = Emoji.transform.GetChild(1).gameObject;
    //    UIEventListener.Get(ExitEmoji).onClick = Open;
    //}

    //// Update is called once per frame
    //private void Open(GameObject g)
    //{
    //    Emoji.SetActive(true);
    //}

    //private void Cloce(GameObject g)
    //{
    //    Emoji.SetActive(false);
    //}
    public UIInput input;
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
    }
    protected override void f_InitMessage()
    {
        base.f_InitMessage();

        f_RegClickEvent("ExitBtn", UI_ExitBtn);
        f_RegClickEvent("Emoji", Ui_InputEmoji);
    }

    private void UI_ExitBtn(GameObject go, object obj1, object obj2)
    {
        //通知HoldPool弹出当前页
        //ccUIHoldPool.GetInstance().f_UnHold();
        //关闭自身
        UI_CLOSE(null);
    }

    private void Ui_InputEmoji(GameObject go, object obj1, object obj2)
    {
        input.value += (f_GetObject("Emoji") as GameObject).GetComponent<InputEmoji>().EmojiID;
    }

    private void Ui_AddEmoji()
    {
        int length=0;
        string inputstring = input.value;
        for (int index=0; index<inputstring.Length; index++)
        {
            char cstr = inputstring[index];
        }
    }
}
