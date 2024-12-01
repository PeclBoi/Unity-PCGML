using UnityEngine;

public class Snowball : MonoBehaviour
{

    public AnimationCurve scaleOverDistanceTravelled;
    public CustomRenderTexture renderTexture;
    public Rect worldBounds;

    public float force = 3.0f;
    public float rotationForce = 15.0f;
    public float snowRadiusMultiplier = 0.25f;

    public Material heightmapMaterial;
    private Rigidbody rb;

    private static readonly string PlayerPositionProperty = "_PlayerPosition";
    private static readonly string PlayerRadiusProperty = "_PlayerRadius";

    private void Awake()
    {
        this.rb = this.GetComponentInChildren<Rigidbody>();

        this.renderTexture.Initialize();
    }

    public Rigidbody GetRigidbody() => this.rb;


    private void Update()
    {
        this.UpdateTexture();
    }

    private void UpdateTexture()
    {
        var playerPos = this.rb.transform.position;
        var relativePos = new Vector2(playerPos.x, playerPos.z) - this.worldBounds.min;
        Vector2 percent = new Vector2(relativePos.x / this.worldBounds.width, relativePos.y  / this.worldBounds.height);

        this.heightmapMaterial.SetVector(PlayerPositionProperty, new Vector4(1 - percent.x, 1 - percent.y, 0, 0));

        this.renderTexture.Update();
    }

    public void FixedUpdate()
    {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 forward = this.rb.transform.forward;

            Vector3 forwardVector = forward * vertical;

            var rot = this.rb.transform.rotation;
            var newRot = rot * Quaternion.AngleAxis(horizontal * this.rotationForce * Time.deltaTime, Vector3.up);

            this.rb.AddForce(forwardVector * this.force);
            this.rb.MoveRotation(newRot);
    }

    private void OnDestroy()
    {
        this.heightmapMaterial.SetVector(PlayerPositionProperty, new Vector4(-0.5f, -0.5f, 0, 0));
        this.heightmapMaterial.SetFloat(PlayerRadiusProperty, 0.01f);
    }

}
