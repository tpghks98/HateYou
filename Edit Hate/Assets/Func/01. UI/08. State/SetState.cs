using UnityEngine;
using System.Collections;

public class SetState : MonoBehaviour {
    public STATEID m_StateID;

    public void OnState( State state)
    {
        state.SetState(m_StateID);
    }
}
