using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace Console
    {
        using PlayerController;

        public class ConsolePlayerParser : ConsoleParser
        {
            public override void Parse(string[] command)
            {
                string commandSection = command[1];

                switch (commandSection)
                {
                    case "setrespawn":
                        SetRespawn();

                        break;
                    case "h":
                        Help(command.Length);
                        break;
                    default:
                        UnknownCommand();
                        break;
                }
            }

            public override void Help(int amArg)
            {
                if (amArg > 2)
                {
                    UnknownCommand();
                    return;
                }

                Debug.Log("Note: [p] indicates mandatory parameter, {p} indicates optional parameter.");
                Debug.Log("     setrespawn, sets player respawn to current location.");
            }

            private void SetRespawn()
            {
                IPlayerRespawn playerRespawn;
                Player player;

                playerRespawn = SceneObjectAssistant.Current.FindObjectWithType<PlayerRespawn>();
                player = SceneObjectAssistant.Current.FindObjectWithType<Player>();

                playerRespawn.PlayerRespawnLocation = player.transform.position;
            }
        }

    }
}

