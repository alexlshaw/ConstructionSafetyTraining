using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class PoissonDiscSampling
{
    public static (List<Vector3>, List<Point>) GeneratePointsOfDifferentSize(Vector2 sampleRegionSize, GeneratedItem[] generatedItems, int numSamplesBeforeRejection = 30)
    {
        // use the min of the radius range for our cell size
        int count = 0;
        List<Vector2> spawnPoints = new List<Vector2>(); // a place we'll try to spawn points around
        List<Vector3> pointsPlusRadius = new List<Vector3>(); // what we'll use to keep track of the points radii
        List<GeneratedItem> itemsToGenerate = generatedItems.ToList();
        List<Point> itemPoints = new List<Point>();
        spawnPoints.Add(sampleRegionSize / 2); // add first spawn point in the center

        //float radius = Mathf.Max(itemsToGenerate[itemsToGenerate.Count-1].xSize, itemsToGenerate[itemsToGenerate.Count - 1].ySize);
        //pointsPlusRadius.Add(new Vector3(sampleRegionSize.x / 2, sampleRegionSize.y / 2, radius));
        //itemsToGenerate.RemoveAt(itemsToGenerate.Count - 1);
        //count += 1;

        while (spawnPoints.Count > 0 && itemsToGenerate.Count > 0)
        {
            int itemSpawnIndex = Random.Range(0, itemsToGenerate.Count);

            int spawnIndex = Random.Range(0, spawnPoints.Count); // pick random guy to spawn
            Vector2 spawnCentre = spawnPoints[spawnIndex];
            bool candidateAccepted = false;
            float radius = Mathf.Max(itemsToGenerate[itemSpawnIndex].xSize, itemsToGenerate[itemSpawnIndex].ySize)/2;
            
            // pick random direction to try
            for (int i = 0; i < numSamplesBeforeRejection; i++)
            {
                float angle = Random.value * 360;
                //Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)); // get dir from angle
                Vector2 dir = new Vector2((float)Mathf.Cos(angle * Mathf.PI / 180), (float)Mathf.Sin(angle * Mathf.PI / 180));
                Vector2 candidate = spawnCentre ;

                if (spawnPoints.Count != 1)
                {
                    candidate = spawnCentre + dir * Mathf.Sqrt(Random.Range(radius * radius, 4 * radius * radius));

                }
                if (IsVaryingRadiusCandidateValid(candidate, sampleRegionSize, radius, pointsPlusRadius))
                {
                    spawnPoints.Add(candidate);
                    pointsPlusRadius.Add(new Vector3(candidate.x, candidate.y, radius));
                    itemPoints.Add(new Point(candidate.x, candidate.y, radius, itemsToGenerate[itemSpawnIndex]));
                    candidateAccepted = true;
                    count += 1;

                    itemsToGenerate.RemoveAt(itemSpawnIndex);
                    if (count >= generatedItems.Length)
                    {
                        return (pointsPlusRadius, itemPoints);
                    }
                    break;
                }
            }
            if (!candidateAccepted)
            {
                spawnPoints.RemoveAt(spawnIndex);

            }

            int spawnPointRetryCount = 0;
            if (spawnPoints.Count == 0 && count < generatedItems.Length)
            {
                Vector2 randomLocation = Vector2.zero;
                bool canUse = false;
                while (!canUse && spawnPointRetryCount<10)
                {
                    randomLocation = new Vector2(Random.value * sampleRegionSize.x, Random.value * sampleRegionSize.y);
                    canUse = IsVaryingRadiusCandidateValid(randomLocation, sampleRegionSize, radius, pointsPlusRadius);
                    spawnPointRetryCount += 1;
                }
                spawnPoints.Add(randomLocation);
            }
        }
        return (pointsPlusRadius, itemPoints);
    }


    static bool IsVaryingRadiusCandidateValid(Vector2 candidate, Vector2 sampleRegionSize, float radius, List<Vector3> pointsPlusRadius)
    {
        // Only look if candidate is within the bounds of the grid, taking its radius into account
        if (candidate.x - radius + 1 >= 0 &&
            candidate.x + radius - 1 < sampleRegionSize.x &&
            candidate.y - radius + 1 >= 0 &&
            candidate.y + radius - 1 < sampleRegionSize.y)
        {
            // Get cell indecies

            foreach (Vector3 vector in pointsPlusRadius)
            {
                if (Vector2.Distance(new Vector2(vector.x, vector.y), candidate) < radius + vector.z)
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }
    public class Point
    {
        public float x, y, radius;
        public GeneratedItem item;
        public Point(float x, float y, float radius, GeneratedItem item)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
            this.item = item;
        }
        public Vector2 getDimensions()
        {
            return new Vector2(x, y);
        }
    }
}

//public static class PoissonDiscSampling
//{
//    public class Point
//    {
//        public float x, y, radius;
//        public Point(float x, float y, float radius)
//        {
//            this.x = x;
//            this.y = y;
//            this.radius = radius;
//        }
//    }

//    public class Cell
//    {
//        public int x, y;
//        public List<Point> points;
//        public Cell(int x, int y)
//        {
//            this.x = x;
//            this.y = y;
//            points = new List<Point>();
//        }
//    }
//    public static List<Point> GeneratePoints(int seed, float maxRadius, float minRadius, float sampleRegionSize, int iterationsPerCell, GeneratedItem[] prefabsToSpawn)
//    {
//        int count = 0;
//        Random.InitState(seed);
//        List<Cell> cells = new List<Cell>();
//        int gridSize = Mathf.FloorToInt(sampleRegionSize / maxRadius);
//        Cell tempCell = null;
//        Point tempPoint = null;
//        float gridX, gridY, d, r;
//        bool invalid = true;
//        for (int i = 0; i <= gridSize; i++)
//            for (int j = 0; j <= gridSize; j++)
//            {
//                GeneratedItem item = prefabsToSpawn[count];
//                tempCell = new Cell(i, j);
//                cells.Add(tempCell);
//                gridX = i * maxRadius;
//                gridY = j * maxRadius;
//                for (int k = 0; k < iterationsPerCell; k++)
//                {
//                    if (invalid)
//                    {
//                        tempPoint = new Point(gridX + Random.Range(0f, maxRadius), gridY + Random.Range(0f, maxRadius), Mathf.Max(item.xSize, item.ySize));
//                        invalid = false;
//                    }
//                    if (tempPoint.x > sampleRegionSize || tempPoint.y > sampleRegionSize)
//                    {
//                        invalid = true;
//                        continue;
//                    }
//                    for (int l = 0; l < cells.Count; l++)
//                    {
//                        if ((cells[l].x + 2 >= i) && (cells[l].x - 2 <= i) && (cells[l].y + 2 >= j))
//                        {
//                            for (int m = 0; m < cells[l].points.Count; m++)
//                            {
//                                d = Mathf.Sqrt(((cells[l].points[m].x - tempPoint.x) * (cells[l].points[m].x - tempPoint.x)) + ((cells[l].points[m].y - tempPoint.y) * (cells[l].points[m].y - tempPoint.y)));
//                                r = cells[l].points[m].radius + tempPoint.radius;
//                                if (d > r)
//                                    continue;
//                                tempPoint.radius = tempPoint.radius + d - r;
//                                if (tempPoint.radius < minRadius)
//                                {
//                                    invalid = true;
//                                    break;
//                                }
//                            }
//                            if (invalid)
//                                break;
//                        }
//                    }
//                    if (!invalid)
//                    {
//                        tempCell.points.Add(tempPoint);
//                        count++;
//                    }
//                    invalid = true;
//                }
//                if(count > prefabsToSpawn.Length - 1)
//                {
//                    return cells.SelectMany(R => R.points).ToList();
//                }
//            }
//        return cells.SelectMany(R => R.points).ToList();
//    }

//    public static List<Point> GeneratePoints(int seed, float radius, float sampleRegionSize, int iterationsPerCell)
//    {
//        Random.InitState(seed);
//        List<Cell> cells = new List<Cell>();
//        int gridSize = Mathf.FloorToInt(sampleRegionSize / radius);
//        Cell tempCell = null;
//        Point tempPoint = null;
//        float gridX, gridY, d, r;
//        bool invalid = true;
//        for (int i = 0; i <= gridSize; i++)
//            for (int j = 0; j <= gridSize; j++)
//            {
//                tempCell = new Cell(i, j);
//                cells.Add(tempCell);
//                gridX = i * radius;
//                gridY = j * radius;
//                for (int k = 0; k < iterationsPerCell; k++)
//                {
//                    if (invalid)
//                    {
//                        tempPoint = new Point(gridX + Random.Range(0f, radius), gridY + Random.Range(0f, radius), radius);
//                        invalid = false;
//                    }
//                    if (tempPoint.x > sampleRegionSize || tempPoint.y > sampleRegionSize)
//                    {
//                        invalid = true;
//                        continue;
//                    }
//                    for (int l = 0; l < cells.Count; l++)
//                    {
//                        if ((cells[l].x + 2 >= i) && (cells[l].x - 2 <= i) && (cells[l].y + 2 >= j))
//                        {
//                            for (int m = 0; m < cells[l].points.Count; m++)
//                            {
//                                d = Mathf.Sqrt(((cells[l].points[m].x - tempPoint.x) * (cells[l].points[m].x - tempPoint.x)) + ((cells[l].points[m].y - tempPoint.y) * (cells[l].points[m].y - tempPoint.y)));
//                                r = cells[l].points[m].radius + tempPoint.radius;
//                                if (d < r)
//                                    invalid = true;
//                            }
//                            if (invalid)
//                                break;
//                        }
//                    }
//                    if (!invalid)
//                        tempCell.points.Add(tempPoint);
//                    invalid = true;
//                }
//            }
//        return cells.SelectMany(R => R.points).ToList();
//    }
//}