using UnityEngine;
using System.Collections;

public class Button_Retry : MonoBehaviour
{
    int Num;
    void Start()
    {
        Num = PlayerPrefs.GetInt("Mode");
    }
    void OnClick()
    {
        switch (Num)
        {
            case 6:
                Application.LoadLevel("Game_6×6");
                break;
            case 9:
                Application.LoadLevel("Game_9×9");
                break;
        }
    }
}
