using UnityEngine;
using System.Collections;

namespace IMF{
	
public class GameDefine
{
		public enum eMapType
		{
			Road = 1,
			Station = 2,
			StartPoint = 100,
			GoalPoint = 101,
		}

		public const string MAP_CSV_NAME = "CSV/map.csv";
}
}