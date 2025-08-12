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

    // ���� ����
    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        // ... ���ӿ��� UI ó��
    }

    // ��ǥ�޼�
    public void GameClear()
    {
        if (isGameClear) return;

        isGameClear = true;

        // ... ����Ŭ���� UI ó��
        Debug.Log("���� Ŭ����!");
    }
}
