using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Excel;
using System.IO;
using System.Data;

public class QuestTypeExporter
{

	public static QuestTypeData ReadExcel()
	{
		string excelName = System.Environment.CurrentDirectory + "/配置表/QuestType_任务条件类型.xlsx";
		FileStream stream = File.Open(excelName, FileMode.Open, FileAccess.Read, FileShare.Read);
		IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
		DataSet result = excelReader.AsDataSet();

		System.Data.DataTable wookSheet = result.Tables["QuestType"];
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
		QuestTypeData Data = ScriptableObject.CreateInstance<QuestTypeData>();
		for (int row = startRow; row < endRow; row++)
		{
			string checkStr = ExcelHelper.GetSheetValue(wookSheet, row, headerColumns[keys[0]]);
			if(checkStr.Equals(string.Empty))
				continue;
			QuestTypeProperty property = new QuestTypeProperty();
			property._id = StrParser.ParseDecInt(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["Id"]), 0);
			property._name = StrParser.ParseStr(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["name"]), string.Empty);
			property._description = StrParser.ParseStr(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["description"]), string.Empty);
			property._clearType = StrParser.ParseDecInt(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["clearType"]), 0);
			Data._properties.Add(property);
		}
		return Data;
	}
}
