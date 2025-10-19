using UnityEngine;

public class ShipEngine : MonoBehaviour
{
    [SerializeField] GameObject turnedOnImage;
    [SerializeField] GameObject turnedOffImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TurnOn();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TurnOn()
    {
        turnedOnImage.SetActive(true);
        turnedOffImage.SetActive(false);
    }

    public void TurnOff()
    {
        turnedOnImage.SetActive(false);
        turnedOffImage.SetActive(true);
    }
}
