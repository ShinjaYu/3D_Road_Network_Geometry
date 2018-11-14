using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

class OsmWay:BaseOsm
{
    public ulong ID { get; private set; }
    public bool Visible { get; private set; }
    public List<ulong> NodeIDs { get; private set; }
    public string Type { get; private set; } //Road, Boundary, Building, footway,cycleway
    public bool IsBoundary { get; private set; }
    public bool IsFootway { get; private set; }
    public bool IsRoad { get; private set; }
    public bool IsBuilding { get; private set; }

    public float Height { get; private set; }
    public string Name { get; private set; }
    public int Lanes { get; private set; }

    public OsmWay(XmlNode node)// ID visible
    {
        NodeIDs = new List<ulong>();
        Height = 3.0f;
        Lanes = 2;
        Name = "";
        ID = GetAttribute<ulong>("id", node.Attributes);
        Visible = GetAttribute<bool>("visible", node.Attributes);
        Type = "Road";
        IsRoad = false;
        XmlNodeList nds = node.SelectNodes("nd");
        foreach (XmlNode n in nds)
        {
            ulong refNo = GetAttribute<ulong>("ref", n.Attributes);
            NodeIDs.Add(refNo);
        }

        //TODO : Determine what type of way this is; Is it a road / boundary etc. 

        if (NodeIDs.Count > 1)
        {
            //IsBoundary = (NodeIDs[0] == NodeIDs[NodeIDs.Count - 1]);// if the 1st element = the last element, we have the boundary
            if (NodeIDs[0] == NodeIDs[NodeIDs.Count - 1])
                Type = "Boundary";
        }
        
        
        XmlNodeList tags = node.SelectNodes("tag");// 这里node已经是way类型的了
        foreach (XmlNode t in tags) //对1个way的每一个tag进行check
        {
            string key = GetAttribute<string>("k", t.Attributes);

            
            if (key == "building:levels")
            {
                Height = 3.0f * GetAttribute<float>("v", t.Attributes);
            }
            else if (key == "height")
            {
                Height = 0.3048f * GetAttribute<float>("v", t.Attributes); //(10ft = 3.048 meters )
            }
            else if (key == "building")
            {
                IsBuilding = true; // GetAttribute<string>("v", t.Attributes) == "yes";
                Type = "Building";
            }
            else if (key == "highway")
            {
                IsRoad = true;
                string value = GetAttribute<string>("v", t.Attributes);
                switch (value)
                {
                    case "footway":
                        IsRoad = false;
                        IsFootway = true;
                        Type = "footway";
                        break;
                    case "cycleway":
                        IsRoad = false;
                        Type = "footway";
                        break;
                    case "service":
                        IsRoad = false;
                        break;
                    case "Unclassified":
                        IsRoad = false;
                        break;
                    default:
                        break;
                }
           
            }
            else if (key == "lanes")
            {
                Lanes = GetAttribute<int>("v", t.Attributes);
            }
            else if (key == "name")
            {
                Name = GetAttribute<string>("v", t.Attributes);
            }
        }


    }
}

