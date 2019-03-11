using UnityEngine;

[RequireComponent (typeof (LineRenderer))]
public class GrappleEffect : MonoBehaviour
{
	[Header ("Animation")]
	public AnimationCurve effectOverTime = new AnimationCurve ();
	[Header ("Curve")]
	public AnimationCurve curve = new AnimationCurve ();
	public float curveAmount = 1f;
	public float curveSize = 3f;
	public AnimationCurve curveEffectCurve = new AnimationCurve ();
	public float scrollSpeed = 5f;
	public int segments = 100;

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
		if (lineRenderer.positionCount != segments)
			lineRenderer.positionCount = segments;
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

		scrollOffset += scrollSpeed * Time.deltaTime;

		var difference = grapplePoint - transform.position;
		var direction = difference.normalized;
		var distance = difference.magnitude;

		for (int i = 0; i < lineRenderer.positionCount; i++)
		{
			var t = (float)i / lineRenderer.positionCount;
			var position = transform.position;
			var forwardOffset = direction * (t * distance);
			position += forwardOffset;

			var verticalOffset = transform.up * curve.Evaluate (forwardOffset.magnitude / curveSize) * curveAmount;
			verticalOffset *= effectOverTime.Evaluate (scrollOffset);
			verticalOffset *= curveEffectCurve.Evaluate (t);
			position += verticalOffset;

			var horizontalOffset = transform.right * curve.Evaluate (forwardOffset.magnitude / curveSize + 0.25f) * curveAmount;
			horizontalOffset *= effectOverTime.Evaluate (scrollOffset);
			horizontalOffset *= curveEffectCurve.Evaluate (t);
			position += horizontalOffset;

			lineRenderer.SetPosition (i, position);
		}
	}
}
