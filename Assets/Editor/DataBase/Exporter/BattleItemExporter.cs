using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Excel;
using System.IO;
using System.Data;

public class BattleItemExporter
{

	public static BattleItemData ReadExcel()
	{
		string excelName = System.Environment.CurrentDirectory + "/配置表/BattleItem_游戏内道具配置.xlsx";
		FileStream stream = File.Open(excelName, FileMode.Open, FileAccess.Read, FileShare.Read);
		IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
		DataSet result = excelReader.AsDataSet();

		System.Data.DataTable wookSheet = result.Tables["BattleItem"];
		int startRow = 5;
		int endRow = wookSheet.Rows.Count;
		int columnCount = wookSheet.Columns.Count;

		List<string> keys = new List<string>();
		for (int index = 0; index < columnCount; index++)
		{
			string keyText = wookSheet.Rows[2][index].ToString();
			if (!keyText.Equals(string.Empty))
			keys.Add(keyText);
		}
		var headerColumns = ExcelHelper.GetColumnsHeader(wookSheet, keys);
		BattleItemData Data = ScriptableObject.CreateInstance<BattleItemData>();
		for (int row = startRow; row < endRow; row++)
		{
			string checkStr = ExcelHelper.GetSheetValue(wookSheet, row, headerColumns[keys[0]]);
			if(checkStr.Equals(string.Empty))
				continue;
			BattleItemProperty property = new BattleItemProperty();
			property._id = StrParser.ParseDecInt(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["ID"]), 0);
			property._desc = StrParser.ParseStr(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["Desc"]), string.Empty);
			property._weights = StrParser.ParseDecIntList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["Weights"]), 0);
			property._appMode = StrParser.ParseDecInt(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["AppMode"]), 0);
			property._itemType = StrParser.ParseDecInt(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["ItemType"]), 0);
			property._passiveItem = StrParser.ParseDecInt(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["PassiveItem"]), 0);
			property._itemDuration = StrParser.ParseDecInt(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["ItemDuration"]), 0);
			property._itemDelay = StrParser.ParseDecInt(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["ItemDelay"]), 0);
			property._itemTargetType = StrParser.ParseDecInt(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["ItemTargetType"]), 0);
			property._itemEffects = StrParser.ParseDecIntList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["ItemEffects"]), 0);
			property._itemEffectValues = StrParser.ParseDecIntList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["ItemEffectValues"]), 0);
			property._itemTriggerTimes = StrParser.ParseDecInt(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["ItemTriggerTimes"]), 0);
			property._castingEffects = StrParser.ParseStrList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["CastingEffects"]), new string[0]);
			property._castingUIEffects = StrParser.ParseStrList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["CastingUIEffects"]), new string[0]);
			property._chargingEffects = StrParser.ParseStrList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["ChargingEffects"]), new string[0]);
			property._chargingUIEffects = StrParser.ParseStrList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["ChargingUIEffects"]), new string[0]);
			property._warningEffects = StrParser.ParseStrList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["WarningEffects"]), new string[0]);
			property._warningUIEffects = StrParser.ParseStrList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["WarningUIEffects"]), new string[0]);
			property._hitEffects = StrParser.ParseStrList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["HitEffects"]), new string[0]);
			property._hitUIEffects = StrParser.ParseStrList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["HitUIEffects"]), new string[0]);
			property._auraEffects = StrParser.ParseStrList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["AuraEffects"]), new string[0]);
			property._auraUIEffects = StrParser.ParseStrList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["AuraUIEffects"]), new string[0]);
			property._auraRemoveEffects = StrParser.ParseStrList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["AuraRemoveEffects"]), new string[0]);
			property._auraRemoveUIEffects = StrParser.ParseStrList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["AuraRemoveUIEffects"]), new string[0]);
			property._castingSfx = StrParser.ParseDecIntList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["CastingSfx"]), 0);
			property._chargingSfx = StrParser.ParseDecIntList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["ChargingSfx"]), 0);
			property._warningSfx = StrParser.ParseDecIntList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["WarningSfx"]), 0);
			property._hitSfx = StrParser.ParseDecIntList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["HitSfx"]), 0);
			property._auraSfx = StrParser.ParseDecIntList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["AuraSfx"]), 0);
			property._auraRemoveSfx = StrParser.ParseDecIntList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["AuraRemoveSfx"]), 0);
			property._itemIcon = StrParser.ParseStr(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["ItemIcon"]), string.Empty);
			Data._properties.Add(property);
		}
		return Data;
	}
}
