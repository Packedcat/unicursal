using UnityEngine;
using System.Collections;

public class Button_Next : MonoBehaviour
{
    private int Count;

	void Start ()
    {
        Count = 1;
	}

    void OnClick()
    {
        switch(Count)
        {
            case 1:
                GameObject.Find("Step").GetComponent<UISprite>().spriteName = "Guide_2";
                GameObject.Find("Label_Step").GetComponent<UILabel>().text = "Next(2/5)";
                Count++;
                break;
            case 2:
                GameObject.Find("Step").GetComponent<UISprite>().spriteName = "Guide_3";
                GameObject.Find("Label_Step").GetComponent<UILabel>().text = "Next(3/5)";
                Count++;
                break;
            case 3:
                GameObject.Find("Step").GetComponent<UISprite>().spriteName = "Guide_4";
                GameObject.Find("Label_Step").GetComponent<UILabel>().text = "Next(4/5)";
                Count++;
                break;
            case 4:
                GameObject.Find("Step").GetComponent<UISprite>().spriteName = "Guide_5";
                GameObject.Find("Label_Step").GetComponent<UILabel>().text = "Back(5/5)";
                Count++;
                break;
            case 5:
                Application.LoadLevel("Start");
                break;
        }
        
    }
}