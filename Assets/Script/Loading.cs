using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour
{
    private AsyncOperation async;
    private uint process;
    private int Num;
	void Start ()
    {
        Num = PlayerPrefs.GetInt("Sence", 0);
        if (Num == 0)
        {
            StartCoroutine(StartLoading("Tutorial"));
            PlayerPrefs.SetInt("Sence", 1);
        }
        else
        {
            StartCoroutine(StartLoading("Start"));
        }
	}
    
    IEnumerator StartLoading(string sence)
    {
        async = Application.LoadLevelAsync(sence);
        async.allowSceneActivation = false;
        yield return async;
    }

    void Update ()
    {
        if (async == null)
        {
            return;
        }
        uint toProcess;
        Debug.Log(async.progress * 100);
        if (async.progress < 0.9f)
        {
            toProcess = (uint)(async.progress * 100);
        }
        else
        {
            toProcess = 100;
        }

        if (process < toProcess)
        {
            process++;
        }

        GameObject.Find("Loading").GetComponent<UISlider>().value = process / 100f;
        GameObject.Find("Persent").GetComponent<UILabel>().text = process + "%";
        if (process == 100)
        {
            async.allowSceneActivation = true;
        }
    }
}
