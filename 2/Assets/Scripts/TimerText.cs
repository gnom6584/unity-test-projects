
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
        Text.text = $"����� ���:\n{Mathf.RoundToInt(Game.PlayTime)} ������";
    }
}
