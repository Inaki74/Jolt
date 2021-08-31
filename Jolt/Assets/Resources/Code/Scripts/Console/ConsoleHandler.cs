using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jolt
{
    namespace Console
    {
        public class ConsoleHandler : MonoBehaviour
        {
            [SerializeField] private InputField _console;

            private bool _shown;

            private void Start()
            {
                DisableConsole();
            }

            private void Update()
            {
                if (!_shown && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.V))
                {
                    _shown = true;
                    _console.enabled = true;
                }
            }

            private void DisableConsole()
            {
                _console.enabled = false;
                _shown = false;
            }

            public void ParseInput()
            {
                // "/lss command section"
                string input = _console.text;

                if (string.IsNullOrEmpty(input))
                {
                    DisableConsole();
                    return;
                }

                _console.textComponent.text = "";

                string[] separated = input.Split(' ');

                ConsoleParser parser;
                switch (separated[0])
                {
                    case "lss":
                        parser = new ConsoleSectionsParser();
                        break;
                    default:
                        Debug.LogError("Console Error: Unidentified command.");
                        DisableConsole();
                        return;
                }

                parser.Parse(separated);
                DisableConsole();
            }
        }
    }
}


