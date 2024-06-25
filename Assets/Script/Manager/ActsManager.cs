using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActsManager : Singleton<ActsManager>
{
    // 涉及玩家存档
    public static int currentLayer;
    public static int step;
    private bool startCurrentAct;
    private int generateCombatReward;
    public static BoxType currentBoxType;
    public static string currentEnemy = "AcidSlimeEnermy";
    public Action<int, int> OnPlayerMove;

    public GameObject MapCanvas;

    private List<Enemy> allEnemyList = new List<Enemy>();
    private List<Enemy> enemiesEncountered = new List<Enemy>();
    private List<string> eventEncountered = new List<string>();

    // 引用的script
    private ShopManager shopManager;
    private EventManager eventManager;
    private PlayerBehavior playerBehavior;
    private MapLayout mapLayout;
    private SaveAndLoadManager saveAndLoadManager;
    private GameSetting gameSetting;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        shopManager = FindAnyObjectByType<ShopManager>();
        eventManager = FindAnyObjectByType<EventManager>();
        playerBehavior = FindAnyObjectByType<PlayerBehavior>();
        mapLayout = FindAnyObjectByType<MapLayout>();
        saveAndLoadManager = FindAnyObjectByType<SaveAndLoadManager>();
        gameSetting = FindAnyObjectByType<GameSetting>();
    }

    public void LoadData(PlayerData playerData)
    {
        StartCoroutine(LoadDataHelper(playerData));
    }

    IEnumerator LoadDataHelper(PlayerData playerData)
    {
        LoadEnermyAndEventList(playerData);
        if (playerData.nextAct == null)
        {
            currentLayer = 1;
            step = 0;
            TowerBoxBehavior currentBox = mapLayout.FindBox(playerBehavior.row, playerBehavior.column);
            playerBehavior.transform.position = currentBox.transform.position;
        }
        else
        {
            currentLayer = playerData.nextAct.layer;
            step = playerData.nextAct.step;
            TowerBoxBehavior currentBox = mapLayout.FindBox(playerBehavior.row, playerBehavior.column);
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
                Debug.Log("No Act, Do nothing");
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

    private void LoadEnermyAndEventList(PlayerData playerData)
    {
        allEnemyList = CardDataModel.Instance.enemyList;
        HelperFunction.Shuffle(allEnemyList, GameSetting.randForInitialize);

        if (playerData.nextAct != null)
        {
            // 移除所有已经遇到过的敌人
            enemiesEncountered = playerData.nextAct.EnemiesEncountered;
            allEnemyList.RemoveAll(enemyB => enemiesEncountered.Exists(enemyA => enemyA.name == enemyB.name));
        }
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
        step += 1;
        startCurrentAct = true;
        saveAndLoadManager.SaveData();

        //_boxType = BoxType.Merchant;
        // 生成战斗需要的random
        gameSetting.GenerateNewStepRand();
        Enemy newEnemy;
        switch (_boxType)
        {
            case BoxType.NormalFight:
                currentBoxType = _boxType;
                newEnemy = FindEnemy(currentLayer, EnemyType.Normal);
                currentEnemy = newEnemy.scriptLocation;
                enemiesEncountered.Add(newEnemy);

                InGameStateManager.Instance.CombatStart();
                MapCanvas.SetActive(false);
                break;

            case BoxType.EliteFight:
                currentBoxType = _boxType;
                newEnemy = FindEnemy(currentLayer, EnemyType.Elite);
                currentEnemy = newEnemy.scriptLocation;
                enemiesEncountered.Add(newEnemy);

                InGameStateManager.Instance.CombatStart();
                MapCanvas.SetActive(false);
                break;

            case BoxType.BossFight:
                currentBoxType = _boxType;
                newEnemy = FindEnemy(currentLayer, EnemyType.Boss);
                currentEnemy = newEnemy.scriptLocation;
                enemiesEncountered.Add(newEnemy);

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
                RewardManager.Instance.GenerateDNAReward(CardRarity.Rare);
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

        TowerBoxBehavior currentBox = mapLayout.FindBox(playerBehavior.row, playerBehavior.column);

        // 当进入下一层
        if (currentBox.boxType == BoxType.BossFight)
        {
            if (currentLayer >= 3)
            {
                Debug.Log("GameOver, You Win");
            }

            currentLayer += 1;
            step = 0;

            mapLayout.EnterNewLayer();
        }

        saveAndLoadManager.SaveData();
    }
}
