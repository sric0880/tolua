using UnityEngine;

namespace LuaFramework
{
	public class LuaLooperBehaviour : MonoBehaviour
	{
		private LuaLooper loop = null;

		void Start()
		{
			LuaCoroutine.Register(LuaManager.Instance.lua, this);
			loop = gameObject.AddComponent<LuaLooper>();
			loop.luaState = LuaManager.Instance.lua;
		}

		void OnDestroy()
		{
			loop.Destroy();
			loop = null;
		}
	}
}
