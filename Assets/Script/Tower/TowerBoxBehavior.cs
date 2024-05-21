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

    public void SetupBox(BoxType _boxType)
    {
        boxType = _boxType;
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
            ActivateAct(boxType);
        }
    }

    private bool CheckLegality()
    {
        // 查看是否在玩家的上方或者右边一位index
        if (((player.row - 1) == row) && (player.column == column))
        {
            return true;
        }
        else if ((player.row == row) && ((player.column + 1) == column))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ActivateAct(BoxType _boxType)
    {
        switch (_boxType)
        {
            case BoxType.NormalFight:
                SceneManager.LoadScene("BattleScene");
                break;

            case BoxType.EliteFight:
                SceneManager.LoadScene("BattleScene");
                break;

            case BoxType.BossFight:
                SceneManager.LoadScene("BattleScene");
                break;

            case BoxType.Events:
                RewardManager.Instance.GenerateReward();
                break;

            case BoxType.Merchant:
                RewardManager.Instance.GenerateReward();
                break;

            case BoxType.Treasure:
                RewardManager.Instance.GenerateReward();
                break;

            default:
                break;
        }
            
    }
}
