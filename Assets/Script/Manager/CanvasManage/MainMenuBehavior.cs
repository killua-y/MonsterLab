using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehavior : MonoBehaviour
{
    private string playerDataLocation = "/Datas/InGameData/playerData.json";

    public static CardColor character = CardColor.Red;

    public void StartGame()
    {
        SceneManager.LoadScene("BattleScene");
    }

    public void StartNewGame()
    {
        string path = Application.dataPath + playerDataLocation;
        // Check if the file exists
        if (File.Exists(path))
        {
            // Delete the file
            File.Delete(path);
            path = path + ".meta";
            File.Delete(path);
            Debug.Log("File deleted at: " + path);
        }

        SceneManager.LoadScene("BattleScene");
    }
}
