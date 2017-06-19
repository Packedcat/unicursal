using UnityEngine;
using System.Collections;

public class Slider : MonoBehaviour
{
	void Start ()
    {
        if (this.GetComponent<UISlider>().value >= 0.5f)
        {
            this.GetComponent<UISlider>().value = 1.0f;
        }
        else
        {
            this.GetComponent<UISlider>().value = 0f;
        }
	}

	void Update ()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (this.GetComponent<UISlider>().value >= 0.5f)
            {
                this.GetComponent<UISlider>().value = 1.0f;
            }
            else
            {
                this.GetComponent<UISlider>().value = 0f;
            }
        }
	}

    void Onclick()
    {
        if(this.GetComponent<UISlider>().value == 1.0f)
        {
            this.GetComponent<UISlider>().value = 0;
        }
        else
        {
            this.GetComponent<UISlider>().value = 1.0f;
        }
    }
}
