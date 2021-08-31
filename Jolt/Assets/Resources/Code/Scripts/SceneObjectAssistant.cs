using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    public class SceneObjectAssistant : MonoSingleton<SceneObjectAssistant>
    {
        public T FindObjectWithType<T>() where T : Object
        {
            return FindObjectOfType<T>();
        }
    }
}


