using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActsManager : Manager<ActsManager>
{
    // 涉及玩家存档
    public static int currentLayer = 1;
    public static EnemyType currentEnemyType;
    public static string currentEnemy = "AcidSlimeEnermy";

    public GameObject MapCanvas;
    public GameObject EventCanvas;

    private List<Enemy> allEnemyList = new List<Enemy>();

    // Start is called before the first frame update
    void Start()
    {
        LoadEnermyList();
    }

    private void LoadEnermyList()
    {
        allEnemyList = CardDataModel.Instance.enemyList;
    }

    private Enemy FindEnemy(int _layer, EnemyType _enemyType)
    {
        Enemy enemyFind = null;
        for (int i = 0; i < allEnemyList.Count; i++)
        {
            if ((allEnemyList[i].layer == _layer) && (allEnemyList[i].enemyType == _enemyType))
            {
                enemyFind = allEnemyList[i];
                allEnemyList.RemoveAt(i);
            }
        }
        return enemyFind;
    }

    public void ActivateAct(BoxType _boxType)
    {
        switch (_boxType)
        {
            case BoxType.NormalFight:
                currentEnemyType = EnemyType.Normal;
                currentEnemy = FindEnemy(currentLayer, EnemyType.Normal).scriptLocation;

                InGameStateManager.Instance.GameStart();
                MapCanvas.SetActive(false);
                break;

            case BoxType.EliteFight:
                currentEnemyType = EnemyType.Elite;
                currentEnemy = FindEnemy(currentLayer, EnemyType.Elite).scriptLocation;

                InGameStateManager.Instance.GameStart();
                MapCanvas.SetActive(false);
                break;

            case BoxType.BossFight:
                currentEnemyType = EnemyType.Boss;
                currentEnemy = FindEnemy(currentLayer, EnemyType.Boss).scriptLocation;

                InGameStateManager.Instance.GameStart();
                MapCanvas.SetActive(false);
                break;

            case BoxType.Events:
                EventCanvasBehavior.instance.LoadEvent(currentLayer);
                break;

            case BoxType.Merchant:
                CanvasManager.Instance.OpenShopCanvas();
                break;

            case BoxType.Treasure:
                RewardManager.Instance.GenerateReward(0, 1);
                break;

            default:
                break;
        }
    }

    public void LeaveEvent()
    {

    }
}
