using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.SimpleLocalization.Scripts;
using TMPro;
using System.Collections.Generic;

namespace Assets.SimpleLocalization
{
	/// <summary>
	/// Asset usage example.
	/// </summary>
	public class Example1 : MonoBehaviour
	{
		public Text FormattedText;
		[SerializeField] public TMP_FontAsset FontAssetEnglish;
		[SerializeField] public TMP_FontAsset FontAssetChinese;
		public List<TextMeshPro> textMeshProObjects;
		public List<TextMeshProUGUI> textMeshProUiObjects;
		[SerializeField] public Image ControlsGraphic;
		[SerializeField] public Sprite Eng, Chi;

		/// <summary>
		/// Called on app start.
		/// </summary>
		public void Awake()
		{
			LocalizationManager.Read();

			switch (LocalizationManager.Language)
			{
				case "Chinese":
					ChangeFont(FontAssetChinese);
					ControlsGraphic.sprite = Chi;
					break;
				default:
					LocalizationManager.Language = "English";
					ChangeFont(FontAssetEnglish);
					ControlsGraphic.sprite = Eng;
					break;
			}

			// This way you can localize and format strings from code.
			// FormattedText.text = LocalizationManager.Localize("Settings.Example.PlayTime", TimeSpan.FromHours(10.5f).TotalHours);

			// This way you can subscribe to LocalizationChanged event.

		}

		/// <summary>
		/// Change localization at runtime.
		/// </summary>
		public void SetLocalization(string localization)
		{
			LocalizationManager.Language = localization;
			switch (localization)
			{
				case "English":
					ChangeFont(FontAssetEnglish);
					ControlsGraphic.sprite = Eng;
					break;
				case "Chinese":
					ChangeFont(FontAssetChinese);
					ControlsGraphic.sprite = Chi;
					break;
			}


		}

		public void ChangeFont(TMP_FontAsset fontObject)
		{

			foreach (TextMeshProUGUI textMeshProUi in textMeshProUiObjects)
			{
				if (textMeshProUi.text == "中文")
				{
					continue;
				}
				textMeshProUi.font = fontObject;
			}
		}
	}
}