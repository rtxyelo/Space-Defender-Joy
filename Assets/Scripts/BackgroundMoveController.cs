using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundMoveController : MonoBehaviour
{
    [SerializeField]
    private float backgroundMoveSpeed = 1f;

    private float backgroundPositionY = 0f;
    private float backgroundSizeY;

    void Start()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        backgroundSizeY = spriteRenderer.bounds.size.y;
    }

    private void Update()
    {
        backgroundPositionY += Time.deltaTime * backgroundMoveSpeed;
        backgroundPositionY = Mathf.Repeat(backgroundPositionY, backgroundSizeY);
        transform.position = new Vector3(0, -backgroundPositionY, 0);
    }
}
