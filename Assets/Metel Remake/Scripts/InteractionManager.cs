using UnityEngine;

public class InteractionManager : MonoBehaviour
{
	[SerializeField] Camera _camera;
	[SerializeField] LayerMask targedMask;

	private void Update()
	{
		GameObject finded = Finded();

		if (finded != null)
		{
			if (Input.GetMouseButtonDown(0))
			{
				switch(finded.tag)
                {
					case "door": finded.GetComponentInParent<Door>().Interact(); break;
					case "light swither": finded.GetComponentInParent<LightSwither>().Interact(); break;
				}
			}
		}
	}

	GameObject Finded()
	{
		GameObject finded = null;

		if (Physics.Raycast(_camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out RaycastHit hit, 1.2f, targedMask))
		{
			if (hit.collider != null)
			{
				finded = hit.collider.gameObject;
			}
		}

		return finded;
	}
}
