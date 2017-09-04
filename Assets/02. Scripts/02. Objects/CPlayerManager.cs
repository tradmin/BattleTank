using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerManager : MonoBehaviour {
    /// <summary>
    /// Current turret rotation and shooting direction.
    /// </summary>
    [HideInInspector]
    public short turretRotation;

    /// <summary>
    /// Delay between shots.
    /// </summary>
    public float fireRate = 0.75f;

    /// <summary>
    /// Turret to rotate with look direction.
    /// </summary>
    public Transform m_tfTurret;

    /// <summary>
    /// 포격 발사 이펙터 위치
    /// </summary>
    public Transform m_tfFirePoz;

    /// <summary>
    /// 포격 발사 이펙터
    /// </summary>
    public GameObject m_goFireEffect;

    /// <summary>
    /// 포탄
    /// </summary>
    public GameObject m_goBullet;

    /// <summary>
    /// Array of available bullets for shooting.
    /// </summary>
    public GameObject[] m_goArrBullets;

    /// <summary>
    /// Reference to the camera following component.
    /// </summary>
    [HideInInspector]
    public CTargetFollow m_cTargetFollow;

    //timestamp when next shot should happen
    private float nextFire;

    //reference to this rigidbody
#pragma warning disable 0649
    //    public Rigidbody m_rdRigidbody;
    private Rigidbody m_rdRigidbody;
#pragma warning restore 0649

    // Use this for initialization
    void Start () {
        //get components and set camera target
        m_rdRigidbody = GetComponent<Rigidbody>();
        m_cTargetFollow = Camera.main.GetComponent<CTargetFollow>();
        m_cTargetFollow.target = m_tfTurret;

#if !UNITY_STANDALONE && !UNITY_WEBGL
        CGameManager.Instance.m_cUIInGameManager.m_ctlJoystick[0].onDrag += Move;
        CGameManager.Instance.m_cUIInGameManager.m_ctlJoystick[0].onDragEnd += MoveEnd;

        CGameManager.Instance.m_cUIInGameManager.m_ctlJoystick[1].onDragBegin += ShootBegin;
        CGameManager.Instance.m_cUIInGameManager.m_ctlJoystick[1].onDrag += RotateTurret;
        CGameManager.Instance.m_cUIInGameManager.m_ctlJoystick[1].onDrag += Shoot;
#endif
    }

    // Update is called once per frame
    void Update () {
		
	}

    //continously check for input on desktop platforms
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
    void FixedUpdate()
    {
        //movement variables
        Vector2 moveDir;
        Vector2 turnDir;

        //reset moving input when no arrow keys are pressed down
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            moveDir.x = 0;
            moveDir.y = 0;
        }
        else
        {
            //read out moving directions and calculate force
            moveDir.x = Input.GetAxis("Horizontal");
            moveDir.y = Input.GetAxis("Vertical");
            Move(moveDir);
        }

        //cast a ray on a plane at the mouse position for detecting where to shoot 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.up);
        float distance = 0f;
        Vector3 hitPos = Vector3.zero;
        //the hit position determines the mouse position in the scene
        if (plane.Raycast(ray, out distance))
        {
            hitPos = ray.GetPoint(distance) - transform.position;
        }

        //we've converted the mouse position to a direction
        turnDir = new Vector2(hitPos.x, hitPos.z);

        //rotate turret to look at the mouse direction
        RotateTurret(new Vector2(hitPos.x, hitPos.z));

        //shoot bullet on left mouse click
        if (Input.GetButton("Fire1"))
            Shoot();

        //replicate input to mobile controls for illustration purposes
#if UNITY_EDITOR
        CGameManager.Instance.m_cUIInGameManager.m_ctlJoystick[0].position = moveDir;
        CGameManager.Instance.m_cUIInGameManager.m_ctlJoystick[1].position = turnDir;
#endif
    }
#endif

    //moves rigidbody in the direction passed in
    void Move(Vector2 direction = default(Vector2))
    {
        //if direction is not zero, rotate player in the moving direction relative to camera
        if (direction != Vector2.zero)
            transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y))
                                 * Quaternion.Euler(0, m_cTargetFollow.camTransform.eulerAngles.y, 0);

        //create movement vector based on current rotation and speed
        //Vector3 movementDir = transform.forward * moveSpeed * Time.deltaTime;
        Vector3 movementDir = transform.forward * 2.5f * Time.deltaTime;

        //apply vector to rigidbody position
        m_rdRigidbody.MovePosition(m_rdRigidbody.position + movementDir);
    }


    //on movement drag ended
    void MoveEnd()
    {
        //reset rigidbody physics values
        m_rdRigidbody.velocity = Vector3.zero;
        m_rdRigidbody.angularVelocity = Vector3.zero;
    }


    //rotates turret to the direction passed in
    void RotateTurret(Vector2 direction = default(Vector2))
    {
        //don't rotate without values
        if (direction == Vector2.zero)
            return;

        //get rotation value as angle out of the direction we received
        turretRotation = (short)Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y)).eulerAngles.y;
        OnTurretRotation();
    }

    //on shot drag start set small delay for first shot
    void ShootBegin()
    {
        nextFire = Time.time + 0.25f;
    }


    //shoots a bullet in the direction passed in
    //we do not rely on the current turret rotation here, because we send the direction
    //along with the shot request to the server to absolutely ensure a synced shot position
    protected void Shoot(Vector2 direction = default(Vector2))
    {
        //if shot delay is over  
        if (Time.time > nextFire)
        {
            //set next shot timestamp
            nextFire = Time.time + fireRate;

            // make fire effect.
            Instantiate(m_goFireEffect, m_tfFirePoz.position, m_tfFirePoz.rotation);

            // make ball
//            Instantiate(m_goBullet, m_tfFirePoz.position, m_tfFirePoz.rotation);

            //send current client position and turret rotation along to sync the shot position
            //also we are sending it as a short array (only x,z - skip y) to save additional bandwidth
            short[] pos = new short[] { (short)(m_tfFirePoz.position.x * 10), (short)(m_tfFirePoz.position.z * 10) };
            //send shot request with origin to server
            //            this.photonView.RPC("CmdShoot", PhotonTargets.AllViaServer, pos, turretRotation);
            //calculate center between shot position sent and current server position (factor 0.6f = 40% client, 60% server)
            //this is done to compensate network lag and smoothing it out between both client/server positions
            Vector3 shotCenter = Vector3.Lerp(m_tfFirePoz.position, new Vector3(pos[0] / 10f, m_tfFirePoz.position.y, pos[1] / 10f), 0.6f);
            Quaternion syncedRot = m_tfTurret.rotation = Quaternion.Euler(0, turretRotation, 0);

            //spawn bullet using pooling
            GameObject obj = CPoolManager.Spawn(m_goArrBullets[0], shotCenter, syncedRot);
            obj.GetComponent<CBullet>().owner = gameObject;
        }
    }

    //hook for updating turret rotation locally
    void OnTurretRotation()
    {
        //we don't need to check for local ownership when setting the turretRotation,
        //because OnPhotonSerializeView PhotonStream.isWriting == true only applies to the owner
        m_tfTurret.rotation = Quaternion.Euler(0, turretRotation, 0);
    }
}
