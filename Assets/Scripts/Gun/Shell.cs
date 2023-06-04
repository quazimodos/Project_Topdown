using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    [SerializeField] private Rigidbody myRigidbody;
    [SerializeField] private float forceMin;
    [SerializeField] private float forceMax;

	private float lifetime = .5f;
	private float fadetime = 2;

	void Start()
	{
		float force = Random.Range(forceMin, forceMax);
		myRigidbody.AddForce(transform.right * force);
		myRigidbody.AddTorque(Random.insideUnitSphere * force);

		StartCoroutine(Fade());
	}

	IEnumerator Fade()
	{
		yield return new WaitForSeconds(lifetime);

		float percent = 0;
		float fadeSpeed = 1 / fadetime;
		Material mat = GetComponent<Renderer>().material;
		Color initialColour = mat.color;

		while (percent < 1)
		{
			percent += Time.deltaTime * fadeSpeed;
			mat.color = Color.Lerp(initialColour, Color.clear, percent);
			yield return null;
		}

		Destroy(gameObject);
	}
}
