using UnityEngine;
using System.Collections;

namespace IMF{
	
	public class MainController : SingletonMonoBehaviour<MainController> 
	{
		private CSVReader mapReader = null;
	
		[SerializeField]
		private GameObject _normalObj = null;

		[SerializeField]
		private GameObject _roadObject = null;

		[SerializeField]
		private GameObject _startPoint = null;

		[SerializeField]
		private Camera _mainCamera = null;

		[SerializeField]
		private Camera _subCamera = null;


		public void Init()
		{
			//マップを読み込み
			mapReader = new CSVReader ();
			mapReader.Load (GameDefine.MAP_CSV_NAME);

			//マップを作成する（非同期にしたいが夢のまた夢）

		}
			
	
	// Update is called once per frame
	void Update () {
	
	}
}
}
