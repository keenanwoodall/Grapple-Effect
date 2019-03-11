using UnityEngine;

public class GrappleTester : MonoBehaviour
{
	[SerializeField] private GrappleEffect grappleEffect;

	private new Camera camera;

	private void Awake ()
	{
		camera = Camera.main;

		if (grappleEffect == null)
			grappleEffect = GetComponent<GrappleEffect> ();
	}

	private void Update ()
	{
		if (Input.GetMouseButtonDown (0))
		{
			RaycastHit hit;
			if (Physics.Raycast (camera.ScreenPointToRay (Input.mousePosition), out hit))
			{
				grappleEffect.transform.LookAt (hit.point);
				grappleEffect.Do ();
			}
		}
	}
}
