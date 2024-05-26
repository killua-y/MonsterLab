using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatesManager : Manager<PlayerStatesManager>
{
    public TextMeshProUGUI playerHealthText;

    public static int maxCost = 3;
    public static int playerHealthPoint = 3;
    public static int maxUnit = 5;

    private List<DNA> playerDNAList = new List<DNA>();

    // Start is called before the first frame update
    void Start()
    {

        playerDNAList = CardDataModel.Instance.GetPlayerDNA();
    }
}
