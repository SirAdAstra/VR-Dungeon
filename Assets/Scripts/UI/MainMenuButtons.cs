using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private AudioMixer audioMixer;

    public void StartBtn()
    {
        SceneManager.LoadScene(1);
    }

    public void SettingsBtn()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void SettingsBackBtn()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void ExitBtn()
    {
        Application.Quit();
    }

    public void ToggleMusicOn()
    {
        audioMixer.SetFloat("Music", 0f);
    }

    public void ToggleMusicOff()
    {
        audioMixer.SetFloat("Music", -80f);
    }

    public void ToggleSoundsOn()
    {
        audioMixer.SetFloat("Sound", 0f);
    }

    public void ToggleSoundsOff()
    {
        audioMixer.SetFloat("Sound", -80f);
    }

    public void SetDurLow()
    {
        PlayerPrefs.SetInt("room", 6);
        PlayerPrefs.SetInt("floor", 3);
    }

    public void SetDurNormal()
    {
        PlayerPrefs.SetInt("room", 8);
        PlayerPrefs.SetInt("floor", 4);
    }

    public void SetDurHigh()
    {
        PlayerPrefs.SetInt("room", 10);
        PlayerPrefs.SetInt("floor", 5);
    }

    public void MainMenuBtn()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartBtn()
    {
        SceneManager.LoadScene(1);
    }
}
