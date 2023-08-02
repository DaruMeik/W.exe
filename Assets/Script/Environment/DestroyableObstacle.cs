using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObstacle : MonoBehaviour
{
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public Material whiteFlashMat;
    [SerializeField] public Collider2D col;
    public Material defaultMat;
    public float flashWhiteTimer = 0;
    public float damagedAnimationTimer = 0;
    public bool show;
    public int currentHP;

    private void OnEnable()
    {
        show = true;
        defaultMat = spriteRenderer.material;
        damagedAnimationTimer = 0f;
        flashWhiteTimer = 0f;
        currentHP = 50;
    }
    private void OnDisable()
    {
        if(AstarPath.active != null)
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
    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
    public virtual void TakeDamage(int damage)
    {
        damagedAnimationTimer = Time.time;
        currentHP -= damage;
        if(currentHP < 0)
        {
            SelfDestruct();
        }
    }
}
