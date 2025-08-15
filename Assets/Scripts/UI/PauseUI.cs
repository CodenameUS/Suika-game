using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;

    // ���� �Ͻ�����
    public void Pause()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Sfx.Button);
        GameManager.Instance.isPaused = true;
        pauseUI.SetActive(true);
    }

    // ���� �簳
    public void Resume()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Sfx.Button);
        GameManager.Instance.isPaused = false;
        pauseUI.SetActive(false);
    }

    // ���� ����
    public void Quit()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Sfx.Button);
        Application.Quit();
    }
}
