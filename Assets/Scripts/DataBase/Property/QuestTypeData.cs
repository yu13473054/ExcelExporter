using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestTypeData : ScriptableObject
{
	public List<QuestTypeProperty> _properties = new List<QuestTypeProperty>();
}

[Serializable]
public class QuestTypeProperty
{
	public int _id;
	public string _name;
	public string _description;
	public int _clearType;
}
