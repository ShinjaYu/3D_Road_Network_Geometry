using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

class OsmBounds : BaseOsm
{
    public float MinLat { get; private set; }
    public float MaxLat { get; private set; }
    public float MinLon { get; private set; }
    public float MaxLon { get; private set; }

    public Vector3 Centre { get; private set; }
    //<bounds minlat="59.3356100" minlon="18.0864400" maxlat="59.3394800" maxlon="18.0984100"/>

    public OsmBounds(XmlNode node)
    {
        MinLat = GetAttribute<float>("minlat",node.Attributes);
        MaxLat = GetAttribute<float>("maxlat", node.Attributes);
        MinLon = GetAttribute<float>("minlon", node.Attributes);
        MaxLon = GetAttribute<float>("maxlon", node.Attributes);

        //calculate the center
        float x = (float)((MercatorProjection.lonToX(MaxLon) + MercatorProjection.lonToX(MinLon)) / 2);
        float y = (float)((MercatorProjection.latToY(MaxLat) + MercatorProjection.latToY(MinLat)) / 2);
        Centre = new Vector3(x, 0, y);
        //这个center point 可以设到（0，0，0），让我们所有的点都与之相关联
    }
}
    
