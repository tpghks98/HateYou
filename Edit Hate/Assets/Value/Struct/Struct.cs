using UnityEngine;
using System.Collections;

// unsigned 16 bit x , y;
public struct ST_POINT
{
    public short x;
    public short y;

    public ST_POINT(short nX, short nY)
    {
        x = nX;
        y = nY;
    }

    public static bool operator==( ST_POINT pt1, ST_POINT pt2 )
    {
        if (pt1.x == pt2.x &&
            pt1.y == pt2.y)
        {
            return true;
        }
        return false;
    }
    public static bool operator!=(ST_POINT pt1, ST_POINT pt2)
    {
        if (pt1.x != pt2.x ||
            pt1.y != pt2.y)
        {
            return true;
        }
        return false;
    }

    public override bool Equals(object obj)
    {
        return x == ((ST_POINT)obj).x && y == ((ST_POINT)obj).y;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}


public struct ST_POS_INFOR
{
    public ST_POINT pt;
    public VIEWSTATE vs;
};


public struct ST_ITEM_DATA
{
    public ITEMID nID;
    public ST_POS_INFOR stPosInfo;
};

public struct ST_UNDO_DATA
{
    public bool IsColorChange;
    public bool IsCanUse;
    public ST_POS_INFOR stPrev;
    public ST_ITEM_DATA[] stItemData;
    public BaseAbility pPrevAbility;
};


public struct ST_MOVE_DATA
{
    public Vector2 m_vStart;
    public Vector2 m_vEnd;
};