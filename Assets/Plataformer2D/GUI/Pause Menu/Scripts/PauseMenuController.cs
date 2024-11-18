using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
	[SerializeField] private int _selectedOption = 0;
	private int _moveInput;
	private int _timeHolded = 0;
	private Animator[] _guiOptions;
	private Animator _selectAnimator;

    void Start()
    {
        _guiOptions = transform.Find("gui").GetComponentsInChildren<Animator>();
        _selectAnimator = transform.Find("select").gameObject.GetComponent<Animator>();
        _guiOptions[0].SetBool("Selected", true);
        StartCoroutine("HoldPress");
    }

    // Update is called once per frame
    void Update()
    {
        _moveInput = Input.GetKey(KeyCode.W) ? -1 : Input.GetKey(KeyCode.S) ? 1 : 0;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
		{
			_timeHolded = 1;
			int newOption = _selectedOption + _moveInput;

			if (newOption > -1 && newOption < _guiOptions.Length)
			{
				_guiOptions[_selectedOption].SetBool("Selected", false);
				_guiOptions[newOption].SetBool("Selected", true);
				_selectAnimator.SetInteger("Position", newOption);
				_selectAnimator.SetTrigger("Change");
				_selectedOption = newOption;
			}
		}
    }

	IEnumerator HoldPress()
	{
		while (true)
		{
			if (_timeHolded > 1)
			{
				int newOption = _selectedOption + _moveInput;

				if (newOption > -1 && newOption < _guiOptions.Length)
				{
					_guiOptions[_selectedOption].SetBool("Selected", false);
					_guiOptions[newOption].SetBool("Selected", true);
					_selectAnimator.SetInteger("Position", newOption);
					_selectAnimator.SetTrigger("Change");
					_selectedOption = newOption;
				}
			}

			_timeHolded += _moveInput != 0 ? 1 : -_timeHolded + 1;

			yield return new WaitForSeconds(0.5f / _timeHolded);
		}
	}
}
