using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _current;
        public static T Current
        {
            get
            {
                if (_current == null)
                {
                    Debug.Log("No " + typeof(T) + " Instantiated");
                }

                return _current;
            }
        }

        private void Awake()
        {
            Debug.Log(typeof(T) + " Instantiated");
            _current = (T)this;
            Init();
        }

        public virtual void Init()
        {

        }
    }
}

