using System.Collections;
using System.Collections.Generic;

namespace Jolt
{
    namespace Console
    {
        public class ConsoleSectionsParser : ConsoleParser
        {
            public override void Parse(string[] command)
            {
                string commandSection = command[1];

                switch (commandSection)
                {
                    case "tp":
                        Teleport(command[2]);
                        break;
                    default:
                        UnknownCommand();
                        break;
                }
            }

            private void Teleport(string location)
            {

            }
        }
    }
}
