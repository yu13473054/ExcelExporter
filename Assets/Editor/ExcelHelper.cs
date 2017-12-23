using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using UnityEngine;

public class Range
{
    public int row = 0;
    public int col = 0;

    public int Row
    {
        get { return this.row; }
        set { this.row = value; }
    }

    public int Column
    {
        get { return this.col; }
        set { this.col = value; }
    }
};

public class ExcelHelper
{
    public static Range RangeFind(string content, DataTable dt)
    {
        Range Range = new Range();
        bool flag = false;
        int i = 0;
        for (i = 0; i < dt.Rows.Count; i++)
        {
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                if (dt.Rows[i][j].Equals(content))
                {

                    Range.row = i;
                    Range.col = j;

                    flag = true;
                    break;
                }
            }
            if (flag) break;
        }
        if (i == dt.Rows.Count) return null;
        return Range;
    }

    public static Dictionary<string, int> GetColumnsHeader(System.Data.DataTable dt, string[] keyWords)
    {
        Dictionary<string, int> temp = new Dictionary<string, int>();
        for (int i = 0; i < keyWords.Length; i++)
        {
            Range iRange = RangeFind(keyWords[i], dt);                       
            if (iRange != null) 
                temp.Add(keyWords[i], iRange.Column);
        }
        return temp;
    }

    public static Dictionary<string, int> GetColumnsHeader(System.Data.DataTable dt, List<string> keyWords)
    {
        Dictionary<string, int> temp = new Dictionary<string, int>();
        for (int i = 0; i < keyWords.Count; i++)
        {
            Range iRange = RangeFind(keyWords[i], dt);
            if (iRange != null)
                temp.Add(keyWords[i], iRange.Column);
        }
        return temp;
    }

    public static string GetSheetValue(System.Data.DataTable dt, int row, int column)
    {
        if (row >= dt.Rows.Count)
        {
            Debug.LogError("dt.name" + dt.TableName + " row " + row + " 该行不存在");
            return "";
        }
        if (column >= dt.Columns.Count)
        {
            Debug.LogError("dt.name" + dt.TableName + "  row: " + row + " column " + column + " 该列不存在");
            return "";
        }
        return dt.Rows[row][column].ToString();
    }
}