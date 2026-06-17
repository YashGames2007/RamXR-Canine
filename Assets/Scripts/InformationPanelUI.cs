using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Meta.WitAi.TTS.Utilities;

public class InformationPanelUI : MonoBehaviour
{
    [Header("Event Channel")]
    public PartFocusEnteredEventChannelSO focusEventChannel;

    [Header("UI References")]
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public Button previousButton;
    public Button nextButton;

    [Header("TTS")]
    public TTSSpeaker ttsSpeaker;
    public Button listenButton;

    private ModelPartDataSO currentData;
    private int currentIndex;

    private void Awake()
    {
        AudioSettings.Reset(AudioSettings.GetConfiguration());

        AudioSource audio = ttsSpeaker.GetComponent<AudioSource>();
        if (audio != null)
        {
            audio.spatialBlend = 0f;
            audio.volume = 1.0f;
            audio.spatialize = false;
        }
    }

    private void OnEnable()
    {
        focusEventChannel.OnEventRaised += OnPartSelected;
        previousButton.onClick.AddListener(ShowPrevious);
        nextButton.onClick.AddListener(ShowNext);
        listenButton.onClick.AddListener(OnListenClicked);
    }

    private void OnDisable()
    {
        focusEventChannel.OnEventRaised -= OnPartSelected;
        previousButton.onClick.RemoveListener(ShowPrevious);
        nextButton.onClick.RemoveListener(ShowNext);
        listenButton.onClick.RemoveListener(OnListenClicked);
        StopAllCoroutines();
    }

    private void OnPartSelected(ModelPartDataSO data)
    {
        currentData = data;
        currentIndex = 0;

        // stop any playing audio when new part is selected
        if (ttsSpeaker != null) ttsSpeaker.Stop();
        listenButton.interactable = true;

        UpdateUI();
    }

    private void ShowPrevious()
    {
        if (currentData == null) return;
        currentIndex--;

        if (ttsSpeaker != null) ttsSpeaker.Stop();
        listenButton.interactable = true;

        UpdateUI();
    }

    private void ShowNext()
    {
        if (currentData == null) return;
        currentIndex++;

        if (ttsSpeaker != null) ttsSpeaker.Stop();
        listenButton.interactable = true;

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (currentData == null || currentData.descriptionChunks.Length == 0)
            return;

        titleText.text = currentData.partName;
        descriptionText.text = currentData.descriptionChunks[currentIndex];

        UpdateButtons();
    }

    private void UpdateButtons()
    {
        previousButton.interactable = currentIndex > 0;
        nextButton.interactable = currentIndex < currentData.descriptionChunks.Length - 1;
    }

    private void OnListenClicked()
    {
        if (ttsSpeaker == null || descriptionText == null) return;

        string text = descriptionText.text;
        if (string.IsNullOrWhiteSpace(text)) return;

        listenButton.interactable = false;
        ttsSpeaker.Stop();
        ttsSpeaker.SpeakQueued(text);

        StartCoroutine(WaitForSpeakToFinish());
    }

    private System.Collections.IEnumerator WaitForSpeakToFinish()
    {
        yield return null;

        AudioSource audio = ttsSpeaker.GetComponent<AudioSource>();

        if (audio != null)
        {
            yield return new WaitUntil(() => audio.isPlaying);
            yield return new WaitWhile(() => audio.isPlaying);
        }
        else
        {
            int wordCount = descriptionText.text.Split(' ').Length;
            yield return new WaitForSeconds(wordCount * 0.4f);
        }

        listenButton.interactable = true;
    }
}