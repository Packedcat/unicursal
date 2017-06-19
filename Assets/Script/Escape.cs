using UnityEngine;
using System.Collections;

public class Escape : MonoBehaviour
{
    public GameObject Button_Shadow_GameOver;
    public GameObject Button_Shadow_Warning;
    public GameObject Button_Shadow_Pop;
	// Use this for initialization
	void Start ()
    {}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Button_Shadow_GameOver.activeSelf)
            {
                if (Button_Shadow_Pop.activeSelf)
                {
                    if (this.name == "Warning_Game")
                    {
                        GameObject.Find("Cover").GetComponent<BoxCollider>().enabled = false;
                    }
                    else
                    {
                        Time.timeScale = 1;
                    }
                    Button_Shadow_Pop.GetComponent<TweenAlpha>().PlayReverse();
                    Invoke("Delay", 0.5f);
                    GameObject.Find("Pop").GetComponent<TweenPosition>().PlayReverse();
                }
                else
                {
                    if (this.name == "Warning_Game")
                    {
                        GameObject.Find("Cover").GetComponent<BoxCollider>().enabled = true;
                    }
                    this.GetComponent<TweenPosition>().enabled = true;
                    this.GetComponent<TweenPosition>().PlayForward();
                    Button_Shadow_Warning.SetActive(true);
                    Button_Shadow_Warning.GetComponent<TweenAlpha>().PlayForward();
                }
            }
        }
	}
    void Delay()
    {
        Button_Shadow_Pop.SetActive(false);
    }
}
