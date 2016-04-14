using UnityEngine;
using System.Collections;

public class Block : Structure {

    private Renderer m_pRenderer;
    private bool m_IsWhite;

    public bool IsWhite
    {
        get { return m_IsWhite; }
    }
    public override void Initialize()
    {
        m_IsWhite = false;
        m_pRenderer = GetComponent<Renderer>();
    }
    public override void OnCollisionPlayer()
    {
        ReverseColor();
    }

    public void ChangeColor(bool IsWhite)
    {
        m_IsWhite = IsWhite;
        m_pRenderer.material =
            InGameController.Instance.GetMaterial(IsWhite);
    }

    private void ReverseColor()
    {
        m_IsWhite = !m_IsWhite;
        m_pRenderer.material =
            InGameController.Instance.GetMaterial(m_IsWhite);
    }

	void Update () {
	
	}
}
