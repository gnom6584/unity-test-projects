using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    public Team Red;

    public Team Blue;

    public UnityEvent OnPlay;

    public UnityEvent OnStop;

    public UnityEvent OnRedWon;

    public UnityEvent OnBlueWon;

    public float PlayTime { private set; get; }

    bool _started;

    void Start()
    {
        Red = new Team
        {
            Color = Color.red
        };
        Blue = new Team
        {
            Color = Color.blue
        };
    }

    public void Play()
    {
        OnPlay?.Invoke();
        _started = true;
        var command = new CommandAttackNearby();
        Red.SendCommand(command);
        Blue.SendCommand(command);
        PlayTime = 0.0f;
    }

    void Stop()
    {
        OnStop?.Invoke();
        if (Red.Members.Count > Blue.Members.Count)
            OnRedWon?.Invoke();
        else if (Blue.Members.Count > Red.Members.Count)
            OnBlueWon?.Invoke();

        _started = false;
    }

    public void ClearTeams()
    {
        ClearTeam(Red);
        ClearTeam(Blue);
    }

    static void ClearTeam(Team team)
    {
        foreach(var member in team.Members.Select(it => it as MonoBehaviour))
            Destroy(member.gameObject);       
    }

    private void Update()
    {
        if (_started)
        {
            PlayTime += Time.deltaTime;

            if (Red.Members.Count is 0 || Blue.Members.Count is 0)
                Stop();
        }

    }
}
