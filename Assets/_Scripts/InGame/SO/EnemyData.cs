using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.InGame
{
    public enum EnemyType
    {
        Basic,
        Fast,
        Power,
        Armor
    }

    [Serializable]
    public class EnemyInMap
    {
        public EnemyType[] enemies = new EnemyType[20];
    }
    [CreateAssetMenu(menuName = "SO/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        public List<EnemyInMap> EnemyInMaps=new List<EnemyInMap>();

        public EnemyType[] GetEnemies(int level)
        {
            return EnemyInMaps[level - 1].enemies;
        }
    }
}