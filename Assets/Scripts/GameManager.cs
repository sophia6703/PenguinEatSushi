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
    public GameObject[] sushiList;
    public GameObject bornSushiPosition;

    public GameObject startBtn;
    public GameObject optionPrefab;
    private GameObject optionPanel;
    public GameObject parent;

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
        highestScoreText.text = " " + highestScore;
        
        CreateSushi();
        gameState = GameState.StandBy;// step1
        startBtn.SetActive(false);
        
    }

    // delay sushi creation
    public void InvokeCreateSushi(float invokeTime)
    {
        Invoke("CreateSushi", invokeTime);
        

    }
    //waiting sushi dont have gravity
    public void CreateSushi()
    {
        
        int index = Random.Range(0,5);
        // The first five objects appear randomly
        if (sushiList.Length>=index&&sushiList[index]!=null)
        {
            GameObject sushiObj = sushiList[index];
            
            var currentSushi = Instantiate(sushiObj, bornSushiPosition.transform.position, sushiObj.transform.rotation);
            currentSushi.GetComponent<Sushi>().sushiState = SushiState.StandBy;//Sushi step1
        }

    }

    //当前碰撞的水果类型cuttentSushiType
    //当前碰撞的水果位置currentPos
    //碰撞的对方位置collisionPos
    //合成的水果要有重力
    public void CombineNewSushi(SushiType currentSushiType, Vector3 currentPos, Vector3 collisionPos)
    {
        Vector3 centerPos = (currentPos + collisionPos) / 2;
        int index = (int)currentSushiType +1;
        GameObject combineSushiObj = sushiList[index];
        var combineSushi = Instantiate(combineSushiObj, centerPos, combineSushiObj.transform.rotation);
        combineSushi.GetComponent<Rigidbody2D>().gravityScale = 1f;
        combineSushi.GetComponent<Sushi>().sushiState = SushiState.Collision;
        combineSushi.transform.localScale = combineScale;

        combineSource.Play();

    

    }

    public void OnclickOptionsButtonHandler()
    {
         if(!optionPanel)
         {
            optionPanel = Instantiate(optionPrefab,new Vector3(0,0,0),Quaternion.identity);
            optionPanel.transform.SetParent(parent.GetComponent<Transform>());
            optionPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
            optionPanel.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);


         }
         optionPanel.SetActive(true);
    }
}
