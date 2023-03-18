using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomsCreator : MonoBehaviour
{
    public int buildingWidth, buildingLength;
    public int roomWidthMin, roomLengthMin;
    public int maxIterations;
    public int doorWidth;

    public float scale;
    public int totalFloors;

    public GameObject wallVertical, wallHorizontal;
    public GameObject doorVertical, doorHorizontal;
    public GameObject windowVertical, windowHorizontal;

    public GameObject floorTile;
    public GameObject buildingSteps;
    public GameObject peakedRoof;
    [Range(0, 1)]
    public float windowProbability;

    List<Vector3> possibleDoorVerticalPosition;
    List<Vector3> possibleDoorHorizontalPosition;
    List<Vector3> possibleWallHorizontalPosition;
    List<Vector3> possibleWallVerticalPosition;

    List<GameObject> floorsList;

    private GameObject building;
    Vector2 doorToOutsideLocation;
    // Start is called before the first frame update
    void Start()
    {
        buildingWidth = Globals.buildingWidth;
        buildingLength = Globals.buildingLength;
        maxIterations = Globals.maxIterations;
        totalFloors = Globals.buildingFloors;
        floorsList = new List<GameObject>();
        CreateDungeon();

        GameEvents.current.onStartGame += DeactivateBuilding;
        GameEvents.current.onFoundationFilled += ActivateBuilding;
        GameEvents.current.onFoundationFilled += firstLevel;

        GameEvents.current.onResetLevel += reset;
        GameEvents.current.onStartSecondLevel += ActivateBuilding;

    }
    void reset()
    {

    }
    public void ActivateBuilding()
    {
        building.SetActive(true);
        for (int i = 1; i < floorsList.Count; i++)
        {
            floorsList[i].SetActive(true);
        }
    }
    public void DeactivateBuilding()
    {
        building.SetActive(false);
    }

    public void firstLevel()
    {
        floorsList[0].SetActive(true);
        for (int i = 1; i < floorsList.Count; i++)
        {
            floorsList[i].SetActive(false);
        }
    }

    public void secondLevel()
    {
        for (int i = 0; i < floorsList.Count; i++)
        {
            floorsList[i].SetActive(true);
        }
    }
    public void CreateDungeon()
    {
        DestroyAllChildren();

        doorToOutsideLocation = CalculateDoorToOutsideLocation();

        building = new GameObject("Building");

        for (int floor = 0; floor < totalFloors; floor++)
        {
            RoomsGenerator generator = new RoomsGenerator(buildingWidth, buildingLength);
            var (listOfRooms, listOfDoors) = generator.CalculateRooms(maxIterations,
                roomWidthMin,
                roomLengthMin,
                doorWidth);

            GameObject floorParent = new GameObject("Floor" + floor);
            GameObject wallParent = new GameObject("Walls");
            GameObject doorParent = new GameObject("Doors");
            GameObject roomFloors = new GameObject("FloorTiles");
            GameObject roofParent = new GameObject("Roof");

            Vector3 stairsPosition = new Vector3(Random.Range(0, buildingWidth), 0, Random.Range(0, buildingLength));

            possibleDoorVerticalPosition = new List<Vector3>();
            possibleDoorHorizontalPosition = new List<Vector3>();
            possibleWallHorizontalPosition = new List<Vector3>();
            possibleWallVerticalPosition = new List<Vector3>();

            for (int i = 0; i < listOfRooms.Count; i++)
            {
                CreateRoom(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner, listOfDoors, roomFloors, floor, stairsPosition);
                if (floor == totalFloors - 1)
                {
                    CreateRoof(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner, roofParent);
                }
            }
            CreateWalls(wallParent);
            CreateDoors(doorParent);

            wallParent.transform.parent = floorParent.transform;
            doorParent.transform.parent = floorParent.transform;
            roomFloors.transform.parent = floorParent.transform;
            roofParent.transform.parent = floorParent.transform;


            floorParent.transform.position += new Vector3(0, floor * scale, 0);
            floorParent.transform.SetParent(building.transform);
            floorParent.AddComponent<FloorController>();
            floorsList.Add(floorParent);
        }
        building.transform.parent = transform;
        building.transform.parent.localScale = new Vector3(2f, 2f, 2f);
        building.transform.localPosition = new Vector3((float)-(buildingWidth - 1) / 2 * 3, building.transform.localPosition.y, (float)-(buildingLength - 1) / 2 * 3);

    }
    private void CreateRoof(Vector2 bottomLeftCorner, Vector2 topRightCorner, GameObject parent)
    {
        int direction = Random.Range(0, 2);
        if (direction == 1)
        {
            float roomWidth = topRightCorner.x - bottomLeftCorner.x;
            float roomHeight = topRightCorner.y - bottomLeftCorner.y;
            GameObject roof = GameObject.Instantiate(peakedRoof);
            roof.transform.localScale = new Vector3(roomWidth, (roomWidth + roomHeight) / 2, roomHeight);
            roof.transform.position = new Vector3((bottomLeftCorner.x + roomWidth / 2) * scale - scale / 2, scale, (bottomLeftCorner.y + roomHeight / 2) * scale - scale / 2);
            roof.transform.parent = parent.transform;
        }
        else
        {
            float roomWidth = topRightCorner.x - bottomLeftCorner.x;
            float roomHeight = topRightCorner.y - bottomLeftCorner.y;
            GameObject roof = GameObject.Instantiate(peakedRoof);
            roof.transform.localScale = new Vector3(roomHeight, (roomWidth + roomHeight) / 2, roomWidth);
            roof.transform.position = new Vector3((bottomLeftCorner.x + roomWidth / 2) * scale - scale / 2, scale, (bottomLeftCorner.y + roomHeight / 2) * scale - scale / 2);
            roof.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            roof.transform.parent = parent.transform;
        }

    }

    private void CreateRoom(Vector2 bottomLeftCorner, Vector2 topRightCorner, List<Node> listOfCorridors, GameObject parent, int floor, Vector3 stairsLocation)
    {
        for (float x = bottomLeftCorner.x; x < topRightCorner.x; x++)
        {
            for (float z = bottomLeftCorner.y; z < topRightCorner.y; z++)
            {
                if (floor > 0 && Vector2.Distance(new Vector2(x, z), new Vector2(stairsLocation.x, stairsLocation.z)) < 0.5)
                {
                    GameObject.Instantiate(buildingSteps, new Vector3(x * scale, 0f * scale, z * scale), Quaternion.identity, parent.transform); //create floor tile
                }
                else
                {
                    GameObject.Instantiate(floorTile, new Vector3(x * scale, 0f * scale, z * scale), Quaternion.identity, parent.transform); //create floor tile
                }
            }
        }
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0, topRightCorner.y);
        for (float row = bottomLeftV.x; row < bottomRightV.x; row++)
        {
            Vector3 wallPosition = new Vector3(row * scale, 0f, bottomLeftV.z * scale - (scale / 2));
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition, listOfCorridors, floor);
        }
        for (float row = topLeftV.x; row < topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row * scale, 0, topRightV.z * scale - (scale / 2));
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition, listOfCorridors, floor);
        }
        for (float col = bottomLeftV.z; col < topLeftV.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftV.x * scale - (scale / 2), 0, col * scale);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition, listOfCorridors, floor);
        }
        for (float col = bottomRightV.z; col < topRightV.z; col++)
        {
            var wallPosition = new Vector3(bottomRightV.x * scale - (scale / 2), 0, col * scale);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition, listOfCorridors, floor);
        }
    }

    private void CreateWalls(GameObject wallParent)
    {
        foreach (var wallPosition in possibleWallHorizontalPosition)
        {
            float random = Random.Range(0f, 1f);
            CreateWall(wallParent, wallPosition, random < windowProbability ? windowHorizontal : wallHorizontal);
        }
        foreach (var wallPosition in possibleWallVerticalPosition)
        {
            float random = Random.Range(0f, 1f);
            CreateWall(wallParent, wallPosition, random < windowProbability ? windowVertical : wallVertical);
        }
    }

    private void CreateWall(GameObject wallParent, Vector3 wallPosition, GameObject wallPrefab)
    {
        Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
    }

    private void CreateDoors(GameObject wallParent)
    {
        foreach (var wallPosition in possibleDoorHorizontalPosition)
        {
            CreateDoor(wallParent, wallPosition, doorHorizontal);
        }
        foreach (var wallPosition in possibleDoorVerticalPosition)
        {
            CreateDoor(wallParent, wallPosition, doorVertical);
        }
    }

    private void CreateDoor(GameObject wallParent, Vector3 wallPosition, GameObject doorPrefab)
    {
        Instantiate(doorPrefab, wallPosition, Quaternion.identity, wallParent.transform);
    }

    private void AddWallPositionToList(Vector3 wallPosition, List<Vector3> wallList, List<Vector3> doorList, List<Node> listOfCorridors, int floor)
    {

        for (int i = 0; i < listOfCorridors.Count; i++)
        {
            if (Vector2.Distance(new Vector2(wallPosition.x, wallPosition.z), listOfCorridors[i].BottomLeftAreaCorner * 3) < scale || ((Vector2.Distance(new Vector2(wallPosition.x, wallPosition.z), doorToOutsideLocation) < 0.2) && floor == 0))
            {
                doorList.Add(wallPosition);
                wallList.Remove(wallPosition);
                return;
            }
        }
        if (!wallList.Contains(wallPosition))
        {
            wallList.Add(wallPosition);

        }
    }

    private Vector2 CalculateDoorToOutsideLocation()
    {
        int direction = Random.Range(0, 4);
        int location;
        Vector2 doorLocation;
        switch (direction)
        {
            case 0: //North
                location = Random.Range(0, buildingWidth);
                doorLocation = new Vector2(location * scale, -scale / 2);
                break;
            case 1: //South
                location = Random.Range(0, buildingWidth);
                doorLocation = new Vector2(location * scale, buildingLength * scale - scale / 2);
                break;
            case 2: //East
                location = Random.Range(0, buildingLength);
                doorLocation = new Vector2(-scale / 2, location * scale);
                break;
            case 3: //West
                location = Random.Range(0, buildingLength);
                doorLocation = new Vector2(buildingWidth * scale - scale / 2, location * scale);
                break;
            default:
                doorLocation = new Vector2(0, 1);
                break;
        }
        return doorLocation;

    }

    private void DestroyAllChildren()
    {
        while (transform.childCount != 0)
        {
            foreach (Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }
}