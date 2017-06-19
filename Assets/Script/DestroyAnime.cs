using UnityEngine;
using System.Collections;

public class DestroyAnime : MonoBehaviour
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
    ArrayList pic = new ArrayList();
    SpriteRenderer script;
    int i = 0;

    void Start()
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
        script = this.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
            if (i < 10)
            {
                script.sprite = (Sprite)pic[i];
                i++;
            }
            else
            {
                GameObject.Destroy(this.gameObject);
            }
    }
}
