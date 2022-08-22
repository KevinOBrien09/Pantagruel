using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent( typeof( TMP_Text ) )]
public partial class TMP_Typewriter : MonoBehaviour
{
	public bool typing;
	[SerializeField] private TMP_Text m_textUI = null;
	string m_parsedText;
	Action m_onComplete;
	[SerializeField] AudioSource source;
	Tween m_tween;

	Queue<char> q = new Queue<char>();
	
	
	void Reset()
	{m_textUI = GetComponent<TMP_Text>();}
	
	void OnDestroy()
	{
		m_tween?.Kill();

		m_tween = null;
		m_onComplete = null;
	}
	
	public void Play( string text, float speed, Action onComplete )
	{
	
		m_textUI.text = text;
		m_onComplete = onComplete;
	typing = true;
		m_textUI.ForceMeshUpdate();
// 		if(source!= null){
// 			if(source.clip != null){
// SetUpSound(text);
// 			}

		//}
		
		m_parsedText = m_textUI.GetParsedText();

		var length = m_parsedText.Length;
		var duration = 1 / speed * length;

		OnUpdate( 0 );

		m_tween?.Kill();
		m_tween = DOTween
			.To( value => OnUpdate( value ), 0, 1, duration )
			.SetEase( Ease.Linear )
			.OnComplete( () => OnComplete() )
		;
	}
	
	public void Skip( bool withCallbacks = true )
	{
		m_tween?.Kill();
		m_tween = null;

		OnUpdate( 1 );

		if ( !withCallbacks ) return;

		m_onComplete?.Invoke();
		m_onComplete = null;
	}
	
	public void Pause()
	{
		m_tween?.Pause();
	}

	public void SetUpSound(string word)
	{
		char[] wordLength = word.ToCharArray();
		
		for (int i = 0; i < wordLength.Length; i++)
		{
			
			q.Enqueue(wordLength[i]);
		}
		StartCoroutine(PlaySound())	;
	}

	public IEnumerator PlaySound()
	{
		if(q.Count > 0)
		{
			q.Dequeue();
			source.Play();
			yield return new WaitForSeconds(source.clip.length);
			StartCoroutine(PlaySound());
		}
		else
		{
			q.Clear();
		}


	}


	public void Resume()
	{
		m_tween?.Play();
	}
	
	void OnUpdate( float value )
	{	
	
		
		var current = Mathf.Lerp( 0, m_parsedText.Length, value );
		var count = Mathf.FloorToInt( current );
		
		m_textUI.maxVisibleCharacters = count;
	}
	
	void OnComplete()
	{
		
		typing = false;
		m_tween = null;
		m_onComplete?.Invoke();
		m_onComplete = null;
	}
}
