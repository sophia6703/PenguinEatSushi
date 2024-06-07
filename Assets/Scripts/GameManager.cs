using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Default ready
//Click start to click the mouse to control the fruit position to StandBy,
//Release the mouse and the fruit falls, Inprogress
//After the fruit falls and collides with the floor or other fruits, roll back to StandBy,
//The fruit is out of the boundary, gameover
//After the game ends, delay 0.5s and start calculating score
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

    public static GameManager gameManagerInstance;//Static instances can be used in other classes

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

    //The type of fruit currently colliding cuttentSushiType
    //The position of the fruit currently colliding currentPos
    //The position of the other party colliding collisionPos
    //The synthesized fruit must have gravity
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
