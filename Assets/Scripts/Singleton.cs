using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class Singleton : MonoBehaviour
{
	static Dictionary<Type, MonoBehaviour> singletons = new Dictionary<Type, MonoBehaviour>();

	public static T Get<T>() where T : MonoBehaviour
	{
		return singletons.ContainsKey(typeof(T)) ? singletons[typeof(T)] as T : null;
	}

	public static ICollection<MonoBehaviour> GetAll()
	{
		return singletons.Values;
	}

	protected virtual void Awake()
	{
		if (singletons.ContainsKey(GetType()))
		{
			Debug.LogError("There is already a singleton for " + GetType());
			return;
		}
		singletons[GetType()] = this;
	}

	protected virtual void OnDestroy()
	{
		singletons.Remove(GetType());
	}
}