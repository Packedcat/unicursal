using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class Button_Share : MonoBehaviour
{
    void Start()
    {
        UIEventListener.Get(GameObject.Find("Button_Confirm")).onClick = OnConfirmClick;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.GetComponent<TweenScale>().enabled = true;
            this.GetComponent<TweenScale>().PlayReverse();
        }
    }

    void OnConfirmClick(GameObject button)
    {
#if UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("ShareToFriend", new object[] { "El psy congroo", Application.persistentDataPath + "/screencapture.png" });  //common share

#endif
    }
}
