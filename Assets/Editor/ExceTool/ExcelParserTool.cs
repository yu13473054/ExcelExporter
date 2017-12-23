using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using Excel;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class ExcelParserTool
{
    private static string TableDir = "/配置表";
    private static string ExcelDirPath = System.Environment.CurrentDirectory + TableDir;

    private static string PropertyDirPath = Application.dataPath + "/Scripts/DataBase/Property";
    private static string ExporterDirPath = Application.dataPath + "/Editor/ExceTool/Exporter";
    private static string AssetDataDirPath = "Assets/Res/AssetData";

    [MenuItem("表格数据/生成数据结构文件")]
    public static void GenerateDataStructure()
    {
        if (!Directory.Exists(ExcelDirPath))
        {
            Directory.CreateDirectory(ExcelDirPath);
        }
        if (!Directory.Exists(PropertyDirPath))
        {
            Directory.CreateDirectory(PropertyDirPath);
        }
        if (!Directory.Exists(ExporterDirPath))
        {
            Directory.CreateDirectory(ExporterDirPath);
        }

        //清理之前的Property数据文件
        ClearOldFiles(PropertyDirPath);
        //清理之前的Exporter数据文件
        ClearOldFiles(ExporterDirPath);

        //获得所有的excel文件
        string[] files = Directory.GetFiles(ExcelDirPath, "*.xlsx");
        for (int i = 0; i < files.Length; i++)
        {
            string singleFile = files[i];
            string fileName = Path.GetFileNameWithoutExtension(singleFile);
            EditorUtility.DisplayProgressBar("正在生成数据结构", Path.GetFullPath(singleFile), (i + 1) * 1f / files.Length);
            ParserSingleFile(singleFile, fileName);
        }
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
        Debug.Log("所有Excel文件对应的解析文件创建完成");
    }

    [MenuItem("表格数据/解析生成程序使用的数据文件")]
    public static void ParseDataToFile()
    {
        if (!Directory.Exists(AssetDataDirPath))
        {
            Directory.CreateDirectory(AssetDataDirPath);
        }
        ClearOldFiles(AssetDataDirPath);

        string[] excelExporters = Directory.GetFiles(ExporterDirPath, "*.cs");
        for (int i = 0; i < excelExporters.Length; i++)
        {
            string exporterName = Path.GetFileNameWithoutExtension(excelExporters[i]);
            string propertyName = exporterName.Replace("Exporter", "Data");
            EditorUtility.DisplayProgressBar("正在导出数据...", propertyName, (i + 1) * 1f / excelExporters.Length);
            MethodInfo method = Type.GetType(exporterName).GetMethod("ReadExcel");
            Object result = (Object) method.Invoke(null,null);
            AssetDatabase.CreateAsset(result,string.Format("{0}/{1}.asset", AssetDataDirPath, propertyName));
        }
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
        Debug.Log("Excel数据导出成功！！");
    }

    static void ClearOldFiles(string dirPath)
    {
        string[] files = Directory.GetFiles(dirPath);
        for (int i = 0; i < files.Length; i++)
        {
            File.Delete(files[i]);
        }
    }

    static void ParserSingleFile(string filePath, string fileName)
    {
        FileStream stream = null;
        try
        {
            stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
        catch (System.Exception ex)
        {
            string err = string.Format("读取配置表[{0}]失败, 检查是否用Excel打开了这个配置表", fileName);
            EditorUtility.DisplayDialog("", err, "失败");
            Debug.LogError(err);
            return;
        }
        using (stream)
        {

            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            DataSet result = excelReader.AsDataSet();

            //一张表格中只使用第一个sheet
            System.Data.DataTable wookSheet = result.Tables[0];
            if (wookSheet == null)
            {
                Debug.LogError("Excel中没有Sheet");
                return;
            }
            string propertyName = wookSheet.TableName.Substring(0, 1).ToUpper() + wookSheet.TableName.Substring(1);
            List<string> keys = new List<string>();
            List<string> typeChecks = new List<string>();
            List<string> values = new List<string>();
            int columnCount = wookSheet.Columns.Count;
            for (int index = 0; index < columnCount; index++)
            {
                string keyText = wookSheet.Rows[0][index].ToString();
                if (!keyText.Equals(string.Empty))
                {
                    keys.Add(keyText.ToLower());
                    typeChecks.Add(wookSheet.Rows[1][index].ToString());
                    values.Add(wookSheet.Rows[2][index].ToString());
                }
            }

            CreatCSFile(propertyName, keys, typeChecks, values);
            CreatExporter(fileName, wookSheet.TableName, propertyName, keys, typeChecks, values);
        }
    }

    private static void CreatCSFile(string csPrefix, List<string> keys, List<string> typeChecks, List<string> values)
    {
        string scriptPath = PropertyDirPath + "/" + csPrefix + "Data.cs";
        FileStream fs = new FileStream(scriptPath, FileMode.Create, FileAccess.Write);
        StreamWriter sr = new StreamWriter(fs);

        //命名空间的引入
        sr.WriteLine("using System;");
        sr.WriteLine("using System.Collections.Generic;");
        sr.WriteLine("using UnityEngine;");
        sr.WriteLine();
        //创建ScriptableObject代码
        sr.WriteLine("public class {0}Data : ScriptableObject", csPrefix);
        sr.WriteLine("{");
        sr.WriteLine("\tpublic List<{0}Property> _properties = new List<{0}Property>();", csPrefix);
        sr.WriteLine("}");
        sr.WriteLine();
        //创建单个对象的解析类
        sr.WriteLine("[Serializable]");
        sr.WriteLine("public class {0}Property", csPrefix);
        sr.WriteLine("{");

        for (int i = 0; i < keys.Count; i++)
        {
            //默认为int
            string valueType = "int";

            string checkName = typeChecks[i].ToLower();

            if (checkName.Contains("list"))
            {
                if (checkName.Contains("int"))
                    valueType = "int[]";
                else if (checkName.Contains("float"))
                    valueType = "float[]";
                else if (checkName.Contains("string"))
                    valueType = "string[]";
            }
            else
            {
                valueType = keys[i];
            }
            sr.WriteLine("\tpublic {0} _{1};", valueType, GetValueType(values[i]));
        }

        sr.WriteLine("}");
        sr.Close();
        fs.Close();
    }

    private static void CreatExporter(string fileName, string tableName, string propertyName, List<string> keys, List<string> typeChecks, List<string> values)
    {
        string editorPath = ExporterDirPath + "/" + propertyName + "Exporter.cs";
        FileStream exporFs = new FileStream(editorPath, FileMode.Create, FileAccess.Write);
        StreamWriter exporSw = new StreamWriter(exporFs);

        exporSw.WriteLine("using System.Collections.Generic;");
        exporSw.WriteLine("using UnityEngine;");
        exporSw.WriteLine("using UnityEditor;");
        exporSw.WriteLine("using Excel;");
        exporSw.WriteLine("using System.IO;");
        exporSw.WriteLine("using System.Data;");
        exporSw.WriteLine();
        exporSw.WriteLine("public class {0}Exporter", propertyName);
        exporSw.WriteLine("{");
        exporSw.WriteLine();
        exporSw.WriteLine("\tpublic static {0}Data ReadExcel()", propertyName);
        exporSw.WriteLine("\t{");
        exporSw.WriteLine("\t\tstring excelName = System.Environment.CurrentDirectory + \"{0}/{1}.xlsx\";", TableDir, fileName);
        exporSw.WriteLine("\t\tFileStream stream = File.Open(excelName, FileMode.Open, FileAccess.Read, FileShare.Read);");
        exporSw.WriteLine("\t\tIExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);");
        exporSw.WriteLine("\t\tDataSet result = excelReader.AsDataSet();");
        exporSw.WriteLine();
        exporSw.WriteLine("\t\tSystem.Data.DataTable wookSheet = result.Tables[\"{0}\"];", tableName);
        exporSw.WriteLine("\t\tint startRow = 5;");
        exporSw.WriteLine("\t\tint endRow = wookSheet.Rows.Count;");
        exporSw.WriteLine("\t\tint columnCount = wookSheet.Columns.Count;");
        exporSw.WriteLine();
        exporSw.WriteLine("\t\tList<string> keys = new List<string>();");
        exporSw.WriteLine("\t\tfor (int index = 0; index < columnCount; index++)");
        exporSw.WriteLine("\t\t{");
        exporSw.WriteLine("\t\t\tstring keyText = wookSheet.Rows[2][index].ToString();");
        exporSw.WriteLine("\t\t\tif (!keyText.Equals(string.Empty))");
        exporSw.WriteLine("\t\t\tkeys.Add(keyText);");
        exporSw.WriteLine("\t\t}");
        exporSw.WriteLine("\t\tvar headerColumns = ExcelHelper.GetColumnsHeader(wookSheet, keys);");
        exporSw.WriteLine("\t\t{0}Data Data = ScriptableObject.CreateInstance<{0}Data>();", propertyName);

        exporSw.WriteLine("\t\tfor (int row = startRow; row < endRow; row++)");
        exporSw.WriteLine("\t\t{");
        exporSw.WriteLine("\t\t\tstring checkStr = ExcelHelper.GetSheetValue(wookSheet, row, headerColumns[keys[0]]);");
        exporSw.WriteLine("\t\t\tif(checkStr.Equals(string.Empty))");
        exporSw.WriteLine("\t\t\t\tcontinue;");
        exporSw.WriteLine("\t\t\t{0}Property property = new {0}Property();", propertyName);
        for (int index = 0; index < values.Count; index++)
        {
            exporSw.WriteLine("\t\t\tproperty._{0} = {1}", GetValueType(values[index]), GetKeyType(keys[index], typeChecks[index], values[index]));
        }

        exporSw.WriteLine("\t\t\tData._properties.Add(property);");
        exporSw.WriteLine("\t\t}");
        exporSw.WriteLine("\t\treturn Data;");
        exporSw.WriteLine("\t}");
        exporSw.WriteLine("}");
        exporSw.Close();
        exporFs.Close();
    }

    public static string GetKeyType(string keyName, string checkName, string columnName)
    {
        //默认为int
        string valueType = "";
        //先小写
        checkName = checkName.ToLower();
        //如果第二列没有List标志
        if (checkName.Contains("list"))
        {
            if (checkName.Contains("int"))
                return string.Format("StrParser.ParseDecIntList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns[\"{0}\"]), 0);", columnName);
            else if (checkName.Contains("float"))
                return string.Format("StrParser.ParseFloatList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns[\"{0}\"]), 0f);", columnName);
            else if (checkName.Contains("string"))
                return string.Format("StrParser.ParseStrList(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns[\"{0}\"]), new string[0]);", columnName);
        }
        else
        {
            if (keyName.Equals("int"))
                return string.Format("StrParser.ParseDecInt(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns[\"{0}\"]), 0);", columnName);
            else if (keyName.Equals("float"))
                return string.Format("StrParser.ParseFloat(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns[\"{0}\"]), 0f);", columnName);
            else if (keyName.Equals("string"))
                return string.Format("StrParser.ParseStr(ExcelHelper.GetSheetValue(wookSheet, row, headerColumns[\"{0}\"]), string.Empty);", columnName);
        }

        return valueType;
    }

    public static string GetValueType(string value)
    {
        //连续两个字母都为大写则全部小写
        if (value.Length > 1 && !(char.IsUpper(value.ToCharArray()[0]) && char.IsUpper(value.ToCharArray()[1])))
            return value.Substring(0, 1).ToLower() + value.Substring(1);

        return value.ToLower();
    }
}