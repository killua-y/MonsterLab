using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoadManager : MonoBehaviour
{
    public static SaveAndLoadManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void LoadData()
    {
        CardDataModel.Instance.LoadPlayerData();
    }

    public void SaveData()
    {
        CardDataModel.Instance.SavePlayerData();
    }

}
