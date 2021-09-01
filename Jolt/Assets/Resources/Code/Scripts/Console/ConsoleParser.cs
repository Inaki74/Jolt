using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace Console
    {
        public abstract class ConsoleParser
        {
            public abstract void Parse(string[] command);

            public virtual void UnknownCommand()
            {
                Debug.LogError("Console Error: Unidentified command.");
            }
        }
    }

}


