using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveItemControl_matchB : MonoBehaviour {
    Vector3 m_OriginalPos;
    Vector3 m_DragStartPos;
    Vector3 m_DragEndPos;
    Vector3 m_offsetToMouse;
    MoveArea moveArea;

    GameObject m_ColliderObj;
    GameObject[] moveObjs;

    void Start()
    {
        m_OriginalPos = this.transform.position;
        moveArea = FindObjectOfType<MoveArea>();
        BoxCollider2D BCollider = this.gameObject.AddComponent<BoxCollider2D>();
        BCollider.isTrigger = true;
        Vector2 OriColliderSize = BCollider.size;
        BCollider.size = OriColliderSize * 0.9f;

        //抓場景所有的移動物件
        moveObjs = GameObject.FindGameObjectsWithTag("moveItem");
    }

    #region OnMouse
    private void OnMouseDown()
    {
        //關閉所有移動物件collider
        foreach (var obj in moveObjs)
        {
            if (obj == this.gameObject)
            {
                continue;
            }
            obj.GetComponent<BoxCollider2D>().enabled = false;
        }

        m_DragStartPos = transform.position;
        m_offsetToMouse = m_DragStartPos - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));

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

        float moveDis = Vector2.Distance(m_DragStartPos, m_DragEndPos);
        if (moveDis < 0.2f)
        {//移動不夠回到原位
            this.transform.position = m_OriginalPos;
        }
        else if (posX < 0 || posY < 0 || posY > Screen.height || posX > Screen.width)
        {

            this.transform.position = m_OriginalPos;
        }
        else if (m_ColliderObj != null && m_ColliderObj.tag == "matchPosItem")
        {//移動夠多，如果有碰到感應區，則成為子物件
            this.transform.SetParent(m_ColliderObj.transform);         
        }

        //開啟所有移動物件collider
        foreach (var obj in moveObjs)
        {
            if (obj == this.gameObject)
            {
                continue;
            }
            obj.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    #endregion

    #region OnTrigger
     private void OnTriggerStay2D(Collider2D collision)
    {
        //判斷tag     
        if (collision.tag == "matchPosItem" )
        {
            m_ColliderObj = collision.gameObject;
        }
        else
        {
            m_ColliderObj = moveArea.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "matchPosItem")
        {
            m_ColliderObj = null;
        }
    }
    #endregion
}
