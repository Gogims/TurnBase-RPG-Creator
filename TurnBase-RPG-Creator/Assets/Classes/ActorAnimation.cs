using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
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
    AnimationClip Clip;

    public ActorAnimation(string name, List<Sprite> sprites, int fps, bool raiseEvent = false)
    {
        int framecount = sprites.Count;
        float frameLength = 1f / 30f;

        Clip = new AnimationClip();
        Clip.frameRate = fps;

        AnimationUtility.GetAnimationClipSettings(Clip).loopTime = true;

        EditorCurveBinding curveBinding = new EditorCurveBinding();
        curveBinding.type = typeof(SpriteRenderer);
        curveBinding.propertyName = "m_Sprite";
        curveBinding.path = "test";

        ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[framecount];

        for (int i = 0; i < framecount; i++)
        {
            ObjectReferenceKeyframe kf = new ObjectReferenceKeyframe();
            kf.time = i * frameLength;
            kf.value = sprites[i];
            keyFrames[i] = kf;
        }

        AnimationCurve curve = AnimationUtility.GetEditorCurve(Clip, curveBinding);
        Clip.name = name;
        //AnimationUtility.SetEditorCurve(Clip, curveBinding, curve);
        Clip.SetCurve("TestAnimation", typeof(SpriteRenderer), "Sprite", curve);

        Clip.wrapMode = WrapMode.Once;
        //setAnimationLoop(clip);
        //AnimationUtility.SetObjectReferenceCurve(Clip, curveBinding, keyFrames);


        if (raiseEvent)
        {
            //AnimationUtility.SetAnimationEvents(clip, new[] { new AnimationEvent() { time = clip.length, functionName = "on" + name } });
        }
        //clip.AddEvent(e);
    }

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