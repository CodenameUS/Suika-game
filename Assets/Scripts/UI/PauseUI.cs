using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;

    // 게임 일시정지
    public void Pause()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Sfx.Button);
        GameManager.Instance.isPaused = true;
        pauseUI.SetActive(true);
    }

    // 게임 재개
    public void Resume()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Sfx.Button);
        GameManager.Instance.isPaused = false;
        pauseUI.SetActive(false);
    }

    // 게임 종료
    public void Quit()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Sfx.Button);
        Application.Quit();
    }
}
