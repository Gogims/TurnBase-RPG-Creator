using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

public class ActorAnimation : MonoBehaviour
{
    /*
        Character sprites positions:

        0 => Walking down 1
        1 => Standing down
        2 => Walking down 2
        3 => Walking left 1
        4 => Standing left
        5 => Walking left 2
        6 => Walking right 1
        7 => Standing right
        8 => Walking right 2
        9 => Walking up 1
        10 => Standing up
        11 => Walking up 2

     */

    private string currentAnimation = "StandDown";
    private int frame = 0;
    private int animationFrames = 20; // PAR NUMBER

    private Sprite[] sprites = new Sprite[12];
    private SpriteRenderer rend;

    Animator animator = new Animator();

    public void Awake()
    {
        rend = (SpriteRenderer)this.GetComponent<SpriteRenderer>();
    }

    public void Start()
    {

    }

    public void Update()
    {
        if (frame == animationFrames) frame = 0;
        frame++;

        callAnimation();
    }

    private void callAnimation()
    {
        if (currentAnimation == "WalkUp") WalkUp();
        else if (currentAnimation == "WalkDown") WalkDown();
        else if (currentAnimation == "WalkLeft") WalkLeft();
        else if (currentAnimation == "WalkRight") WalkRight();

        else if (currentAnimation == "StandUp") StandUp();
        else if (currentAnimation == "StandDown") StandDown();
        else if (currentAnimation == "StandLeft") StandLeft();
        else if (currentAnimation == "StandRight") StandRight();
    }

    private void changeOnFrame(int f1, int f2)
    {
        if (frame == 1) rend.sprite = sprites[f1];
        else if (frame == animationFrames / 2) rend.sprite = sprites[f2];
        else if (frame == animationFrames) rend.sprite = sprites[f1];
    }

    private void WalkUp()
    {
        changeOnFrame(9, 11);
    }

    private void WalkDown()
    {
        changeOnFrame(0, 2);
    }

    private void WalkLeft()
    {
        changeOnFrame(3, 5);
    }

    private void WalkRight()
    {
        changeOnFrame(6, 8);
    }

    private void StandUp()
    {
        if (frame == 1) rend.sprite = sprites[10];
    }

    private void StandDown()
    {
        if (frame == 1) rend.sprite = sprites[1];
    }

    private void StandLeft()
    {
        if (frame == 1) rend.sprite = sprites[4];
    }

    private void StandRight()
    {
        if (frame == 1) rend.sprite = sprites[7];
    }

    public void setSprites(Sprite[] sprites)
    {
        this.sprites = sprites;
    }

    public void changeAnimation(string animation)
    {
        frame = 0;
        currentAnimation = animation;
    }
}