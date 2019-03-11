using UnityEngine;

[RequireComponent (typeof (LineRenderer))]
public class GrappleEffect : MonoBehaviour
{
	[Header ("Wave")]
	public Vector2 Magnitude = Vector2.one;
	public float Frequency = 0.5f;
	public float Speed = 5f;
	[Header ("Noise")]
	public float Strength = 0.5f;
	public float Scale = 0.25f;

	[Space]
	public int Segments = 100;

	[Header ("Curves")]
	public AnimationCurve Curve = new AnimationCurve ();
	public AnimationCurve MagnitudeOverTime = new AnimationCurve ();
	public AnimationCurve MagnitudeOverDistance = new AnimationCurve ();

	private Vector3 grapplePoint;
	private float timeOffset = 0f;
	private LineRenderer lineRenderer;

	private void Awake ()
	{
		lineRenderer = GetComponent<LineRenderer> ();
	}

	public void Do ()
	{
		RaycastHit hit;
		if (lineRenderer.enabled = Physics.Raycast (transform.position, transform.forward, out hit))
			Do (hit.point);
	}

	public void Do (Vector3 grapplePoint)
	{
		this.grapplePoint = grapplePoint;
		timeOffset = 0f;
		if (lineRenderer.positionCount != Segments)
			lineRenderer.positionCount = Segments;
	}

	// I like this naming convention haha
	public void Dont ()
	{
		lineRenderer.enabled = false;
	}

	public void SetGrapplePoint (Vector3 grapplePoint)
	{
		this.grapplePoint = grapplePoint;
	}

	private void Update ()
	{
		if (!lineRenderer.enabled)
			return;

		timeOffset += Speed * Time.deltaTime;

		var difference = grapplePoint - transform.position;
		var direction = difference.normalized;
		var distance = difference.magnitude;

		for (int i = 0; i < lineRenderer.positionCount; i++)
		{
			var t = (float)i / lineRenderer.positionCount;
			var position = transform.position;
			var forwardOffset = direction * (t * distance);
			position += forwardOffset;

			var verticalOffset = transform.up * Curve.Evaluate (forwardOffset.magnitude * Frequency);
			verticalOffset *= Magnitude.y;
			verticalOffset += transform.up * (Mathf.PerlinNoise (0f, -t * Scale + timeOffset) - 0.5f) * 2f * Strength;
			verticalOffset *= MagnitudeOverTime.Evaluate (timeOffset);
			verticalOffset *= MagnitudeOverDistance.Evaluate (t);
			position += verticalOffset;

			var horizontalOffset = transform.right * Curve.Evaluate (forwardOffset.magnitude * Frequency + 0.25f);
			horizontalOffset *= Magnitude.x;
			horizontalOffset += transform.right * (Mathf.PerlinNoise (-t * Scale + timeOffset, 0f) - 0.5f) * 2f * Strength;
			horizontalOffset *= MagnitudeOverTime.Evaluate (timeOffset);
			horizontalOffset *= MagnitudeOverDistance.Evaluate (t);
			position += horizontalOffset;

			lineRenderer.SetPosition (i, position);
		}
	}
}
