using System.IO;
using LuaInterface;

namespace LuaFramework {
    public class LuaManager : MgrSingleton<LuaManager>{
		public LuaState lua { get; private set; }
		public LuaResLoader luaResLoader;

		public override void OnInit()
		{
			luaResLoader = new LuaResLoader();
			lua = new LuaState(luaResLoader.ReadFile);
			//this.OpenLibs();
			lua.LuaSetTop(0);

			LuaBinder.Bind(lua);

			luaResLoader.LoadluaChunks();

			this.lua.Start();    //启动LUAVM
			this.StartMain();
		}

        //cjson 比较特殊，只new了一个table，没有注册库，这里注册一下
        protected void OpenCJson() {
            lua.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
            lua.OpenLibs(LuaDLL.luaopen_cjson);
            lua.LuaSetField(-2, "cjson");

            lua.OpenLibs(LuaDLL.luaopen_cjson_safe);
            lua.LuaSetField(-2, "cjson.safe");
        }

        void StartMain() {
            lua.DoFile("Main");

            LuaFunction main = lua.GetFunction("Main");
            main.Call();
            main.Dispose();
            main = null;
		}
        
        /// <summary>
        /// 初始化加载第三方库
        /// </summary>
        void OpenLibs() {
            lua.OpenLibs(LuaDLL.luaopen_pb);
			lua.OpenLibs(LuaDLL.luaopen_lpeg);
            lua.OpenLibs(LuaDLL.luaopen_bit);
            lua.OpenLibs(LuaDLL.luaopen_socket_core);

            this.OpenCJson();
        }

        public object[] DoFile(string filename) {
            return lua.DoFile(filename);
        }

        // Update is called once per frame
        public object[] CallFunction(string funcName, params object[] args) {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) {
                return func.Call(args);
            }
            return null;
        }

		public object[] CallModuleFunction(string module, string func, params object[] args)
		{
			return CallFunction(module + "." + func, args);
		}

        public void LuaGC() {
            lua.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
        }

        public void Dispose() {
            lua.Dispose();
            lua = null;
        }
    }
}