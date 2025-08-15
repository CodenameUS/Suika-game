using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameClearUI : MonoBehaviour
{
    [SerializeField] private GameObject greatImage;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        greatImage.transform.localScale = Vector3.one * 4f;
        scoreText.text = GameManager.Instance.score.ToString();
        StartCoroutine(ScaleDown());
    }

    // 이미지 스케일 효과
    private IEnumerator ScaleDown()
    {
        float elapsed = 0f;
        float duration = 1.5f;
      
        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / duration;
            float scale = Mathf.Lerp(4f, 1.5f, t*t);
            greatImage.transform.localScale = Vector3.one * scale;

            yield return null;
        }

        greatImage.transform.localScale = Vector3.one * 1.5f;
    }

    // 게임 재시작
    public void Retry()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Sfx.Button);
        gameObject.SetActive(false);
        GameManager.Instance.RestartGame();
    }
}
