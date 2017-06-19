using UnityEngine;
using System.Collections;

public class Mode : MonoBehaviour
{
    int Num = 0;
    void Start()
    {
        Num = PlayerPrefs.GetInt("Mode", 6);
        if (Num == 6)
        {
            GameObject.Find("Button_6×6").GetComponent<UIButton>().isEnabled = false;
            GameObject.Find("Label_6×6").GetComponent<UIButton>().isEnabled = false;
            GameObject.Find("Button_9×9").GetComponent<UIButton>().isEnabled = true;
            GameObject.Find("Label_9×9").GetComponent<UIButton>().isEnabled = true;
        }
        else
        {
            GameObject.Find("Button_9×9").GetComponent<UIButton>().isEnabled = false;
            GameObject.Find("Label_9×9").GetComponent<UIButton>().isEnabled = false;
            GameObject.Find("Button_6×6").GetComponent<UIButton>().isEnabled = true;
            GameObject.Find("Label_6×6").GetComponent<UIButton>().isEnabled = true;
        }
    }
    void OnClick()
    {
        if(this.name=="Button_6×6")
        {
            PlayerPrefs.SetInt("Mode", 6);
            GameObject.Find("Button_6×6").GetComponent<UIButton>().isEnabled = false;
            GameObject.Find("Label_6×6").GetComponent<UIButton>().isEnabled = false;
            GameObject.Find("Button_9×9").GetComponent<UIButton>().isEnabled = true;
            GameObject.Find("Label_9×9").GetComponent<UIButton>().isEnabled = true;

        }
        if (this.name == "Button_9×9")
        {
            PlayerPrefs.SetInt("Mode", 9);
            GameObject.Find("Button_9×9").GetComponent<UIButton>().isEnabled = false;
            GameObject.Find("Label_9×9").GetComponent<UIButton>().isEnabled = false;
            GameObject.Find("Button_6×6").GetComponent<UIButton>().isEnabled = true;
            GameObject.Find("Label_6×6").GetComponent<UIButton>().isEnabled = true;
        }
    }
}
