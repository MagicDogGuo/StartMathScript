using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using UnityEngine.UI;
using UnityEngine.Events;

public class SelectItem : MonoBehaviour {


    public Image select_BG;
    public Image select_Btn;


    private Sprite BtnSprite;
    private Sprite CreateSprite;

    private int Index;
    public bool IsRightAnswer;

    GameObject m_obj;


    public bool IsClick = false;
    public void InitItem(int index,bool isRightAnswer,Sprite btnSprite,Sprite createSprite)
    {
        Index = index;
        BtnSprite = btnSprite;
        CreateSprite = createSprite;

        IsRightAnswer = isRightAnswer;

        select_BG.sprite = createSprite;
        select_BG.SetNativeSize();
        select_BG.enabled = false;

        select_Btn.sprite = btnSprite;
        select_Btn.SetNativeSize();
        select_Btn.enabled = true;

        ////////////////////////////////////////////////////
        CreateGameObjSelect_BG(createSprite);
        m_obj.SetActive(false);

    }

    //private void Start()
    //{
    //    AddListener(BtnClick);
    //}

    //private void BtnClick(int Index)
    //{
    //    IsClick = !IsClick;

    //    SetClickState(IsClick);
    //}

    public bool GetThisItemRespond
    {
        get {
            if ((IsClick && IsRightAnswer)||(!IsClick&&!IsRightAnswer))
                return true;
            else
                return false;
        }
    }

    public void SetClickState(bool isClick)
    {
        IsClick = isClick;
        if (isClick)
        {
            //select_BG.enabled = true;////////////////////////////////////////////////////////////
            m_obj.SetActive(true);
        }
        else
        {
           //select_BG.enabled = false;
            m_obj.SetActive(false);

        }
    }
    private int BtnStateIndex = 0;
    public void SetClickState()
    {
        if (BtnStateIndex % 2 == 0)
        {
            IsClick = false;
            SetClickState(false);
        }
        else
        {
            IsClick = true;
            SetClickState(true);
        }   
    }
    public void AddListener(Action<int> clickEvent)
    {
        select_Btn.GetComponent<Button>().onClick.AddListener(()=> {
            BtnStateIndex++;
            clickEvent(Index);
        });
    }


    void CreateGameObjSelect_BG(Sprite createSprite)
    {
        m_obj = new GameObject();
        m_obj.name = createSprite.ToString();
        m_obj.AddComponent<Image>().sprite = createSprite;
        m_obj.transform.parent = this.gameObject.transform;
        m_obj.transform.localScale = Vector3.one;
        m_obj.transform.localPosition = Vector3.zero;
        m_obj.GetComponent<Image>().SetNativeSize();
        m_obj.GetComponent<Image>().raycastTarget = false;
    }



}
