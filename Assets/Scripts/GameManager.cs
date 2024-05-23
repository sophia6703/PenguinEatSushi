using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//默认 ready
//点击开始到鼠标点击控制水果位置为StandBy，
//松开鼠标水果跌落，Inprogress
//水果跌落碰撞到地板或者其他水果之后，回滚StandBy,
//水果超出边界，gameover
//游戏结束之后，延迟0.5s,开始calculatescore
public enum GameState
{
    Ready = 0,
    StandBy = 1,//StandBy~Inprogress
    Inprogress = 2,//Inprogress~Standby
    GameOver = 3,
    CalculateScore = 4,
}

public class GameManager : MonoBehaviour
{
    public GameObject[] fruitList;
    public GameObject bornFruitPosition;

    public GameObject startBtn;

    public static GameManager gameManagerInstance;//静态的实例，可以在别的类使用

    public GameState gameState = GameState.Ready;

    public Vector3 combineScale = new Vector3(0,0,0);

    public float TotalScore = 0f;
    public Text totalScore;
    public Text highestScoreText;

    public AudioSource combineSource;
    public AudioSource hitSource;

    void Awake()
    {
        gameManagerInstance = this;

    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartGame()
    {
        // Game Strat
        Debug.Log("start");

        float highestScore = PlayerPrefs.GetFloat("HighestScore");
        highestScoreText.text = "Highest Score: "+ highestScore;
        
        CreateFruit();
        gameState = GameState.StandBy;// step1
        startBtn.SetActive(false);
        
    }

    // delay fruit creation
    public void InvokeCreateFruit(float invokeTime)
    {
        Invoke("CreateFruit", invokeTime);
    }
    //waiting fruit dont have gravity
    public void CreateFruit()
    {
        int index = Random.Range(0,5);
        // The first five objects appear randomly
        if (fruitList.Length>=index&&fruitList[index]!=null){
            GameObject fruitObj = fruitList[index];
            
            var currentFruit = Instantiate(fruitObj, bornFruitPosition.transform.position, fruitObj.transform.rotation);
            currentFruit.GetComponent<Fruit>().fruitState = FruitState.StandBy;//Fruit step1
        }

    }

    //当前碰撞的水果类型cuttentFruitType
    //当前碰撞的水果位置currentPos
    //碰撞的对方位置collisionPos
    //合成的水果要有重力
    public void CombineNewFruit(FruitType currentFruitType, Vector3 currentPos, Vector3 collisionPos)
    {
        Vector3 centerPos = (currentPos + collisionPos) / 2;
        int index = (int)currentFruitType +1;
        GameObject combineFruitObj = fruitList[index];
        var combineFruit = Instantiate(combineFruitObj, centerPos, combineFruitObj.transform.rotation);
        combineFruit.GetComponent<Rigidbody2D>().gravityScale = 1f;
        combineFruit.GetComponent<Fruit>().fruitState = FruitState.Collision;
        combineFruit.transform.localScale = combineScale;

        combineSource.Play();

    

    }
}
