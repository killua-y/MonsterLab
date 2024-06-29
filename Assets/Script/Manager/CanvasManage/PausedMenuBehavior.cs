using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausedMenuBehavior : MonoBehaviour
{
    public void OpenCanvas()
    {
        Time.timeScale = 0;
        this.gameObject.SetActive(true);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void ReturnToGame()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
