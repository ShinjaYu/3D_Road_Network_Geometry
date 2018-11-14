using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BuildingMaker : InfrastructureBehavior
{
  
    public Material building;// sign the material
    IEnumerator Start()
    {
        while (!map.IsReady)
        {
            yield return null;
        }

        //TODO: process map data to create buildings.
        foreach (var way in map.ways.FindAll((w) => { return w.IsBuilding && w.NodeIDs.Count > 1; }))
        {
            // Create the object
            CreateObject(way, building, "Building");
            yield return null;
        }
        //foreach (var way in map.ways.FindAll((w) => { return w.IsBoundary; }))//find all the boundary nodes
        //    //aka. find all the buildings there (checked in OsmWay "if(key=building)")
        //{   //可以procedural了！ -- Unity Manual --> Using the Mesh Class
        //    //Construct the walls (a quad wall = 2 triangles
        //    //= 4 nodes(2 bottom nodes are already acquired))
        //    GameObject go = new GameObject();
        //    //everyhing to be in a local space
        //    Vector3 LocalOrigin = GetCentre(way);
        //    go.transform.position = LocalOrigin - map.bounds.Centre;// All the mesh data are going to base on the local
        //                                        //point and space we have 

        //    /*
        //     * 
        //     * Check the following part.
        //     * 
        //    */
        //    MeshFilter mf = go.AddComponent<MeshFilter>();
        //    MeshRenderer mr = go.AddComponent<MeshRenderer>();
        //    mr.material = building;
        //    List<Vector3> vectors = new List<Vector3>();
        //    List<Vector3> normals = new List<Vector3>();
        //    List<int> indicies = new List<int>();

        //    //可参考Unity Manual-- > Using the Mesh Class -->Example-creating a Quad
        //    for (int i = 1; i < way.NodeIDs.Count; i++)
        //    {
        //        OsmNode p1 = map.nodes[way.NodeIDs[i - 1]];
        //        OsmNode p2 = map.nodes[way.NodeIDs[i]];

        //        Vector3 v1 = p1 - LocalOrigin;
        //        Vector3 v2 = p2 - LocalOrigin;
        //        // To use the Height of buildings ***注意这里给Height的方式也可以用于给陆加上地形因素 like Overpass等等
        //        Vector3 v3 = v1 + new Vector3(0, way.Height, 0);
        //        Vector3 v4 = v2 + new Vector3(0, way.Height, 0);

        //        vectors.Add(v1);
        //        vectors.Add(v2);
        //        vectors.Add(v3);
        //        vectors.Add(v4);

        //        //uvs.Add(new Vector2(0, 0));
        //        //uvs.Add(new Vector2(1, 0));
        //        //uvs.Add(new Vector2(0, 1));
        //        //uvs.Add(new Vector2(1, 1));

        //        //(Video 1:33:0)triangles are created using these indicies values, the indicies to vectors
        //        normals.Add(-Vector3.forward);
        //        normals.Add(-Vector3.forward);
        //        normals.Add(-Vector3.forward);
        //        normals.Add(-Vector3.forward);
       
        //        int idx1, idx2, idx3, idx4;
        //        idx4 = vectors.Count - 1;
        //        idx3 = vectors.Count - 2;
        //        idx2 = vectors.Count - 3;
        //        idx1 = vectors.Count - 4;

        //        // first triangle v1, v3, v2
        //        indicies.Add(idx1);
        //        indicies.Add(idx3);
        //        indicies.Add(idx2);

        //        // second         v3, v4, v2
        //        indicies.Add(idx3);
        //        indicies.Add(idx4);
        //        indicies.Add(idx2);

        //        // third          v2, v3, v1
        //        indicies.Add(idx2);
        //        indicies.Add(idx3);
        //        indicies.Add(idx1);

        //        // fourth         v2, v4, v3
        //        indicies.Add(idx2);
        //        indicies.Add(idx4);
        //        indicies.Add(idx3);

        //        // And now the roof triangles
        //        //indicies.Add(0);
        //        //indicies.Add(idx3);
        //        //indicies.Add(idx4);

        //        //// Don't forget the upside down one!
        //        //indicies.Add(idx4);
        //        //indicies.Add(idx3);
        //        //indicies.Add(0);
        //    }

        //    mf.mesh.vertices = vectors.ToArray();
        //    mf.mesh.normals = normals.ToArray();
        //    mf.mesh.triangles = indicies.ToArray();

        //    yield return null;
        //}

    }
    protected override void OnObjectCreated(OsmWay way, Vector3 origin, List<Vector3> vectors, List<Vector3> normals, List<Vector2> uvs, List<int> indices)
    {
        // Get the centre of the roof
        Vector3 oTop = new Vector3(0, way.Height, 0);

        // First vector is the middle point in the roof
        vectors.Add(oTop);
        normals.Add(Vector3.up);
        uvs.Add(new Vector2(0.5f, 0.5f));

        for (int i = 1; i < way.NodeIDs.Count; i++)
        {
            OsmNode p1 = map.nodes[way.NodeIDs[i - 1]];
            OsmNode p2 = map.nodes[way.NodeIDs[i]];

            Vector3 v1 = p1 - origin;
            Vector3 v2 = p2 - origin;
            Vector3 v3 = v1 + new Vector3(0, way.Height, 0);
            Vector3 v4 = v2 + new Vector3(0, way.Height, 0);

            vectors.Add(v1);
            vectors.Add(v2);
            vectors.Add(v3);
            vectors.Add(v4);

            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(1, 1));

            normals.Add(-Vector3.forward);
            normals.Add(-Vector3.forward);
            normals.Add(-Vector3.forward);
            normals.Add(-Vector3.forward);

            int idx1, idx2, idx3, idx4;
            idx4 = vectors.Count - 1;
            idx3 = vectors.Count - 2;
            idx2 = vectors.Count - 3;
            idx1 = vectors.Count - 4;

            // first triangle v1, v3, v2
            indices.Add(idx1);
            indices.Add(idx3);
            indices.Add(idx2);

            // second         v3, v4, v2
            indices.Add(idx3);
            indices.Add(idx4);
            indices.Add(idx2);

            // third          v2, v3, v1
            indices.Add(idx2);
            indices.Add(idx3);
            indices.Add(idx1);

            // fourth         v2, v4, v3
            indices.Add(idx2);
            indices.Add(idx4);
            indices.Add(idx3);

            // And now the roof triangles
            indices.Add(0);
            indices.Add(idx3);
            indices.Add(idx4);

            // Don't forget the upside down one!
            indices.Add(idx4);
            indices.Add(idx3);
            indices.Add(0);
        }
    }
}
