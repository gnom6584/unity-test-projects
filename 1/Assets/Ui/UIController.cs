using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject Restart;

    public Text ScoreText;
    
    public Text HealthText; 

    public void SetScore(int score)
    {
        ScoreText.text = $"Score: {score}";
    }

    public void SetHealth(int health)
    {
        HealthText.text = $"Health: {health}";
        if (health is 0)
            Restart.SetActive(true);
        else
            Restart.SetActive(false);
    }

}
