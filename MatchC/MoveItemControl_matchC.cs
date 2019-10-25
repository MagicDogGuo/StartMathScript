using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveItemControl_matchC : MonoBehaviour {
    [HideInInspector]
    public bool CanLeaveClone = false;

    Vector3 m_DragStartPos;
    Vector3 m_DragEndPos;
    Vector3 m_offsetToMouse;
    MoveArea moveArea;

    GameObject m_ColliderObj;

    public float CorrectPosX;
    public float CorrectPosY;

    void Start()
    {
        moveArea = FindObjectOfType<MoveArea>();
        if (GetComponent<BoxCollider2D>() == null)
        {
            BoxCollider2D BCollider = this.gameObject.AddComponent<BoxCollider2D>();
            BCollider.isTrigger = true;
            Vector2 OriColliderSize = BCollider.size;
            BCollider.size = OriColliderSize * 0.9f;
        }
        
    }
    

    #region OnMouse
    private void OnMouseDown()
    {
        if (this.CanLeaveClone == true)
        {
            //點擊生成物件
            GameObject go = Instantiate(this.gameObject);

            go.GetComponent<MoveItemControl_matchC>().CanLeaveClone = true;

            this.CanLeaveClone = false;
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

        float moveDis = Vector3.Distance(m_DragStartPos, m_DragEndPos);
        if (moveDis < 0.2f)
        {//移動不夠刪除物件
            Destroy(this.gameObject);
        }
        else if (posX < 0 || posY < 0 || posY > Screen.height || posX > Screen.width)
        {

            Destroy(this.gameObject);
        }
        else if (m_ColliderObj != null && m_ColliderObj.tag == "matchPosItem")
        {//移動夠多，如果有碰到感應區，則移動到感應區位置
            this.transform.SetParent(m_ColliderObj.transform);
            transform.position = new Vector3(this.m_ColliderObj.transform.position.x + CorrectPosX, this.m_ColliderObj.transform.position.y + CorrectPosY, -2);
        }
    }

    #endregion

    #region OnTrigger
    private void OnTriggerStay2D(Collider2D collision)
    {
        //判斷數量跟tag     
        if (collision.tag == "matchPosItem")
        {
            if (collision.GetComponent<MatchPosItemControl_matchC>().OnCollidionrObjCount() == 0)
            {
                m_ColliderObj = collision.gameObject;
            }
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
