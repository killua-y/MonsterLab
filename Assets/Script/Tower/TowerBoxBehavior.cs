using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TowerBoxBehavior : MonoBehaviour
{
    public TextMeshProUGUI boxTypeText;
    public BoxType boxType;
    public int row;
    public int column;
    private PlayerClimbing player;

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

    public void SetupBox(BoxType boxType)
    {
        boxTypeText.text = boxType.ToString();
    }


    public void OnPointDown()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerClimbing>();
        }

        // 查看是否可以移动到这
        if (CheckLegality())
        {
            player.transform.position = this.transform.position;
            player.row = row;
            player.column = column;
            //Invoke("ActivateAct", 1);
        }
    }

    private bool CheckLegality()
    {
        // 查看是否在玩家的上方或者右边一位index
        if(((player.row - 1 == row) && (player.column == column)) ||
            ((player.column + 1 == column) && (player.row == row)))
        {
            return true;
        }

        return false;
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
