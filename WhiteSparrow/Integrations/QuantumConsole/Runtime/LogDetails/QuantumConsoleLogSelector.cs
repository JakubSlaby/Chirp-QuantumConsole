using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WhiteSparrow.Integrations.QC.LogDetails
{
	public class QuantumConsoleLogSelector : UIBehaviour, IPointerClickHandler
	{
		[SerializeField] 
		private TextMeshProUGUI _textComponent;

		[SerializeField] 
		private ChirpQuantumConsole _chirpQuantumConsole;

		public void OnPointerClick(PointerEventData eventData)
		{
			var line = TMP_TextUtilities.FindIntersectingLine(_textComponent, eventData.position, null);
			if (line < 0)
				return;
			int characterClosest = TMP_TextUtilities.FindNearestCharacterOnLine(_textComponent, eventData.position, line, null, true);

			string strippedText = ChirpConsoleUtils.StripTags(_textComponent.text);
			Debug.Log($"{_textComponent.text.Length} = {strippedText.Length}");
			int logLine = ChirpConsoleUtils.CountLineBreaks(strippedText, 0, characterClosest);

			var log = _chirpQuantumConsole.FindLog(logLine);
			if (log == null)
				return;
			_chirpQuantumConsole.ShowLogDetails(log);
		}

#if UNITY_EDITOR
		protected override void OnValidate()
		{
			if (_textComponent == null)
				_textComponent = this.GetComponent<TextMeshProUGUI>();
		}
#endif
	}
}