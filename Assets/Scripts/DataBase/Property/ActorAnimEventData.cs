using System;
using System.Collections.Generic;
using UnityEngine;

public class ActorAnimEventData : ScriptableObject
{
	public List<ActorAnimEventProperty> _properties = new List<ActorAnimEventProperty>();
}

[Serializable]
public class ActorAnimEventProperty
{
	public string _id;
	public string _aniType;
	public string _info;
	public float[] _moveOutPos;
	public float _moveOutTime;
	public float[] _moveBackPos;
	public float _moveBackTime;
	public int[] _effect_SpeedFoot;
	public int[] _effect_HoldEffect_L;
	public int[] _effect_HoldEffect_R;
	public int[] _effect_HoldEffect_U;
	public int[] _playMusic;
}
