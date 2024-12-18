﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jolt
{
    namespace PlayerController
    {
        using PlayerStates;

        [RequireComponent(typeof(Player))]
        [RequireComponent(typeof(Rigidbody2D))]
        public class PlayerCollisions : MonoBehaviour
        {
            public bool IsTouchingNode { get; private set; }
            public bool IsTouchingRail { get; private set; }
            public RailController FirstRail { get; private set; }
            public Collider2D NodeInfo { get; private set; } = null;

            private Player _player;

            private void Awake()
            {
                _player = GetComponent<Player>();
            }

            private void OnTriggerEnter2D(Collider2D collision)
            {
                TriggerEnter(collision, _player.StateMachine.CurrentState);
            }

            private void OnTriggerStay2D(Collider2D collision)
            {
                TriggerStay(collision, _player.StateMachine.CurrentState);
            }

            private void OnTriggerExit2D(Collider2D collision)
            {
                TriggerExit(collision, _player.StateMachine.CurrentState);
            }

            private void OnCollisionEnter2D(Collision2D collision)
            {
                if (collision.gameObject.tag == "Rubber")
                {
                    _player.IsDead = true;
                }
            }

            private void OnCollisionExit2D(Collision2D collision)
            {
                if (collision.gameObject.tag == "Rubber")
                {
                    _player.IsDead = false;
                }
            }

            private void TriggerEnter(Collider2D collision, PlayerState state)
            {
                TriggerEnterNode(collision, state);

                TriggerEnterRail(collision, state);

                if (collision.tag == "Checkpoint")
                {
                    //_player.checkpoint = collision.transform.position;
                }

                if (collision.gameObject.tag == "Rubber")
                {
                   // _player.IsDead = true;
                }
            }

            private void TriggerStay(Collider2D collision, PlayerState state)
            {
                if (collision.tag == "Node")
                {
                    //IsTouchingNode = false;
                }

                if (collision.tag == "Rail Entrance" || collision.tag == "Rail Exit")
                {
                    IsTouchingRail = false;
                }
            }

            private void TriggerExit(Collider2D collision, PlayerState state)
            {
                if (collision.tag == "Node")
                {
                    IsTouchingNode = false;
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

            private void TriggerEnterNode(Collider2D collision, PlayerState state)
            {
                if (collision.tag == "Node")
                {
                    IsTouchingNode = true;
                    NodeInfo = collision;
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