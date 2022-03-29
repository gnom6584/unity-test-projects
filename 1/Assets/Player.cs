using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [Serializable]
    public struct ScoreSpeedMapValue
    {
        public int Score;
        public float Speed;
    }

    public UnityEvent<int> ScoreChanged;

    public UnityEvent<int> LifeChanged;

    public float ScopeSpeedTransitionTime;

    public ScoreSpeedMapValue[] ScoreSpeedMapper;

    public DirectionalPool Pool;

    public float InitSpeed;

    public int InitLifeCount = 3;

    public int InitScoreCount = 0;

    public Vector3 ForwardAxis;

    public Vector3 SideAxis;

    Rigidbody _rigidBody;

    float _inputSide;

    float _currentSpeed;

    int _currentScore;

    int _currentLife;

    Coroutine _coroutine;

    public void MoveSide(float modifier) => _inputSide = Mathf.Clamp(modifier, -1.0f, 1.0f);

    public void Start() {
        gameObject.SetActive(true);
        _rigidBody = GetComponent<Rigidbody>();

        if (_rigidBody == null)
            _rigidBody = gameObject.AddComponent<Rigidbody>();

        _rigidBody.useGravity = false;

        _currentSpeed = InitSpeed;
        _currentScore = InitScoreCount;
        _currentLife = InitLifeCount;

        ScoreChanged?.Invoke(_currentScore);
        LifeChanged?.Invoke(_currentLife);

        transform.position = Vector3.zero;
        
        Pool.InvalidateAdapter();
        
    }

    void Update()
    {
        _rigidBody.velocity = _currentSpeed * ForwardAxis + _currentSpeed * _inputSide * SideAxis;

        transform.Rotate(Time.deltaTime * 60.0f * _currentSpeed, 0f, 0f);

        _inputSide = 0f;
        Pool.Scroll = -transform.position.y;
    }

    IEnumerator IncreaseSpeedCoroutine(float targetSpeed, float duration)
    {
        float timer = 0;
        float startSpeed = _currentSpeed;
        while (timer < duration)
        {
            _currentSpeed = Mathf.Lerp(startSpeed, targetSpeed, timer);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    void IncreaseSpeed(float targetSpeed, float duration)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(IncreaseSpeedCoroutine(targetSpeed, duration));
    }

    void OnTriggerEnter(Collider collider)
    {
        var go = collider.gameObject;
        var obstacle = go.GetComponent<Obstacle>();

        obstacle?.Destroy();

        if (obstacle is KillObstacle killObstacle)
        {
            --_currentLife; 
            LifeChanged?.Invoke(_currentLife);
            if (_currentLife == 0)
                Destroy();
        }
        else if (obstacle is ScoreObstacle scoreObstacle)
        {
            ++_currentScore;
            ScoreChanged?.Invoke(_currentScore);
            ProcessScoreIncrease();

        }
    }

    void ProcessScoreIncrease()
    {
        foreach (var value in ScoreSpeedMapper)
            if (_currentScore == value.Score)
                IncreaseSpeed(value.Speed, ScopeSpeedTransitionTime);
    }

    void Destroy()
    {
        gameObject.SetActive(false);
    }
}
