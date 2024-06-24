using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameSetting : MonoBehaviour
{
    public static System.Random cardRewardRand;
    public static System.Random CurrentActRand;

    public static float scaleFactor;

    // 用于生成敌人顺序，事件顺序，DNA顺序
    public static System.Random randForInitialize;

    public static int seed;

    protected void Awake()
    {
        AdjustForScreenResolution();
    }

    private void AdjustForScreenResolution()
    {
        float referenceWidth = 1920f;
        float referenceHeight = 1080f;

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float widthScale = screenWidth / referenceWidth;
        float heightScale = screenHeight / referenceHeight;

        scaleFactor = (widthScale + heightScale) / 2;
    }

    // Save the state of the random number generator
    public void LoadData(PlayerData playerData)
    {
        seed = playerData.Seed;
        randForInitialize = new System.Random(seed);
        if ((playerData.randomState == null) || (true))
        {
            cardRewardRand = new System.Random(seed);
        }
        else
        {
            cardRewardRand = InitializeRandom(seed, playerData.randomState.cardRewardRandNextValue);
        }
    }

    private System.Random InitializeRandom(int seed, int nextValue)
    {
        System.Random rand = new System.Random(seed);
        for (int i = 0; i < nextValue; i++)
        {
            rand.Next(); // Advance the random number generator to the correct state
        }
        return rand;
    }

    // Load the state of the random number generator
    public RandomState SaveData()
    {
        return new RandomState
        {
            cardRewardRandNextValue = GetNextValue(cardRewardRand)
        };
    }

    private int GetNextValue(System.Random rand)
    {
        return rand.Next(); // Generate and return the next random value
    }

    public void GenerateNewStepRand()
    {
        int InCombatRandIndex = ActsManager.currentLayer * 6 + ActsManager.step;
        int InCombatRandSeed = 0;
        System.Random rand = new System.Random(seed);
        for (int i = 0; i < InCombatRandIndex; i++)
        {
            InCombatRandSeed = rand.Next();
        }
        if (InCombatRandSeed != 0)
        {
            CurrentActRand = new System.Random(InCombatRandSeed);
        }
        else
        {
            Debug.Log("Did not go through loop");
        }
    }

    public System.Random GenerateNewRand(int index)
    {
        // 123分别为123层的地图rand
        // 
        System.Random rand = new System.Random(seed);
        System.Random resultRand;
        int randIndex = 0;

        for (int i = 0; i < index; i++)
        {
            randIndex = rand.Next();
        }

        if (randIndex != 0)
        {
            resultRand = new System.Random(randIndex);
        }
        else
        {
            Debug.Log("Did not go through loop for generate rand");
            return null;
        }

        return resultRand;
    }
}


[System.Serializable]
public class RandomState
{
    public int cardRewardRandNextValue;
}