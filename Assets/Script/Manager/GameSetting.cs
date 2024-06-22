using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameSetting : MonoBehaviour
{
    public static System.Random cardRewardRand;
    public static System.Random DNARewardRand;
    public static System.Random InCombatRand;
    public static System.Random BoxLayoutRand;

    public static float scaleFactor;

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
        int seed = playerData.Seed;
        if ((playerData.randomState == null) || (true))
        {
            cardRewardRand = new System.Random(seed);
            DNARewardRand = new System.Random(seed);
            InCombatRand = new System.Random(seed);
            BoxLayoutRand = new System.Random(seed);
        }
        else
        {
            cardRewardRand = InitializeRandom(seed, playerData.randomState.cardRewardRandNextValue);
            DNARewardRand = InitializeRandom(seed, playerData.randomState.DNARewardRandNextValue);
            InCombatRand = InitializeRandom(seed, playerData.randomState.InCombatRandNextValue);
            BoxLayoutRand = InitializeRandom(seed, playerData.randomState.BoxLayoutRandNextValue);
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
            cardRewardRandNextValue = GetNextValue(cardRewardRand),
            DNARewardRandNextValue = GetNextValue(DNARewardRand),
            InCombatRandNextValue = GetNextValue(InCombatRand),
            BoxLayoutRandNextValue = GetNextValue(BoxLayoutRand)
        };
    }

    private int GetNextValue(System.Random rand)
    {
        return rand.Next(); // Generate and return the next random value
    }
}


[System.Serializable]
public class RandomState
{
    public int cardRewardRandNextValue;
    public int DNARewardRandNextValue;
    public int InCombatRandNextValue;
    public int BoxLayoutRandNextValue;
}