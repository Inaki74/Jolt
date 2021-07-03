using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions
{
    public bool IsTouchingNode { get; private set; }
    public bool IsTouchingRail { get; private set; }
    public RailController FirstRail { get; private set; }
    public Collider2D NodeInfo { get; private set; }

    private Player player;
    
    public PlayerCollisions(Player player)
    {
        this.player = player;
    }

    public void TriggerEnter(Collider2D collision, PlayerState state)
    {
        if (collision.tag == "Node" && state.ToString() == "DashingState")
        {
            IsTouchingNode = true;
            NodeInfo = collision;
            collision.GetComponent<SpriteRenderer>().color = Color.cyan;
        }

        if ((collision.tag == "Rail Entrance" || collision.tag == "Rail Exit") && state.ToString() == "DashingState")
        {
            IsTouchingRail = true;
            FirstRail = collision.GetComponentInParent<RailController>();

            SetPosition(collision.transform.position);

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

        if (collision.tag == "Checkpoint")
        {
            player.checkpoint = collision.transform.position;
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

            if (StateMachine.GetState() == "DashingState")
            {
                IsTouchingRail = false;
            }
        }
    }

    public void CollisionEnter(Collision2D collision, PlayerState state)
    {
        if (collision.gameObject.tag == "Rubber")
        {
            isDead = true;
        }
    }

    public void CollisionExit(Collision2D collision, PlayerState state)
    {
        if (collision.gameObject.tag == "Rubber")
        {
            isDead = false;
        }
    }

    public void SetPosition(Vector2 pos)
    {
        player.transform.position = pos;
    }
}
