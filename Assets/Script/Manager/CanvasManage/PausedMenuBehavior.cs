using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausedMenuBehavior : MonoBehaviour
{
    public TextMeshProUGUI seedText;
    private bool isOpen = false;

    public void OpenCanvas()
    {
        if (isOpen)
        {
            ReturnToGame();
            return;
        }

        isOpen = true;
        Time.timeScale = 0;
        this.gameObject.SetActive(true);
        UpdateSeed();
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void ReturnToGame()
    {
        isOpen = false;
        this.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    private void UpdateSeed()
    {
        string seedString = MainMenuBehavior.seedString;
        seedText.text = "seed: " + seedString;
    }
}
