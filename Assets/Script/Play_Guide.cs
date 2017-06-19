using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Play_Guide : MonoBehaviour
{
    public GameObject Guide_3;
    public GameObject Limits;
    public GameObject destroyAnime_b;
    public GameObject destroyAnime_w;
    public GameObject[] Cubes;
    public GameObject[] Anime;
    public GameObject[,] cubeArray = new GameObject[6, 6];
    public bool[,] tempArray = new bool[6, 6];
    public int[,] typeArray = new int[6, 6];
    public RaycastHit Hit;
    public Ray myRay;
    public GameObject Pitch;
    public GameObject Line;
    List<Vector3> coordinate = new List<Vector3>();
    List<GameObject> tempPitch = new List<GameObject>();
    List<GameObject> tempLine = new List<GameObject>();
    private int rowSames = 1;
    private int columSames = 1;
    private int guideStep = 0;
    private GameObject Limit;

	void Start ()
    {
        Limit = Instantiate(Limits, new Vector3(2.5f, 3f, -2f), Quaternion.identity) as GameObject;
        for (int i = 0; i < 6; i++)//置状态数组初值
        {
            for (int j = 0; j < 6; j++)
            {
                tempArray[i, j] = false;
            }
        }
        typeArray = new int[,] { 
        { 1, 1, 1, 0, 1, 1 },
        { 0, 0, 0, 1, 0, 0 },
        { 0, 0, 0, 1, 0, 0 },
        { 0, 0, 0, 1, 0, 0 },
        { 0, 0, 0, 1, 0, 0 },
        { 1, 1, 1, 0, 1, 1 }};
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                cubeArray[i, j] = Instantiate(Cubes[typeArray[i, j]], new Vector3(i, j, 0), Quaternion.identity) as GameObject;
            }
        }
    }

    void CreateCube()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (cubeArray[i, j] != null)
                {
                    Destroy(cubeArray[i, j]);
                    cubeArray[i, j] = null;
                }
            }
        }
        switch(guideStep)
        {
            case 0:
                typeArray = new int[,] { 
                { 1, 1, 1, 0, 1, 1 },
                { 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 7, 0, 0 },
                { 0, 0, 0, 1, 0, 0 },
                { 1, 1, 1, 0, 1, 1 }};
                guideStep++;
                GameObject.Find("Guide_1").GetComponent<TweenAlpha>().enabled = true;
                GameObject.Find("Guide_1").GetComponent<TweenAlpha>().PlayReverse();
                GameObject.Find("Guide_1").GetComponent<UISprite>().spriteName = "Guide_3";
                GameObject.Find("Guide_2").GetComponent<UISprite>().spriteName = "Guide_4";
                GameObject.Find("Step").GetComponent<UISprite>().spriteName = "Step_2";
                GameObject.Find("Label_Step").GetComponent<UILabel>().text = "Next(3/5)";
                GameObject.Find("Label_Back").GetComponent<UILabel>().text = "Next(4/5)";
                break;
            case 1:
                typeArray = new int[,] {
                { 0, 0, 1, 1, 1, 1 },
                { 1, 0, 0, 1, 1, 1 },
                { 1, 1, 0, 0, 1, 1 },
                { 1, 1, 1, 0, 0, 1 },
                { 1, 1, 1, 1, 0, 0 },
                { 1, 1, 1, 1, 1, 0 } };
                Guide_3.SetActive(true);
                guideStep++;
                break;
        }
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                cubeArray[i, j] = Instantiate(Cubes[typeArray[i, j]], new Vector3(i, j + 6, 0), Quaternion.identity) as GameObject;
                iTween.MoveTo(cubeArray[i, j], new Vector3(i, j, 0), 0.5f);
            }
        }
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                tempArray[i, j] = false;
            }
        }
        Limit = Instantiate(Limits, new Vector3(2.5f, 3f, -2f), Quaternion.identity) as GameObject;
    }

	void Update ()
    {
        myRay = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(myRay, out Hit))//不断获取坐标
        {
            if (Input.GetMouseButton(0))//鼠标单击或者触屏
            {
                if (coordinate.Contains(Hit.collider.transform.position))//已包含鼠标所指的方块（即还停留在方块上）
                {
                    if (coordinate.Count > 1)//计数大于1
                    {
                        if (Hit.collider.transform.position != coordinate[coordinate.Count - 1])//防止鼠标停留在当前方块重复的进行销毁与创建
                        {
                            int index = coordinate.Count - coordinate.IndexOf(Hit.collider.transform.position);
                            for (int i = 0; i < index - 1; i++)//销毁路径以外的方块
                            {
                                Destroy(tempPitch[coordinate.Count - 1]);
                                Destroy(tempLine[coordinate.Count - 2]);
                                tempLine.RemoveAt(coordinate.Count - 2);
                                tempPitch.RemoveAt(coordinate.Count - 1);
                                coordinate.RemoveAt(coordinate.Count - 1);
                            }
                        }
                    }
                }
                else
                {
                    if (CheckLine(Hit.collider.transform.position))//只有方块四周的方块能使用
                    {
                        coordinate.Add(Hit.collider.transform.position);
                        GameObject temp = Instantiate(Pitch, Hit.collider.transform.position + new Vector3(0, 0, -1), Quaternion.identity) as GameObject;
                        tempPitch.Add(temp);
                        if (coordinate.Count > 1)
                            WirteLine();
                    }

                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (coordinate.Count >= 4)
            {
                foreach (Vector3 tmp in coordinate)//调用翻转函数
                {
                    TurnCube((int)tmp.x, (int)tmp.y);
                }
                //GameObject.Find("sound").GetComponent<AudioSource>().Play();
            }
            foreach (GameObject temp in tempPitch)//清除容器内容
            {
                Destroy(temp);
            }
            foreach (GameObject temp in tempLine)
            {
                Destroy(temp);
            }
            coordinate.Clear();
            tempPitch.Clear();
            tempLine.Clear();
            Check();
            if (guideStep == 1)
            {
                Invoke("MagnetTrigger", 0.3f);
                Invoke("CheckTrigger", 0.6f);
            }
            else
            {
                Invoke("CheckTrigger", 0.3f);
            }
        }
	}

    void MagnetTrigger()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (tempArray[i, j])
                {
                    iTween.MoveTo(cubeArray[i, j], new Vector3(3, 3, 0), 0.3f);
                }
            }
        }
    }

    void TurnCube(int x, int y)//翻转方块，用名字做判别方式
    {
        if (cubeArray[x, y].name == "black(Clone)")
        {
            Destroy(cubeArray[x, y]);
            PlayAnime(1, x, y);
            cubeArray[x, y] = Instantiate(Cubes[1], new Vector3(x, y, 0), Quaternion.identity) as GameObject;

        }
        else if (cubeArray[x, y].name == "white(Clone)")
        {
            Destroy(cubeArray[x, y]);
            PlayAnime(0, x, y);
            cubeArray[x, y] = Instantiate(Cubes[0], new Vector3(x, y, 0), Quaternion.identity) as GameObject;
        }
        else if (cubeArray[x, y].name == "boom_b(Clone)")
        {
            Destroy(cubeArray[x, y]);
            PlayAnime(3, x, y);
            cubeArray[x, y] = Instantiate(Cubes[3], new Vector3(x, y, 0), Quaternion.identity) as GameObject;
        }
        else if (cubeArray[x, y].name == "boom_w(Clone)")
        {
            Destroy(cubeArray[x, y]);
            PlayAnime(2, x, y);
            cubeArray[x, y] = Instantiate(Cubes[2], new Vector3(x, y, 0), Quaternion.identity) as GameObject;
        }
        else if (cubeArray[x, y].name == "cross_b(Clone)")
        {
            Destroy(cubeArray[x, y]);
            PlayAnime(5, x, y);
            cubeArray[x, y] = Instantiate(Cubes[5], new Vector3(x, y, 0), Quaternion.identity) as GameObject;
        }
        else if (cubeArray[x, y].name == "cross_w(Clone)")
        {
            Destroy(cubeArray[x, y]);
            PlayAnime(4, x, y);
            cubeArray[x, y] = Instantiate(Cubes[4], new Vector3(x, y, 0), Quaternion.identity) as GameObject;
        }
        else if (cubeArray[x, y].name == "magnet_b(Clone)")
        {
            Destroy(cubeArray[x, y]);
            PlayAnime(7, x, y);
            cubeArray[x, y] = Instantiate(Cubes[7], new Vector3(x, y, 0), Quaternion.identity) as GameObject;
        }
        else if (cubeArray[x, y].name == "magnet_w(Clone)")
        {
            Destroy(cubeArray[x, y]);
            PlayAnime(6, x, y);
            cubeArray[x, y] = Instantiate(Cubes[6], new Vector3(x, y, 0), Quaternion.identity) as GameObject;
        }
    }
    
    void WirteLine()//根据两个方块的坐标绘画路径
    {
        Vector3 oldCube = coordinate[coordinate.Count - 2];
        Vector3 newCube = coordinate[coordinate.Count - 1];
        if (oldCube.x == newCube.x)//垂直
        {
            if (oldCube.y < newCube.y)//向上
            {
                GameObject temp = Instantiate(Line, (oldCube + newCube) / 2 + new Vector3(0, 0, -1), Quaternion.Euler(0f, 0f, 90f)) as GameObject;
                tempLine.Add(temp);
            }
            else//向下
            {
                GameObject temp = Instantiate(Line, (oldCube + newCube) / 2 + new Vector3(0, 0, -1), Quaternion.Euler(0f, 0f, -90f)) as GameObject;
                tempLine.Add(temp);
            }
        }
        if (oldCube.y == newCube.y)//水平
        {
            if (oldCube.x < newCube.x)//向右
            {
                GameObject temp = Instantiate(Line, (oldCube + newCube) / 2 + new Vector3(0, 0, -1), Quaternion.Euler(0f, 0f, 0f)) as GameObject;
                tempLine.Add(temp);
            }
            else//向左
            {
                GameObject temp = Instantiate(Line, (oldCube + newCube) / 2 + new Vector3(0, 0, -1), Quaternion.Euler(0f, 0f, 180f)) as GameObject;
                tempLine.Add(temp);
            }
        }
    }

    bool CheckLine(Vector3 newCube)//检查下一方块的坐标是否在原方块附近
    {
        if (coordinate.Count != 0)
        {
            bool isRight = false;
            Vector3 temp = coordinate[coordinate.Count - 1];
            if (newCube == new Vector3(temp.x, temp.y + 1, temp.z))
                isRight = true;
            if (newCube == new Vector3(temp.x, temp.y - 1, temp.z))
                isRight = true;
            if (newCube == new Vector3(temp.x - 1, temp.y, temp.z))
                isRight = true;
            if (newCube == new Vector3(temp.x + 1, temp.y, temp.z))
                isRight = true;
            if (coordinate.Count > 1)
                if (newCube == coordinate[coordinate.Count - 2])
                    isRight = false;
            return isRight;
        }
        return true;
    }

    void Check()//检查是否满足消除条件，用tag做判别方式
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (j == 0)
                {
                    rowSames = 1;
                }
                else
                {
                    if (cubeArray[i, j - 1].tag == cubeArray[i, j].tag)
                    {
                        rowSames++;
                    }
                    if (rowSames == 6)
                    {
                        for (int n = 0; n < 6; n++)
                        {
                            tempArray[i, n] = true;
                        }
                        rowSames = 1;
                    }
                }
                if (j == 0)
                {
                    columSames = 1;
                }
                else
                {
                    if (cubeArray[j - 1, i].tag == cubeArray[j, i].tag)
                    {
                        columSames++;
                    }
                    if (columSames == 6)
                    {
                        for (int n = 0; n < 6; n++)
                        {
                            tempArray[n, i] = true;
                        }
                        columSames = 1;
                    }
                }
            }
        }
    }
    
    void CheckTrigger()
    {
        //若tempArray不为空则调用消除函数
        int x = 0;
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (!tempArray[i, j])
                {
                    x++;
                }
            }
        }
        if (x != 36)
        {
            //Invoke("DestroyCube", 0.6f);
            DestroyCube();
        }
    }
   
    void DestroyCube()
    {
        GameObject.Find("Gesture").GetComponent<TweenPosition>().enabled = false;
        GameObject.Find("Gesture").GetComponent<TweenAlpha>().PlayReverse();
        Destroy(Limit);
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (tempArray[i, j])
                {
                    if (cubeArray[i, j].tag == "black")
                    {
                        Instantiate(destroyAnime_b, new Vector3(cubeArray[i, j].transform.position.x, cubeArray[i, j].transform.position.y, 0), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(destroyAnime_w, new Vector3(cubeArray[i, j].transform.position.x, cubeArray[i, j].transform.position.y, 0), Quaternion.identity);
                    }
                    Destroy(cubeArray[i, j]);
                    cubeArray[i, j] = null;
                }
            }
        }
        Invoke("CreateCube", 0.3f);
    }
    
    void PlayAnime(int type, int x, int y)
    {
        GameObject playanime;
        switch (type)
        {
            case 0:
                playanime = Instantiate(Anime[0], new Vector3(x, y, -1), Quaternion.identity) as GameObject;
                playanime.GetComponent<TurnAnime>().isWhiteToBlack = true;
                break;
            case 1:
                playanime = Instantiate(Anime[0], new Vector3(x, y, -1), Quaternion.identity) as GameObject;
                playanime.GetComponent<TurnAnime>().isWhiteToBlack = false;
                break;
            case 2:
                playanime = Instantiate(Anime[2], new Vector3(x, y, -1), Quaternion.identity) as GameObject;
                break;
            case 3:
                playanime = Instantiate(Anime[3], new Vector3(x, y, -1), Quaternion.identity) as GameObject;
                break;
            case 4:
                playanime = Instantiate(Anime[1], new Vector3(x, y, -1), Quaternion.identity) as GameObject;
                playanime.GetComponent<TurnAnime>().isWhiteToBlack = true;
                break;
            case 5:
                playanime = Instantiate(Anime[1], new Vector3(x, y, -1), Quaternion.identity) as GameObject;
                playanime.GetComponent<TurnAnime>().isWhiteToBlack = false;
                break;
            case 6:
                playanime = Instantiate(Anime[4], new Vector3(x, y, -1), Quaternion.identity) as GameObject;
                break;
            case 7:
                playanime = Instantiate(Anime[5], new Vector3(x, y, -1), Quaternion.identity) as GameObject;
                break;
            default:
                break;
        }
    }

}
