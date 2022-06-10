using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager 
{
    public MyPlayerController Me { get; set; }

    private Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();
    
    public void Add() {}
}
