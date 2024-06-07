using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SushiType
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
//Default :Ready
//Standby and mouse control to move position: StandBy
//Dropping
//Colliding with the floor or other fruits: Collision
//
public enum SushiState
{
    Ready = 0,
    StandBy = 1,
    Dropping = 2,
    Collision = 3,
}


public class Sushi : MonoBehaviour
{
    //Public variables or parameters in Unity scripts can be modified visually in the Unity engine, Inspector view
    public SushiType sushiType = SushiType.One;

    private bool IsMove = false;

    public SushiState sushiState = SushiState.Ready;

    public float limit_x = 2f;
    public Vector3 originalScale = Vector3.zero;
    public float scaleSpeed = 0.1f;

    public float sushiScore = 1f;

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
        
        //Game status StandBy & fruit status StandBy, you can click the mouse to control the movement, and release the mouse to fall.
        if (GameManager.gameManagerInstance.gameState == GameState.StandBy && sushiState == SushiState.StandBy)
        {
            if (Input.GetMouseButtonDown(0))
            {
                IsMove = true;
            }
            //Release the mouse
            if (Input.GetMouseButtonUp(0)&&IsMove)
            {
                IsMove = false;
                // change the gravity , the object falling
                this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
                sushiState = SushiState.Dropping;//Sushi step2
                GameManager.gameManagerInstance.gameState = GameState.Inprogress; //Step2

                // create new waiting object
                GameManager.gameManagerInstance.InvokeCreateSushi(0.5f);
            }
            if (IsMove)
            {
                // move the object
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//Convert screen coordinates to unity world coordinates
                this.gameObject.GetComponent<Transform>().position = new Vector3(mousePos.x, this.gameObject.GetComponent<Transform>().position.y, this.gameObject.GetComponent<Transform>().position.z);
            }

        }
        //limit x
        if (this.transform.position.x > limit_x)
        {
            this.transform.position = new Vector3(limit_x, this.transform.position.y, this.transform.position.z);
        }
    
        if (this.transform.position.x < -limit_x)
        {
            this.transform.position = new Vector3(-limit_x, this.transform.position.y, this.transform.position.z);
        }

        //Size recovery
        if (this.transform.localScale.x < originalScale.x)
        {
            this.transform.localScale += new Vector3 (1,1,1) * scaleSpeed;
        }
        if (this.transform.localScale.x > originalScale.x)
        {
            this.transform.localScale = originalScale;
        }



    }

    
    
    //When sushi's game object collides with Collision
    //continue to supervise
    //There is collision detection on each fruit

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (sushiState == SushiState.Dropping)
        {
            //Collision with Floor
            if (collision.gameObject.tag.Contains("Floor"))
            {
                GameManager.gameManagerInstance.gameState = GameState.StandBy;
                sushiState = SushiState.Collision;// Sushi Step3

                GameManager.gameManagerInstance.hitSource.Play();
            }
            //Collision with sushi
            if (collision.gameObject.tag.Contains("Sushi"))
            {
                GameManager.gameManagerInstance.gameState = GameState.StandBy;
                sushiState = SushiState.Collision;// Sushi Step3
            }

        }
        
        
        
        
        
        //Dropping, Collision

        if ((int)sushiState>=(int)SushiState.Dropping)
        {
            if (collision.gameObject.tag.Contains("Sushi"))
            {
                if (sushiType==collision.gameObject.GetComponent<Sushi>().sushiType&& sushiType!= SushiType.Eleven)
                {
                    //Limit the synthesis to one execution
                    float thisPosxy = this.transform.position.x + this.transform.position.y;
                    float collisionPosxy = collision.transform.position.x + collision.transform.position.y;
                    if (thisPosxy > collisionPosxy)
                    {
                        //Synthesis, a new larger sushi appears at the collision position, and the size changes from small to large
                        //Two location information, sushiType
                        GameManager.gameManagerInstance.CombineNewSushi(sushiType,this.transform.position,collision.transform.position);
                        //scores
                        GameManager.gameManagerInstance.TotalScore += sushiScore;
                        GameManager.gameManagerInstance.totalScore.text = GameManager.gameManagerInstance.TotalScore.ToString();

                        Destroy(this.gameObject);
                        Destroy(collision.gameObject);

                    }
                    
            
                }
            }

        }
    }
   
}
