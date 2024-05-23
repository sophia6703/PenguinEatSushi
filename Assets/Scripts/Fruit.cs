using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum FruitType
{
    One=0,
    Two=1,
    Three=2,
    Four=3,
    Five=4,
    Six=5,
    Seven=6,
    Eight=7,
    Nine=8,
    Ten=9,
    Eleven=10,
}
//
//默认Ready
//待命以及鼠标控制移动位置时候 StandBy
//跌落 dropping
//碰撞到地板或者其他水果 Collision
//
public enum FruitState
{
    Ready = 0,
    StandBy = 1,
    Dropping = 2,
    Collision = 3,
}


public class Fruit : MonoBehaviour
{
    // Unity 脚本中public 的变量或者参数，可以在Unity引擎，Inspector 检查器视图中可视化的修改
    public FruitType fruitType = FruitType.One;

    private bool IsMove = false;

    public FruitState fruitState = FruitState.Ready;

    public float limit_x = 2f;
    public Vector3 originalScale = Vector3.zero;
    public float scaleSpeed = 0.1f;

    public float fruitScore = 1f;

    void Awake()
    {
        originalScale = new Vector3 (0.6f,0.6f,0.6f);
    }

    //Use this for intialization
    void Start()
    {

    }

    //Update is called once per frame
    void Update()
    {
        
        //游戏状态StandBy&水果状态StandBy，可以鼠标点击控制移动，以及松开鼠标跌落。
        if (GameManager.gameManagerInstance.gameState == GameState.StandBy && fruitState == FruitState.StandBy)
        {
            if (Input.GetMouseButtonDown(0))
            {
                IsMove = true;
            }
            //松开鼠标
            if (Input.GetMouseButtonUp(0)&&IsMove)
            {
                IsMove = false;
                // change the gravity , the object falling
                this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
                fruitState = FruitState.Dropping;//Fruit step2
                GameManager.gameManagerInstance.gameState = GameState.Inprogress; //Step2

                // create new waiting object
                GameManager.gameManagerInstance.InvokeCreateFruit(0.5f);
            }
            if (IsMove)
            {
                // move the object
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//屏幕坐标转换成unity世界坐标
                this.gameObject.GetComponent<Transform>().position = new Vector3(mousePos.x, this.gameObject.GetComponent<Transform>().position.y, this.gameObject.GetComponent<Transform>().position.z);
            }

        }
        //X方向的范围限制
        if (this.transform.position.x > limit_x)
        {
            this.transform.position = new Vector3(limit_x, this.transform.position.y, this.transform.position.z);
        }
    
        if (this.transform.position.x < -limit_x)
        {
            this.transform.position = new Vector3(-limit_x, this.transform.position.y, this.transform.position.z);
        }

        //尺寸恢复
        if (this.transform.localScale.x < originalScale.x)
        {
            this.transform.localScale += new Vector3 (1,1,1) * scaleSpeed;
        }
        if (this.transform.localScale.x > originalScale.x)
        {
            this.transform.localScale = originalScale;
        }



    }

    
    //碰撞
    //Fruit的游戏对象碰撞到Collision的时候
    //会一直不停的执行监督
    //每一个水果身上都有碰撞检测

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (fruitState == FruitState.Dropping)
        {
            //碰撞到Floor
            if (collision.gameObject.tag.Contains("Floor"))
            {
                GameManager.gameManagerInstance.gameState = GameState.StandBy;
                fruitState = FruitState.Collision;// Fruit Step3

                GameManager.gameManagerInstance.hitSource.Play();
            }
            //碰撞到fruit
            if (collision.gameObject.tag.Contains("Fruit"))
            {
                GameManager.gameManagerInstance.gameState = GameState.StandBy;
                fruitState = FruitState.Collision;// Fruit Step3
            }

        }
        
        
        
        
        
        //Dropping, Collision, 可以合成

        if ((int)fruitState>=(int)FruitState.Dropping)
        {
            if (collision.gameObject.tag.Contains("Fruit"))
            {
                if (fruitType==collision.gameObject.GetComponent<Fruit>().fruitType&& fruitType!= FruitType.Eleven)
                {
                    //限制只执行一次合成
                    float thisPosxy = this.transform.position.x + this.transform.position.y;
                    float collisionPosxy = collision.transform.position.x + collision.transform.position.y;
                    if (thisPosxy > collisionPosxy)
                    {
                        //合成，在碰撞的位置出现新的大一号的水果，尺寸由小变大
                        //两个位置信息，fruitType
                        GameManager.gameManagerInstance.CombineNewFruit(fruitType,this.transform.position,collision.transform.position);
                        //scores
                        GameManager.gameManagerInstance.TotalScore += fruitScore;
                        GameManager.gameManagerInstance.totalScore.text = GameManager.gameManagerInstance.TotalScore.ToString();

                        Destroy(this.gameObject);
                        Destroy(collision.gameObject);

                    }
                    
            
                }
            }

        }
    }
   
}
