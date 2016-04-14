using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectMgr : SingleTon<ObjectMgr> {

    private List<ObjBase> m_lstObj = new List<ObjBase>();
    private GameObject m_goParent = null;


    public T CreateObject<T>(Vector3 v, string strBaseName = "") where T : ObjBase
    {
        T Temp = CreateObject<T>(strBaseName);
        Temp.transform.position = v;
        return Temp;
    }
    public T CreateObject<T>( string strBaseName = "") 
        where T : ObjBase
    {
        T tReturnValue  =   default(T);

        GameObject go = null;
        if (strBaseName.Length == 0)
        {
            go = new GameObject();
        }
        else
        {
            go = Instantiate<GameObject>(Resources.Load(
                strBaseName, typeof(GameObject)) as GameObject);
        }
        if (go == null)
        {
            return null;
        }

        
        tReturnValue = go.AddComponent<T>();
        tReturnValue.Initialize();
        go.name = "";
        go.name = tReturnValue.ToString();


        var Scale = go.AddComponent<Scaling>();
        Scale.m_vStartScale = new Vector3(0, 0, 0);
        Scale.m_vEndScale = new Vector3(1, 1, 1);
        Scale.m_fMaxTime = 1.0f;

        Scale.gameObject.transform.localScale = Vector3.zero;

        if( m_goParent == null )
        {
            m_goParent = GameObject.Find("ObjectParent");
        }
        if (m_goParent != null)
        {
            go.transform.parent = m_goParent.transform;
        }
        return tReturnValue;
    }

	void Start () {

	}
	
	void Update () {
	
	}

    public void RotatePos( ref Vector3 v , Vector3 vRot, GameObject go )
    {
        Matrix4x4 mat = new Matrix4x4();
        mat.SetTRS(Vector3.zero, Quaternion.Euler( vRot ), Vector3.one);



        v = mat.MultiplyVector(v);
    }
}
