using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Enemy : MonoBehaviour {
    // Movement Speed
    public float speed = 0.05f;

    // Current movement Direction
    Vector2 dir = Vector2.right;

    // Upwards push force
    public float upForce = 800;

	public int deathTimer = 60;
	public bool marioIsDead = false;

    void FixedUpdate() {
        // Set the Velocity
        GetComponent<Rigidbody2D>().velocity = dir * speed;

		if (marioIsDead == true) {
			deathTimer -= 1;
		}

		if (deathTimer <= 0) {
			Scene loadedLevel = SceneManager.GetActiveScene ();
			SceneManager.LoadScene (loadedLevel.buildIndex);
		}
    }

    //Move turtle back and forth
    void OnTriggerEnter2D(Collider2D coll) {

        // Hit a destination? Then move into other direction 
        transform.localScale = new Vector2(-1 * transform.localScale.x,transform.localScale.y);

        // And mirror it
        dir = new Vector2(-1 * dir.x, dir.y);
    }

    void OnCollisionEnter2D(Collision2D coll) {
        // Collided with Boy?
        if (coll.gameObject.name == "Boy") {
            // Is the collision above?
            if (coll.contacts[0].point.y > transform.position.y) {

				// Play Animation
                GetComponent<Animator>().SetTrigger("Died");

                // Disable collider so it falls downwards
                GetComponent<Collider2D>().enabled = false;

				GetComponent<Rigidbody2D> ().gravityScale = 10;
                
                // Push Boy upwards
                coll.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * upForce);
                
                // Die in a few seconds
                Invoke("Die", 5);
            } else {
				// Kill Boy
				marioIsDead = true;
                Destroy(coll.gameObject);

            }
        }
    }

    void Die() {
		Destroy(this.gameObject);
    }
}