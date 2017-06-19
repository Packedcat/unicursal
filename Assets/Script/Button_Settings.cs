using UnityEngine;
using System.Collections;

public class Button_Settings : MonoBehaviour
{
    void OnClick()
    {
        GameObject.Find("Cover").GetComponent<BoxCollider>().enabled = true;
    }
}
