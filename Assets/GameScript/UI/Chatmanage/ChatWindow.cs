

using UnityEngine;
using System.Collections;
using System.Text;

public class ChatWindow : MonoBehaviour
{
    private EmojiExtension mEmojiExt;
    private UIScrollView mScrollView;
    private UIInput mInput;
    private GameObject mOkButton;

    void Awake()
    {
        mEmojiExt = transform.Find("Chat").GetComponent<EmojiExtension>();
        //mScrollView = transform.Find("ChatBg/scrollview").GetComponent<UIScrollView>();
        mInput = GetComponent<UIInput>();
        mOkButton = transform.Find("Send").gameObject;
        UIEventListener.Get(mOkButton).onClick = OnSubmit;
    }
    private void OnSubmit(GameObject go)
    {
        mEmojiExt.Add( mInput.value, EM_ChatChan.eChan_Legion);
        mInput.value = mInput.defaultText;
        mScrollView.ResetPosition();
    }
}
