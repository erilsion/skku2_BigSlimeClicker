using UnityEngine;
using System;

public class Dog
{
    public string Name;
    public int Age;

    public Dog(string name, int age)
    {
        if(string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("이름은 비어있을 수 없습니다.");
        }
        if (age <= 0)
        {
            throw new ArgumentException("나이는 0보다 작을 수 없습니다.");
        }
    }
}
