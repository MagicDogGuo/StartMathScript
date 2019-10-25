using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickPosItemControl_photoHuntA : MonoBehaviour {
    [HideInInspector]
    public Sprite ClickedPosItemSprite;
    [HideInInspector]
    public AudioClip ClickedPosItemSound;
    [HideInInspector]
    public GameObject CorrectObj;
    [HideInInspector]
    public Vector2 CorrectObjPos;

    AudioSource m_AS = null;
    bool isAllreadyCheck = false;
    
    private void Start()
    {
        //產生Collider
        BoxCollider2D BCollider = this.gameObject.AddComponent<BoxCollider2D>();
        BCollider.isTrigger = true;
        //音效
        m_AS = gameObject.AddComponent<AudioSource>();
        m_AS.clip = ClickedPosItemSound;
        m_AS.Stop();

        isAllreadyCheck = false;
    }


    private void OnMouseUp()
    {
        if (PhotoHuntAManager.IsItemSoundPlaying)
            return;
        if (!isAllreadyCheck)
        {
            this.GetComponent<SpriteRenderer>().sprite = ClickedPosItemSprite;
            StartCoroutine(IE_PlaySound());
        }      
    }

    private IEnumerator IE_PlaySound()
    {
        PhotoHuntAManager.IsItemSoundPlaying = true;
        GamePublicAudioControl.Instance.DownSceneMusic();
        
        m_AS.Stop();
        m_AS.Play();

        
        yield return null;
        while (m_AS.isPlaying)
            yield return null;

        GamePublicAudioControl.Instance.UpSceneMusic();
        yield return new WaitForSeconds(1f);
        isAllreadyCheck = true;
        PhotoHuntAManager.IsItemSoundPlaying = false;
    }
}
