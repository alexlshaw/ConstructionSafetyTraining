
using UnityEngine;

public static class PointsLostHandler
{
    public static int HP = 100;
    public static int points = 0;
    public static int collisions = 0;
    public static int falls = 0;
    public static float dust = 0;
    public static int tips = 0;
    public static int other = 0;
    public static float currentPoints = 0; 

    public static void resetPoints()
    {
        Debug.Log("collisions" + collisions + "falls" + falls + "dust" + dust + "tips" + tips + "falling objects" + other);
        collisions = 0;
        falls = 0;
        dust = 0;
        tips = 0;
        other = 0;
    }

    public static void printPoints()
    {
        Debug.Log("collisions" + collisions + "falls" + falls + "dust" + dust + "tips" + tips + "falling objects" + other);

    }

    public static float calculatePoints()
    {
        currentPoints = 1000 - 50 * collisions - 30 * falls - 50 * tips - dust;
        return currentPoints;
    }
}
