using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawDropShadow : MonoBehaviour
{
    float dropShadowPosition;
    float shadowOffsetY;
    Vector3 parentBounds;
    SpriteRenderer spriteRender;
    LayerMask groundLayer;

    // Use this for initialization
    void Start()
    {
        groundLayer = LayerMask.NameToLayer("Ground");

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
        // change this to a raycast to closest solid layer

        // call from playerbehavior after jump if grounded and ypos changed much
        RaycastHit2D hit = Physics2D.Raycast(transform.parent.position, Vector2.down, 15, groundLayer);
        if (hit) { Debug.Log("Raycast hit below at distance of" + hit.distance); }
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
