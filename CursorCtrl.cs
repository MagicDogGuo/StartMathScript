using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorCtrl : MonoBehaviour {

    CursorCtrl Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            DestroyImmediate(gameObject);
    }

    public Texture2D moveItemCursor;

    public CursorMode cursorMode;

    bool IsChangedCursor = false;
 
    void Update()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
     
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 10, -1);
        if (!hit.collider)
        {
            if (IsChangedCursor)
            {
                Cursor.SetCursor(null, Vector2.zero, cursorMode);
                IsChangedCursor = false;
            }
        }else
        {
            var moveItemRig = hit.collider.GetComponent<Rigidbody2D>();
            if (moveItemRig != null)
            {
                if (!IsChangedCursor)
                {
                    Cursor.SetCursor(moveItemCursor, Vector2.zero, cursorMode);
                    IsChangedCursor = true;
                }
            }
        }
    }

    private void OnMouseEnter()
    {
        Debug.Log("Mouse Enter");
    }

    private void OnMouseExit()
    {
        Debug.Log("Mouse Exit");
    }




}
