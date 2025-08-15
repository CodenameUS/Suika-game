using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                            << GameManager >>

        - ���� ���ھ� ����

        - ���� ���� �Ǵ�
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

    // ���� ����
    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        AudioManager.Instance.PlaySFX(AudioManager.Sfx.GameOver);
        // ... ���ӿ��� UI ó��
        gameOverUI.SetActive(true);
    }

    // ��ǥ�޼�
    public void GameClear()
    {
        if (isGameClear) return;

        isGameClear = true;

        AudioManager.Instance.PlaySFX(AudioManager.Sfx.GameOver);
        // ... ����Ŭ���� UI ó��
        gameClearUI.SetActive(true);
    }

    // ���� �����
    public void RestartGame()
    {
        // �����ʱ�ȭ
        score = 0;
        isGameClear = false;
        isGameOver = false;

        FruitManager.Instance.Reset();
    }
}
