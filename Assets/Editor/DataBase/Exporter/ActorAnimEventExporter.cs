using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Excel;
using System.IO;
using System.Data;

public class ActorAnimEventExporter
{

	public static ActorAnimEventData ReadExcel()
	{
		string excelName = System.Environment.CurrentDirectory + "/配置表/ActorAnimEvent_角色动画事件 包含位移音效等.xlsx";
		FileStream stream = File.Open(excelName, FileMode.Open, FileAccess.Read, FileShare.Read);
		IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
		DataSet result = excelReader.AsDataSet();

		System.Data.DataTable wookSheet = result.Tables["ActorAnimEvent"];
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
		ActorAnimEventData Data = ScriptableObject.CreateInstance<ActorAnimEventData>();
		for (int row = startRow; row < endRow; row++)
		{
			string checkStr = ExcelHelper.GetSheetValue(wookSheet, row, headerColumns[keys[0]]);
			if(checkStr.Equals(string.Empty))
				continue;
			ActorAnimEventProperty property = new ActorAnimEventProperty();
			property._id = StrParser.ParseStr(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["ID"]), string.Empty);
			property._aniType = StrParser.ParseStr(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["AniType"]), string.Empty);
			property._info = StrParser.ParseStr(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["Info"]), string.Empty);
			property._moveOutPos = StrParser.ParseFloatList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["MoveOutPos"]), 0f);
			property._moveOutTime = StrParser.ParseFloat(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["MoveOutTime"]), 0f);
			property._moveBackPos = StrParser.ParseFloatList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["MoveBackPos"]), 0f);
			property._moveBackTime = StrParser.ParseFloat(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["MoveBackTime"]), 0f);
			property._effect_SpeedFoot = StrParser.ParseDecIntList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["Effect_SpeedFoot"]), 0);
			property._effect_HoldEffect_L = StrParser.ParseDecIntList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["Effect_HoldEffect_L"]), 0);
			property._effect_HoldEffect_R = StrParser.ParseDecIntList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["Effect_HoldEffect_R"]), 0);
			property._effect_HoldEffect_U = StrParser.ParseDecIntList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["Effect_HoldEffect_U"]), 0);
			property._playMusic = StrParser.ParseDecIntList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["PlayMusic"]), 0);
			Data._properties.Add(property);
		}
		return Data;
	}
}
