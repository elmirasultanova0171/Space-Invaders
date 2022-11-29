using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    public projectile laserPrefab;
    public float speed=2.0f;
    public bool laserActive { get; private set; }

    private void Update()
    {
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.position += Vector3.left * this.speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position += Vector3.right * this.speed * Time.deltaTime;
        }
        
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
         
           
            projectile laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.destroyed += LaserDestroyed;
             laserActive = true;

    }

    private void LaserDestroyed()
    {
        laserActive = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("invader") ||
            other.gameObject.layer == LayerMask.NameToLayer("laser2"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
