using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        using PlayerStates;

        public class PlayerCollisions
        {
            public bool IsTouchingNode { get; private set; }
            public bool IsTouchingRail { get; private set; }
            public RailController FirstRail { get; private set; }
            public Collider2D NodeInfo { get; private set; }

            private Player _player;

            public PlayerCollisions(Player player)
            {
                this._player = player;
            }

            public void TriggerEnter(Collider2D collision, PlayerState state)
            {
                TriggerEnterNode(collision, state);

                TriggerEnterRail(collision, state);

                if (collision.tag == "Checkpoint")
                {
                    //_player.checkpoint = collision.transform.position;
                }
            }

            public void TriggerStay(Collider2D collision, PlayerState state)
            {
                if (collision.tag == "Node")
                {
                    IsTouchingNode = false;
                }

                if (collision.tag == "Rail Entrance" || collision.tag == "Rail Exit")
                {
                    IsTouchingRail = false;
                }
            }

            public void TriggerExit(Collider2D collision, PlayerState state)
            {
                if (collision.tag == "Node" && state.ToString() == "DashingState")
                {
                    IsTouchingNode = false;
                    collision.GetComponent<SpriteRenderer>().color = Color.grey;
                }

                if (collision.tag == "Rail Entrance" || collision.tag == "Rail Exit")
                {
                    collision.GetComponent<SpriteRenderer>().color = Color.grey;

                    if (state.ToString() == "DashingState")
                    {
                        IsTouchingRail = false;
                    }
                }
            }

            public void CollisionEnter(Collision2D collision, PlayerState state)
            {
                if (collision.gameObject.tag == "Rubber")
                {
                    _player.IsDead = true;
                }
            }

            public void CollisionExit(Collision2D collision, PlayerState state)
            {
                if (collision.gameObject.tag == "Rubber")
                {
                    _player.IsDead = false;
                }
            }

            private void TriggerEnterNode(Collider2D collision, PlayerState state)
            {
                if (collision.tag == "Node" && state.ToString() == "DashingState")
                {
                    IsTouchingNode = true;
                    NodeInfo = collision;
                    collision.GetComponent<SpriteRenderer>().color = Color.cyan;
                }
            }

            private void TriggerEnterRail(Collider2D collision, PlayerState state)
            {
                if ((collision.tag == "Rail Entrance" || collision.tag == "Rail Exit") && state.ToString() == "DashingState")
                {
                    IsTouchingRail = true;
                    FirstRail = collision.GetComponentInParent<RailController>();

                    _player.SetPosition(collision.transform.position);

                    if (collision.tag == "Rail Entrance" && FirstRail.Inverted)
                    {
                        FirstRail.InvertControlPoints();
                    }

                    if (collision.tag == "Rail Exit" && !FirstRail.Inverted)
                    {
                        FirstRail.InvertControlPoints();
                    }

                    collision.GetComponent<SpriteRenderer>().color = Color.cyan;
                }
            }
        }
    }
}