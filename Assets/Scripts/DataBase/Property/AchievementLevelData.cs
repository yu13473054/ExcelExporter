using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementLevelData : ScriptableObject
{
	public List<AchievementLevelProperty> _properties = new List<AchievementLevelProperty>();
}

[Serializable]
public class AchievementLevelProperty
{
	public int _level;
	public string _desc;
	public int _requireBonus;
	public string _image;
}
