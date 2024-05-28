using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehavior : MonoBehaviour
{
    public static CardColor character = CardColor.Red;

    public void StartGame()
    {
        SceneManager.LoadScene("BattleScene");
    }
}
