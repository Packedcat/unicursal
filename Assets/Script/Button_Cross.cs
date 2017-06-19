using UnityEngine;
using System.Collections;

public class Button_Cross : MonoBehaviour
{
    void OnClick()
    {
        GameObject.Find("Cover").GetComponent<BoxCollider>().enabled = false;
    }
}
