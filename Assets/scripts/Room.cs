using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Room 
{
    public float width = 5;
    public float length = 5;
    public float height = 3;
    public float wallWidth = 0.2f;
    public Door doorPrefab;

    GameObject northWall;
    GameObject eastWall;
    GameObject westWall;
    GameObject southWall;
    List<int> occupiedSpotsInWalls;
    public float totalLength => 2 * width + 2 * length;

    public void Generate(Vector3 position, GameObject wallPrefab, Door doorPrefab)
    {
        position += Vector3.up * height / 2;

        northWall = GameObject.Instantiate(wallPrefab, position + Vector3.forward * length / 2, Quaternion.identity);
        southWall = GameObject.Instantiate(wallPrefab, position - Vector3.forward * length / 2, Quaternion.identity);
        eastWall = GameObject.Instantiate(wallPrefab, position + Vector3.left * width / 2, Quaternion.identity);
        westWall = GameObject.Instantiate(wallPrefab, position - Vector3.left * width / 2, Quaternion.identity);
        northWall.name = "North Wall";
        southWall.name = "South Wall";
        eastWall.name = "East Wall";
        westWall.name = "West Wall";
        northWall.transform.localScale = new Vector3(width, height, wallWidth);
        southWall.transform.localScale = new Vector3(width, height, wallWidth);
        eastWall.transform.localScale = new Vector3(wallWidth, height, length);
        westWall.transform.localScale = new Vector3(wallWidth, height, length);

        //InsertDoor((int)(5), doorPrefab);
        //Test(doorPrefab);
    }

    void Test(Door doorPrefab)
    {
        InsertDoor((int)length / 2, doorPrefab);
        InsertDoor((int)length + (int)width / 2, doorPrefab);
        InsertDoor((int)length + (int)width + (int)length / 2, doorPrefab);
        InsertDoor((int)length + (int)width + (int)length + (int)width / 2, doorPrefab);

        //InsertDoor((int)(length + width*0.7), doorPrefab);
        //InsertDoor(70, doorPrefab);
    }

    public void InsertDoor(int length, Door doorPrefab)
    {
        if(length < 0)
        { throw new NotImplementedException("Reversed positioning not implemented yet"); }

        if(length < this.length)
        {
            SplitWall(length, eastWall, Vector3.forward, doorPrefab);
            return;
        }
        else if(length < this.length+this.width)
        {
            length -= (int)this.length;
            SplitWall(length, northWall, Vector3.right, doorPrefab);
            return;
        }
        else if (length < 2*this.length + this.width)
        {
            length -= (int)this.length+(int)this.width;
            SplitWall(length, westWall, Vector3.back, doorPrefab);
            return;
        }
        else if (length < 2 * this.length + 2 * this.width)
        {
            length -= (int)this.length + (int)this.width + (int)this.length;
            SplitWall(length, southWall, Vector3.left, doorPrefab);
            return;
        }
        else
        {
            InsertDoor((int)(length - totalLength), doorPrefab);
        }
    }
    private void SplitWall(int length, GameObject wall, Vector3 aligment, Door door)
    {
        Vector3 scale = Vector3.Scale(wall.transform.localScale, aligment);
        Vector3 start = wall.transform.position - scale/2;
        Vector3 end = wall.transform.position + scale/2;
        //GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = start;
        // GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = end;

        Vector3 doorPos = start + (aligment*length);
        door = Insert(doorPos, Quaternion.LookRotation(aligment, Vector3.up) * Quaternion.AngleAxis(90, Vector3.up));
        //GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = doorPos;

        GameObject WallParent = new GameObject(wall.name + "1");
        GameObject WallParent2 = new GameObject(wall.name + "2");
        WallParent.transform.position = start;
        WallParent2.transform.position = end;

        wall.transform.localScale -= VectorAbs(scale) + aligment.normalized;
        GameObject wall2 = GameObject.Instantiate(wall);

        wall.transform.position = start + 0.5f * aligment;
        wall2.transform.position = end - 0.5f * aligment;

        wall.transform.parent = WallParent.transform;
        wall2.transform.parent = WallParent2.transform;

        float wallLength = (end - start).magnitude;

        float partLength1 = (doorPos - start).magnitude - door.doorWidth / 2 - 1;//door.transform.localScale.x/2 -1;
        float partLength2 = (end - doorPos).magnitude - door.doorWidth / 2 - 1; //door.transform.localScale.x/2 -1;

        WallParent.transform.localScale += VectorAbs(aligment) * partLength1;
        WallParent2.transform.localScale += VectorAbs(aligment.normalized) * partLength2;
    }
    private Door Insert( Vector3 doorPos, Quaternion rotation)
    {
        return GameObject.Instantiate(doorPrefab, doorPos, rotation);
    }
    internal void InsertChest(Chest chestPrefab)
    {
        float x = UnityEngine.Random.Range(eastWall.transform.position.x + 1.5f, westWall.transform.position.x - 1.5f);
        float z = UnityEngine.Random.Range(southWall.transform.position.x + 1.5f, northWall.transform.position.x - 1.5f);
        GameObject.Instantiate(chestPrefab, new Vector3(x, 0, z), Quaternion.identity);
    }

    static Vector3 VectorAbs(Vector3 v)
    {
        return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
    }
}

