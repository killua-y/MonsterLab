using System.Collections.Generic;
using UnityEngine;

public class BoxLayout : MonoBehaviour
{
    private int rows = 4;
    private int columns = 5;
    public float spacing = 1.0f;
    public int number = 0; // Public variable to determine how many objects to instantiate
    public GameObject prefab; // Prefab to instantiate

    private List<TowerBoxBehavior> objectsToArrange = new List<TowerBoxBehavior>(); // List to hold the child objects

    void Start()
    {
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
            if ((box.row == 0 && box.column == 4) ||
                (box.row == 3 && box.column == 0))
            {
                continue;
            }

            validBoxes.Add(box);
        }

        // Create a list of box types with the specified counts
        List<BoxType> boxTypes = new List<BoxType>
        {
            BoxType.EliteFight, BoxType.EliteFight, BoxType.EliteFight,
            BoxType.NormalFight, BoxType.NormalFight, BoxType.NormalFight, BoxType.NormalFight, BoxType.NormalFight, BoxType.NormalFight,
            BoxType.Events, BoxType.Events, BoxType.Events, BoxType.Events, BoxType.Events, BoxType.Events,
            BoxType.Merchant, BoxType.Merchant,
            BoxType.Treasure
        };

        // Shuffle the list of box types
        System.Random random = new System.Random();
        int n = boxTypes.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            BoxType value = boxTypes[k];
            boxTypes[k] = boxTypes[n];
            boxTypes[n] = value;
        }

        // Assign the box types to the valid boxes
        for (int i = 0; i < validBoxes.Count; i++)
        {
            validBoxes[i].SetupBox(boxTypes[i]);
        }
    }
}
