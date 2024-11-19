using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
	[SerializeField] private int _selectedOption = 0;
	private int _moveInput;
	private int _timeHolded = 0;
	private Animator[] _guiOptionsAnimator;
	private Animator _selectAnimator;
	private Animator _pauseMenuAnimator;

	private bool _isPaused;

    void Awake()
    {
		GameObject graphics = transform.Find("Graphics").gameObject;
		_pauseMenuAnimator = GetComponent<Animator>();
		_guiOptionsAnimator = graphics.transform.Find("gui").GetComponentsInChildren<Animator>();
        _selectAnimator = graphics.transform.Find("select").gameObject.GetComponent<Animator>();
        _guiOptionsAnimator[0].SetBool("Selected", true);
        StartCoroutine("HoldPress");
    }

    // Update is called once per frame
    void Update()
    {
		if (!_isPaused)
		{
			if (Input.GetKeyDown(KeyCode.P))
				Pause();
			else
				return;
		} 
		else if (Input.GetKeyDown(KeyCode.X))	
			Resume();

        _moveInput = Input.GetKey(KeyCode.W) ? -1 : Input.GetKey(KeyCode.S) ? 1 : 0;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
		{
			_timeHolded = 1;
			int newOption = _selectedOption + _moveInput;

			if (newOption > -1 && newOption < _guiOptionsAnimator.Length)
			{
				_guiOptionsAnimator[_selectedOption].SetBool("Selected", false);
				_guiOptionsAnimator[newOption].SetBool("Selected", true);
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

				if (newOption > -1 && newOption < _guiOptionsAnimator.Length)
				{
					_guiOptionsAnimator[_selectedOption].SetBool("Selected", false);
					_guiOptionsAnimator[newOption].SetBool("Selected", true);
					_selectAnimator.SetInteger("Position", newOption);
					_selectAnimator.SetTrigger("Change");
					_selectedOption = newOption;
				}
			}

			_timeHolded += _moveInput != 0 ? 1 : -_timeHolded + 1;

			yield return new WaitForSeconds(0.5f / _timeHolded);
		}
	}

	private void Pause()
	{
		_isPaused = true;
		_pauseMenuAnimator.SetBool("Pause", true);
		Time.timeScale = 0f;
	}

	private void Resume()
	{
		_isPaused = false;
		_pauseMenuAnimator.SetBool("Pause", false);
		Time.timeScale = 1f;
	}
}
