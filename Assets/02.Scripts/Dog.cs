using UnityEngine;
using System;
using Firebase.Firestore;

[Serializable]
[FirestoreData]
public class Dog
{
    [FirestoreDocumentId]           // 문서의 고유 식별자가 자동으로 맵핑된다.
    public string Id { get; set; }  // FirestoreDocumentId 어트리뷰트를 사용하여 문서 ID를 맵핑할 수 있다.

    [FirestoreProperty]
    public string Name { get; set; }  // 필드가 아니라 get/set이 있는 프로퍼티여야 한다.

    [FirestoreProperty]
    public int Age { get; set; }

    public Dog() { }  // Firestore가 객체를 생성할 때 기본 생성자가 무조건 있어야 한다.

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
        Name = name;
        Age = age;
    }
}
