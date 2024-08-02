using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Card;

public class HelperFunction : MonoBehaviour
{
    // Generic method to convert a string to any enum type
    public static T ConvertToEnum<T>(string value) where T : struct
    {
        if (Enum.TryParse<T>(value, true, out T result))
        {
            return result;
        }
        else
        {
            throw new ArgumentException($"Invalid or non-existent enum value provided for type {typeof(T).Name}: {value}");
        }
    }

    public static Tile GetTileUnder()
    {
        RaycastHit2D hit =
            Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100, LayerMask.GetMask("Tile"));

        if (hit.collider != null)
        {
            //Released over something!
            Tile t = hit.collider.GetComponent<Tile>();
            return t;
        }

        return null;
    }

    // 洗list helper method
    public static void Shuffle<T>(List<T> list, System.Random rand)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    // return一个随机单位，不会把list重洗
    public static T GetRandomItem<T>(List<T> list, System.Random rand)
    {
        if (list == null || list.Count == 0)
        {
            throw new ArgumentException("The list is null or empty.");
        }

        int randomIndex = rand.Next(list.Count);
        return list[randomIndex];
    }

    public static Card LoadCard(string[] rowArray, List<string> keywords)
    {
        Card resultCard = null;
        if (rowArray[0] == "m")
        {
            int id = int.Parse(rowArray[1]);
            string cardName = rowArray[2];
            CardColor color = HelperFunction.ConvertToEnum<CardColor>(rowArray[3]);
            CardRarity cardRarity = HelperFunction.ConvertToEnum<CardRarity>(rowArray[4]);
            int cost = int.Parse(rowArray[5]);
            CastType castType = HelperFunction.ConvertToEnum<CastType>(rowArray[6]);
            int effectData = int.Parse(rowArray[7]);
            string effectText = rowArray[8];
            string scriptLocation = rowArray[9];
            string imageLocation = rowArray[10];

            int rank = int.Parse(rowArray[11]);
            MonsterType type = HelperFunction.ConvertToEnum<MonsterType>(rowArray[12]);
            int attackPower = int.Parse(rowArray[13]);
            int healthPoint = int.Parse(rowArray[14]);
            float attackRange = float.Parse(rowArray[15]);
            int mana = int.Parse(rowArray[16]);
            string modelLocation = rowArray[17];
            string skillScriptLocation = rowArray[18];
            string smallIconLocation = rowArray[19];

            resultCard = new MonsterCard(id, cardName, color, cardRarity,
                cost, castType, effectData, effectText, scriptLocation, imageLocation,
                rank, type, attackPower, healthPoint, attackRange, mana, modelLocation,
                skillScriptLocation, smallIconLocation);
        }
        else if (rowArray[0] == "s")
        {
            int id = int.Parse(rowArray[1]);
            string cardName = rowArray[2];
            CardColor color = HelperFunction.ConvertToEnum<CardColor>(rowArray[3]);
            CardRarity cardRarity = HelperFunction.ConvertToEnum<CardRarity>(rowArray[4]);
            int cost = int.Parse(rowArray[5]);
            CastType castType = HelperFunction.ConvertToEnum<CastType>(rowArray[6]);
            int effectData = int.Parse(rowArray[7]);
            string effectText = rowArray[8];
            string scriptLocation = rowArray[9];
            string imageLocation = rowArray[10];

            resultCard = new SpellCard(id, cardName, color, cardRarity, cost, castType, effectData,
                effectText, scriptLocation, imageLocation);

            //Debug.Log("Load magic card: " + name);
        }
        else if (rowArray[0] == "i")
        {
            int id = int.Parse(rowArray[1]);
            string cardName = rowArray[2];
            CardColor color = HelperFunction.ConvertToEnum<CardColor>(rowArray[3]);
            CardRarity cardRarity = HelperFunction.ConvertToEnum<CardRarity>(rowArray[4]);
            int cost = int.Parse(rowArray[5]);
            CastType castType = HelperFunction.ConvertToEnum<CastType>(rowArray[6]);
            int effectData = int.Parse(rowArray[7]);
            string effectText = rowArray[8];
            string scriptLocation = rowArray[9];
            string imageLocation = rowArray[10];

            resultCard = new ItemCard(id, cardName, color, cardRarity, cost, castType, effectData,
                effectText, scriptLocation, imageLocation);

            //Debug.Log("Load item card: " + name);
        }
        else
        {
            Debug.Log("Card cvs data error, the first string is : " + rowArray[0]);
        }

        resultCard.effectText = ColorKeywordsInString(resultCard.effectText, keywords);
        resultCard.keyWords = FindKeywordsInstring(resultCard.effectText, keywords);

        return resultCard;
    }

    public static DNA LoadDNA(string[] rowArray, List<string> keywords)
    {
        DNA resultDNA = null;

        int id = int.Parse(rowArray[1]);
        string DNAName = rowArray[2];
        CardColor NDAColor = HelperFunction.ConvertToEnum<CardColor>(rowArray[3]);
        CardRarity DNARarity = HelperFunction.ConvertToEnum<CardRarity>(rowArray[4]);
        int effectData = int.Parse(rowArray[5]);
        string effectText = rowArray[6];
        string scriptLocation = rowArray[7];
        string imageLocation = rowArray[8];
        resultDNA = new DNA(id, DNAName, NDAColor, DNARarity, effectData, effectText, scriptLocation, imageLocation);

        resultDNA.effectText = ColorKeywordsInString(resultDNA.effectText, keywords);
        resultDNA.keyWords = FindKeywordsInstring(resultDNA.effectText, keywords);

        return resultDNA;
    }

    // Helper method to find keywords at the start of a string
    public static List<string> FindKeywordsInstring(string input, List<string> keywords)
    {
        List<string> matchedKeywords = new List<string>();

        foreach (var keyword in keywords)
        {
            if (input.Contains(keyword))
            {
                matchedKeywords.Add(keyword);
            }
        }

        return matchedKeywords;
    }

    // Helper method to find keywords in a string and color them orange
    public static string ColorKeywordsInString(string input, List<string> keywords)
    {
        foreach (var keyword in keywords)
        {
            if (input.Contains(keyword))
            {
                input = input.Replace(keyword, $"<b><color=#FFA500>{keyword}</color></b>");
            }
        }
        return input;
    }

    // Helper method 给一张卡增加关键词
    public static Card AddKeyWordToCard(Card card, String keyWord)
    {
        if (card.effectText.Contains("Normal Monster"))
        {
            card.effectText = "";
        }

        card.effectText = $"<b><color=#FFA500>{keyWord}</color></b>" + " " + card.effectText;

        card.keyWords.Add(keyWord);
        return card;
    }
}
