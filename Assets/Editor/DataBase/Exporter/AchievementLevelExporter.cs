using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Excel;
using System.IO;
using System.Data;

public class AchievementLevelExporter
{

	public static AchievementLevelData ReadExcel()
	{
		string excelName = System.Environment.CurrentDirectory + "/配置表/AchievementLevel_成就等级.xlsx";
		FileStream stream = File.Open(excelName, FileMode.Open, FileAccess.Read, FileShare.Read);
		IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
		DataSet result = excelReader.AsDataSet();

		System.Data.DataTable wookSheet = result.Tables["AchievementLevel"];
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
		AchievementLevelData Data = ScriptableObject.CreateInstance<AchievementLevelData>();
		for (int row = startRow; row < endRow; row++)
		{
			string checkStr = ExcelHelper.GetSheetValue(wookSheet, row, headerColumns[keys[0]]);
			if(checkStr.Equals(string.Empty))
				continue;
			AchievementLevelProperty property = new AchievementLevelProperty();
			property._level = StrParser.ParseDecInt(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["Level"]), 0);
			property._desc = StrParser.ParseStr(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["Desc"]), string.Empty);
			property._requireBonus = StrParser.ParseDecInt(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["RequireBonus"]), 0);
			property._image = StrParser.ParseStr(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["Image"]), string.Empty);
			Data._properties.Add(property);
		}
		return Data;
	}
}
