using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPosItemControl_matchASum : MonoBehaviour {

    public string ID;
    private void Start()
    {
        //產生trigger
        BoxCollider2D BCollider = this.gameObject.AddComponent<BoxCollider2D>();
        BCollider.isTrigger = true;

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

   
    public int GetChildPoint()
    {
        var objs = GetComponentsInChildren<MoveItemControl_matchASum>();
        int totalPoint = 0;
        for (int i = 0; i < objs.Length; i++)
        {
            totalPoint += objs[i].score;
        }
        return totalPoint;
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
