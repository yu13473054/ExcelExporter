using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Excel;
using System.IO;
using System.Data;

public class ARExporter
{

	public static ARData ReadExcel()
	{
		string excelName = System.Environment.CurrentDirectory + "/配置表/AR_AR配置.xlsx";
		FileStream stream = File.Open(excelName, FileMode.Open, FileAccess.Read, FileShare.Read);
		IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
		DataSet result = excelReader.AsDataSet();

		System.Data.DataTable wookSheet = result.Tables["AR"];
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
		ARData Data = ScriptableObject.CreateInstance<ARData>();
		for (int row = startRow; row < endRow; row++)
		{
			string checkStr = ExcelHelper.GetSheetValue(wookSheet, row, headerColumns[keys[0]]);
			if(checkStr.Equals(string.Empty))
				continue;
			ARProperty property = new ARProperty();
			property._id = StrParser.ParseDecInt(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["ID"]), 0);
			property._itemID = StrParser.ParseDecInt(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["itemID"]), 0);
			property._sceneName = StrParser.ParseStr(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["sceneName"]), string.Empty);
			Data._properties.Add(property);
		}
		return Data;
	}
}
