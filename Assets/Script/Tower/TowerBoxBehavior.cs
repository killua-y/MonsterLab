using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TowerBoxBehavior : MonoBehaviour
{
    public BoxType boxType;
    private GameObject player;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointDown()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        // 查看是否可以移动到这
        if (CheckLegality())
        {
            player.transform.position = this.transform.position;

            Invoke("ActivateAct", 1);
        }
    }

    private bool CheckLegality()
    {
        // 查看是否在玩家的下方或者右边

        return true;
    }

    private void ActivateAct()
    {
        int randomNumber = Random.Range(1, 11);

        SceneManager.LoadScene("BattleScene");
        // 20%战斗
        if ((randomNumber == 1) || (randomNumber == 2))
        {
            //
        }
    }
}
