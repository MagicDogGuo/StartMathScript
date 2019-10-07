using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 無限制互換- 容器內互換 matchD
/// </summary>
public class MoveItemControl_matchD : MonoBehaviour {
    Vector3 m_OriginalPos;
    Vector3 m_DragStartPos;
    Vector3 m_DragEndPos;
    Vector3 m_offsetToMouse;
    MoveArea moveArea;

    /// <summary>
    /// 目前Collider到的item放置區
    /// </summary>
    GameObject m_ColliderObj;

    /// <summary>
    /// 暫存用的ColliderObj
    /// </summary>
    GameObject m_TempColliderObj;

    public float CorrectPosX;
    public float CorrectPosY;
    /// <summary>
    /// 是否顯示Log
    /// </summary>
    private bool mIsShowLog = false;
    /// <summary>
    /// 是否在拖拉狀態
    /// </summary>
    private bool mIsDragMode = false;

    void Start()
    {
        m_OriginalPos = this.transform.position;
        moveArea = FindObjectOfType<MoveArea>();
        BoxCollider2D BCollider = this.gameObject.AddComponent<BoxCollider2D>();
        BCollider.isTrigger = true;
        Vector2 OriColliderSize = BCollider.size;
        BCollider.size = OriColliderSize * 0.8f;

    }

    #region OnMouse
    private void OnMouseDown()
    {
        if (m_ColliderObj != null) m_TempColliderObj = m_ColliderObj;

        m_DragStartPos = transform.position;
        m_offsetToMouse = m_DragStartPos - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
        SetLog("onMouseDown, mTempColliderObj : " + ((m_ColliderObj == null) ? "null" : ", name:" + m_ColliderObj.name), LogType.Log);
        mIsDragMode = true;
    }

