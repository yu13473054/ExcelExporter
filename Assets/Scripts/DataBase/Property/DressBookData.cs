using System;
using System.Collections.Generic;
using UnityEngine;

public class DressBookData : ScriptableObject
{
	public List<DressBookProperty> _properties = new List<DressBookProperty>();
}

[Serializable]
public class DressBookProperty
{
	public int _id;
	public int _tabNameId;
	public string _tabName;
	public int[] _dressSetIdArray;
	public string _icon;
	public int _belongs;
}
