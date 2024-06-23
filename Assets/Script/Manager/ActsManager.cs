using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActsManager : Singleton<ActsManager>
{
    // 涉及玩家存档
    public static int currentLayer = 1;
    private bool startCurrentAct;
    private int generateCombatReward;
    public static BoxType currentBoxType;
    public static string currentEnemy = "AcidSlimeEnermy";
    public Action<int, int> OnPlayerMove;

    public GameObject MapCanvas;

    private List<Enemy> allEnemyList = new List<Enemy>();

    // 引用的script
    private ShopManager shopManager;
    private EventManager eventManager;
    private PlayerBehavior playerBehavior;
    private BoxLayout boxLayout;
    private SaveAndLoadManager saveAndLoadManager;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        shopManager = FindAnyObjectByType<ShopManager>();
        eventManager = FindAnyObjectByType<EventManager>();
        playerBehavior = FindAnyObjectByType<PlayerBehavior>();
        boxLayout = FindAnyObjectByType<BoxLayout>();
        saveAndLoadManager = FindAnyObjectByType<SaveAndLoadManager>();
    }

    public void LoadData(PlayerData playerData)
    {
        StartCoroutine(LoadDataHelper(playerData));
    }

    IEnumerator LoadDataHelper(PlayerData playerData)
    {
        LoadEnermyList();
        if (playerData.playerStates == null)
        {
            TowerBoxBehavior currentBox = boxLayout.FindBox(playerBehavior.row, playerBehavior.column);
            playerBehavior.transform.position = currentBox.transform.position;
        }
        else
        {
            TowerBoxBehavior currentBox = boxLayout.FindBox(playerBehavior.row, playerBehavior.column);
            playerBehavior.transform.position = currentBox.transform.position;
            if (playerData.nextAct.startCurrentAct)
            {
                yield return null;
                currentBox.ActivateAct();
            }
            else if (playerData.nextAct.generateCombatReward >= 0)
            {
                yield return null;
                generateCombatReward = playerData.nextAct.generateCombatReward;
                currentBoxType = currentBox.boxType;
                RewardManager.Instance.GenerateReward(generateCombatReward);
            }
            else
            {
                Debug.Log("Do nothing");
            }
        }
    }

    public NextAct SaveData()
    {
        return new NextAct
        {
            layer = currentLayer,
            startCurrentAct = startCurrentAct,
            generateCombatReward = generateCombatReward
        };
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
        startCurrentAct = true;
        saveAndLoadManager.SaveData();
        //_boxType = BoxType.Merchant;
        switch (_boxType)
        {
            case BoxType.NormalFight:
                currentBoxType = _boxType;
                currentEnemy = FindEnemy(currentLayer, EnemyType.Normal).scriptLocation;

                InGameStateManager.Instance.CombatStart();
                MapCanvas.SetActive(false);
                break;

            case BoxType.EliteFight:
                currentBoxType = _boxType;
                currentEnemy = FindEnemy(currentLayer, EnemyType.Elite).scriptLocation;

                InGameStateManager.Instance.CombatStart();
                MapCanvas.SetActive(false);
                break;

            case BoxType.BossFight:
                currentBoxType = _boxType;
                currentEnemy = FindEnemy(currentLayer, EnemyType.Boss).scriptLocation;

                InGameStateManager.Instance.CombatStart();
                MapCanvas.SetActive(false);
                break;

            case BoxType.Events:
                eventManager.LoadEvent(currentLayer);
                break;

            case BoxType.Merchant:
                shopManager.GenerateShop();
                break;

            case BoxType.Treasure:
                RewardManager.Instance.GenerateReward(0, 1);
                break;

            default:
                break;
        }
    }

    public void OnCombatEnd(int remainningTurns)
    {
        startCurrentAct = false;
        generateCombatReward = remainningTurns;
        saveAndLoadManager.SaveData();
        RewardManager.Instance.GenerateReward(remainningTurns);
    }

    public void LeaveScene()
    {
        startCurrentAct = false;
        generateCombatReward = -1;

        TowerBoxBehavior currentBox = boxLayout.FindBox(playerBehavior.row, playerBehavior.column);

        // 当进入下一层
        if (currentBox.boxType == BoxType.BossFight)
        {
            if (currentLayer >= 3)
            {
                Debug.Log("GameOver, You Win");
            }

            currentLayer += 1;

            boxLayout.EnterNewLayer();
        }

        saveAndLoadManager.SaveData();
    }
}
