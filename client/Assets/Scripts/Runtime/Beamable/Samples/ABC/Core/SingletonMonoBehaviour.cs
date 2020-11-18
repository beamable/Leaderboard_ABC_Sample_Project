using UnityEngine;

namespace Beamable.Samples.ABC.Core
{
	/// <summary>
	/// Base class for any <see cref="MonoBehaviour"/> which follows the popular 
	/// Singleton design patter.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class SingletonMonobehavior<T> : MonoBehaviour where T : MonoBehaviour
	{
		//  Properties ------------------------------------------
		public static bool IsInstantiated
		{
			get
			{
				return _Instance != null;
			}
		}

		public static T Instance
		{
			get
			{
				if (!IsInstantiated)
				{
					Instantiate();
				}
				return _Instance;
			}
			set
			{
				_Instance = value;
			}

		}

		//  Fields -------------------------------------------------
		private static T _Instance; 
		public delegate void OnInstantiateCompletedDelegate(T instance);
		public static OnInstantiateCompletedDelegate OnInstantiateCompleted;

		//  Instantiation ------------------------------------------

		public static T Instantiate()
		{
			
			if (!IsInstantiated)
			{
				_Instance = GameObject.FindObjectOfType<T>();
				if (_Instance == null)
            {
					GameObject go = new GameObject();
					_Instance = go.AddComponent<T>();
					go.name = _Instance.GetType().FullName;
					DontDestroyOnLoad(go);
				}
				if (OnInstantiateCompleted != null)
				{
					OnInstantiateCompleted(_Instance);
				}
			}
			return _Instance;
		}


		//  Unity Methods ------------------------------------------
		protected virtual void Awake()
		{
			Instantiate();
		}

		public static void Destroy()
		{

			if (IsInstantiated)
			{
				if (Application.isPlaying)
            {
					Destroy(_Instance.gameObject);
				}
				else
            {
					DestroyImmediate(_Instance.gameObject);
				}
				
				_Instance = null;
			}
		}

	}
}