using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBullet : MonoBehaviour {
    /// <summary>
    /// Projectile travel speed in units.
    /// </summary>
    public float speed = 10;

    /// <summary>
    /// Damage to cause on a player that gets hit.
    /// </summary>
    public int damage = 3;

    /// <summary>
    /// Delay until despawned automatically when nothing gets hit.
    /// </summary>
    public float despawnDelay = 1f;

    /// <summary>
    /// Bounce count of walls and other environment obstactles.
    /// </summary>
    public int bounce = 0;

    /// <summary>
    /// Clip to play when a player gets hit.
    /// </summary>
    public AudioClip hitClip;

    /// <summary>
    /// Clip to play when this projectile gets despawned.
    /// </summary>
    public AudioClip explosionClip;

    /// <summary>
    /// Object to spawn when a player gets hit.
    /// </summary>
    public GameObject hitFX;

    /// <summary>
    /// Object to spawn when this projectile gets despawned.
    /// </summary>
    public GameObject explosionFX;

    //reference to rigidbody component
    private Rigidbody myRigidbody;
    //reference to collider component
    private SphereCollider sphereCol;
    //caching maximum count of bounces for restore
    private int maxBounce;

    /// <summary>
    /// Player gameobject that spawned this projectile.
    /// </summary>
    [HideInInspector]
    public GameObject owner;


    //get component references
    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        sphereCol = GetComponent<SphereCollider>();
        maxBounce = bounce;
    }


    //set initial travelling velocity
    //On Host, add automatic despawn coroutine
    void OnSpawn()
    {
        myRigidbody.velocity = speed * transform.forward;
        CPoolManager.Despawn(gameObject, despawnDelay);
    }


    //check what was hit on collisions
    void OnTriggerEnter(Collider col)
    {
        //cache corresponding gameobject that was hit
        GameObject obj = col.gameObject;
        //try to get a player component out of the collided gameobject
        CPlayerManager player = obj.GetComponent<CPlayerManager>();

        //we actually hit a player
        //do further checks
        if (player != null)
        {
            /*
            //ignore ourselves & disable friendly fire (same team index)
            if (player.gameObject == owner) return;
            else if (player.GetView().GetTeam() == owner.GetComponent<Player>().GetView().GetTeam()) return;
            */

            //create clips and particles on hit
            if (hitFX) CPoolManager.Spawn(hitFX, transform.position, Quaternion.identity);
            // if (hitClip) AudioManager.Play3D(hitClip, transform.position);

            //on the player that was hit, set the killing player to the owner of this bullet
            //maybe this owner really killed the player, but that check is done in the Player script
            // player.killedBy = owner;
        }
        else if (bounce > 0)
        {
/*
            //a player was not hit but something else, and we still have some bounces left
            //create a ray that points in the direction this bullet is currently flying to
            Ray ray = new Ray(transform.position - transform.forward, transform.forward);
            RaycastHit hit;

            //perform spherecast in the flying direction, on the default layer
            if (Physics.SphereCast(ray, sphereCol.radius, out hit, speed, 1 << 0))
            {
                //something was hit in the direction this projectile is flying to
                //get new reflected (bounced off) direction of the colliding object
                Vector3 dir = Vector3.Reflect(ray.direction, hit.normal);
                //rotate bullet to face the new direction
                transform.rotation = Quaternion.LookRotation(dir);
                //reassign velocity with the new direction in mind
                OnSpawn();

                //play clip at the collided position
                if (hitClip) AudioManager.Play3D(hitClip, transform.position);
                //substract bouncing count by one
                bounce--;
                //exit execution until next collision
                return;
            }
*/
        }

        //despawn gameobject
 //       PoolManager.Despawn(gameObject);

        //the previous code is not synced to clients at all, because all that clients need is the
        //initial position and direction of the bullet to calculate the exact same behavior on their end.
        //at this point, continue with the critical game aspects only on the server
        if (!PhotonNetwork.isMasterClient) return;
        //apply bullet damage to the collided player
 //       if (player) player.TakeDamage(this);
    }


    //set despawn effects and reset variables
    void OnDespawn()
    {
        //create clips and particles on despawn
        if (explosionFX) CPoolManager.Spawn(explosionFX, transform.position, transform.rotation);
//        if (explosionClip) AudioManager.Play3D(explosionClip, transform.position);

        //reset modified variables to the initial state
        myRigidbody.velocity = Vector3.zero;
        myRigidbody.angularVelocity = Vector3.zero;
        bounce = maxBounce;
    }
}
