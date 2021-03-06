﻿using System.IO;
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
			DelegateFactory.Init();

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

        public T ToDelegate<T>(string funcName) where T : class
		{
            LuaFunction luaFunc = lua.GetFunction(funcName);
			return luaFunc.ToDelegate<T>();
        }

		public LuaFunction GetLuaFunc(string tablename, string func)
		{
			return lua.GetFunction(tablename + "." + func);
		}

		public LuaFunction GetLuaFunc(string func)
		{
			return lua.GetFunction(func);
		}

		public LuaTable GetLuaTable(string filepath)
		{
			return lua.GetTable(filepath);
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