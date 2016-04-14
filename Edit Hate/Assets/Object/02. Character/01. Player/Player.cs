using UnityEngine;
using System.Collections;

public class Player : Character {

    private Vector2 m_vPrevMousePosition;


    public override void Initialize()
    {


        m_stUndoData.IsCanUse = false;
        m_stUndoData.IsColorChange = false;
        m_vPrevMousePosition = new Vector2(-1, -1);
        m_nMoveCount = 1;
        OnAnimation("Idle");
    }
	
    public void OnMouseReset()
    {
        m_vPrevMousePosition = new Vector2(-1, -1);
    }
	// Update is called once per frame
	void Update () {
        if( Input.GetKeyDown( KeyCode.A ) )
        {
            Undo();
        }
        if (!InGameController.Instance.IsDie &&
            Time.timeScale != 0.0f)
        {
            if (m_fRestTime > 0.0f)
            {
                m_fRestTime -= Time.deltaTime;
            }
            KeyInput();
        }
	}

    private void OnSlide( Vector2 vDirection )
    {
        if (Mathf.Abs(vDirection.x) + Mathf.Abs(vDirection.y) > 50.0f)
        {
            vDirection.Normalize();
            Move(ref vDirection);
        }
        else
        {
            if (m_pAbility != null)
            {
                if (m_pAbility.GetType() == typeof(WarpAbility))
                {
                    m_pAbility.OnAbility();
                }
            }
        }
    }
    private void KeyInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    m_vPrevMousePosition = touch.position;
                    break;
                case TouchPhase.Ended:
                    if (m_vPrevMousePosition != new Vector2(-1, -1))
                    {
                        Vector2 vDirection = InGameController.Instance.
                            CalcTouchDir(m_vPrevMousePosition, touch.position, m_vsView);
                        OnSlide(vDirection);
                    }
                    break;
            }
        }
        if( Input.GetMouseButtonDown( 0 ) )
        {
            m_vPrevMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (m_vPrevMousePosition != new Vector2(-1, -1))
            {
                Vector2 vDirection = InGameController.Instance.
                    CalcTouchDir(m_vPrevMousePosition, Input.mousePosition, m_vsView);
                OnSlide(vDirection);
            }
        }
    }


}
