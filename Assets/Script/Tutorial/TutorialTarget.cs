using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTarget : DestroyableObstacle
{
    [SerializeField] private Animator hiddenPathAnimator;
    private bool isHit = false;
    private void OnEnable()
    {
        show = true;
        defaultMat = spriteRenderer.material;
        damagedAnimationTimer = 0f;
        flashWhiteTimer = 0f;
        currentHP = 50;
        isHit = false;
    }
    public override void TakeDamage(int damage)
    {
        damagedAnimationTimer = Time.time;
        if (isHit)
            return;
        hiddenPathAnimator.SetTrigger("Reveal");
    }
}
