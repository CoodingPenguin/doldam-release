using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    // 게임 스테이트 정의
    public enum GameState
    {
        START, SETTING, DEVELOP, PLAY, PAUSE, GAMEOVER
    };

    public enum TestEnvironment
    {
        PC, MOBILE
    };

    public GameState gameState;
    public TestEnvironment testEnvironment;

    public int feverState = 0;

    private float scrollSpeed = 10f;
    private float maxSpeed = 100f;

    public int score;
    public Text scoreText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void Start()
    {
        // 나중에 START로 바꿔 둘 것!
        // 지금은 Test용
        gameState = GameState.PLAY;
        score = 0;
    }

    public void AddScore(int plus)
    {
        score += plus;
        scoreText.text = score.ToString();
    }

    public float GetScrollSpeed()
    {
        return scrollSpeed;
    }

    public bool SetScrollSpeed(float speed)
    {
        if (speed > maxSpeed || speed < 0)
            return false;
        else
        {
            scrollSpeed = speed;
            return true;
        }
    }
}