    private void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z)) + m_offsetToMouse;
        this.transform.SetParent(moveArea.transform);
    }

    private void OnMouseUp()
    {
        m_DragEndPos = transform.position;
        
        var colliderCenter = GetComponent<Collider2D>().bounds.center;
        var posX = Camera.main.WorldToScreenPoint(colliderCenter).x;
        var posY = Camera.main.WorldToScreenPoint(colliderCenter).y;   

        float moveDis = Vector3.Distance(m_DragStartPos, m_DragEndPos);

        SetLog(this.name + " OnMouseUp, m_TempColliderObj == null : " + (m_TempColliderObj == null) + ((m_TempColliderObj == null) ? "" : ", name : " + m_TempColliderObj.name)
            + "\n" + "m_colliderObj == null : " + (m_ColliderObj == null) + ((m_ColliderObj == null) ? "" : ", name : " + m_ColliderObj.name), LogType.Log);

        if (m_TempColliderObj != null && m_TempColliderObj.tag == "matchPosItem")
        {
            SetPosAndParent(m_TempColliderObj);

            if (m_TempColliderObj.transform.childCount > 2)
            {
                SetLog("====Child Cound > 2", LogType.Error);
            }

            //檢查到超過兩個的話
            if (m_TempColliderObj.transform.childCount > 1)
            {
           
                MoveItemControl_matchD prev_item = m_TempColliderObj.transform.GetChild(0).GetComponent<MoveItemControl_matchD>();
                
                if (prev_item != null)
                {
                    if(m_ColliderObj != null)
                    {
                        //回到指定的碰撞區域
                        SetLog("A, m_ColliderObj: " + m_ColliderObj.name + " , m_tempColldier.name:" + m_TempColliderObj.name, LogType.Log);
                        prev_item.transform.SetParent(m_ColliderObj.transform);
                        prev_item.transform.position = m_ColliderObj.transform.position;
                        prev_item.m_ColliderObj = m_ColliderObj;
                        prev_item.m_TempColliderObj = null;
                        SetLog("A,2, prevTempCollider : " + (prev_item.m_TempColliderObj == null ? "null" : prev_item.m_TempColliderObj.name) + " , prevCollider.name:" + (prev_item.m_ColliderObj == null ? "null" : prev_item.m_ColliderObj.name), LogType.Log);
                    }
                    else
                    {
                        //回到MoveArea
                        SetLog("B, m_ColliderObj: " + (m_ColliderObj == null ? "null" : m_ColliderObj.name) + " , m_tempColldier.name:" + (m_TempColliderObj == null ? "null" : m_TempColliderObj.name), LogType.Log);
                        prev_item.transform.SetParent(moveArea.transform);
                        prev_item.transform.position = prev_item.m_OriginalPos;
                        prev_item.m_ColliderObj = null;
                        prev_item.m_TempColliderObj = null;
                        SetLog("B,2, prevTempCollider : " + (prev_item.m_TempColliderObj == null ? "null" : prev_item.m_TempColliderObj.name) + " , prevCollider.name:" + (prev_item.m_ColliderObj == null ? "null" : prev_item.m_ColliderObj.name), LogType.Log);
                    }
                }
            }

            //m_ColliderObj = m_TempColliderObj;
            //m_TempColliderObj = null;
      
        }
        else if (m_ColliderObj != null)
        {
            SetPosAndParent(m_ColliderObj);
        }
        else if (m_TempColliderObj == null)
        {
            this.transform.position = m_OriginalPos;
        }

        m_ColliderObj = m_TempColliderObj;
        m_TempColliderObj = null;
        mIsDragMode = false;

        SetLog(this.name + " ===OnMouseUp finished==== "
  + "\n" + "tempColliderName:" + (m_TempColliderObj == null ? "null" : m_TempColliderObj.name) + " ; m_ColliderObj.Name:" + (m_ColliderObj == null ? "null" : m_ColliderObj.name), LogType.Log);
    }

    #endregion OnMouse

    /// <summary>
    /// //直接把item設在parent的最後一個位置，這樣有需要移除child的話就移除第一個就好
    /// </summary>
    private void SetPosAndParent(GameObject colliderObj)
    {
        //直接把item設在parent的最後一個位置，這樣有需要移除child的話就移除第一個就好
        this.transform.SetParent(colliderObj.transform);
        this.transform.SetAsLastSibling();
        transform.position = new Vector3(colliderObj.transform.position.x + CorrectPosX, colliderObj.transform.position.y + CorrectPosY, -2);
    }

    #region OnTrigger


    //private void OnTriggerStay2D(Collider2D collision)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "moveItem")
            return;

        //判斷數量跟tag     
        if (collision.tag == "matchPosItem")
        {
            //temp collider的話判斷是否為拖拉狀態在進行設定
            m_TempColliderObj = mIsDragMode ? collision.gameObject : null;
            if(!mIsDragMode)
            {
                m_ColliderObj = collision.gameObject;
                SetPosAndParent(m_ColliderObj);
            }

            SetLog(this.name + " ===trigger Enter==== " + collision.name
            + "\n" + "TempColliderName:" + (m_TempColliderObj == null ? "null" : m_TempColliderObj.name) + " ; Collider.Name:" + (m_ColliderObj == null ? "null" : m_ColliderObj.name), LogType.Warning);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "moveItem")
            return;

        if (collision.tag == "matchPosItem")
        {
            m_TempColliderObj = null;
            if (!mIsDragMode) m_ColliderObj = null;

            SetLog(this.name + " ===OnTriggerExit2D==== " + collision.name
        + "\n" + "TempColliderName:" + (m_TempColliderObj == null ? "null" : m_TempColliderObj.name) + " ; Collider.Name:" + (m_ColliderObj == null ? "null" : m_ColliderObj.name), LogType.Log);
        }

    }
    #endregion OnTrigger


    private void SetLog(string info, LogType logType)
    {
        if (!mIsShowLog) return;

        switch(logType)
        {
            case LogType.Log:
                Debug.Log(info);
                break;
            case LogType.Warning:
                Debug.LogWarning(info);
                break;

            case LogType.Error:
                Debug.LogError(info);
                break;
        }
    }
}
