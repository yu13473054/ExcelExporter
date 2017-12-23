using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.IO;

public static class StrParser
{
    public static readonly char[] splitter = { '|' };

    public static bool ParseBool(string str, bool defVal)
    {
        if (string.IsNullOrEmpty(str))
            return defVal;

        int v;
        if (Int32.TryParse(str, NumberStyles.Integer, provider, out v))
            return v != 0;
        else
        {
            Debug.LogError(string.Format("Invalid parameter when parsing Bool value : {0}", str));        
            return defVal;
        }
    }

    public static int ParseDecInt(string str, int defVal,out bool tf)
    {
        return (int)Math.Round(ParseFloat(str, defVal, out tf));
    }

    public static int ParseDecInt(string str, int defVal)
    {
        // Parse as float as int
        //bool tf;
        //return ParseDecInt(str, defVal/*, out tf*/);

        if (string.IsNullOrEmpty(str))
            return defVal;
        int result = defVal;
        if (!int.TryParse(str, out result))
        {
            Debug.LogError(string.Format("Invalid parameter when parsing Float value : {0}", str));
        }
        return result;
    }

    public static long ParseDecLong(string str, long defVal)
    {
        if (string.IsNullOrEmpty(str))
            return defVal;

        long v;
        if (Int64.TryParse(str, NumberStyles.Integer, provider, out v))
            return v;
        else
        {
            Debug.LogError(string.Format("Invalid parameter when parsing DecLong value : {0}", str));
            return defVal;
        }
    }

    public static long ParseHexLong(string str, long defVal)
    {
        if (string.IsNullOrEmpty(str))
            return defVal;

        long v;
        if (Int64.TryParse(str, NumberStyles.HexNumber, provider, out v))
            return v;
        else
        {
            Debug.LogError(string.Format("Invalid parameter when parsing HexLong value : {0}", str));
            return defVal;
        }
    }

    public static float ParseFloat(string str, float defVal, out bool tf)
    {
        tf = false;

        if (string.IsNullOrEmpty(str))
            return defVal;

        float v = 0;
        if (tf = Single.TryParse(str, out v))
            return (float)v;
        else
        {
            Debug.LogError(string.Format("Invalid parameter when parsing Float value : {0}", str));
            return defVal;
        }
    }

    public static float ParseFloat(string str, float defVal)
    {
        bool tf;
        return ParseFloat(str, defVal, out tf);
    }

    public static float[] ParseFloatList(string str, float defVal)
    {
        List<float> values = new List<float>();

        if (str == null)
            return new float[0];

        string[] vecs = str.Split(splitter);

        for (int i = 0; i < vecs.Length; i++)
            values.Add(ParseFloat(vecs[i], defVal));

        return values.ToArray();
    }

    public static double ParseDouble(string str, double defVal)
    {
        if (string.IsNullOrEmpty(str))
            return defVal;

        double v = 0;
        if (Double.TryParse(str, out v))
            return v;
        else
        {
            Debug.LogError(string.Format("Invalid parameter when parsing Double value : {0}", str));
            return defVal;
        }
    }

    public static int[] ParseDecIntList(string str, int defVal)
    {
        List<int> values = new List<int>();

        if (str == null)
            return new int[0];

        string[] vecs = str.Split(splitter);

        for (int i = 0; i < vecs.Length; i++)
            values.Add(ParseDecInt(vecs[i], defVal));

        return values.ToArray();
    } 

    public static string ParseStr(string str, string defValue)
    {
        return str == null ? defValue : str;
    }

    public static string[] ParseStrList(string str, string[] defValue)
    {
        return string.IsNullOrEmpty(str) ? defValue : str.Split(splitter);
    }

    private static CultureInfo provider = CultureInfo.InvariantCulture;
}