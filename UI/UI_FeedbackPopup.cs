using mixpanel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_FeedbackPopup : UI_BaseClass
{
    private GameObject container;

    private GameObject validationPage;
    private Button yesButton;
    private Button noButton;

    private GameObject page1;
    private TMP_InputField ageInput;
    private Button ratingButton1;
    private Button ratingButton2;
    private Button ratingButton3;
    private Button ratingButton4;
    private Button ratingButton5;
    private Button nextPageButton;

    private GameObject page2;
    private TMP_InputField feedbackInput;
    private Button backButton;
    private Button sendButton;

    private UI_FeedbackData uI_FeedbackData;
    private bool isAgeSet;
    private bool isRatingSet;

    private bool canGoToNextPage => isAgeSet && isRatingSet;

    public void ToggleContainer() => Helpers.SetActive_Toggle(container);

    protected override void Awake()
    {
        base.Awake();
        uI_FeedbackData = new UI_FeedbackData();

        SetDefaultValues();

        validationPage.SetActive(true);
        page1.SetActive(false);
        page2.SetActive(false);
    }

    protected override void AddListeners()
    {
        noButton.onClick.AddListener(delegate { ToggleContainer(); });
        yesButton.onClick.AddListener(delegate { StartFeedbackForm(); });

        nextPageButton.onClick.AddListener(GoToNextPage);
        ratingButton1.onClick.AddListener(delegate { SetRating(1); });
        ratingButton2.onClick.AddListener(delegate { SetRating(2); });
        ratingButton3.onClick.AddListener(delegate { SetRating(3); });
        ratingButton4.onClick.AddListener(delegate { SetRating(4); });
        ratingButton5.onClick.AddListener(delegate { SetRating(5); });

        backButton.onClick.AddListener(GoToPreviousPage);
        sendButton.onClick.AddListener(SendFeedback);
        sendButton.onClick.AddListener(ToggleContainer);

        ageInput.onValueChanged.AddListener(delegate { SetAge(); });
        feedbackInput.onValueChanged.AddListener(delegate { SetFeedbackText(); });
    }

    protected override void GetObjectsAndButtons()
    {
        container = transform.Find("canvas").Find("container").gameObject;

        validationPage = container.transform.Find("validationPage").gameObject;
        noButton = validationPage.transform.Find("noButton").GetComponent<Button>();
        yesButton = validationPage.transform.Find("yesButton").GetComponent<Button>();

        page1 = container.transform.Find("page1").gameObject;
        ageInput = page1.transform.Find("question_Age").Find("input").GetComponent<TMP_InputField>();
        var ratingButtons = page1.transform.Find("question_Rating").Find("ratingButtons");
        ratingButton1 = ratingButtons.Find("ratingButton1").GetComponent<Button>();
        ratingButton2 = ratingButtons.Find("ratingButton2").GetComponent<Button>();
        ratingButton3 = ratingButtons.Find("ratingButton3").GetComponent<Button>();
        ratingButton4 = ratingButtons.Find("ratingButton4").GetComponent<Button>();
        ratingButton5 = ratingButtons.Find("ratingButton5").GetComponent<Button>();
        nextPageButton = page1.transform.Find("nextPageButton").GetComponent<Button>();

        page2 = container.transform.Find("page2").gameObject;
        feedbackInput = page2.transform.Find("feedbackInput").GetComponent<TMP_InputField>();
        backButton = page2.transform.Find("backButton").GetComponent<Button>();
        sendButton = page2.transform.Find("sendButton").GetComponent<Button>();
    }

    private void CheckIfPage1FieldsHaveBeenFilled()
    {
        if (canGoToNextPage)
            nextPageButton.interactable = true;
    }

    private void StartFeedbackForm()
    {
        validationPage.SetActive(false);
        page1.SetActive(true);
    }

    private void SendFeedback()
    {
        var data = new Value();
        data["age"] = uI_FeedbackData.Age;
        data["rating"] = uI_FeedbackData.Rating;
        data["feedbackText"] = uI_FeedbackData.FeedbackText;
        Mixpanel.Track("Feedback Sent", data);

        uI_MenuController.TogglePrizePopup();
    }

    private void SetDefaultValues()
    {
        ageInput.text = string.Empty;
        feedbackInput.text = string.Empty;
        isAgeSet = false;
        isRatingSet = false;
        nextPageButton.interactable = false;
    }

    private void SetFeedbackText() => uI_FeedbackData.FeedbackText = feedbackInput.text;

    private void SetAge()
    {
        int parsedAge;
        bool parsedSuccesfully = int.TryParse(ageInput.text, out parsedAge);

        if (parsedSuccesfully)
        {
            uI_FeedbackData.Age = parsedAge;
            isAgeSet = true;
        }

        CheckIfPage1FieldsHaveBeenFilled();
    }

    private void SetRating(int rating)
    {
        uI_FeedbackData.Rating = rating;
        isRatingSet = true;

        CheckIfPage1FieldsHaveBeenFilled();
    }

    private void GoToNextPage()
    {
        page1.SetActive(false);
        page2.SetActive(true);
    }

    private void GoToPreviousPage()
    {
        page1.SetActive(true);
        page2.SetActive(false);
    }
}
