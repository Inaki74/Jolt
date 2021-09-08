using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Jolt
{
    namespace Console
    {
        using LevelSections;
        using PlayerController;

        public class ConsoleSectionsParser : ConsoleParser
        {
            public override void Parse(string[] command)
            {
                string commandSection = command[1];

                switch (commandSection)
                {
                    case "tp":
                        if(command.Length == 3)
                        {
                            Teleport(command[2], "", command.Length);
                        }
                        else
                        {
                            Teleport(command[2], command[3], command.Length);
                        }

                        break;
                    case "h":
                        Help(command.Length);
                        break;
                    default:
                        UnknownCommand();
                        break;
                }
            }

            private void Teleport(string location, string gatewayName, int amArg)
            {
                if (amArg > 4)
                {
                    UnknownCommand();
                    return;
                }

                bool found = false;
                ISectionTransitionController gateway = null;
                IPlayerRespawn playerRespawn = null;
                Player thePlayer = null;
                try
                {
                    foreach (ISection section in SectionManager.Current.Sections)
                    {
                        if (string.IsNullOrEmpty(gatewayName))
                        {
                            if (section.SectionTransitioners.Any(g => g.ToID == location))
                            {
                                found = true;
                                gateway = section.SectionTransitioners.First(g => g.ToID == location);
                            }
                        }
                        else
                        {
                            if (section.SectionTransitioners.Any(g => g.ToID == location && g.GatewayName == gatewayName))
                            {
                                found = true;
                                gateway = section.SectionTransitioners.First(g => g.ToID == location && g.GatewayName == gatewayName);
                            }
                            
                        }

                        if (found)
                        {
                            break;
                        }
                    }

                    playerRespawn = SceneObjectAssistant.Current.FindObjectWithType<PlayerRespawn>();
                    thePlayer = SceneObjectAssistant.Current.FindObjectWithType<Player>();
                }
                catch(Exception e)
                {
                    Debug.LogError(e);
                    Debug.LogError("Did you put the right tp location? What about gateway?");
                    return;
                }

                SectionManager.Current.OnPlayerTransitionedSection(location);

                thePlayer.transform.position = gateway.RespawnTransform.position;
                playerRespawn.PlayerRespawnLocation = gateway.RespawnTransform.position;
            }

            public override void Help(int amArg)
            {
                if (amArg > 2)
                {
                    UnknownCommand();
                    return;
                }

                Debug.Log("Note: [p] indicates mandatory parameter, {p} indicates optional parameter.");
                Debug.Log("     tp [sectionName] {gatewayName}, teleports to a specific section to a certain gateway. No gateway teleports to default gateway.");
                Debug.Log("     e.g: tp T_04 g2, teleports to section T_04, gateway g2.");
            }
        }
    }
}
