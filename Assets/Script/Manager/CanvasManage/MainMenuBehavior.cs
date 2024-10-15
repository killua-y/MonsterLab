using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class MainMenuBehavior : MonoBehaviour
{
    public static int seed = 2;
    public static string seedString;
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
        seedInputField.onValueChanged.AddListener(ConvertToUppercase);
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
            seedString = ToBase36(seed);
        }
        else
        {
            GenerateBase36Seed();
        }

        SceneManager.LoadScene("BattleScene");
    }

    // 转化所以输入为大写
    void ConvertToUppercase(string input)
    {
        seedInputField.text = input.ToUpper();  // Set text to uppercase
    }

    // 种子生成部分
    void OnToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            //Debug.Log("custom seed turned ON");
            usingInputSeed = true;
            seedInputField.gameObject.SetActive(true);
        }
        else
        {
            //Debug.Log("custom seed turned OFF");
            usingInputSeed = false;
            seedInputField.gameObject.SetActive(false);
        }
    }

    public int ConvertInputToSeed(string code)
    {
        int seed = 0;
        int base36 = 36;

        // Ensure the input code is in lowercase to handle both 'a-z' uniformly
        code = code.ToLower();

        // Loop through each character in the input string
        for (int i = 0; i < code.Length; i++)
        {
            char c = code[i];

            // Determine the numerical value of the character
            int value;
            if (char.IsDigit(c))
            {
                value = c - '0';  // Convert '0'-'9' to 0-9
            }
            else if (char.IsLetter(c))
            {
                value = c - 'a' + 10;  // Convert 'a'-'z' to 10-35
            }
            else
            {
                throw new ArgumentException("Invalid character in seed code. Only 0-9 and a-z are allowed.");
            }

            // Accumulate the value by treating it as a 36-base number
            seed = seed * base36 + value;
        }

        return seed;
    }

    public void GenerateBase36Seed()
    {
        // Generate a random integer value within the range [0, int.MaxValue]
        int randomValue = UnityEngine.Random.Range(0, int.MaxValue);

        seed = randomValue;

        seedString = ToBase36(randomValue);
        Debug.Log("Generated Base-36 Seed: " + seedString);
    }


    // Convert an int value to a base-36 string
    private string ToBase36(int value)
    {
        int base36Length = 6;
        const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        string result = string.Empty;

        do
        {
            result = chars[value % 36] + result;
            value /= 36;
        } while (value > 0);

        // Convert the random number to a base-36 string and pad with leading zeros
        result = result.PadLeft(base36Length, '0');

        return result;
    }

}
