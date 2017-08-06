using UnityEngine;

namespace LuaFramework {
	public partial class LuaBehaviour : MonoBehaviour{

        void Awake() {
			DoInject(false);
			var awakeFunc = LuaManager.Instance.GetLuaFunc(name, "Awake");
			if (awakeFunc != null)
			{
				awakeFunc.Call(gameObject);
			}
        }

		void OnEnable()
		{
			if (hasEnableFunc)
			{
				var enableFunc = LuaManager.Instance.GetLuaFunc(name, "OnEnable");
				if (enableFunc!= null)
				{
					enableFunc.Call();
				}
			}
		}

        void Start() {
			if (hasStartFunc)
			{
				var startFunc = LuaManager.Instance.GetLuaFunc(name, "Start");
				if (startFunc != null)
				{
					startFunc.Call();
				}
			}
        }

        void OnDestroy() {
			var destroyFunc = LuaManager.Instance.GetLuaFunc(name, "OnDestroy");
			if (destroyFunc != null)
			{
				destroyFunc.Call();
			}
			DoInject(true);
			injections = null;
			LuaManager.Instance.LuaGC();
        }

		private void DoInject(bool isRelease)
		{
			var luatable = LuaManager.Instance.GetLuaTable(name);
			foreach (var injection in injections)
			{
				if (string.IsNullOrEmpty(injection.name) || injection.gameObject == null)
				{
					Log.Error("[LuaBehaviour:%s] injections contains null");
				}
				else
				{
					luatable[injection.name] = isRelease ? null : injection.gameObject;
				}
			}
		}
    }
}