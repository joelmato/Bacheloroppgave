using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestArea : MonoBehaviour
{

    public GameObject sphere;
    public GameObject testObjectParent;


    public GameObject topRightBackCorner;
    public GameObject bottomRightFrontCorner;
    public GameObject bottomLeftBackCorner;


    public void Spawn()
    {
        Instantiate(sphere, testObjectParent.transform);
    }

    
    public Dictionary<string, float> GetTestAreaBounds()
    {
        // Dictionary that contains min and max values for x, y and z positions for objects within the test area
        var dict = new Dictionary<string, float>(){
                    {"minX", bottomRightFrontCorner.transform.position.x},
                    {"maxX", topRightBackCorner.transform.position.x},
                    {"minY", bottomRightFrontCorner.transform.position.y},
                    {"maxY", topRightBackCorner.transform.position.y},
                    {"minZ", topRightBackCorner.transform.position.z },
                    {"maxZ", bottomLeftBackCorner.transform.position.z}
        };

        return dict;
    }
}
