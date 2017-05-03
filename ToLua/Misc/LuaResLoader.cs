/*
Copyright (c) 2015-2016 topameng(topameng@qq.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class LuaResLoader {

	private readonly Dictionary<string, byte[]> luaChunks = new Dictionary<string, byte[]>();
	private readonly static List<string> luaDirName = new List<string> { "lua/main/{0}.lua", "lua/tolua/{0}.lua" };

	private static byte[] EditorReadLuaFile(string fileName)
	{
		string path = null;
		foreach (var dir in luaDirName)
		{
			path = Path.GetFullPath(string.Format(dir, fileName));
			if (File.Exists(path))
			{
				return File.ReadAllBytes(path);
			}
		}
		return null;
	}

    public byte[] ReadFile(string fileName)
    {
		if (Application.isEditor)
		{
			return EditorReadLuaFile(fileName);
		}
		else
		{
			return luaChunks.ContainsKey(fileName) ? luaChunks[fileName] : null;
		}
    }

	//加载bytecode
	public void LoadluaChunks()
	{
		if (!Application.isEditor)
		{
			string path = Path.Combine(FileUtils.binary_config_folder, "cb");
			using (BinaryReader br = new BinaryReader(FileUtils.OpenRead(path)))
			{
				while (br.BaseStream.Position != br.BaseStream.Length) //Detect eof of the stream
				{
					var name = br.ReadString();
					var contentLength = br.ReadInt32();
					var chunk = br.ReadBytes(contentLength);
					luaChunks[name] = chunk;
				}
			}
		}
	}
}
