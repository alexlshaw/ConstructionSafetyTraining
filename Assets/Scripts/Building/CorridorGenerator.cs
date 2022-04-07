using System;
using System.Collections.Generic;
using System.Linq;

internal class DoorsGenerator
{
    public List<Node> CreateDoor(List<RoomNode> allNodesCollection, int doorWidth)
    {
        List<Node> doorsList = new List<Node>();
        Queue<RoomNode> structuresToCheck = new Queue<RoomNode>(
            allNodesCollection.OrderByDescending(node => node.TreeLayerIndex).ToList());
        while (structuresToCheck.Count > 0)
        {
            var node = structuresToCheck.Dequeue();
            if (node.ChildrenNodeList.Count == 0)
            {
                continue;
            }
            DoorNode door = new DoorNode(node.ChildrenNodeList[0], node.ChildrenNodeList[1], doorWidth);
            doorsList.Add(door);
        }
        return doorsList;
    }
}