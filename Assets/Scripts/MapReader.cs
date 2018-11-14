//using System;
//using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

class MapReader : MonoBehaviour { // 这里本来有public 但删了从而使得 这个能public inside the assembl but not outside the game 
    [HideInInspector] //***
    public Dictionary<ulong, OsmNode> nodes;

    [HideInInspector]
    public List<OsmWay> ways;

    //[HideInInspector]
    //public OsmIntersection intersections;

    [HideInInspector]
    public OsmBounds bounds;

    [Tooltip("The resource file that contains the OSM map data")]
    public string resourceFile;

    public bool IsReady { get; private set; }// dont want people access the MapReader until the data has been loaded

	// Use this for initialization
	void Start () {

        nodes = new Dictionary<ulong, OsmNode>();
        ways = new List<OsmWay>();

        var txtAsset = Resources.Load<TextAsset>(resourceFile);

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(txtAsset.text);

        /*parcel all the nodes here*/
        SetBounds(doc.SelectSingleNode("/osm/bounds"));// the path using the slash to denote how the hiearchy looks,root is "osm"  
        GetNodes(doc.SelectNodes("/osm/node"));//all the nodes of the path
        GetWays(doc.SelectNodes("/osm/way"));// 
        SetRelations(doc.SelectNodes("/osm/relation"));

        IsReady = true;
	}
    void SetRelations(XmlNodeList xmlNodeList)
    {
        foreach (XmlNode node in xmlNodeList)
        {
            // .............
        }

    }

    void GetWays(XmlNodeList xmlNodeList)
    {
        foreach (XmlNode node in xmlNodeList)
        {
            OsmWay way = new OsmWay(node);
            ways.Add(way);
        }
       
    }

    void GetNodes(XmlNodeList xmlNodeList)
    {
        foreach (XmlNode n in xmlNodeList)
        {
            OsmNode node = new OsmNode(n);
            nodes[node.ID] = node;
        }
    }

    void SetBounds(XmlNode xmlNode)
    {
        bounds = new OsmBounds(xmlNode);
    }

    // Update is called once per frame
    void Update () {
        foreach (OsmWay w in ways) //draw debug line for each those ways
        {
            if (w.Visible)
            {
                Color c = Color.gray;               // cyan for buildings  (a boundary could be a building)
                if (!w.IsBoundary) c = Color.red; // red for roads (not a boundary)
                if (w.IsRoad) c = Color.yellow;
                if (w.IsBuilding) c = Color.blue;
                DrawSpline(w,c);
                
            }
        }
    }

    void DrawSpline(OsmWay w, Color c)
    {

        for (int i = 1; i < w.NodeIDs.Count; i++)//draw a lines between point to point
        {
            //OsmNode p1 = nodes[w.NodeIDs[i - 1]];
            //OsmNode p2 = nodes[w.NodeIDs[i]];

            //Vector3 v1 = p1 - bounds.Centre;
            //Vector3 v2 = p2 - bounds.Centre;

            //Debug.DrawLine(v1, v2, c);

            OsmNode p = nodes[w.NodeIDs[i]];
            Vector3 v = p - bounds.Centre;

        }

    }
}
