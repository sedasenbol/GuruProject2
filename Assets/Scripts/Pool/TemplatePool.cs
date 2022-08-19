using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace Pool
{
    public class TemplatePool : Pool
    {
        private static TemplatePool instance;
        public static TemplatePool Instance
        {
            get
            {
                if (instance != null) return instance;
            
                instance = FindObjectOfType<TemplatePool>();

                if (instance != null) return instance;
            
                GameObject newGo = new GameObject();
                instance = newGo.AddComponent<TemplatePool>();
                return instance;
            }
        }

        protected void Awake()
        {
            instance = this as TemplatePool;
        }
    }
}