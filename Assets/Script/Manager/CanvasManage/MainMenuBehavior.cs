using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;
using TMPro;

public class MainMenuBehavior : MonoBehaviour
{
    public static int seed = 2;
    public static CardColor character = CardColor.Red;

    public GameObject continueButton;

    private string playerDataLocation = "/Datas/InGameData/playerData.json";

    public Toggle seedToggle;  // 是否开启种子按钮
    private bool usingInputSeed;
    public TMP_InputField seedInputField;  // 种子输入框


    private void Start()
    {
        // 检查是否需要生成继续按钮
        string path = Application.dataPath + playerDataLocation;
        if (File.Exists(path))
        {
            continueButton.gameObject.SetActive(true);
        }

        usingInputSeed = false;
        seedToggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    // 加载游戏
    public void StartGame()
    {
        SceneManager.LoadScene("BattleScene");
    }

    // 开始新游戏
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

        // 是否使用随机种子
        if (usingInputSeed)
        {
            seed = ConvertInputToSeed(seedInputField.text);
        }
        else
        {
            seed = 4;
        }

        SceneManager.LoadScene("BattleScene");
    }

    void OnToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("custom seed turned ON");
            usingInputSeed = true;
            seedInputField.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("custom seed turned OFF");
            usingInputSeed = false;
            seedInputField.gameObject.SetActive(false);
        }
    }

    int ConvertInputToSeed(string input)
    {
        int seed = 0;

        // Convert each character to its ASCII value or numeric value and sum them
        foreach (char c in input)
        {
            if (char.IsDigit(c))
            {
                seed += c - '0';  // Convert character '0'-'9' to integer 0-9
            }
            else if (char.IsLetter(c))
            {
                seed += c;  // Use the ASCII value of the letter (both uppercase and lowercase)
            }
        }

        return seed;
    }

    string ConvertSeedToString(int seed)
    {
        StringBuilder result = new StringBuilder();

        // Convert the integer back to a string based on digit and ASCII values
        while (seed > 0)
        {
            int value = seed % 128; // Use modulo to get a value in the ASCII range
            seed /= 128; // Shift to the next character

            if (value >= '0' && value <= '9') // Digits
            {
                result.Insert(0, (char)value); // Insert at the beginning of the string
            }
            else if ((value >= 'A' && value <= 'Z') || (value >= 'a' && value <= 'z')) // Letters
            {
                result.Insert(0, (char)value); // Insert at the beginning of the string
            }
            else
            {
                result.Insert(0, '_'); // Add an underscore or another symbol for invalid characters
            }
        }

        return result.ToString();
    }
}
