using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveItemControl_matchA : MonoBehaviour {

 

    Vector3 m_OriginalPos;
    Vector3 m_DragStartPos;
    Vector3 m_DragEndPos;
    Vector3 m_offsetToMouse;
    MoveArea moveArea;

    GameObject m_ColliderObj;

    public float CorrectPosX;
    public float CorrectPosY;
   public bool m_IsCloseBackToOriPos;

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
        {//移動不夠回到原位
            if(!m_IsCloseBackToOriPos) this.transform.position = m_OriginalPos;
        }
        else if (posX < 0 || posY < 0 || posY > Screen.height  || posX > Screen.width)
        {
            
            this.transform.position = m_OriginalPos;
        }
        else if (m_ColliderObj != null && m_ColliderObj.tag == "matchPosItem")
        {//移動夠多，如果有碰到感應區，則移動到感應區位置
            this.transform.SetParent(m_ColliderObj.transform);
            transform.position = new Vector3(this.m_ColliderObj.transform.position.x+ CorrectPosX, this.m_ColliderObj.transform.position.y + CorrectPosY,-2);
        }
    }

    #endregion

    #region OnTrigger

    float m_DistnaceTemp = 0;
    bool m_isHaveMatcPosItem=false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        float nowDistance = 0;
        //判斷數量跟tag     
        if (collision.tag == "matchPosItem")
        {
            nowDistance = Vector3.Distance(this.transform.position, collision.transform.position);

            if (collision.GetComponent<MatchPosItemControl_matchA>().OnCollidionrObjCount() == 0)
            {
                if(m_DistnaceTemp >= nowDistance)
                m_ColliderObj = collision.gameObject;
            }
            m_DistnaceTemp = nowDistance;
            m_isHaveMatcPosItem = true;
        }
        else
        {
            if(!m_isHaveMatcPosItem)
            m_ColliderObj = moveArea.gameObject;
        }

        //Debug.Log(collision.name+ "====nowDistance" + nowDistance+ "=== m_DistnaceTemp: "+ m_DistnaceTemp);
        //Debug.Log("========="+m_ColliderObj );

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "matchPosItem")
        {
            m_isHaveMatcPosItem = false;
            m_ColliderObj = null;
        }
    }
    #endregion
}
