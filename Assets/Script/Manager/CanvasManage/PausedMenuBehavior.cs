using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausedMenuBehavior : MonoBehaviour
{
    public void OpenCanvas()
    {
        Time.timeScale = 0;
        InGameStateManager.gamePased = true;
        this.gameObject.SetActive(true);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Invoke("ReturnHelper", 0);
    }

    public void ReturnToGame()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1;
        InGameStateManager.gamePased = false;
    }

    void ReturnHelper()
    {
        InGameStateManager.gamePased = false;
        Time.timeScale = 1;
    }

}
