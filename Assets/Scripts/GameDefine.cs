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
            Plus = 2,
            Minus = 3,
            Station = 51,
            GoalPoint = 100,
            Player1Point = 101,
            Player2Point = 102,
        }

        public enum eTurnState
        {
            Wait,
            Dice,
            PlayerAct,
            Event,
        }

        public enum ePlayerState
        {
            Wait,
            Dice,
            DiceWait,
            Move,
            EventWait,
            Event,
        }


        public const string MAP_CSV_NAME = "CSV/map";
        public const int MAX_PLAYER_NUM = 2;
    }
}