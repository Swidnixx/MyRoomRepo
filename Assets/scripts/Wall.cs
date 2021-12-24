using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


class Wall : MonoBehaviour
{
    List<Wall> parts = new List<Wall>();

    Transform wallObject;
    public Wall SetupAndConstruct(GameObject prefab, Vector3 position, Vector3 scale, string name)
    {
        transform.position = position;
        gameObject.name = name;
        wallObject = Instantiate(prefab, position, Quaternion.identity, transform).transform;
        wallObject.localScale = scale;
        return null;
    }
}

