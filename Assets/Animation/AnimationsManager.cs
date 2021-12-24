using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsManager
{
    Animator playerAnimator;


    public AnimationsManager(Animator playerAnimator)
    {
        this.playerAnimator = playerAnimator;
    }

    public void PlayerGo()
    {
        playerAnimator.SetBool("isWalking", true);
    }

    public void PlayerStop()
    {
        playerAnimator.SetBool("isWalking", false);
    }
}
