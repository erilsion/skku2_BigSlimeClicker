using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WebGetStudentCSVTest : MonoBehaviour
{
    private void Start()
    {
        ParseCSV();
    }

    private void ParseCSV()
    {
        string path = Application.dataPath + "/WebAPITutorial/students.csv";
        List<Person> people = new List<Person>();
        // 1. 읽어온 CSV 파일을 파싱해서 (파싱 방법은 블로그 또는 llm 활용)
        StreamReader reader = new StreamReader(path);
        bool isFinished = false;

        // 2. Person 도메인 클래스에 넣고 people에 추가
        while (isFinished == false)
        {
            string data = reader.ReadLine();

            // 더 이상 읽을 데이터가 없으면 종료
            if (data == null)
            {
                isFinished = true;
                break;
            }

            string[] splitData = data.Split(',');
            Person person = new Person();

            person.id = int.Parse(splitData[0]);
            person.name = splitData[1];
            person.age = int.Parse(splitData[2]);
            people.Add(person);

            // 3. List<Person> persons 순회하면서 출력
            Debug.Log($"id: {person.id}, name: {person.name}, age: {person.age}");
        }
    }

    public class Person
    {
        public int id;
        public string name;
        public int age;
    }
}
