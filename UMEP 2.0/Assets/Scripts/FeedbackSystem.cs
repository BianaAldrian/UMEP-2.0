using UnityEngine;
using UnityEngine.UI;

public class FeedbackSystem : MonoBehaviour
{
    public InputField feedbackInputField;
    public Text feedbackDisplayText;

    public void SubmitFeedback()
    {
        string userFeedback = feedbackInputField.text;
        feedbackDisplayText.text = "Feedback: " + userFeedback;
        feedbackInputField.text = ""; // Clear the input field
    }
}
