using System;
using System.Collections.Generic;
using UnityEngine;

// 캐릭터 세부 스탯과 관련된 클래스이다.
[Serializable]
public class MapleCharacterStat
{
    public string date;
    public string character_class;
    public List<MapleFinalStat> final_stat;
    public int remain_ap;
}

// 최종 스탯은 배열 형태로 되어 있는 정보라 별도 클래스로 분리했다.
[Serializable]
public class MapleFinalStat
{
    public string stat_name;
    public string stat_value;
}
