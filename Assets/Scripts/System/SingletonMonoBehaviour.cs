using UnityEngine;
using System.Collections;

namespace IMF{
	

	public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
	{
		protected static T _instance;
		public static T instance {
			get {
				if (_instance == null) {
					_instance = (T)FindObjectOfType (typeof(T));

					if (_instance == null) {
						Debug.LogWarning (typeof(T) + "is nothing");
					}
				}

				return _instance;
			}
		}

		protected void Awake()
		{
			CheckInstance();
		}

		protected bool CheckInstance()
		{
			if( _instance == null)
			{
				_instance = (T)this;
				return true;
			}else if( instance == this )
			{
				return true;
			}

			Destroy(this);
			return false;
		}
	}
}