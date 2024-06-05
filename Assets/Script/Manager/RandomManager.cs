using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class RandomManager : Manager<RandomManager>
{
    public static System.Random cardRewardRand;
    public static System.Random DNARewardRand;
    public static System.Random shuffleCardRand;
    public static System.Random BoxLayoutRand;
    private int seed;

    private new void Awake()
    {
        base.Awake();
        if (cardRewardRand == null)
        {
            seed = 2;
            InitializeRand();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeRand()
    {
        cardRewardRand = new System.Random(seed);
        DNARewardRand = new System.Random(seed);
        shuffleCardRand = new System.Random(seed);
        BoxLayoutRand = new System.Random(seed);
    }


    // 暂时不用
    // Save the state of the random number generator
    public void SaveState(string filePath)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePath);

        RandomState state = new RandomState();
        state.Seed = seed;
        state.NextValue = cardRewardRand.Next();

        bf.Serialize(file, state);
        file.Close();
    }

    // Load the state of the random number generator
    public void LoadState(string filePath)
    {
        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);

            RandomState state = (RandomState)bf.Deserialize(file);
            file.Close();

            seed = state.Seed;
            cardRewardRand = new System.Random(seed);
            for (int i = 0; i < state.NextValue; i++)
            {
                cardRewardRand.Next();
            }

            // Re-shuffle lists using the seed

        }
        else
        {
            Debug.LogError("Save file not found");
        }
    }
}


[System.Serializable]
public class RandomState
{
    public int Seed;
    public int NextValue;
}