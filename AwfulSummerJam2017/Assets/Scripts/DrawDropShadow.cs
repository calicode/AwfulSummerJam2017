using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawDropShadow : MonoBehaviour
{
    float dropShadowPosition;
    float shadowOffsetY;
    Vector3 parentBounds;
    SpriteRenderer spriteRender;

    // Use this for initialization
    void Start()
    {


        spriteRender = GetComponent<SpriteRenderer>();
        UpdateShadowHeight();
    }

    public void UpdateShadowSize()
    {
        float scaleChange = (transform.parent.position.y + transform.position.y);
        // this needs to be modified  so the scaling is less drastic

        //Debug.Log(+scaleChange);
        transform.localScale = new Vector2(scaleChange, scaleChange);

    }

    public void UpdateShadowHeight()
    {
        // call from playerbehavior after jump if grounded and ypos changed much
        parentBounds = transform.parent.GetComponent<Renderer>().bounds.extents;
        shadowOffsetY = -(parentBounds.y * 2);

    }


    void Update()
    {
        UpdateShadowHeight();
        UpdateShadowSize();

        transform.position = new Vector2(transform.position.x, shadowOffsetY);
    }
}
