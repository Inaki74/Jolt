using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Jolt
{
    namespace Console
    {
        using PlayerController;

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
                if (!_shown && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D))
                {
                    _shown = true;
                    _console.gameObject.SetActive(true);
                    FindObjectOfType<PlayerInputManager>().Disabled = true;

                    _console.ActivateInputField();
                    _console.Select();
                }
            }

            private void DisableConsole()
            {
                _console.gameObject.SetActive(false);
                _shown = false;
                FindObjectOfType<PlayerInputManager>().Disabled = false;
            }

            public void ParseInput()
            {
                // "/lss command section gateway"
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


