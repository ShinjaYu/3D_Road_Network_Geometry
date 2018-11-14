using System.Collections.Generic;
using System.Xml;

class OsmRelation : BaseOsm
{
    public ulong way_ID { get; private set; }
    public bool way_Visible { get; private set; }
    public List<ulong> NodeIDs { get; private set; }

    public OsmRelation(XmlNode node)
    {
        

        way_ID = GetAttribute<ulong>("id", node.Attributes);
        way_Visible = GetAttribute<bool>("visible", node.Attributes);

        XmlNodeList nds = node.SelectNodes("nd");
        XmlNodeList tags = node.SelectNodes("tag");


    }
    



}

