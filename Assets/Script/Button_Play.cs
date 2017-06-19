using UnityEngine;
using System.Collections;

public class Button_Play : MonoBehaviour
{
    int Num = 0;
    void OnClick()
    {
        Num = PlayerPrefs.GetInt("Mode", 6);
        if (Num == 6)
        {
            PlayerPrefs.SetInt("Mode", 6);
            Application.LoadLevel("Game_6×6");
            if (this.name == "Button_Zen")
            {
                Play_6.isStrokeMode = true;
            }
            else
            {
                Play_6.isStrokeMode = false;
            }
        }
        else
        {
            Application.LoadLevel("Game_9×9");
            if (this.name == "Button_Zen")
            {
                Play_9.isStrokeMode = true;
            }
            else
            {
                Play_9.isStrokeMode = false;
            }
        }
    }
    
}
