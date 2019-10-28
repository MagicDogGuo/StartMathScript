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
    public GameObject m_ColliderObj;

    /// <summary>
    /// 暫存用的ColliderObj
    /// </summary>
    List<GameObject> mTempColliderObjList;

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
        mTempColliderObjList = new List<GameObject>();
    }

    #region OnMouse
    private void OnMouseDown()
    {
        //if (m_ColliderObj != null) m_TempColliderObj = m_ColliderObj;
        if (m_ColliderObj != null)
            mTempColliderObjList.Add(m_ColliderObj);

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
        //      m_DragEndPos = transform.position;

        //      var colliderCenter = GetComponent<Collider2D>().bounds.center;
        //      var posX = Camera.main.WorldToScreenPoint(colliderCenter).x;
        //      var posY = Camera.main.WorldToScreenPoint(colliderCenter).y;   

        //      float moveDis = Vector3.Distance(m_DragStartPos, m_DragEndPos);

        //      SetLog(this.name + " OnMouseUp, m_TempColliderObj == null : " + (m_TempColliderObj == null) + ((m_TempColliderObj == null) ? "" : ", name : " + m_TempColliderObj.name)
        //          + "\n" + "m_colliderObj == null : " + (m_ColliderObj == null) + ((m_ColliderObj == null) ? "" : ", name : " + m_ColliderObj.name), LogType.Log);

        //      if (m_TempColliderObj != null && m_TempColliderObj.tag == "matchPosItem")
        //      {
        //          SetPosAndParent(m_TempColliderObj);

        //          if (m_TempColliderObj.transform.childCount > 2)
        //          {
        //              SetLog("====Child Cound > 2", LogType.Error);
        //          }

        //          //檢查到超過兩個的話
        //          if (m_TempColliderObj.transform.childCount > 1)
        //          {

        //              MoveItemControl_matchD prev_item = m_TempColliderObj.transform.GetChild(0).GetComponent<MoveItemControl_matchD>();

        //              if (prev_item != null)
        //              {
        //                  if(m_ColliderObj != null)
        //                  {
        //                      //回到指定的碰撞區域
        //                      SetLog("A, m_ColliderObj: " + m_ColliderObj.name + " , m_tempColldier.name:" + m_TempColliderObj.name, LogType.Log);
        //                      prev_item.transform.SetParent(m_ColliderObj.transform);
        //                      prev_item.transform.position = m_ColliderObj.transform.position;
        //                      prev_item.m_ColliderObj = m_ColliderObj;
        //                      prev_item.m_TempColliderObj = null;
        //                      SetLog("A,2, prevTempCollider : " + (prev_item.m_TempColliderObj == null ? "null" : prev_item.m_TempColliderObj.name) + " , prevCollider.name:" + (prev_item.m_ColliderObj == null ? "null" : prev_item.m_ColliderObj.name), LogType.Log);
        //                  }
        //                  else
        //                  {
        //                      //回到MoveArea
        //                      SetLog("B, m_ColliderObj: " + (m_ColliderObj == null ? "null" : m_ColliderObj.name) + " , m_tempColldier.name:" + (m_TempColliderObj == null ? "null" : m_TempColliderObj.name), LogType.Log);
        //                      prev_item.transform.SetParent(moveArea.transform);
        //                      prev_item.transform.position = prev_item.m_OriginalPos;
        //                      prev_item.m_ColliderObj = null;
        //                      prev_item.m_TempColliderObj = null;
        //                      SetLog("B,2, prevTempCollider : " + (prev_item.m_TempColliderObj == null ? "null" : prev_item.m_TempColliderObj.name) + " , prevCollider.name:" + (prev_item.m_ColliderObj == null ? "null" : prev_item.m_ColliderObj.name), LogType.Log);
        //                  }
        //              }
        //          }

        //          //m_ColliderObj = m_TempColliderObj;
        //          //m_TempColliderObj = null;

        //      }
        //      else if (m_ColliderObj != null)
        //      {
        //          SetPosAndParent(m_ColliderObj);
        //      }
        //      else if (m_TempColliderObj == null)
        //      {
        //          this.transform.position = m_OriginalPos;
        //      }

        //      m_ColliderObj = m_TempColliderObj;
        //      m_TempColliderObj = null;
        //      mIsDragMode = false;

        //      SetLog(this.name + " ===OnMouseUp finished==== "
        //+ "\n" + "tempColliderName:" + (m_TempColliderObj == null ? "null" : m_TempColliderObj.name) + " ; m_ColliderObj.Name:" + (m_ColliderObj == null ? "null" : m_ColliderObj.name), LogType.Log);

        m_DragEndPos = transform.position;

        var colliderCenter = GetComponent<Collider2D>().bounds.center;
        var posX = Camera.main.WorldToScreenPoint(colliderCenter).x;
        var posY = Camera.main.WorldToScreenPoint(colliderCenter).y;

        float moveDis = Vector3.Distance(m_DragStartPos, m_DragEndPos);

        //SetLog(this.name + " OnMouseUp, m_TempColliderObj == null : " + (m_TempColliderObj == null) + ((m_TempColliderObj == null) ? "" : ", name : " + m_TempColliderObj.name)
        //    + "\n" + "m_colliderObj == null : " + (m_ColliderObj == null) + ((m_ColliderObj == null) ? "" : ", name : " + m_ColliderObj.name), LogType.Log);

        if(mTempColliderObjList.Count > 0)
        {
            //有超過兩個temp碰撞區的話，找哪個碰撞區最近，就設定他為parent
            GameObject tempColliderObj = mTempColliderObjList[mTempColliderObjList.Count - 1];
            Vector2 selfPos = this.transform.position;
            if(mTempColliderObjList.Count >= 2)
            {
                GameObject nearestCollider = mTempColliderObjList[0];
                for(int idx = 1; idx < mTempColliderObjList.Count; idx++)
                {
                    GameObject compareCollider = mTempColliderObjList[idx];
                    float diffA = Vector2.Distance(selfPos, nearestCollider.transform.position);
                    float diffB = Vector2.Distance(selfPos, compareCollider.transform.position);
                    if (diffB < diffA)
                        nearestCollider = compareCollider;
                }
                tempColliderObj = nearestCollider;
            }
            
            SetPosAndParent(tempColliderObj);

            //log += "tempColliderobj.name:" + tempColliderObj.name+"\n";
            //for(int i = 0; i < tempColliderObj.transform.childCount; i++)
            //{
            //    log += "child idx: " + tempColliderObj.transform.GetChild(i).name + "\n";
            //}



            //Debug.LogError(log);

            //檢查到超過兩個的話
            if (tempColliderObj.transform.childCount > 1)
            {
                

                //MoveItemControl_matchD prev_item = tempColliderObj.transform.GetChild(0).GetComponent<MoveItemControl_matchD>();
                MoveItemControl_matchD prev_item = tempColliderObj.transform.GetChild(0).GetComponent<MoveItemControl_matchD>();

                if (prev_item != null)
                {
                    if (m_ColliderObj != null)
                    {
                        //讓先前的item回到指定的碰撞區域
                        SetLog("A, m_ColliderObj: " + m_ColliderObj.name + " , m_tempColldier.name:" + tempColliderObj.name, LogType.Log);
                        prev_item.transform.SetParent(m_ColliderObj.transform);
                        SetLog("A1,", LogType.Log);
                        prev_item.transform.position = m_ColliderObj.transform.position;
                        SetLog("A2,", LogType.Log);
                        prev_item.m_ColliderObj = m_ColliderObj;
                        SetLog("A3,", LogType.Log);
                        prev_item.mTempColliderObjList.Clear();
                        SetLog("A4, prevTempCollider : " + (prev_item.mTempColliderObjList.Count)  + " , prevCollider.name:" + (prev_item.m_ColliderObj == null ? "null" : prev_item.m_ColliderObj.name), LogType.Log);
                    }
                    else
                    {
                        //回到MoveArea
                        SetLog("B, m_ColliderObj: " + (m_ColliderObj == null ? "null" : m_ColliderObj.name) + " , m_tempColldier.name:" + (tempColliderObj == null ? "null" : tempColliderObj.name), LogType.Log);
                        prev_item.transform.SetParent(moveArea.transform);
                        prev_item.transform.position = prev_item.m_OriginalPos;
                        prev_item.m_ColliderObj = null;
                        prev_item.mTempColliderObjList.Clear();
                        SetLog("B,2, prevTempCollider count : " + (prev_item.mTempColliderObjList.Count) + " , prevCollider.name:" + (prev_item.m_ColliderObj == null ? "null" : prev_item.m_ColliderObj.name), LogType.Log);
                    }
                }
                m_ColliderObj = tempColliderObj;
            }

            //m_ColliderObj = m_TempColliderObj;
            //m_TempColliderObj = null;
        }
        else if (m_ColliderObj != null)
        {
            Debug.LogError("m_ColliderObj != null ");
            SetPosAndParent(m_ColliderObj);
        }
        else if (mTempColliderObjList.Count == 0)
        {
            Debug.LogError("mTempColliderObjList.Count == " + mTempColliderObjList.Count);
            this.transform.position = m_OriginalPos;
        }

        //m_ColliderObj = m_TempColliderObj;
        mTempColliderObjList.Clear();
        mIsDragMode = false;

  //      SetLog(this.name + " ===OnMouseUp finished==== "
  //+ "\n" + "tempColliderName:" + (m_TempColliderObj == null ? "null" : m_TempColliderObj.name) + " ; m_ColliderObj.Name:" + (m_ColliderObj == null ? "null" : m_ColliderObj.name), LogType.Log);
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
            if (mIsDragMode)
            {
                mTempColliderObjList.Add(collision.gameObject);
            }
            else
            {
                mTempColliderObjList.Remove(collision.gameObject);
            }

            SetLog(this.name + " ===trigger Enter==== " + collision.name + ", isDrag : " + mIsDragMode
+ "\n" + "TempColliderName:" + (mTempColliderObjList.Count) + " ; Collider.Name:" + (m_ColliderObj == null ? "null" : m_ColliderObj.name), LogType.Warning);
            if (!mIsDragMode)
            {
                m_ColliderObj = collision.gameObject;
                SetPosAndParent(m_ColliderObj);
            }

            SetLog(this.name + " ===trigger Enter==== " + collision.name
            + "\n" + "TempColliderName:" + (mTempColliderObjList.Count) + " ; Collider.Name:" + (m_ColliderObj == null ? "null" : m_ColliderObj.name), LogType.Warning);
        }

        //Debug.Log("OnTrigger2D, Item: " + collision.name + ", tag: " + collision.tag);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "moveItem")
            return;


        if (collision.tag == "matchPosItem" )
        {
            mTempColliderObjList.Remove(collision.gameObject);

            SetLog(this.name + " ===OnTriggerExit2D==== " + collision.name + ", isDrag : " + mIsDragMode
+ "\n" + "TempColliderName:" + (mTempColliderObjList.Count) + " ; Collider.Name:" + (m_ColliderObj == null ? "null" : m_ColliderObj.name), LogType.Log);

            //if (!mIsDragMode)
            //{
            //    MatchPosItemControl_matchD comp = this.GetComponentInParent<MatchPosItemControl_matchD>();
            //    if(comp != null)
            //    {
            //        m_ColliderObj = comp.gameObject;
            //    }
            //    else
            //    {
            //        m_ColliderObj = null;
            //    }
            //}
                

            SetLog(this.name + " ===OnTriggerExit2D==== " + collision.name + ", isDrag : " + mIsDragMode
        + "\n" + "TempColliderName:" + (mTempColliderObjList.Count) + " ; Collider.Name:" + (m_ColliderObj == null ? "null" : m_ColliderObj.name), LogType.Log);
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
