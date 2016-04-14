using UnityEngine;
using System.Collections;

public abstract class ObjBase : MonoBehaviour {

    // Variable
    protected bool m_IsDelete     =   false;
    protected ST_POINT m_vPosition = new ST_POINT(0, 0);
    protected VIEWSTATE m_vsView    =   VIEWSTATE.TOP;

    // Property
    public bool Delete
    {
        get { return m_IsDelete; }
        set { m_IsDelete = value; }
    }
    public ST_POINT Point
    {
        get { return m_vPosition; }
        set { m_vPosition = value; }
    }
    public VIEWSTATE View
    {
        get { return m_vsView; }
        set { m_vsView = value; }
    }

    // Function
    public abstract void Initialize();

    public void DestroyObject()
    {
        if (!m_IsDelete)
        {
            m_IsDelete = true;
          //  gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }
}
