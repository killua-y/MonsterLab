using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehavior : MonoBehaviour
{
    public static int seed;
    public static CardColor character = CardColor.Red;

    public GameObject continueButton;

    private string playerDataLocation = "/Datas/InGameData/playerData.json";

    private void Start()
    {
        // 检查是否需要生成继续按钮
        string path = Application.dataPath + playerDataLocation;
        if (File.Exists(path))
        {
            continueButton.gameObject.SetActive(true);
        }
    }

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
        }

        SceneManager.LoadScene("BattleScene");
    }
}
