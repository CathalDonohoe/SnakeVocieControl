using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{

    public GameObject snakePrefab;
    public Snake head;
    public Snake tail;
    public int NESW;
    public Vector2 nextPos;

    public int maxSize;
    public int currentSize;

    public int xBound;
    public int yBound;
    public int score;
    public Text scoreText;
    public GameObject foodPrefab;
    public GameObject currentFood;


    
    void OnEnable()
    {
        Snake.hit += hit;
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("TimerInvoke", 0, .5f);
        FoodFunc();
    }

    void OnDisable()
    {
        Snake.hit -= hit;
    }

    // Update is called once per frame
    void Update()
    {
        CompChangeD();
    }

    void TimerInvoke()
    {
        Movement();
        if(currentSize >= maxSize)
        {
            TailFunc();
        }
        
        else
        {
            currentSize ++;
        }
    }


    void Movement()
    {
        GameObject temp;
        nextPos = head.transform.position;
        switch (NESW)
        {
            case 0:
                nextPos = new Vector2(nextPos.x, nextPos.y + 1);
                break;
            case 1:
                nextPos = new Vector2(nextPos.x + 1, nextPos.y);
                break;
            case 2:
                nextPos = new Vector2(nextPos.x, nextPos.y - 1);
                break;
            case 3:
                nextPos = new Vector2(nextPos.x - 1, nextPos.y);
                break;
        }

        temp = (GameObject)Instantiate(snakePrefab, nextPos, transform.rotation);

        head.SetNext(temp.GetComponent<Snake>());
        head = temp.GetComponent<Snake>();
        return;

    }

    void CompChangeD(){

        if(NESW != 2 && Input.GetKeyDown(KeyCode.W))
        {
            NESW = 0;
        }

        if(NESW != 3 && Input.GetKeyDown(KeyCode.D))
        {
            NESW = 1;
        }

        if(NESW != 0 && Input.GetKeyDown(KeyCode.S))
        {
            NESW = 2;
        }

        if(NESW != 1 && Input.GetKeyDown(KeyCode.A))
        {
            NESW = 3;
        }
    }

    void TailFunc()
    {
        Snake tempSnake = tail;
        tail = tail.GetNext();
        tempSnake.RemoveTail();
    }

    void FoodFunc()
    {
        int xPos = Random.Range(-xBound, xBound);
        int yPos = Random.Range(-yBound, yBound);

        currentFood = (GameObject)Instantiate(foodPrefab, new Vector2(xPos, yPos), transform.rotation);

        StartCoroutine(CheckRender(currentFood));
    }

    IEnumerator CheckRender(GameObject IN)
    {
        yield return new WaitForEndOfFrame();
        if(IN.GetComponent<Renderer>().isVisible == false)
        {
            if(IN.tag == "Food")
            {
                Destroy(IN);
                FoodFunc();
            }
        }
    }

    void hit(string WhatIHit){
        if(WhatIHit == "Food")
        {
            FoodFunc();
            maxSize++;
            score++;
            scoreText.text = score.ToString();

            int temp = PlayerPrefs.GetInt("HighScore");
            if(score > temp)
            {
                PlayerPrefs.SetInt("HighScore", score);
            }
        }

        if(WhatIHit == "Snake")
        {
            CancelInvoke("TimerInvoke");
            Exit();
        }
    }


    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
