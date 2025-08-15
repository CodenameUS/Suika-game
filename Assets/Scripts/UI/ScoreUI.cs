using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
                            << ScoreUI >>

        - 게임 스코어 UI 관리
 */

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Update()
    {
        ShowScore();
    }

    private void ShowScore()
    {
        scoreText.text = GameManager.Instance.score.ToString();
    }
}
