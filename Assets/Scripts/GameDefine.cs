using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IMF
{
    public class GameDefine
    {
        public enum eMapType
        {
            None = 0,
            Road = 1,
            Station = 51,
            GoalPoint = 100,
            Player1Point = 101,
        }

        public const string MAP_CSV_NAME = "CSV/map";
    }
}