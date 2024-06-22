using System.Collections.Generic;
using UnityEngine;

public class BoxLayout : MonoBehaviour
{
    private int rows = 4;
    private int columns = 5;
    public float spacing = 1.0f;
    public int number = 0; // Public variable to determine how many objects to instantiate
    public GameObject prefab; // Prefab to instantiate
    private PlayerBehavior player;

    private List<TowerBoxBehavior> objectsToArrange = new List<TowerBoxBehavior>(); // List to hold the child objects

    void Start()
    {
        player = FindAnyObjectByType<PlayerBehavior>();
        EnterNewLayer();
    }

    void EnterNewLayer()
    {
        foreach (TowerBoxBehavior towerbox in objectsToArrange)
        {
            Destroy(towerbox.gameObject);
        }

        // Instantiate new GameObjects if number is not zero
        if (number != 0)
        {
            for (int i = objectsToArrange.Count; i < number; i++)
            {
                GameObject newObject = Instantiate(prefab, transform);
                TowerBoxBehavior newBox = newObject.GetComponent<TowerBoxBehavior>();
                objectsToArrange.Add(newBox);
            }
        }

        if (objectsToArrange.Count > 0)
        {
            ArrangeGrid();
            GenerateBoxType(objectsToArrange);
        }
        else
        {
            Debug.LogWarning("No child GameObjects found to arrange.");
        }

        // 移动玩家位置到（3，0）
        player.transform.position = objectsToArrange.Find(obj => obj.row == 3 && obj.column == 0).transform.position;
    }

    void ArrangeGrid()
    {
        // Calculate the center offset
        float xOffset = (columns - 1) * spacing / 2;
        float yOffset = (rows - 1) * spacing / 2;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                int index = i * columns + j;
                if (index < objectsToArrange.Count)
                {
                    // Calculate the position
                    Vector3 position = new Vector3(j * spacing - xOffset, -i * spacing + yOffset, 0);
                    // Move the object to the calculated position
                    objectsToArrange[index].transform.localPosition = position;

                    // Assign row and column to the TowerBoxBehavior component
                    TowerBoxBehavior towerBox = objectsToArrange[index].GetComponent<TowerBoxBehavior>();
                    if (towerBox != null)
                    {
                        towerBox.row = i;
                        towerBox.column = j;
                    }
                    else
                    {
                        Debug.LogWarning("TowerBoxBehavior component not found on " + objectsToArrange[index].name);
                    }
                }
                else
                {
                    // If there are not enough objects, stop the loop
                    return;
                }
            }
        }
    }

    void GenerateBoxType(List<TowerBoxBehavior> allBox)
    {
        List<TowerBoxBehavior> validBoxes = new List<TowerBoxBehavior>();
        List<TowerBoxBehavior> remaingBoxes = new List<TowerBoxBehavior>();

        // Exclude specific boxes
        foreach (var box in allBox)
        {
            if (box.row == 0 && box.column == 4)
            {
                box.SetupBox(BoxType.BossFight);
                continue;
            }
            else if (box.row == 3 && box.column == 0)
            {
                box.SetupBox(BoxType.CannotGetTo);
                continue;
            }

            validBoxes.Add(box);
        }

        List<TowerBoxBehavior> boxs = new List<TowerBoxBehavior>();
        // Get two boxes from row 0, column 0 to 3
        boxs = GetRandomBoxesFromRange(0, 0, 0, 3, 2);
        boxs[0].SetupBox(BoxType.EliteFight);
        boxs[1].SetupBox(BoxType.Merchant);
        validBoxes.Remove(boxs[0]);
        validBoxes.Remove(boxs[1]);

        // Get two boxes from column 3, row 1 to 3
        boxs = GetRandomBoxesFromRange(1, 3, 3, 3, 2);
        boxs[0].SetupBox(BoxType.EliteFight);
        boxs[1].SetupBox(BoxType.Merchant);
        validBoxes.Remove(boxs[0]);
        validBoxes.Remove(boxs[1]);

        // Get two boxes from column 4, row 1 to 3
        boxs = GetRandomBoxesFromRange(1, 3, 4, 4, 2);
        boxs[0].SetupBox(BoxType.EliteFight);
        boxs[1].SetupBox(BoxType.Treasure);
        validBoxes.Remove(boxs[0]);
        validBoxes.Remove(boxs[1]);

        // Create a list of box types with the specified counts
        List<BoxType> boxTypes = new List<BoxType>
        {
            BoxType.NormalFight, BoxType.NormalFight, BoxType.NormalFight, BoxType.NormalFight, BoxType.NormalFight, BoxType.NormalFight,
            BoxType.Events, BoxType.Events, BoxType.Events, BoxType.Events, BoxType.Events, BoxType.Events,
        };

        HelperFunction.Shuffle(validBoxes, GameSetting.BoxLayoutRand);

        // Assign the box types to the valid boxes
        for (int i = 0; i < validBoxes.Count; i++)
        {
            validBoxes[i].SetupBox(boxTypes[i]);
        }
    }

    private TowerBoxBehavior FindBox(int row, int column)
    {
        foreach (var box in objectsToArrange)
        {
            if ((box.row == row) && (box.column == column))
            {
                return box;
            }
        }
        return null;
    }

    private List<TowerBoxBehavior> GetRandomBoxesFromRange(int rowStart, int rowEnd, int columnStart, int columnEnd, int count)
    {
        List<TowerBoxBehavior> selectedBoxes = new List<TowerBoxBehavior>();

        List<TowerBoxBehavior> rangeBoxes = new List<TowerBoxBehavior>();
        for (int row = rowStart; row <= rowEnd; row++)
        {
            for (int column = columnStart; column <= columnEnd; column++)
            {
                var box = FindBox(row, column);
                if (box != null)
                {
                    rangeBoxes.Add(box);
                }
            }
        }

        for (int i = 0; i < count; i++)
        {
            if (rangeBoxes.Count == 0)
            {
                break;
            }

            int randomIndex = GameSetting.BoxLayoutRand.Next(0, rangeBoxes.Count);
            selectedBoxes.Add(rangeBoxes[randomIndex]);
            rangeBoxes.RemoveAt(randomIndex);
        }

        HelperFunction.Shuffle(selectedBoxes, GameSetting.BoxLayoutRand);
        return selectedBoxes;
    }
}
