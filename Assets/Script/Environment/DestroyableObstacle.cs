using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObstacle : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Material whiteFlashMat;
    [SerializeField] private Collider2D col;
    private Material defaultMat;
    private float flashWhiteTimer = 0;
    private float damagedAnimationTimer = 0;
    private bool show;
    private int currentHP;

    private void OnEnable()
    {
        show = true;
        defaultMat = spriteRenderer.material;
        damagedAnimationTimer = 0f;
        flashWhiteTimer = 0f;
        currentHP = 100;
    }
    private void OnDisable()
    {
        AstarPath.active.UpdateGraphs(col.bounds);
    }
    private void Update()
    {
        if (Time.time - damagedAnimationTimer < 0.6f)
        {
            if (Time.time - flashWhiteTimer >= 0.2f)
            {
                if (show)
                {
                    spriteRenderer.material = defaultMat;
                }
                else
                {
                    spriteRenderer.material = whiteFlashMat;
                }
                show = !show;
                flashWhiteTimer = Time.time;
            }
        }
        else
        {
            spriteRenderer.material = defaultMat;
        }
    }

    public void TakeDamage(int damage)
    {
        damagedAnimationTimer = Time.time;
        currentHP -= damage;
        if(currentHP < 0)
        {
            Destroy(gameObject);
        }
    }
}
