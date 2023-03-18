using System.Collections.Generic;
using UnityEngine;

//Tracks data about an area of the construction site and what should be going on in there
public class Region
{
    public GameObject DemarcationPolygon;  //A polygon used for convenient marking of what space belongs to a region
    public List<Vector3> Vertices;

    public Region(List<Vector3> vertices)
    {
        Vertices = vertices;
        //We require that vertices are ordered in a loop that defines the outer boundary of the region
        //This way if we need to access the edges of the polygon or manually triangulate it
        //we have enough information to do so
    }

    public bool PointInRegion(Vector3 point)
    {
        //TODO
        //Rather than doing a point-in-polygon test for vertices
        //Easier to just do a vertical raycast against demarcationPolygon
        //Demarcation polygons should be in distinct layer for this
        throw new System.NotImplementedException();
    }
}


//Architectural thoughts:

//Different types of region are going to need to store fairly different sets of information, 
//but the way that the generator interacts with them is going to be largely the same
//That is to say, it will create them one at a time, then instantiate content within them
//This is probably a good case for subclassing or an interface depending on the degree of overlap