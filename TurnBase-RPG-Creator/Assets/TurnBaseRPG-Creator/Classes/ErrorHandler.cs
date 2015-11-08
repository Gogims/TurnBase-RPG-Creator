using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

public class ErrorHandler  {
    /// <summary>
    /// Estilo del mensage
    /// </summary>
    private GUIStyle style;
    /// <summary>
    /// Diccionario con las propiedades que se van a validar.
    /// </summary>
    private Dictionary<string, Field> properties;
    /// <summary>
    /// Constructor.
    /// </summary>
    public ErrorHandler()
    {   
        style = new GUIStyle();
        properties = new Dictionary<string, Field>();
        style.normal.textColor = Color.red;
    }
    /// <summary>
    // Muestra los mensajes usando GUILayout.
    /// </summary>
    public void ShowErrorsLayout()
    {
        foreach (string i in properties.Keys)
        {
            if (properties[i].Error)
                GUILayout.Label("* " + properties[i].Message, style);
        }
    }
    /// <summary>
    /// Muestra los mensajes utilizando la ubicacion de cada mensaje.
    /// </summary>
    public void ShowErrors() 
    {
        foreach (string i in properties.Keys) { 
            if (properties[i].Error)
                EditorGUI.LabelField((Rect)properties[i].Position, new GUIContent(properties[i].Message));
        }
    }
    public void UpdateValue(string property, int val) {
        properties[property].Value = val;
    }
    /// <summary>
    /// Inserta una propiedad nueva con su mensaje de error y 
    /// su posicion en dado caso que se requiera especificar.
    /// </summary>
    /// <param name="property">Nombre de la propiedad</param>
    /// <param name="errorMessage">Mensaje que se mostrara</param>
    /// <param name="positionError">Posicion donde se va mostrar el mensaje(opcional)</param>
    public void InsertPropertyError(string property, int val,  string errorMessage, Nullable<Rect> positionError = null) 
    {
        properties.Add(property, new Field(errorMessage, positionError,val));
    }
    /// <summary>
    /// Inserta una condicion a una propiedad.
    /// </summary>
    /// <param name="property">Propiedad</param>
    /// <param name="value">Valor de la condicion</param>
    /// <param name="Condition">Condicion que se va evaluar(mayor, menor , igual,etc.)</param>
    /// <param name="logic">Condicion logica de esta propiedad (AND,OR,NONE) </param>
    public void InsertCondition(string property, Nullable<int> value , ErrorCondition Condition, LogicalCondition logic)
    {
        properties[property].Validations.Add(Condition,new Values(value,logic));
    }
    /// <summary>
    /// Verifica si se cumplen todas las condiciones.
    /// </summary>
    /// <returns>retorna false si todas las condiciones se cumplen de lo contrario retorna true</returns>
    public bool CheckErrors() {
        bool Err=false;
        foreach (string i in properties.Keys)
        {
           
            bool result = false; 
            foreach(ErrorCondition j in properties[i].Validations.Keys){
                bool innerResult = false;
                //Debug.Log(i + ":" + properties[i].Value + " validacion:" + properties[i].Validations[j].Value);
                if ( j == ErrorCondition.Equal) {
                    if ( properties[i].Validations[j].Value == properties[i].Value )
                        innerResult = true;
                    else 
                        innerResult = false;
                }
                else if (j == ErrorCondition.Different)
                {
                    if (properties[i].Validations[j].Value != properties[i].Value)
                        innerResult = true;
                    else
                        innerResult = false;
                }
                else if (j == ErrorCondition.Greater)
                {
                    if (properties[i].Value > properties[i].Validations[j].Value )
                    {
                       // Debug.Log(i + " Cumple Mayor");
                          innerResult = true;
                    }
                    else
                        innerResult = false;
                }
                else if (j == ErrorCondition.GreaterOrEqual)
                {
                    if (properties[i].Value >= properties[i].Validations[j].Value)
                    {
                       // Debug.Log(i + " Cumple Mayor o igual ");
                        innerResult = true;
                    }
                    else
                        innerResult = false;
                }
                else if (j == ErrorCondition.Less)
                {
                    if (properties[i].Value < properties[i].Validations[j].Value)
                    {
                       // Debug.Log(i+" Cumple Menor");
                        innerResult = true;
                    }
                    else
                        innerResult = false;
                }
                else if (j == ErrorCondition.LessOrEqual)
                {
                    if (properties[i].Value <= properties[i].Validations[j].Value)
                    {
                     //   Debug.Log(i + " Cumple menor o igual ");
                        innerResult = true;
                    }
                    else
                        innerResult = false;
                }
                if (properties[i].Validations[j].logic == LogicalCondition.None) {
                    result = innerResult;
                    break;
                }
                else if (properties[i].Validations[j].logic == LogicalCondition.OR && innerResult)
                {
                    result = innerResult;
                    break;
                }
                else if (properties[i].Validations[j].logic == LogicalCondition.AND && !innerResult)
                {
                    result = innerResult;
                    break;
                }
            }

            if (!result)
            {
                if (!Err)
                    Err = true;
                properties[i].Error = true;
            }
            else
            {
                properties[i].Error = false;
            }

        }
        return Err;
    }
    /// <summary>
    /// Clase que almacena el valor de la propiedad
    /// </summary>
    private class Values {
        public LogicalCondition logic { get; set; }
        public Nullable<int> Value { get; set; }
        public Values(Nullable<int> v, LogicalCondition l)
        {
            logic = l;
            Value = v;
        }
    }
    /// <summary>
    /// Clase que representa una propiedad.
    /// </summary>
    private class Field
    {
        public string Message { get; set; }
        public Nullable<Rect> Position { get; set; }
        public Dictionary<ErrorCondition,Values> Validations { get; set; }
        public int Value { get; set; }
        public bool Error { get; set; }
        public Field(string msg, Nullable<Rect> pos,int val)
        {
            Message = msg;
            Position = pos;
            Value = val;
            Error = false;
            Validations = new Dictionary<ErrorCondition, Values>();
        }

    }
}
