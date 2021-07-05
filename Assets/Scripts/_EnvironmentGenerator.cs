using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour
{
    public List<Region> Regions;
    public List<Region> ExteriorRegions;

    public GameObject EnvironmentRoot;  //Parent object for all stuff placed as part of generation
    public GameObject DemarcationRoot;  //Parent object for all markers for regions, boundaries, etc

    public int Seed = 39284756;
    public int NumberOperationSites = 1;
    public int OverallSiteMaxWidth = 1000;
    public int OverallSiteMaxHeight = 1000;

    public bool GenerationComplete = false; //In general, this class should signal to other classes that are dependent on its state to proceed, but in case anything needs to wait until it finishes, this is how it shows that it's complete

    // Start is called before the first frame update
    void Start()
    {
        //Later on this should probably go out to a config file for easy adjustment during user study
        Random.InitState(Seed);
        Regions = new List<Region>();
        ExteriorRegions = new List<Region>();
        LayoutEnvironment();
        BuildEnvironment();
        GenerationComplete = true;
    }

    //Determine the set of regions that occupy the construction site (e.g. which type of stuff is going to be where)
    void LayoutEnvironment()
    {
        //Define environment boundary
        //This is the outside bounds of what can be part of the construction site
        //Once all regions within the site are placed,  any excess can be trimmed
        //For now just a rectangle within bounds {0...OverallSiteMax[width,height]}
        CreateBoundaryPolygon();

        //place regions in order of importance
        //importance is decided by constraints, not semantics
        //Add these to Regions list

        //TODO: What's the cleanest way of tracking unoccupied space for
        //      checking what needs to be filled in?
        //One possible approach is to subdivide boundary as we go
        //But this has potential issues of dividing in a poor manner
        //Approach 2 is to maintain a grid, and simply track which cells have been filled in
        //This approach will lend itself to fairly regular layouts, but if key components
        //Are not axis-aligned, then it may not be too noticeable

        //1. Vehicle operation sites (e.g. excavators)
        for (int i = 0; i < NumberOperationSites; i++)
        {
            //TODO: Multiple sites should consider each other's placement
            //If creating multiple sites, create in order of importance
            Region site = CreateOperationSite();
            Regions.Add(site);
        }
        //2. Vehicle access routes (drive paths from site entrance(s) to op sites
            //Starting at site of most importance, define path to overall boundary (any side)
            //Iterate over subsequent sites and either define their own path to boundary
            //Or connect to an existing path
        //3. Entrance associated buildings
            //Adjacent to intersection point of primary path to boundary
            //Create region representing entrance/briefing/coffee/medical/etc building
        //4. Large buildings
        //5. Crane sites (just where the base of the crane is, not the entire area covered by its operation)
        //6. Small buildings
        //7. Misc work sites (e.g. concrete pouring)

        //Define an additional set of regions that cover the area outside of the construction site
        //These are just for placement of background scenery
        //Place these in ExteriorRegions list
    }

    //Place the objects that are the content visible/interactible in each region
    void BuildEnvironment()
    {
        //TODO: all of this
        //1. Go through all regions, place content
            //A: Instantiate an object to parent the region's content (as a child of EnvironmentRoot)
            //B: Instantiate region content
        //2. Go around boundary, place fencing/barriers
        //3. Go through all exterior regions, place scenery
    }


    Region CreateOperationSite()
    {
        //For now just create an axis-aligned rectangle 10-20m per side
        float w = Random.Range(10.0f, 20.0f);
        float h = Random.Range(10.0f, 20.0f);

        
        float x = Random.Range(2.0f * w, OverallSiteMaxWidth - (2.0f * w));
        float y = Random.Range(2.0f * h, OverallSiteMaxHeight - (2.0f * h));
        Vector2 topLeft = new Vector2(x, y);

        List<Vector3> verts = new List<Vector3>();
        verts.Add(topLeft);
        verts.Add(topLeft + new Vector2(w, 0.0f));
        verts.Add(topLeft + new Vector2(w, h));
        verts.Add(topLeft + new Vector2(0.0f, h));

        Region site = new Region(verts);
        //Now construct the polygon
        site.DemarcationPolygon = CreateDemarcationPolygon(site);
        return site;
    }

    GameObject CreateDemarcationPolygon(Region region)
    {
        //we do this here so that we have convenient access to parent objects
        throw new System.NotImplementedException();
    }

    void CreateBoundaryPolygon()
    {
        //Create a flat polygon that shows the boundaries of the construction site
        //Just for convenient visualisation
    }
}
