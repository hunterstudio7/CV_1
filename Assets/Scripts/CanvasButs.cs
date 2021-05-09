using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasButs : MonoBehaviour
{
    public Sprite musicOn, musicOff;
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadInsta()
    {
        Application.OpenURL("https://www.instagram.com/hunterstudio7/");
    }
    public void MusicWork()
    {
        if (PlayerPrefs.GetString("music") == "No")
        {
            PlayerPrefs.SetString("music","Yes");
            GetComponent<Image>().sprite = musicOn;
        }
        else
        {
            PlayerPrefs.SetString("music", "No");
            GetComponent<Image>().sprite = musicOff;
        }


    }
    public void LoadShop()
    {
        SceneManager.LoadScene(1);

    }
    public void CloseShop()
    {
        SceneManager.LoadScene(0);

    }
}
