using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public int seed;
    public int xSize;
    public int ySize;
    private Vector2 offsetFromZero;
    private List<List<bool>> bool_array = new List<List<bool>>();
    public List<GameObject> prefab_list = new List<GameObject>();
    public GameObject borderPrefab;
    private GameObject currentPrefab;

    void Start()
    {
        Random.InitState(seed);
        offsetFromZero = new Vector2(xSize / 2 - 0.5f, ySize / 2 - 0.5f);
        generateFilledBoolArray();
        generateThings();
    }

    void generateFilledBoolArray()
    {
        for (int y = 0; y < ySize; y++)
        {
            List<bool> tempList = new List<bool>();
            for (int x = 0; x < xSize; x++)
            {
                tempList.Add(false);
            }
            bool_array.Add(tempList);
        }
    }
    void generateThings()
    {
        for (int h = 0; h < ySize; h++){
            for (int w = 0; w < xSize; w++){
                float new_h = h - offsetFromZero.y;
                float new_w = w - offsetFromZero.x;
                bool conditionToSpawn = Random.value > 0.5;
                currentPrefab = prefab_list[Random.Range(0, prefab_list.Count)];
                if (/*conditionToSpawn &&*/ bool_array[h][w] == false){
                    bool_array[h][w] = true;
                    int prefabX = currentPrefab.GetComponent<GeneratedItem>().xSize+1;             //+1 to exclude prefab 0,0 location - adds border.
                    int prefabY = currentPrefab.GetComponent<GeneratedItem>().ySize+1;             //Same for Y
                    bool generateBorder = currentPrefab.GetComponent<GeneratedItem>().generateBorder;
                    for (int y = -prefabY+1; y < prefabY; y++){
                        for (int x = -prefabX+1; x < prefabX; x++){
                            if (h + y > 0 && h + y < ySize)                                 //"Block" vertical edges in bool_array to avoid overlap
                            {
                                bool_array[h + y][w] = true;
                                if (generateBorder)
                                {
                                    Instantiate(borderPrefab, new Vector3(new_h + y, 0.1f, new_w), Quaternion.identity);
                                }
                            }
                            if (w + x > 0 && w + x < xSize)                                 //Same for horizontal edges
                            {
                                bool_array[h][w + x] = true;
                                if (generateBorder)
                                {
                                    Instantiate(borderPrefab, new Vector3(new_h, 0.1f, new_w + x), Quaternion.identity);
                                }
                            }
                            if (h + y > 0 && h + y < ySize && w + x > 0 && w + x < xSize)   //Same for diagonal edges - needed because we get diagonal rows with only 4 neighbor "blocking"
                            {
                                bool_array[h + y][w + x] = true;
                                if (generateBorder)
                                {
                                    Instantiate(borderPrefab, new Vector3(new_h + y, 0.1f, new_w + x), Quaternion.identity);
                                }
                            }

                        }
                    }
                    Instantiate(currentPrefab, new Vector3(new_h, 0.5f, new_w), Quaternion.identity);
                }
            }
        }
    }
}
