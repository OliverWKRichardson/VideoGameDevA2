using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja_Player : MonoBehaviour
{
    private Vector3 pos;
    private int streak = 0;
    private int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        // mobile screen setup
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    // Update is called once per frame
    void Update()
    {
        // movement
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount == 1)
            {
                pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 1));
                transform.position = new Vector3(pos.x, pos.y, 3);
                GetComponent<Collider2D>().enabled = true;
                return;
            }
            GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            transform.position = new Vector3(pos.x, pos.y, 3);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // score keeping
        if (other.tag == "Fruit")
        {
            streak++;
            score += streak;
            Debug.Log("Score: " + score + " Streak: " + streak);
            other.gameObject.GetComponent<Cuttable>().Hit();
        }
        if (other.tag == "Enemy")
        {
            streak = 0;
            score -= 2;
            Debug.Log("Score: " + score + " Streak: " + streak);
            other.gameObject.GetComponent<Cuttable>().Hit();
        }
    }

    public void endStreak()
    {
        streak = 0;
        Debug.Log("Score: " + score + " Streak: " + streak);
    }
}