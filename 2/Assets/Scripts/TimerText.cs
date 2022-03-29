
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimerText : MonoBehaviour
{
    public Text Text; 

    public Game Game;

    // Update is called once per frame
    void Update()
    {
        Text.text = $"Время боя:\n{Mathf.RoundToInt(Game.PlayTime)} секунд";
    }
}
