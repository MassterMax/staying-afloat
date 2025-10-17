using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] GameObject boxPrefab;
    float minTimeToNextEvent = 5f;
    float maxTimeToNextEvent = 15f;
    float timeToNextEvent = 10f;
    float timer = 0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeToNextEvent)
        {
            CreateEvent();
            timer = 0f;
            timeToNextEvent = Random.Range(minTimeToNextEvent, maxTimeToNextEvent);
        }
    }

    void CreateEvent()
    {
        Debug.Log("Event Created");
    }

    void SpawnBox()
    {
        Instantiate(boxPrefab, transform.position, Quaternion.identity);
    }
}
