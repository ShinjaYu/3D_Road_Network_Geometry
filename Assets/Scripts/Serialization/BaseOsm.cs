using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Xml;

class BaseOsm // every class bases on this BaseOsm
{
    protected T GetAttribute<T>(string attrName, XmlAttributeCollection attributes)
    {
        //TODO: We are going to assume "attrName" exist in the collection
        string strValue = attributes[attrName].Value;// OSM XML 中的id = “2988298928”
                                                     //是char string,要转换成unsign long number

        return (T)Convert.ChangeType(strValue, typeof(T));

    }

}

