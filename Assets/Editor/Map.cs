using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Map
{
#if UNITY_EDITOR
    [MenuItem("Tools/GenerateMap")]
    public static void GenerateMap()
    {
        GenerateMap("Assets/Resources/StageData");
        GenerateMap("../Shared/StageData");
    }

    private static void GenerateMap(string pathPrefix)
    {
        GameObject stage = Resources.Load<GameObject>("Prefabs/Stages/Stage_001");
        Transform info = Helper.FindChild<Transform>(stage, "Info");
        List<Tilemap> collisions = Helper.FindChildren<Tilemap>(info.gameObject, "Collision");

        int zMax = collisions[0].cellBounds.yMax;
        int zMin = collisions[0].cellBounds.yMin;
        int xMax = collisions[0].cellBounds.xMax;
        int xMin = collisions[0].cellBounds.xMin;

        Directory.CreateDirectory($"{pathPrefix}/{stage.name}");
        using (StreamWriter writer = File.CreateText($"{pathPrefix}/{stage.name}/{stage.name}_Info.txt"))
        {
            writer.WriteLine(0);
            writer.WriteLine(collisions.Count - 1);
            writer.WriteLine(zMin);
            writer.WriteLine(zMax);
            writer.WriteLine(xMin);
            writer.WriteLine(xMax);

            writer.Close();
        }

        for (int y = 0; y < collisions.Count; y++)
        {
            using (StreamWriter writer =
                   File.CreateText($"{pathPrefix}/{stage.name}/{stage.name}_Collision_{y}.txt"))
            {
                for (int z = zMax - 1; z > zMin; z--)
                {
                    for (int x = xMin; x < xMax; x++)
                    {
                        TileBase tile = collisions[y].GetTile(new Vector3Int(x, z, 0));
                        if (tile == null)
                        {
                            writer.Write('0');
                            continue;
                        }

                        switch (tile.name)
                        {
                            //도착 지점
                            case "tileset_4":
                                writer.Write('3');
                                break;
                            //갈 수 없는 지역
                            case "tileset_24":
                                writer.Write('4');
                                break;
                            //낙사
                            case "barrel":
                                writer.Write('5');
                                break;
                            //진자운동 장애물
                            case "rock":
                                writer.Write('6');
                                break;
                            //회전 장애물
                            case "chest":
                                writer.Write('7');
                                break;
                            //출발 리스폰 지역
                            case "tileset_70":
                                writer.Write('8');
                                break;
                            //회전 휠 장애물
                            case "tileset_73":
                                writer.Write('a');
                                break;
                            //회전 휠 장애물 pivot
                            case "tileset_9":
                                writer.Write('b');
                                break;
                        }
                    }

                    writer.WriteLine();
                }
            }
        }
    }
#endif
}