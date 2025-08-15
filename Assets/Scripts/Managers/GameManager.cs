using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                            << GameManager >>

        - 게임 스코어 관리

        - 게임 오버 판단
 */


public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameClearUI;
    [SerializeField] private GameObject gameOverUI;

    public int score;
    public bool isGameOver = false;
    public bool isGameClear = false;
    public bool isPaused = false;

    private static GameManager instance;
    public static GameManager Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        Application.targetFrameRate = 60;
    }

    // 게임 오버
    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        AudioManager.Instance.PlaySFX(AudioManager.Sfx.GameOver);
        // ... 게임오버 UI 처리
        gameOverUI.SetActive(true);
    }

    // 목표달성
    public void GameClear()
    {
        if (isGameClear) return;

        isGameClear = true;

        AudioManager.Instance.PlaySFX(AudioManager.Sfx.GameOver);
        // ... 게임클리어 UI 처리
        gameClearUI.SetActive(true);
    }

    // 게임 재시작
    public void RestartGame()
    {
        // 점수초기화
        score = 0;
        isGameClear = false;
        isGameOver = false;

        FruitManager.Instance.Reset();
    }
}
