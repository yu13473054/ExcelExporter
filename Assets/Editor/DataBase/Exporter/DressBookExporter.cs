using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Excel;
using System.IO;
using System.Data;

public class DressBookExporter
{

	public static DressBookData ReadExcel()
	{
		string excelName = System.Environment.CurrentDirectory + "/配置表/DressBook_精品图鉴.xlsx";
		FileStream stream = File.Open(excelName, FileMode.Open, FileAccess.Read, FileShare.Read);
		IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
		DataSet result = excelReader.AsDataSet();

		System.Data.DataTable wookSheet = result.Tables["dressBook"];
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
		DressBookData Data = ScriptableObject.CreateInstance<DressBookData>();
		for (int row = startRow; row < endRow; row++)
		{
			string checkStr = ExcelHelper.GetSheetValue(wookSheet, row, headerColumns[keys[0]]);
			if(checkStr.Equals(string.Empty))
				continue;
			DressBookProperty property = new DressBookProperty();
			property._id = StrParser.ParseDecInt(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["ID"]), 0);
			property._tabNameId = StrParser.ParseDecInt(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["TabNameId"]), 0);
			property._tabName = StrParser.ParseStr(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["TabName"]), string.Empty);
			property._dressSetIdArray = StrParser.ParseDecIntList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["DressSetIdArray"]), 0);
			property._icon = StrParser.ParseStr(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["Icon"]), string.Empty);
			property._belongs = StrParser.ParseDecInt(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns["Belongs"]), 0);
			Data._properties.Add(property);
		}
		return Data;
	}
}
