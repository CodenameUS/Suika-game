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
    public int score;
    private bool isGameOver = false;
    private bool isGameClear = false;

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
    }

    // 게임 오버
    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        // ... 게임오버 UI 처리
    }

    // 목표달성
    public void GameClear()
    {
        if (isGameClear) return;

        isGameClear = true;

        // ... 게임클리어 UI 처리
        Debug.Log("게임 클리어!");
    }
}
