using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActsManager : Singleton<ActsManager>
{
    // enemy和event数据的存储csv
    private string textEnemyAndEventDataPath = "/Datas/cardsdata - EventAndEnemy.csv";

    // 涉及玩家存档
    private string playerDataLocation = "/Datas/InGameData/playerData.json";
    public static int currentLayer;
    public static int step;
    private bool startCurrentAct;
    private int generateCombatReward;
    public static BoxType currentBoxType;
    public static string currentEnemy;
    public Action<int, int> OnPlayerMove;

    public GameObject MapCanvas;

    private List<Enemy> allEnemyList = new List<Enemy>();
    private List<Enemy> enemiesEncountered = new List<Enemy>();
    private List<QuestionMarkEvent> allEventList = new List<QuestionMarkEvent>();
    private List<QuestionMarkEvent> eventsEncountered = new List<QuestionMarkEvent>();

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
            step = step,
            startCurrentAct = startCurrentAct,
            generateCombatReward = generateCombatReward,
            EnemiesEncountered = enemiesEncountered,
            EventEncountered = eventsEncountered
        };
    }

    private void LoadEnermyAndEventList(PlayerData playerData)
    {
        string path = Application.dataPath + textEnemyAndEventDataPath;
        string[] dataArray = File.ReadAllLines(path);
        foreach (var row in dataArray)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "#")
            {
                continue;
            }
            else if (rowArray[0] == "enemy")
            {
                string name = rowArray[2];
                int layer = int.Parse(rowArray[3]);
                EnemyType enemyType = HelperFunction.ConvertToEnum<EnemyType>(rowArray[4]);
                string scriptLocation = rowArray[5];
                bool easy = false;
                if (rowArray[6] == "TRUE")
                {
                    easy = true;
                }
                allEnemyList.Add(new Enemy(name, layer, enemyType, scriptLocation, easy));
            }
            else if (rowArray[0] == "event")
            {
                string name = rowArray[2];
                int layer = int.Parse(rowArray[3]);
                string scriptLocation = rowArray[4];

                allEventList.Add(new QuestionMarkEvent(EventType.Event, name, layer, scriptLocation));
            }
            else if (rowArray[0] == "baseUnitEvent")
            {
                string name = rowArray[2];
                int layer = int.Parse(rowArray[3]);
                string scriptLocation = rowArray[4];

                allEventList.Add(new QuestionMarkEvent(EventType.BaseUnitEvent, name, layer, scriptLocation));
            }
            else
            {
                Debug.Log("Undefined line detected, start with : " + rowArray[0]);
            }
        }

        HelperFunction.Shuffle(allEnemyList, GameSetting.randForInitialize);
        HelperFunction.Shuffle(allEventList, GameSetting.randForInitialize);

        if (playerData.nextAct != null)
        {
            // 移除所有已经遇到过的敌人
            enemiesEncountered = playerData.nextAct.EnemiesEncountered;
            allEnemyList.RemoveAll(enemyB => enemiesEncountered.Exists(enemyA => enemyA.name == enemyB.name));
            // 移除所有已经遇到过的事件
            eventsEncountered = playerData.nextAct.EventEncountered;
            allEventList.RemoveAll(eventB => eventsEncountered.Exists(eventA => eventA.name == eventB.name));
        }
    }

    // 寻找一个当前layer的enemy
    private Enemy FindEnemy(int _layer, EnemyType _enemyType)
    {
        Enemy enemyFind = null;

        // 为前两步遇到的普通敌人寻找简单的敌人
        if ((_enemyType == EnemyType.Normal) && (step <= 2))
        {
            for (int i = 0; i < allEnemyList.Count; i++)
            {
                if ((allEnemyList[i].layer == _layer) && (allEnemyList[i].enemyType == _enemyType) && allEnemyList[i].easy)
                {
                    enemyFind = allEnemyList[i];
                    allEnemyList.RemoveAt(i);
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < allEnemyList.Count; i++)
            {
                if ((allEnemyList[i].layer == _layer) && (allEnemyList[i].enemyType == _enemyType))
                {
                    enemyFind = allEnemyList[i];
                    allEnemyList.RemoveAt(i);
                    break;
                }
            }
        }

        // 如果所有普通敌人都被打过了
        // 那就把这一层的遇到的普通敌人都添加回list
        // 后期敌人数据库多了可以把这部分删了
        if (enemyFind == null)
        {
            Debug.Log("You have killed all normal enemy this layer");
            foreach (Enemy enemy in enemiesEncountered)
            {
                if ((enemy.enemyType == _enemyType) && (enemy.layer == _layer))
                {
                    allEnemyList.Add(enemy);
                    enemiesEncountered.Remove(enemy);
                }
            }
            HelperFunction.Shuffle(allEnemyList, GameSetting.randForInitialize);
            enemyFind = FindEnemy(_layer, _enemyType);
        }

        return enemyFind;
    }

    // 寻找一个当前layer的event
    private QuestionMarkEvent FindEvent(EventType eventType, int _layer)
    {
        QuestionMarkEvent eventFind = null;

        for (int i = 0; i < allEventList.Count; i++)
        {
            if ((allEventList[i].eventType == eventType) && (allEventList[i].layer == _layer))
            {
                eventFind = allEventList[i];
                allEventList.RemoveAt(i);
                break;
            }
        }

        return eventFind;
    }

    public void ActivateAct(BoxType _boxType)
    {
        startCurrentAct = true;
        saveAndLoadManager.SaveData();

        //_boxType = BoxType.Merchant;
        // 生成战斗需要的random
        step += 1;
        gameSetting.GenerateNewStepRand();
        Enemy newEnemy;
        switch (_boxType)
        {
            case BoxType.NormalFight:
                currentBoxType = _boxType;
                newEnemy = FindEnemy(currentLayer, EnemyType.Normal);
                currentEnemy = newEnemy.scriptLocation;
                currentEnemy = "BombCarrierEnemy";
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
                QuestionMarkEvent currentEvent = FindEvent(EventType.Event, currentLayer);
                eventsEncountered.Add(currentEvent);
                eventManager.LoadEvent(currentEvent.scriptLocation);
                break;

            case BoxType.BaseUnitEvents:
                QuestionMarkEvent currentBaseUnitEvent = FindEvent(EventType.BaseUnitEvent, currentLayer);
                eventsEncountered.Add(currentBaseUnitEvent);
                eventManager.LoadEvent(currentBaseUnitEvent.scriptLocation);
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
            if (currentLayer >= 2)
            {
                Debug.Log("GameOver, You Win");
                CanvasManager.Instance.ShowIndicationText("GameOver, You Win");
            }

            currentLayer += 1;
            step = 0;

            mapLayout.EnterNewLayer();
        }

        saveAndLoadManager.SaveData();
    }

    public void GameOver()
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

        SceneManager.LoadScene("MainMenu");
    }
}
