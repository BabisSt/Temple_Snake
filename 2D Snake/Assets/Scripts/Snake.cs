using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Snake : MonoBehaviour
{

    public AudioSource audioData;
    public PauseMenu pauseMenu;
    public TMP_Text ScoreText;
    public TMP_Text HighScoreText;
    private int Score;
    private Vector2 direction = Vector2.right;
    public List<Transform> segments;
    public Transform _segmentPrefab;
    private Vector2 input;
    private SpriteRenderer mySpriteRenderer;

    
    private void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        segments = new List<Transform>();
        segments.Add(this.transform);

        HighScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

    private void Update()
    {
        InputNROtate();
    }
    private void FixedUpdate()
    {
       
        if (input != Vector2.zero)
        {
            direction = input;
        }


        //feed each segment of the snake in an array backwards, otherwise the get stuck on each other
        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
            segments[i].rotation = segments[i - 1].rotation;
        }

        
        //Moving - round because I dont want the snake to move freely
        float x = Mathf.Round(transform.position.x) + direction.x;
        float y = Mathf.Round(transform.position.y) + direction.y;

        transform.position = new Vector2(x, y);
    }


    //Growing 
    private void Grow()
    {
        Transform segment = Instantiate(this._segmentPrefab);
        segment.position = segments[segments.Count - 1].position - new Vector3(0, 0.2f, 0);    //position the new segment on the after last segment of the list
        segment.rotation = segments[segments.Count - 1].rotation;
        segments.Add(segment);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {
            Grow();
            AddScore();
            
            audioData.Play(0);
        }
      
        else if (other.tag == "Obstacle")
        {
            ResetState();
            pauseMenu.GameOver();
        }


    }

    private void InputNROtate()
    {
        //Movement - Cant move on the opposite direction of movement eg x/-x
        //here for x
        if (direction.x != 0f)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                input = Vector2.up;
               
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 90);
                segments[0].rotation = Quaternion.Euler(segments[0].rotation.eulerAngles.x, segments[0].rotation.eulerAngles.y, 90);

            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                input = Vector2.down;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -90);
                segments[0].rotation = Quaternion.Euler(segments[0].rotation.eulerAngles.x, segments[0].rotation.eulerAngles.y, -90);
            }
        }
        //and for y
        else if (direction.y != 0f)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                input = Vector2.right;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
                mySpriteRenderer.flipY = false;
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                input = Vector2.left;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 180);
                mySpriteRenderer.flipY = true;
            }
        }
    }

    private void ResetState()
    {
        //Clearing Snakes segments
        for (int i = 1; i < segments.Count; i++)
        {
            Destroy(segments[i].gameObject);
        }

        segments.Clear();
        segments.Add(this.transform); //Add head back to list

        this.transform.position = new Vector3(2, 0, 0); //Reset position of head
        Score = 0;
        ScoreText.text = "0";
    }

    void AddScore()
    {
        Score++;
        ScoreText.text = Score.ToString();

        if (Score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", Score);
            HighScoreText.text = Score.ToString();
        }
    }


}
