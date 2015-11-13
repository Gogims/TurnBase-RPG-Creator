using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;

/// <summary>
/// Clase encargada de acceder a los settings de los animation clip
/// </summary>
class AnimationClipSettings
{
    SerializedProperty m_Property;

    private SerializedProperty Get(string property) { return m_Property.FindPropertyRelative(property); }

    public AnimationClipSettings(SerializedProperty prop) { m_Property = prop; }

    public float startTime { get { return Get("m_StartTime").floatValue; } set { Get("m_StartTime").floatValue = value; } }
    public float stopTime { get { return Get("m_StopTime").floatValue; } set { Get("m_StopTime").floatValue = value; } }
    public float orientationOffsetY { get { return Get("m_OrientationOffsetY").floatValue; } set { Get("m_OrientationOffsetY").floatValue = value; } }
    public float level { get { return Get("m_Level").floatValue; } set { Get("m_Level").floatValue = value; } }
    public float cycleOffset { get { return Get("m_CycleOffset").floatValue; } set { Get("m_CycleOffset").floatValue = value; } }

    public bool loopTime { get { return Get("m_LoopTime").boolValue; } set { Get("m_LoopTime").boolValue = value; } }
    public bool loopBlend { get { return Get("m_LoopBlend").boolValue; } set { Get("m_LoopBlend").boolValue = value; } }
    public bool loopBlendOrientation { get { return Get("m_LoopBlendOrientation").boolValue; } set { Get("m_LoopBlendOrientation").boolValue = value; } }
    public bool loopBlendPositionY { get { return Get("m_LoopBlendPositionY").boolValue; } set { Get("m_LoopBlendPositionY").boolValue = value; } }
    public bool loopBlendPositionXZ { get { return Get("m_LoopBlendPositionXZ").boolValue; } set { Get("m_LoopBlendPositionXZ").boolValue = value; } }
    public bool keepOriginalOrientation { get { return Get("m_KeepOriginalOrientation").boolValue; } set { Get("m_KeepOriginalOrientation").boolValue = value; } }
    public bool keepOriginalPositionY { get { return Get("m_KeepOriginalPositionY").boolValue; } set { Get("m_KeepOriginalPositionY").boolValue = value; } }
    public bool keepOriginalPositionXZ { get { return Get("m_KeepOriginalPositionXZ").boolValue; } set { Get("m_KeepOriginalPositionXZ").boolValue = value; } }
    public bool heightFromFeet { get { return Get("m_HeightFromFeet").boolValue; } set { Get("m_HeightFromFeet").boolValue = value; } }
    public bool mirror { get { return Get("m_Mirror").boolValue; } set { Get("m_Mirror").boolValue = value; } }
}

/// <summary>
/// Clase encarga de crear los animation clip y animation controller
/// </summary>
public class ActorAnimation
{
    /// <summary>
    /// Animación cuando el personaje se encuentra parado mirando a la izquierda
    /// </summary>
    public AnimationClip leftIdle;
    /// <summary>
    /// Animación cuando el personaje se encuentra parado mirando hacia arriba
    /// </summary>
    public AnimationClip upIdle;
    /// <summary>
    /// Animación cuando el personaje se encuentra parado mirando a la derecha
    /// </summary>
    public AnimationClip rightIdle;
    /// <summary>
    /// Animación cuando el personaje se encuentra parado mirando hacia abajo
    /// </summary>
    public AnimationClip downIdle;

    /// <summary>
    /// Animación cuando el personaje se mueve para la izquierda
    /// </summary>
    public AnimationClip left;
    /// <summary>
    /// Animación cuando el personaje se mueve para arriba
    /// </summary>
    public AnimationClip up;
    /// <summary>
    /// Animación cuando el personaje se mueve para la derecha
    /// </summary>
    public AnimationClip right;
    /// <summary>
    /// Animación cuando el personaje se mueve para abajo
    /// </summary>
    public AnimationClip down;
    /// <summary>
    /// Tipo de animacion (enemigo o player)
    /// </summary>
    public string type;

    public ActorAnimation(string _type)
    {
        type = _type;
    }

    /// <summary>
    /// Se encarga de construir el clip de animación
    /// </summary>
    /// <param name="sprites">Lista de sprites para ser convertidos en animación</param>
    /// <param name="actorName">Nombre del actor</param>
    /// <param name="animationName">Nombre de la animación</param>
    /// <param name="fps">Cantidad de frames por segundo</param>
    /// <param name="loop">Si se desea que se repita indefinidamente la animación</param>
    /// <returns>Animación</returns>
    public AnimationClip ConstructAnimation(List<Sprite> sprites, int id, string animationName, int fps, bool loop)
    {
        AnimationClip animClip = new AnimationClip();
        EditorCurveBinding spriteBinding = new EditorCurveBinding();

        animClip.frameRate = fps;   // FPS
        animClip.name = animationName;
        spriteBinding.type = typeof(SpriteRenderer);
        spriteBinding.path = "";
        spriteBinding.propertyName = "m_Sprite";
        ObjectReferenceKeyframe[] spriteKeyFrames = new ObjectReferenceKeyframe[sprites.Count];

        for (int i = 0; i < (sprites.Count); i++)
        {
            spriteKeyFrames[i] = new ObjectReferenceKeyframe();
            spriteKeyFrames[i].time = 0.1f * i;
            spriteKeyFrames[i].value = sprites[i];
        }

        AnimationUtility.SetObjectReferenceCurve(animClip, spriteBinding, spriteKeyFrames);

        if (loop)
        {
            ApplyLoop(animClip);
        }

        // Guardando animation clip

        if (!AssetDatabase.IsValidFolder("Assets/Animation/" + type + "/" + id))
        {
            AssetDatabase.CreateFolder("Assets/Animation/" + type, id.ToString());            
        }

        AssetDatabase.CreateAsset(animClip, "Assets/Animation/" + type + "/" + id + "/" + animationName + ".anim");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        return animClip;
    }

