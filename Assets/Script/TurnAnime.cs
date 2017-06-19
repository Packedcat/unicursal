using UnityEngine;
using System.Collections;

public class TurnAnime : MonoBehaviour
{
    public Sprite a_1;
    public Sprite a_2;
    public Sprite a_3;
    public Sprite a_4;
    public Sprite a_5;
    public Sprite a_6;
    public Sprite a_7;
    public Sprite a_8;
    public Sprite a_9;
    public Sprite a_10;
    public Sprite a_11;
    public Sprite a_12;
    public Sprite a_13;
    public bool isWhiteToBlack = true;
    ArrayList pic = new ArrayList();
    SpriteRenderer script;
    int i = 0;
    
	void Start () 
    {
        pic.Add(a_1);
        pic.Add(a_2);
        pic.Add(a_3);
        pic.Add(a_4);
        pic.Add(a_5);
        pic.Add(a_6);
        pic.Add(a_7);
        pic.Add(a_8);
        pic.Add(a_9);
        pic.Add(a_10);
        pic.Add(a_11);
        pic.Add(a_12);
        pic.Add(a_13);
        script = this.GetComponent<SpriteRenderer>();
        if (!isWhiteToBlack)
        {
            this.transform.rotation = new Quaternion(0f, 0f, 180f, 0f);
            i = 12;
        }
        
	}

    void FixedUpdate()
    {
        if (isWhiteToBlack)
        {
            if (i < 13)
            {
                script.sprite = (Sprite)pic[i];
                i++;
            }
            else
            {
                GameObject.Destroy(this.gameObject);
            }
        }
        else
        {
            if (i > -1)
            {
                script.sprite = (Sprite)pic[i];
                i--;
            }
            else
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}
