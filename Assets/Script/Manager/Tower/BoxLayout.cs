using System.Collections.Generic;
using UnityEngine;

public class BoxLayout : MonoBehaviour
{
    public int rows = 5;
    public int columns = 5;
    public float spacing = 1.0f;
    public int number = 0; // Public variable to determine how many objects to instantiate
    public GameObject prefab; // Prefab to instantiate

    private List<GameObject> objectsToArrange = new List<GameObject>(); // List to hold the child objects

    void Start()
    {
        // Populate the list with the existing child GameObjects
        foreach (Transform child in transform)
        {
            objectsToArrange.Add(child.gameObject);
        }

        // Instantiate new GameObjects if number is not zero
        if (number != 0)
        {
            for (int i = objectsToArrange.Count; i < number; i++)
            {
                GameObject newObject = Instantiate(prefab, transform);
                objectsToArrange.Add(newObject);
            }
        }

        if (objectsToArrange.Count > 0)
        {
            ArrangeGrid();
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
                }
                else
                {
                    // If there are not enough objects, stop the loop
                    return;
                }
            }
        }
    }
}
