using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Play_6 : MonoBehaviour
{
    public GameObject Label_Bouns;
    public GameObject Label_Best;
    public GameObject Label_Score;
    public GameObject Label_BestScore;
    public GameObject Label_Score_GameOver;
    public GameObject Label_BestScore_GameOver;
    public GameObject Label_CountDown;
    public GameObject Button_Shadow_GameOver;
    public GameObject Gameover;
    public GameObject Cover;
    public GameObject destroyAnime_b;
    public GameObject destroyAnime_w;
    public GameObject Pitch;
    public GameObject Line;
    public GameObject[] Anime;
    public GameObject[] Cubes;
    public GameObject[,] cubeArray = new GameObject[6, 6];
    public bool[,] tempArray = new bool[6, 6];
    private int rowSames = 1;
    private int columSames = 1;
    private Vector2 tempMagnet = new Vector2(-1, -1);
    List<Vector3> coordinate = new List<Vector3>();
    List<GameObject> tempPitch = new List<GameObject>();
    List<GameObject> tempLine = new List<GameObject>();
    public RaycastHit Hit;
    public Ray myRay;
    int score;
    int stroke = 10;
    int num = 0;
    int second = 0;
    int time = 60;
    public static bool isStrokeMode;

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;//保持唤醒
        Cover.GetComponent<BoxCollider>().enabled = false;
        GameObject.Find("Cover_Se").GetComponent<BoxCollider>().enabled = false;
        Label_Bouns.SetActive(false);//奖励面板失效
        if (!isStrokeMode)//计时模式把倒计时面板重置为时间
        {
            Label_CountDown.GetComponent<UILabel>().text = "01:00";
        }
        for (int i = 0; i < 6; i++)//置状态数组初值
        {
            for (int j = 0; j < 6; j++)
            {
                tempArray[i, j] = false;
            }
        }
        if (isStrokeMode)//读取最高分至最高分面板，默认为0
        {
            Label_BestScore.GetComponent<UILabel>().text = PlayerPrefs.GetInt("StrokeGrade_6", 0).ToString();
            Label_BestScore_GameOver.GetComponent<UILabel>().text = PlayerPrefs.GetInt("StrokeGrade_6", 0).ToString();
        }
        else
        {
            Label_BestScore.GetComponent<UILabel>().text = PlayerPrefs.GetInt("TimeGrade_6", 0).ToString();
            Label_BestScore_GameOver.GetComponent<UILabel>().text = PlayerPrefs.GetInt("TimeGrade_6", 0).ToString();
        }
        Create();//创建原始状态
        Check();
        Invoke("CheckTrigger", 0.3f);
    }

    void Update()
    {
        if (Cover.GetComponent<BoxCollider>().enabled)//当遮罩出现时暂停
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
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
            if (coordinate.Count >= 2)
            {
                foreach (Vector3 tmp in coordinate)//调用翻转函数
                {
                    TurnCube((int)tmp.x, (int)tmp.y);
                }
                if (isStrokeMode)//笔画模式翻转完计数减一
                {
                    stroke -= 1;
                    Label_CountDown.GetComponent<UILabel>().text = stroke.ToString();
                    GameObject.Find("CountDown").GetComponent<UISlider>().value -= 0.1f;
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
            if (tempMagnet.x != -1)
            {
                Invoke("MagnetTrigger", 0.3f);
                Invoke("CheckTrigger", 0.6f);
            }
            else
            {
                Invoke("CheckTrigger", 0.3f);
            }
            Invoke("GameOver", 0.3f);
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
                    iTween.MoveTo(cubeArray[i, j], new Vector3(tempMagnet.x, tempMagnet.y, 0), 0.3f);
                }
            }
        }
        tempMagnet.x = -1;
        tempMagnet.y = -1;
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

    void Create()//开始创建
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                int temp = Choose(Random.Range(0, 100));
                cubeArray[i, j] = Instantiate(Cubes[temp], new Vector3(i, j, 0), Quaternion.identity) as GameObject;
            }
        }
    }

    int Choose(int rate)//随机函数
    {
        if (!isStrokeMode)//记时才使用道具
        {
            if (rate < 2)
            {
                return Random.Range(2, 10);
            }
        }
        return Random.Range(0, 2);
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

    void FixedUpdate()
    {
        if (!isStrokeMode)
        {
            num++;
            GameObject.Find("CountDown").GetComponent<UISlider>().value -= 1 / 3000f;//0.02秒
            if (num % 50 == 0)//一秒一次
            {
                second++;
                UpdateTime();
            }
        }
    }

    Vector2 CurrentTime(int sec)
    {
        int a, b;
        b = sec % 60;//B为分
        a = (sec - b) / 60;//A为秒
        return new Vector2(a, b);
    }

    void UpdateTime()
    {
        Vector2 tmp = CurrentTime(time - second);
        string a, b;
        if (tmp.x <= 9)//单位数前面加零
        {
            a = "0" + ((int)tmp.x).ToString();
        }
        else
            a = ((int)tmp.x).ToString();
        if (tmp.y <= 9)//单位数前面加零
        {
            b = "0" + ((int)tmp.y).ToString();
        }
        else
            b = ((int)tmp.y).ToString();
        string result = a + ":" + b;
        Label_CountDown.GetComponent<UILabel>().text = result;
        if (result == "00:00")
        {
            Label_Score_GameOver.GetComponent<UILabel>().text = score.ToString();
            if (int.Parse(Label_BestScore.GetComponent<UILabel>().text) > int.Parse(Label_BestScore_GameOver.GetComponent<UILabel>().text))
            {
                Label_Best.GetComponent<UILabel>().text = "New";
                if (isStrokeMode)
                {
                    Label_BestScore_GameOver.GetComponent<UILabel>().text = score.ToString();
                }
                else
                {
                    Label_BestScore_GameOver.GetComponent<UILabel>().text = score.ToString();
                }
            }
            Cover.GetComponent<BoxCollider>().enabled = true;
            Gameover.GetComponent<TweenPosition>().enabled = true;
            Gameover.GetComponent<TweenPosition>().PlayForward();
            EventDelegate.Add(Gameover.GetComponent<TweenPosition>().onFinished, OnFinished);
            Button_Shadow_GameOver.SetActive(true);
            Button_Shadow_GameOver.GetComponent<TweenAlpha>().PlayForward();
        }
    }
    IEnumerator GetCapture()
    {
        yield return new WaitForEndOfFrame();
        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        byte[] imagebytes = tex.EncodeToPNG();//转化为png图
        tex.Compress(false);//对屏幕缓存进行压缩
        GameObject.Find("Capture").GetComponent<UITexture>().mainTexture = tex;//对屏幕缓存进行显示（缩略图）
        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/screencapture.png", imagebytes);//存储png图

    }

    void GameOver()
    {
        if (stroke == 0)
        {
            Label_Score_GameOver.GetComponent<UILabel>().text = score.ToString();
            if (int.Parse(Label_BestScore.GetComponent<UILabel>().text) > int.Parse(Label_BestScore_GameOver.GetComponent<UILabel>().text))
            {
                Label_Best.GetComponent<UILabel>().text = "New";
                if (isStrokeMode)
                {
                    Label_BestScore_GameOver.GetComponent<UILabel>().text = score.ToString();
                }
                else
                {
                    Label_BestScore_GameOver.GetComponent<UILabel>().text = score.ToString();
                }
            }
            Cover.GetComponent<BoxCollider>().enabled = true;
            Gameover.GetComponent<TweenPosition>().enabled = true;
            Gameover.GetComponent<TweenPosition>().PlayForward();
            EventDelegate.Add(Gameover.GetComponent<TweenPosition>().onFinished, OnFinished);
            Button_Shadow_GameOver.SetActive(true);
            Button_Shadow_GameOver.GetComponent<TweenAlpha>().PlayForward();
        }
    }
    
    void OnFinished()
    {
        StartCoroutine(GetCapture());
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
                            if (!isStrokeMode)
                            {
                                CheckType(i, n);
                            }
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
                            if (!isStrokeMode)
                            {
                                CheckType(n, i);
                            }
                            tempArray[n, i] = true;
                        }
                        columSames = 1;
                    }
                }
            }
        }
    }

    void Boom(int x, int y)
    {
        if (x == 0 && y == 0)
        {
            if (!tempArray[x + 1, y])
            {
                tempArray[x + 1, y] = true;
                CheckType(x + 1, y);
            }
            if (!tempArray[x, y + 1])
            {
                tempArray[x, y + 1] = true;
                CheckType(x, y + 1);
            }
            if (!tempArray[x + 1, y + 1])
            {
                tempArray[x + 1, y + 1] = true;
                CheckType(x + 1, y + 1);
            }
        }
        if (x == 0 && y == 5)
        {
            if (!tempArray[x + 1, y])
            {
                tempArray[x + 1, y] = true;
                CheckType(x + 1, y);
            }
            if (!tempArray[x, y - 1])
            {
                tempArray[x, y - 1] = true;
                CheckType(x, y - 1);
            }
            if (!tempArray[x + 1, y - 1])
            {
                tempArray[x + 1, y - 1] = true;
                CheckType(x + 1, y - 1);
            }
        }
        if (x == 5 && y == 0)
        {
            if (!tempArray[x - 1, y])
            {
                tempArray[x - 1, y] = true;
                CheckType(x - 1, y);
            }
            if (!tempArray[x, y + 1])
            {
                tempArray[x, y + 1] = true;
                CheckType(x, y + 1);
            }
            if (!tempArray[x - 1, y + 1])
            {
                tempArray[x - 1, y + 1] = true;
                CheckType(x - 1, y + 1);
            }

        }
        if (x == 5 && y == 5)
        {
            if (!tempArray[x - 1, y])
            {
                tempArray[x - 1, y] = true;
                CheckType(x - 1, y);
            }
            if (!tempArray[x, y - 1])
            {
                tempArray[x, y - 1] = true;
                CheckType(x, y - 1);
            }
            if (!tempArray[x - 1, y - 1])
            {
                tempArray[x - 1, y - 1] = true;
                CheckType(x - 1, y - 1);
            }
        }
        if (x == 0 && y > 0 && y < 5)
        {
            if (!tempArray[x + 1, y])
            {
                tempArray[x + 1, y] = true;
                CheckType(x + 1, y);
            }
            if (!tempArray[x, y + 1])
            {
                tempArray[x, y + 1] = true;
                CheckType(x, y + 1);
            }
            if (!tempArray[x, y - 1])
            {
                tempArray[x, y - 1] = true;
                CheckType(x, y - 1);
            }
            if (!tempArray[x + 1, y + 1])
            {
                tempArray[x + 1, y + 1] = true;
                CheckType(x + 1, y + 1);
            }
            if (!tempArray[x + 1, y - 1])
            {
                tempArray[x + 1, y - 1] = true;
                CheckType(x + 1, y - 1);
            }
        }
        if (x == 5 && y > 0 && y < 5)
        {
            if (!tempArray[x - 1, y])
            {
                tempArray[x - 1, y] = true;
                CheckType(x - 1, y);
            }
            if (!tempArray[x, y + 1])
            {
                tempArray[x, y + 1] = true;
                CheckType(x, y + 1);
            }
            if (!tempArray[x, y - 1])
            {
                tempArray[x, y - 1] = true;
                CheckType(x, y - 1);
            }
            if (!tempArray[x - 1, y + 1])
            {
                tempArray[x - 1, y + 1] = true;
                CheckType(x - 1, y + 1);
            }
            if (!tempArray[x - 1, y - 1])
            {
                tempArray[x - 1, y - 1] = true;
                CheckType(x - 1, y - 1);
            }
        }
        if (y == 0 && x > 0 && x < 5)
        {
            if (!tempArray[x + 1, y])
            {
                tempArray[x + 1, y] = true;
                CheckType(x + 1, y);
            }
            if (!tempArray[x - 1, y])
            {
                tempArray[x - 1, y] = true;
                CheckType(x - 1, y);
            }
            if (!tempArray[x, y + 1])
            {
                tempArray[x, y + 1] = true;
                CheckType(x, y + 1);
            }
            if (!tempArray[x - 1, y + 1])
            {
                tempArray[x - 1, y + 1] = true;
                CheckType(x - 1, y + 1);
            }
            if (!tempArray[x + 1, y + 1])
            {
                tempArray[x + 1, y + 1] = true;
                CheckType(x + 1, y + 1);
            }

        }
        if (y == 5 && x > 0 && x < 5)
        {
            if (!tempArray[x + 1, y])
            {
                tempArray[x + 1, y] = true;
                CheckType(x + 1, y);
            }
            if (!tempArray[x - 1, y])
            {
                tempArray[x - 1, y] = true;
                CheckType(x - 1, y);
            }
            if (!tempArray[x, y - 1])
            {
                tempArray[x, y - 1] = true;
                CheckType(x, y - 1);
            }
            if (!tempArray[x + 1, y - 1])
            {
                tempArray[x + 1, y - 1] = true;
                CheckType(x + 1, y - 1);
            }
            if (!tempArray[x - 1, y - 1])
            {
                tempArray[x - 1, y - 1] = true;
                CheckType(x - 1, y - 1);
            }
        }
        if (x > 0 && x < 5 && y > 0 && y < 5)
        {
            if (!tempArray[x + 1, y])
            {
                tempArray[x + 1, y] = true;
                CheckType(x + 1, y);
            }
            if (!tempArray[x - 1, y])
            {
                tempArray[x - 1, y] = true;
                CheckType(x - 1, y);
            }
            if (!tempArray[x, y + 1])
            {
                tempArray[x, y + 1] = true;
                CheckType(x, y + 1);
            }
            if (!tempArray[x, y - 1])
            {
                tempArray[x, y - 1] = true;
                CheckType(x, y - 1);
            }
            if (!tempArray[x + 1, y + 1])
            {
                tempArray[x + 1, y + 1] = true;
                CheckType(x + 1, y + 1);
            }
            if (!tempArray[x - 1, y - 1])
            {
                tempArray[x - 1, y - 1] = true;
                CheckType(x - 1, y - 1);
            }
            if (!tempArray[x + 1, y - 1])
            {
                tempArray[x + 1, y - 1] = true;
                CheckType(x + 1, y - 1);
            }
            if (!tempArray[x - 1, y + 1])
            {
                tempArray[x - 1, y + 1] = true;
                CheckType(x - 1, y + 1);
            }
        }
    }

    void Magnet(int x, int y)
    {
        tempMagnet.x = x;
        tempMagnet.y = y;
        if (cubeArray[x, y].tag == "black")
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (cubeArray[i, j].tag == "black")
                    {
                        if (!tempArray[i, j])
                        {
                            tempArray[i, j] = true;
                            CheckType(i, j);
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (cubeArray[i, j].tag == "white")
                    {
                        if (!tempArray[i, j])
                        {
                            tempArray[i, j] = true;
                            CheckType(i, j);
                        }
                    }
                }
            }
        }
    }

    void Cross(int x, int y)
    {
        for (int i = 1; i < 6; i++)
        {
            if (x + i <= 5)
            {
                if (y + i <= 5)
                {
                    if (!tempArray[x + i, y + i])
                    {
                        tempArray[x + i, y + i] = true;
                        CheckType(x + i, y + i);
                    }
                }
                if (y - i >= 0)
                {
                    if (!tempArray[x + i, y - i])
                    {
                        tempArray[x + i, y - i] = true;
                        CheckType(x + i, y - i);
                    }
                }

            }
            if (x - i >= 0)
            {
                if (y + i <= 5)
                {
                    if (!tempArray[x - i, y + i])
                    {
                        tempArray[x - i, y + i] = true;
                        CheckType(x - i, y + i);
                    }
                }
                if (y - i >= 0)
                {
                    if (!tempArray[x - i, y - i])
                    {
                        tempArray[x - i, y - i] = true;
                        CheckType(x - i, y - i);
                    }
                }
            }
        }
    }

    void CheckType(int x, int y)
    {
        if (cubeArray[x, y].name == "cross_b(Clone)" || cubeArray[x, y].name == "cross_w(Clone)")
        {
            Cross(x, y);
        }
        if (cubeArray[x, y].name == "boom_b(Clone)" || cubeArray[x, y].name == "cross_w(Clone)")
        {
            Boom(x, y);
        }
        if (cubeArray[x, y].name == "magnet_b(Clone)" || cubeArray[x, y].name == "magnet_w(Clone)")
        {
            Magnet(x, y);
        }
    }

    void UpdateScoreAndStroke(int num)
    {

        if (num <= 12)
        {
            score += num * 1;
        }
        else if (num <= 18)
        {
            score += num * 2;
        }
        else if (num <= 24)
        {
            score += num * 4;
        }
        else if (num <= 36)
        {
            score += num * 6;
        }
        Label_Score.GetComponent<UILabel>().text = score.ToString();
        if (score > int.Parse(Label_BestScore.GetComponent<UILabel>().text))
        {
            if (isStrokeMode)
            {
                PlayerPrefs.SetInt("StrokeGrade_6", score);
                Label_BestScore.GetComponent<UILabel>().text = score.ToString();
            }
            else
            {
                PlayerPrefs.SetInt("TimeGrade_6", score);
                Label_BestScore.GetComponent<UILabel>().text = score.ToString();
            }

        }
        if (isStrokeMode)
        {
            if (num > 18)
            {
                stroke += 1;
                Label_CountDown.GetComponent<UILabel>().text = stroke.ToString();
                GameObject.Find("CountDown").GetComponent<UISlider>().value += 0.1f;
                Label_Bouns.SetActive(true);
                Invoke("BonusActive", 1f);
            }

        }
    }

    void BonusActive()
    {
        Label_Bouns.SetActive(false);
    }

    void DestroyCube()
    {
        int score = 0;

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
                    score++;
                }
            }
        }
        GameObject.Find("Cover_Se").GetComponent<BoxCollider>().enabled = true;
        Invoke("CreateCube", 0.3f);
        UpdateScoreAndStroke(score);
    }

    void CreateCube()
    {
        GameObject.Find("Cover_Se").GetComponent<BoxCollider>().enabled = false;
        int sames = 0;
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (cubeArray[i, j] == null)
                {
                    sames++;
                }
            }
            for (int j = 0; j < 6; j++)
            {
                if (cubeArray[i, j] == null)
                {
                    if (j >= 6 - sames)
                    {
                        int temp = Choose(Random.Range(0, 100));
                        cubeArray[i, j] = Instantiate(Cubes[temp], new Vector3(i, j + sames, 0), Quaternion.identity) as GameObject;
                    }
                    else
                    {
                        for (int m = 1; m < 6; m++)
                        {
                            if (cubeArray[i, j + m] != null)
                            {
                                cubeArray[i, j] = cubeArray[i, j + m];
                                cubeArray[i, j + m] = null;
                                break;
                            }
                        }
                    }
                    iTween.MoveTo(cubeArray[i, j], new Vector3(i, j, 0), 0.5f);
                }
            }
            sames = 0;
        }
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                tempArray[i, j] = false;
            }
        }
        Check();
        if (tempMagnet.x != -1)
        {
            Invoke("MagnetTrigger", 0.3f);
            Invoke("CheckTrigger", 0.6f);
        }
        else
        {
            Invoke("CheckTrigger", 0.3f);
        }
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
