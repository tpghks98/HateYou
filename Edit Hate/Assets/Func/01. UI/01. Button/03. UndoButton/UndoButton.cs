using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UndoButton : MonoBehaviour {

    public Sprite[] m_pSprite;
    private Image m_pImage;
    private Button m_pButton;
    private SPRITE_ID m_nSpriteID;

    private enum SPRITE_ID
    {
        NONE_ACTIVE, ACTIVE, MAX
    };

	// Use this for initialization
	void Start () {
        m_pImage = GetComponent<Image>();
        m_pButton = GetComponent<Button>();
        m_nSpriteID = SPRITE_ID.NONE_ACTIVE;
	}
	
	// Update is called once per frame
	void Update () {
        if( InGameController.Instance.Player.GetCanUndo() )
        {
            OnActive();
        }
        else
        {
            OnUnActive();
        }
	}
    public void OnActive()
    {
        if (m_nSpriteID != SPRITE_ID.ACTIVE)
        {
            m_nSpriteID = SPRITE_ID.ACTIVE;
            StateChange();
        }
    }

    public void OnUnActive()
    {
        if (m_nSpriteID != SPRITE_ID.NONE_ACTIVE)
        {
            m_nSpriteID = SPRITE_ID.NONE_ACTIVE;
            StateChange();
        }
    }

    private void StateChange()
    {
        switch( m_nSpriteID )
        {
            case SPRITE_ID.ACTIVE:
                m_pButton.enabled = true;
                break;
            case SPRITE_ID.NONE_ACTIVE:
                m_pButton.enabled = false;
                break;
        }
        m_pImage.sprite = m_pSprite[(int)m_nSpriteID];
        m_pImage.SetNativeSize();

    }

    public void OnUndo()
    {
        if( m_nSpriteID == SPRITE_ID.ACTIVE && !InGameController.Instance.Pause)
        {
            InGameController.Instance.Player.Undo();
        }
    }
}
