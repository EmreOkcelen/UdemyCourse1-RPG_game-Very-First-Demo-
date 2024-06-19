using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    private Player player;
    void Start()
    {
        player = GetComponentInParent<Player>();
    }
    private void AnimationTriggers()
    {
        player.AttackOver(); //in animaiton event we need to attach this func. so we created another script for the animator(gameobject) to rotate the function
    }
}
