using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapManager
{
    public int MinY { get; set; }
    public int MaxY { get; set; }

    public int MinZ { get; set; }
    public int MaxZ { get; set; }

    public int MinX { get; set; }
    public int MaxX { get; set; }

    private int YCount { get; set; }
    private int ZCount { get; set; }
    private int XCount { get; set; }

    private char[,,] _collision;

    public bool CanGo(Vector3Int pos)
    {
        if (pos.x < MinX || pos.x > MaxX)
            return false;
        if (pos.z < MinZ || pos.z > MaxZ)
            return false;
        if (pos.y < MinY || pos.y > MaxY)
            return false;

        int y = pos.y - MinY;
        int z = MaxZ - pos.z;
        int x = MaxX - pos.x;

        Debug.Log($"({y}) ({z}) ({x})");

        switch (_collision[y, z, x])
        {
            case '0':
                return true;
            case '3':
                return false;
            case '4':
                return false;
            case '5':
                return false;
            case '6':
                return true;
            case '7':
                return true;
            default:
                return false;
        }
    }


    public void LoadStage(int mapId)
    {
        string stageName = "Stage_" + mapId.ToString("000");
        // Managers.Resource.Instantiate($"Stages/{stageName}");

        TextAsset txt = Managers.Resource.Load<TextAsset>($"StageData/{stageName}/{stageName}_Info");
        StringReader reader = new StringReader(txt.text);

        MinY = int.Parse(reader.ReadLine());
        MaxY = int.Parse(reader.ReadLine());

        MinZ = int.Parse(reader.ReadLine());
        MaxZ = int.Parse(reader.ReadLine());

        MinX = int.Parse(reader.ReadLine());
        MaxX = int.Parse(reader.ReadLine());

        YCount = MaxY - MinY + 1;
        ZCount = MaxZ - MinZ - 1;
        XCount = MaxX - MinX;

        _collision = new char[YCount, ZCount, XCount];
        for (int y = 0; y < YCount; y++)
        {
            StringReader colReader =
                new StringReader(
                    Managers.Resource.Load<TextAsset>($"StageData/{stageName}/{stageName}_Collision_{y}").text
                );
            for (int z = 0; z < ZCount; z++)
            {
                string line = colReader.ReadLine();
                for (int x = 0; x < XCount; x++)
                {
                    _collision[y, z, x] = line[x];
                }
            }
        }
    }
}