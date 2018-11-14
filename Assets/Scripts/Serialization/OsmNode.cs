//using System;
//using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

class OsmNode : BaseOsm
{
    public ulong ID { get; private set; }

    public float Latitude { get; private set; }

    public float Longitude { get; private set; }

    public float X { get; private set; }

    public float Y { get; private set; }

    public static implicit  operator Vector3 (OsmNode node)
    {
        return new Vector3(node.X, 0, node.Y);
    }
    public OsmNode(XmlNode node) // 读每个node 的id， lat and lon;
    {
        ID = GetAttribute<ulong>("id", node.Attributes);
        Latitude = GetAttribute<float>("lat", node.Attributes);// lat 和 lon 是在球面的，现在要转换成平面
        Longitude = GetAttribute<float>("lon", node.Attributes);

        X = (float)MercatorProjection.lonToX(Longitude);
        Y = (float)MercatorProjection.latToY(Latitude);
    }

}