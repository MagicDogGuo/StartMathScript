using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 無限制互換- 容器內互換 matchD
/// </summary>
public class MatchPosItemControl_matchD : MonoBehaviour {
    //正確的配對物件名字
    [HideInInspector]
    public string CorrectColliderObjName;

    //正確後要生成的物件
    [HideInInspector]
    public GameObject CorrectObj;
    //生成物件的位置
    [HideInInspector]
    public Vector2 CorrectObjPos;

    [Tooltip("BoxCollider碰撞區域縮放比例")]
    /// <summary>
    /// BoxCollider縮放比例
    /// </summary>
    public Vector2 BoxColliderScale;


    private void Start()
    {
        //產生trigger
        BoxCollider2D BCollider = this.gameObject.AddComponent<BoxCollider2D>();
        BCollider.isTrigger = true;
        BCollider.size = new Vector2(BCollider.size.x * BoxColliderScale.x, BCollider.size.y * BoxColliderScale.y);
    }

    /// <summary>
    /// 碰撞到的物件名字，抓取子物件名稱
    /// </summary>
    /// <returns></returns>
    public List<GameObject> OnCollisionObjName()
    {
        List<GameObject> m_OnColliderObjName = new List<GameObject>();
        if (OnCollidionrObjCount() == 0)
        {
            m_OnColliderObjName.Add(null);
        }
        else
        {
            for (int i = 0; i < OnCollidionrObjCount(); i++)
            {
                m_OnColliderObjName.Add(this.transform.GetChild(i).gameObject);
            }
        }
        return m_OnColliderObjName;
    }

    /// <summary>
    /// 子物件數量
    /// </summary>
    /// <returns></returns>
    public int OnCollidionrObjCount()
    {
        int count = 0;
        count = this.transform.childCount;
        return count;
    }


    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "moveItem" )
    //    {
    //        if(ObjInItemCount == 0)
    //        {
    //            //OnColliderObjName.Clear();
    //            //OnColliderObjName.Add(collision.GetComponent<SpriteRenderer>().sprite.name);
    //        }
    //        //ObjInItemCount++;
    //    }
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    //if (collision.tag == "moveItem")
    //    //{
    //    //    if (ObjInItemCount == 1)
    //    //    {
    //    //        collision.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 1f);
    //    //    } 
    //    //}
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.tag == "moveItem")
    //    {
    //        if(ObjInItemCount == 1)
    //        {
    //            //OnColliderObjName.Clear();
    //        }
    //        //ObjInItemCount--;
    //    }
    //}
}
