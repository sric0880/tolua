using UnityEngine;
using System.IO;

public static class LuaConst
{
    public static string luaDir = Application.dataPath + "/tolua/Lua";                //lua逻辑代码目录
    public static string toluaDir = Application.dataPath + "/tolua/ToLua/Lua";        //tolua lua文件目录

	public static string luaExternalDir { get { return Path.Combine(FileUtils.externalFolder, "Lua"); } } //手机运行时lua文件下载目录
	public static string luaInternalDir { get { return Path.Combine(FileUtils.internalFolder, "Lua"); } } //手机运行时lua文件下载目录

    public static bool openLuaSocket = false;            //是否打开Lua Socket库
}