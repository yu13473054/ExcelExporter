using System;
using System.Collections.Generic;
using UnityEngine;

public class ARData : ScriptableObject
{
	public List<ARProperty> _properties = new List<ARProperty>();
}

[Serializable]
public class ARProperty
{
	public int _id;
	public int _itemID;
	public string _sceneName;
}
