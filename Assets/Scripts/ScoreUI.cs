using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
                            << ScoreUI >>

        - ���� ���ھ� UI ����
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
