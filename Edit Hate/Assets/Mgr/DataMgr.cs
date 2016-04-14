using UnityEngine;
using System.Collections;

public class DataMgr : SingleTon<DataMgr> {

    public string GetStringToken(ref string pool, ref int nCount)
    {
        string strTemp = "";
        char c;
        while (true)
        {
            c = (char)pool[nCount++];
            if (c == 'e' ||
                nCount >= pool.Length)
            {
                break;
            }
            strTemp += c;
        }

        return strTemp;
    }

    public int GetIntegerToken( ref string pool, ref int nCount )
    {
        string str = GetStringToken(ref pool, ref nCount);

        return System.Convert.ToInt32(str);
    }
    public short GetShortToken( ref string pool, ref int nCount )
    {
        string str = GetStringToken(ref pool, ref nCount);

        return System.Convert.ToInt16(str);
    }
}
