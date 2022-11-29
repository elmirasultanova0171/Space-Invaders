using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class invaders : MonoBehaviour
{
    public invader[] prefabs;
    public int rows = 5;
    public int columns = 11;
    public AnimationCurve speed;
    public float laser2AttackRate = 1.0f;

    public int amountKilled { get; private set; }
    public int totalInvaders => this.rows * this.columns;
    public float precentKilled => (float)this.amountKilled / (float)this.totalInvaders;

    private Vector3 _direction = Vector2.right;

    public projectile laser2Prefab;

    private void Awake()
    {
        for (int row = 0; row < this.rows; row++)
        {
            float width = 1.0f * (this.columns - 1);
            float height = 1.0f * (this.rows - 1);
            Vector3 centering = new Vector2(-width / 2, -height / 2);
            Vector3 rowPosition = new Vector3(centering.x, centering.y + (row + 1.0f), 0.0f);
            for (int col = 0; col < this.columns; col++)
            {

                invader invader = Instantiate(this.prefabs[row], this.transform);
                invader.killed += InvaderKilled;
                Vector3 position = rowPosition;
                position.x += col * 1.0f;
                invader.transform.localPosition = position;
            }
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(Laser2Attack), this.laser2AttackRate, this.laser2AttackRate);
    }


    private void Update()
    {
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        this.transform.position += _direction * this.speed.Evaluate(this.precentKilled) * Time.deltaTime;

        foreach (Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (_direction == Vector3.right && invader.position.x >= (rightEdge.x - 0.5f))
            {
                AdvanceRow();
            }
            else if (_direction == Vector3.left && invader.position.x <= (leftEdge.x + 0.5f))
            {
                AdvanceRow();
            }
        }
    }

    private void AdvanceRow()
    {
        _direction.x *= -1.0f;
        Vector3 position = this.transform.position;
        position.y -= 0.5f;
        this.transform.position = position;
    }

    private void Laser2Attack()
    {
         int amountAlive = totalInvaders;

         if (amountAlive == 0)
         {
             return;
         }

        foreach (Transform invader in this.transform)
        {

            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (Random.value < (1f / (float)this.totalInvaders))
            {
                Instantiate(this.laser2Prefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }
    private void InvaderKilled()
    {

        this.amountKilled++;

        if(this.amountKilled>= this.totalInvaders)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);  //delete when u put scoring
        }
    }



}


