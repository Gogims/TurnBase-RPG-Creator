using UnityEditor;
using System.Reflection;

public static class ObjectSelectorWrapper
{
    private static System.Type Tipo;
    private static bool oldState = false;
    static ObjectSelectorWrapper()
    {
        Tipo = System.Type.GetType("UnityEditor.ObjectSelector,UnityEditor");
    }

    private static EditorWindow Get()
    {
        PropertyInfo P = Tipo.GetProperty("get", BindingFlags.Public | BindingFlags.Static);
        return P.GetValue(null, null) as EditorWindow;
    }
    public static void ShowSelector(System.Type aRequiredType)
    {
        MethodInfo ShowMethod = Tipo.GetMethod("Show", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        ShowMethod.Invoke(Get(), new object[] { null, aRequiredType, null, true });
    }
    public static T GetSelectedObject<T>() where T : UnityEngine.Object
    {
        MethodInfo GetCurrentObjectMethod = Tipo.GetMethod("GetCurrentObject", BindingFlags.Static | BindingFlags.Public);
        return GetCurrentObjectMethod.Invoke(null, null) as T;
    }
    public static bool isVisible
    {
        get
        {
            PropertyInfo P = Tipo.GetProperty("isVisible", BindingFlags.Public | BindingFlags.Static);
            return (bool)P.GetValue(null, null);
        }
    }
    public static bool HasJustBeenClosed()
    {
        bool visible = isVisible;
        if (visible != oldState && visible == false)
        {
            oldState = false;
            return true;
        }
        oldState = visible;
        return false;
    }
}