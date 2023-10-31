using UnityEngine;
using UnityEngine.UI;

public class FloorChanger : MonoBehaviour
{
    public Image floorHolder; // Specify the Image class from UnityEngine.UI
    public Button fifthButton, sixthButton, seventhButton, eighthButton, ninthButton, tenthButton, eleventhButton, twelfthButton;
    public Sprite fifthImage, sixthImage, seventhImage, eighthImage, ninthImage, tenthImage, eleventhImage, twelfthImage;

    // Start is called before the first frame update
    void Start()
    {
        // You can add your button click listener here.
        fifthButton.onClick.AddListener(FifthFloor);
        sixthButton.onClick.AddListener(SixthFloor);
        seventhButton.onClick.AddListener(SeventhFloor);
        eighthButton.onClick.AddListener(EightFloor);
        ninthButton.onClick.AddListener(NinthFloor);
        tenthButton.onClick.AddListener(TenthFloor);
        eleventhButton.onClick.AddListener(EleventhFloor);
        twelfthButton.onClick.AddListener(TwelfthFloor);
    }

    void FifthFloor()
    {
        floorHolder.sprite = fifthImage;
    }

    void SixthFloor()
    {
        floorHolder.sprite = sixthImage;
    }

    void SeventhFloor()
    {
        floorHolder.sprite = seventhImage;
    }

    void EightFloor()
    {
        floorHolder.sprite = eighthImage;
    }

    void NinthFloor()
    {
        floorHolder.sprite = ninthImage;
    }

    void TenthFloor()
    {
        floorHolder.sprite = tenthImage;
    }

    void EleventhFloor()
    {
        floorHolder.sprite = eleventhImage;
    }

    void TwelfthFloor()
    {
        floorHolder.sprite = twelfthImage;
    }
}
