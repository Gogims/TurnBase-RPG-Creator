using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Tuple<T1, T2>
{
    /// <summary>
    /// Primer elemento utilizado en la estructura
    /// </summary>
    public T1 First;

    /// <summary>
    /// Segundo elemento utilizado en la estructura
    /// </summary>
    public T2 Second;

    public Tuple(T1 first, T2 second)
    {
        First = first;
        Second = second;
    }
}