﻿using UnityEngine;
using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace LuaFramework {
	public class LuaBehaviour : MonoBehaviour{
		public bool hasStartFunc;
		public bool hasEnableFunc;
        private Dictionary<string, LuaFunction> buttons = new Dictionary<string, LuaFunction>();

        void Awake() {
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

        public void AddClick(GameObject go, LuaFunction luafunc) {
            if (go == null || luafunc == null) return;
            buttons.Add(go.name, luafunc);
            go.GetComponent<Button>().onClick.AddListener(
                delegate() {
                    luafunc.Call(go);
                }
            );
        }

        /// <summary>
        /// 删除单击事件
        /// </summary>
        /// <param name="go"></param>
        public void RemoveClick(GameObject go) {
            if (go == null) return;
            LuaFunction luafunc = null;
            if (buttons.TryGetValue(go.name, out luafunc)) {
                luafunc.Dispose();
                luafunc = null;
                buttons.Remove(go.name);
            }
        }

        /// <summary>
        /// 清除单击事件
        /// </summary>
        public void ClearClick() {
            foreach (var de in buttons) {
                if (de.Value != null) {
                    de.Value.Dispose();
                }
            }
            buttons.Clear();
        }

        //-----------------------------------------------------------------
        protected void OnDestroy() {
            ClearClick();
			LuaManager.Instance.LuaGC();
        }
    }
}