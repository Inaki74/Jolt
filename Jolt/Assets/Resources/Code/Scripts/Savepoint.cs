using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    using PlayerController;

    public class Savepoint : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private bool _touched = false;

        public delegate void TouchedPlayer();
        public static event TouchedPlayer OnTouchedPlayer;

        // Start is called before the first frame update
        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Awake()
        {
            OnTouchedPlayer += OnTouchedPlayerAction;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player" && !_touched)
            {
                Player player = collision.GetComponent<Player>();

                IPlayerRespawn playerRespawn = player.GetComponent<PlayerRespawn>();

                playerRespawn.PlayerRespawnLocation = transform.position;

                _spriteRenderer.color = Color.yellow;

                OnTouchedPlayer.Invoke();

                _touched = true;
            }
        }

        private void OnTouchedPlayerAction()
        {
            if (_touched)
            {
                _touched = false;

                _spriteRenderer.color = Color.red;
            }
        }
    }
}


