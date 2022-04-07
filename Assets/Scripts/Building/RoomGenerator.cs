using System;
using System.Collections.Generic;
using UnityEngine;

internal class RoomGenerator
{
    private int maxIterations;
    private int roomLengthMin;
    private int roomWidthMin;

    public RoomGenerator(int maxIterations, int roomLengthMin, int roomWidthMin)
    {
        this.maxIterations = maxIterations;
        this.roomLengthMin = roomLengthMin;
        this.roomWidthMin = roomWidthMin;
    }

    public List<RoomNode> GenerateRooms(List<Node> roomSpaces)
    {
        List<RoomNode> roomsList = new List<RoomNode>();
        foreach(var space in roomSpaces)
        {

            roomsList.Add((RoomNode)space);
        }
        return roomsList;
    }
}