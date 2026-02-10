using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class WebGetWeatherTest : MonoBehaviour
{
    // 목표: 수업 장소의 날씨를 받아오자!
    private const string API_KEY = "06cfdbac13608fc6a1e2085c13c11172";

    private async void Start()
    {
        float lat = 37.4049955f;
        float lon = 127.1060049f;
        string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={API_KEY}";

        Debug.Log(url);

        string jsonString = await GetWebText(url);
        var data = JsonUtility.FromJson<WeatherResponse>(jsonString);
        Debug.Log(data.name);
        Debug.Log(data.main.temp);
        Debug.Log(data.weather[0].description);
    }

    private async UniTask<string> GetWebText(string url)
    {
        var txt = (await UnityWebRequest.Get(url).SendWebRequest()).downloadHandler.text;
        return txt;
    }
}

[Serializable]
public class WeatherResponse
{
    public Coord coord;
    public WeatherInfo[] weather;
    public string @base;          // "base"는 C# 예약어 성격이라 @ 붙임
    public MainInfo main;
    public int visibility;
    public WindInfo wind;
    public CloudsInfo clouds;
    public long dt;
    public SysInfo sys;
    public int timezone;
    public int id;
    public string name;
    public int cod;
}

[Serializable]
public class Coord
{
    public float lon;
    public float lat;
}

[Serializable]
public class WeatherInfo
{
    public int id;
    public string main;
    public string description;
    public string icon;
}

[Serializable]
public class MainInfo
{
    public float temp;
    public float feels_like;
    public float temp_min;
    public float temp_max;
    public int pressure;
    public int humidity;
    public int sea_level;
    public int grnd_level;
}

[Serializable]
public class WindInfo
{
    public float speed;
    public int deg;
}

[Serializable]
public class CloudsInfo
{
    public int all;
}

[Serializable]
public class SysInfo
{
    public int type;
    public int id;
    public string country;
    public long sunrise;
    public long sunset;
}
