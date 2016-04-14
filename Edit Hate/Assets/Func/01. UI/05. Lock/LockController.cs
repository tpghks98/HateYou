using UnityEngine;
using System.Collections;

public class LockController : MonoBehaviour {

    public Lock[] m_ArrLock;

    public void OriSetup()
    {
        for( int n= 0; n < m_ArrLock.Length; ++n )
        {
            m_ArrLock[n].Setup();
        }
    }
    public void AllLocking()
    {
        for( int n =0; n < m_ArrLock.Length; ++n )
        {
            m_ArrLock[n].ImageLock();
        }
    }
    public void UnLocking( int nOpen )
    {
        if( nOpen > m_ArrLock.Length)
        {
            Debug.Log( "Lock Size OverFlow");
            nOpen = m_ArrLock.Length;
        }

        for (int n = 0; n < nOpen; ++n)
        {
            m_ArrLock[n].ImageUnlock();
        }
    }
}
