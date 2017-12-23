using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleItemData : ScriptableObject
{
	public List<BattleItemProperty> _properties = new List<BattleItemProperty>();
}

[Serializable]
public class BattleItemProperty
{
	public int _id;
	public string _desc;
	public int[] _weights;
	public int _appMode;
	public int _itemType;
	public int _passiveItem;
	public int _itemDuration;
	public int _itemDelay;
	public int _itemTargetType;
	public int[] _itemEffects;
	public int[] _itemEffectValues;
	public int _itemTriggerTimes;
	public string[] _castingEffects;
	public string[] _castingUIEffects;
	public string[] _chargingEffects;
	public string[] _chargingUIEffects;
	public string[] _warningEffects;
	public string[] _warningUIEffects;
	public string[] _hitEffects;
	public string[] _hitUIEffects;
	public string[] _auraEffects;
	public string[] _auraUIEffects;
	public string[] _auraRemoveEffects;
	public string[] _auraRemoveUIEffects;
	public int[] _castingSfx;
	public int[] _chargingSfx;
	public int[] _warningSfx;
	public int[] _hitSfx;
	public int[] _auraSfx;
	public int[] _auraRemoveSfx;
	public string _itemIcon;
}
