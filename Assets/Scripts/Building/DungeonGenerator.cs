using System.Collections.Generic;

public class RoomsGenerator
{
    RoomNode rootNode;
    List<RoomNode> allNodesCollection = new List<RoomNode>();
    private int buildingWidth;
    private int buildingLength;

    public RoomsGenerator(int buildingWidth, int buildingLength)
    {
        this.buildingWidth = buildingWidth;
        this.buildingLength = buildingLength;
    }

    public (List<Node>, List<Node>) CalculateRooms(int maxIterations, int roomWidthMin, int roomLengthMin, int corridorWidth)
    {
        BinarySpacePartitioner bsp = new BinarySpacePartitioner(buildingWidth, buildingLength);
        allNodesCollection = bsp.PrepareNodesCollection(maxIterations, roomWidthMin, roomLengthMin);
        List<Node> roomSpaces = StructureHelper.ExtractLowestLeaves(bsp.RootNode);

        RoomGenerator roomGenerator = new RoomGenerator(maxIterations, roomLengthMin, roomWidthMin);
        List<RoomNode> roomList = roomGenerator.GenerateRooms(roomSpaces);

        DoorsGenerator doorsGenerator = new DoorsGenerator();
        var doorsList = doorsGenerator.CreateDoor(allNodesCollection, corridorWidth);

        return (new List<Node>(roomList), new List<Node>(doorsList));
    }
}
