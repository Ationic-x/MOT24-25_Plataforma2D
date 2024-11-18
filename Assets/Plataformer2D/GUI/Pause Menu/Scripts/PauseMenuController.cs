using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
	[SerializeField] private int selectedOption = 0;
	private int moveInput;
	private int timeHolded = 0;
	private Animator[] guiOptions;
	private Animator selectAnimator;

	void Start()
	{
        guiOptions = transform.Find("gui").GetComponentsInChildren<Animator>();
		selectAnimator = transform.Find("select").gameObject.GetComponent<Animator>();
		guiOptions[0].SetBool("Selected", true);
        StartCoroutine("HoldPress");
    }

	// Update is called once per frame
	void Update()
    {
        moveInput = Input.GetKey(KeyCode.W) ? -1 : Input.GetKey(KeyCode.S) ? 1 : 0;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
		{
			timeHolded = 1;
			int newOption = selectedOption + moveInput;

			if (newOption > -1 && newOption < guiOptions.Length)
			{
				guiOptions[selectedOption].SetBool("Selected", false);
				guiOptions[newOption].SetBool("Selected", true);
				selectAnimator.SetInteger("Position", newOption);
				selectAnimator.SetTrigger("Change");
				selectedOption = newOption;
			}
		}
    }

	IEnumerator HoldPress()
	{
		while (true)
		{
			if (timeHolded > 1)
			{
				int newOption = selectedOption + moveInput;

				if (newOption > -1 && newOption < guiOptions.Length)
				{
					guiOptions[selectedOption].SetBool("Selected", false);
					guiOptions[newOption].SetBool("Selected", true);
					selectAnimator.SetInteger("Position", newOption);
					selectAnimator.SetTrigger("Change");
					selectedOption = newOption;
				}
			}

			timeHolded += moveInput != 0 ? 1 : -timeHolded + 1;

			yield return new WaitForSeconds(0.5f / timeHolded);
		}
	}
}
