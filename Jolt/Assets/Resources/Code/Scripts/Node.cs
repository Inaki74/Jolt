using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    using Jolt.PlayerController;

    public class Node : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        // Start is called before the first frame update
        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag == "Player")
            {
                Player player = other.GetComponent<Player>();

                if(player.StateMachine.GetState() == "DashingState")
                {
                    _spriteRenderer.color = Color.cyan;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                Player player = collision.GetComponent<Player>();

                if (player.StateMachine.GetState() == "DashingState")
                {
                    _spriteRenderer.color = Color.grey;
                }
            }
        }
    }
}


