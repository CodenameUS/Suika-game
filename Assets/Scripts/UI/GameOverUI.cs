using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        scoreText.text = GameManager.Instance.score.ToString();
    }

    // 게임 재시작
    public void Retry()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Sfx.Button);
        gameObject.SetActive(false);
        GameManager.Instance.RestartGame();
    }
}
