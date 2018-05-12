using UnityEngine;
using System.Collections;

public class Mashroom_SC : Platform_SC
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public override void OnPlayerJumpOnPlatform()
    {
        anim.Play("Active");
    }
}