    /// <summary>
    /// Se encarga de construir el clip de animación
    /// </summary>
    /// <param name="s">Sprite para convertir en animación</param>
    /// <param name="actorName">Nombre del actor</param>
    /// <param name="animationName">Nombre de la animación</param>
    /// <param name="fps">Cantidad de frames por segundo</param>
    /// <param name="loop">Si se desea que se repita indefinidamente la animación</param>
    /// <returns>Animación</returns>
    public AnimationClip ConstructAnimation(Sprite s, int id, string animationName, int fps, bool loop)
    {
        List<Sprite> sprites = new List<Sprite>();
        sprites.Add(s);

        return ConstructAnimation(sprites, id, animationName, fps, loop);
    }

    /// <summary>
    /// Construye el controlador de animación a partir de todos las animaciones obtenidas
    /// </summary>
    /// <param name="name">Nombre del controlador de animación</param>
    public AnimatorController ConstructAnimationControl(int id)
    {
        BlendTree idle = new BlendTree();
        string path = "Assets/AnimationController/" + type + "/" + id.ToString() + ".controller";

        // Creates the controller

        if (AssetDatabase.FindAssets(path).Length > 0)
        {
            AssetDatabase.DeleteAsset(path);
        }

        var controller = AnimatorController.CreateAnimatorControllerAtPath(path);

        // Add parameters
        controller.AddParameter("left", AnimatorControllerParameterType.Bool);
        controller.AddParameter("down", AnimatorControllerParameterType.Bool);
        controller.AddParameter("up", AnimatorControllerParameterType.Bool);
        controller.AddParameter("right", AnimatorControllerParameterType.Bool);
        controller.AddParameter("Idle", AnimatorControllerParameterType.Float);

        // Creating blend tree
        var root = controller.CreateBlendTreeInController("Idle", out idle);
        idle.blendParameter = "Idle";
        root.speed = downIdle.length;

        if (downIdle != null)
        {
            idle.AddChild(downIdle, 0);            
        }

        if (leftIdle != null)
        {
            idle.AddChild(leftIdle, 0.333f);
        }

        if (upIdle != null)
        {
            idle.AddChild(upIdle, 0.666f);
        }

        if (rightIdle != null)
        {
            idle.AddChild(rightIdle, 1);
        }

        //idle.useAutomaticThresholds = true;  // Problema con esta linea de codigo. Dice que esta propiedad no esta definida
        idle.blendType = BlendTreeType.Simple1D;        

        // Add StateMachines        
        var rootStateMachine = controller.layers[0].stateMachine;
        var stateLeft = rootStateMachine.AddStateMachine("walking left");
        var stateUp = rootStateMachine.AddStateMachine("walking up");
        var stateRight = rootStateMachine.AddStateMachine("walking right");
        var stateDown = rootStateMachine.AddStateMachine("walking down");

        // Add States
        var leftState = stateLeft.AddState("left");
        leftState.motion = left;
        leftState.AddExitTransition().AddCondition(AnimatorConditionMode.IfNot, 0, "left");

        var upState = stateUp.AddState("up");
        upState.motion = up;
        upState.AddExitTransition().AddCondition(AnimatorConditionMode.IfNot, 0, "up");

        var rightState = stateRight.AddState("right");
        rightState.motion = right;
        rightState.AddExitTransition().AddCondition(AnimatorConditionMode.IfNot, 0, "right");

        var downState = stateDown.AddState("down");
        downState.motion = down;
        downState.AddExitTransition().AddCondition(AnimatorConditionMode.IfNot, 0, "down");

        // Add exit transition
        rootStateMachine.AddStateMachineTransition(stateLeft, root).AddCondition(AnimatorConditionMode.IfNot, 0, "left");
        rootStateMachine.AddStateMachineTransition(stateUp, root).AddCondition(AnimatorConditionMode.IfNot, 0, "up");
        rootStateMachine.AddStateMachineTransition(stateRight, root).AddCondition(AnimatorConditionMode.IfNot, 0, "right");
        rootStateMachine.AddStateMachineTransition(stateDown, root).AddCondition(AnimatorConditionMode.IfNot, 0, "down");

        // Add entry transition
        root.AddTransition(leftState).AddCondition(AnimatorConditionMode.If, 0, "left");
        root.AddTransition(upState).AddCondition(AnimatorConditionMode.If, 0, "up");
        root.AddTransition(rightState).AddCondition(AnimatorConditionMode.If, 0, "right");
        root.AddTransition(downState).AddCondition(AnimatorConditionMode.If, 0, "down");

        return controller;
    }

    /// <summary>
    /// Le aplica un bucle a la animación
    /// </summary>
    /// <param name="Clip">Animación que se desea modificar</param>
    /// <returns>True si pudo ser modificado el loop</returns>
    private static bool ApplyLoop(AnimationClip Clip)
    {
        try
        {
            SerializedObject serializedClip = new SerializedObject(Clip);
            AnimationClipSettings clipSettings = new AnimationClipSettings(serializedClip.FindProperty("m_AnimationClipSettings"));
            clipSettings.loopTime = true;
            serializedClip.ApplyModifiedProperties();
        }
        catch
        {
            return false;
        }

        return true;
    }

}