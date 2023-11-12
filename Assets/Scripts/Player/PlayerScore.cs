using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private int streak = 0;
    private int score = 0;

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