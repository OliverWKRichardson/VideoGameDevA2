using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja_Player : MonoBehaviour
{
    private Vector3 pos;
    private int streak = 1;
    public int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    // Update is called once per frame
    void Update()
    {
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
        if (other.tag == "Fruit")
        {
            score += streak;
            Debug.Log("Score: " + score + " Streak: " + streak);
            streak++;
            other.gameObject.GetComponent<Fruit2D>().Hit("Fruit");
        }
        if (other.tag == "Enemy")
        {
            score -= 2;
            Debug.Log("Score: " + score + " Streak: " + streak);
            streak = 1;
            other.gameObject.GetComponent<Fruit2D>().Hit("Bomb");
        }
    }

    public void endStreak()
    {
        Debug.Log("Score: " + score + " Streak: " + streak);
        streak = 1;
    }
}