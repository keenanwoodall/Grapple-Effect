using UnityEngine;

[RequireComponent (typeof (LineRenderer))]
public class GrappleEffect : MonoBehaviour
{
	public float Magnitude = 1f;
	public float Frequency = 0.5f;
	public float Speed = 5f;
	public int Segments = 100;
	public AnimationCurve Curve = new AnimationCurve ();
	public AnimationCurve MagnitudeOverTime = new AnimationCurve ();
	public AnimationCurve MagnitudeOverDistance = new AnimationCurve ();

	private Vector3 grapplePoint;
	private float scrollOffset = 0f;
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
		scrollOffset = 0f;
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

		scrollOffset += Speed * Time.deltaTime;

		var difference = grapplePoint - transform.position;
		var direction = difference.normalized;
		var distance = difference.magnitude;

		for (int i = 0; i < lineRenderer.positionCount; i++)
		{
			var t = (float)i / lineRenderer.positionCount;
			var position = transform.position;
			var forwardOffset = direction * (t * distance);
			position += forwardOffset;

			var verticalOffset = transform.up * Curve.Evaluate (forwardOffset.magnitude * Frequency) * Magnitude;
			verticalOffset *= MagnitudeOverTime.Evaluate (scrollOffset);
			verticalOffset *= MagnitudeOverDistance.Evaluate (t);
			position += verticalOffset;

			var horizontalOffset = transform.right * Curve.Evaluate (forwardOffset.magnitude * Frequency + 0.25f) * Magnitude;
			horizontalOffset *= MagnitudeOverTime.Evaluate (scrollOffset);
			horizontalOffset *= MagnitudeOverDistance.Evaluate (t);
			position += horizontalOffset;

			lineRenderer.SetPosition (i, position);
		}
	}
}
