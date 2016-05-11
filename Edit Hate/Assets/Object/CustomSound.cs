using UnityEngine;
using System.Collections;

public class CustomSound : MonoBehaviour {

    public AudioSource m_pAudioSource { get; set; }
    public string m_strSoundName = null;
    public bool m_IsRepeat = false;
    public bool m_IsDelete = false;


    void Awake()
    {
        m_pAudioSource = 
            gameObject.AddComponent<AudioSource>();
    }
	// Use this for initialization
    public void SetupSound( string str , bool IsRepeat)
    {
        if( m_pAudioSource.isPlaying )
        {
            m_pAudioSource.Stop();
        }

        m_strSoundName = str;
        m_pAudioSource.clip = Resources.Load("Sound/" + str, typeof(AudioClip)) as AudioClip;
        m_IsRepeat = IsRepeat;

        
        m_pAudioSource.Play();
    }
	
	// Update is called once per frame
	void Update () {
        if (!m_IsDelete)
        {
            if (!m_pAudioSource.isPlaying)
            {
                if (m_IsRepeat)
                {
                    m_pAudioSource.Play();
                }
                else
                {
                    m_IsDelete = true;
                }
            }
        }
	}
}
