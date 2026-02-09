using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IGameSaveRepository
{
    UniTask Save(GameSaveData data);
    UniTask<GameSaveData> Load();
}
