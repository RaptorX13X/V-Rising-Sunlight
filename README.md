# V-Rising sunlight mechanic

### About the project
In this project I interpreted the damaging sunlight mechanic from the game V-Rising

### What does it do
The directional light changes its angle over time, creating a day and night cycle that keeps repeating. If the player If the player is exposed to the sunlight for a specified amount of time, they will start getting damage every second. 
While if the player is obscured from the sun by a shadow cast by surrounding structures, or currently its night time - they are safe from the sun's damage.

### How does it work
```csharp
public class Sun : MonoBehaviour
{
    [SerializeField] private PlayerDamage player;
    [SerializeField] private Collider playerCollider;
    [SerializeField] private float lightAngle;
    [SerializeField] private GameObject sunlight;
    [SerializeField] private LayerMask terrainLayer;
    private bool night;

    private float timer;
    private void Start()
    {
        StartCoroutine(DayNightCycle());
    }

    private void Update()
    {
        Vector3 sunAngle = sunlight.transform.forward;
        Ray ray = new Ray(player.transform.position, -sunAngle);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainLayer)) 
        {
            if (hit.collider || night)
            {
                player.exposed = false;
                Debug.DrawRay(player.transform.position, -sunAngle * 100, Color.green);
            }
        }
        else if (!night)
        {
            player.exposed = true;
            Debug.DrawRay(player.transform.position, -sunAngle * 100, Color.red);
        }
    }

    IEnumerator DayNightCycle()
    {
        while (true)
        {
            night = false;
            sunlight.SetActive(true);
            while (timer < 15)
            {
                timer += Time.deltaTime;
                var t = timer / 15;
                lightAngle = Mathf.LerpAngle(45.0f, 135.0f, t);
                yield return null;
                sunlight.transform.rotation = Quaternion.Euler(lightAngle, 0f, 0f);
            }
            night = true;
            sunlight.SetActive(false);
            yield return new WaitForSeconds(15f);
            timer = 0;
        }
    }
}
```
When the script starts to work it calls the day-night cycle coroutine to start. 
- The whole coroutine is repeating endlessly.
- It sets the night check to be false, activates the game object responsible for sunlight and starts a timer of 15 seconds of daylight.
- During these 15 seconds the angle of the sun changes over time from 45 degrees to 135 degrees through the Lerp method.
- After the time passes the night check is changed to true, sunlight game object gets turned off, coroutine waits for 15 seconds of night time, timer gets reset and coroutine repeats.
- In the update method the raycast going from the abstract sun to the player is being calculated.
- The angle of the sun is the same as the sunlights game object forward vector.
- The ray is being calculated going from the players transform to the negative vector of the suns angle.
- The raycast check has no range limit and checks for the collision with the terrain layer.
- If the raycast hits the terrain collider, or the night check is true the player is not exposed, and the engine draws a green ray toward the sun
- If the raycast hits the player and the night check is false, then the player is exposed, and the engine draws a red ray toward the sun
### What happens next
In the next script responsible for the player damage, depending on the exposure bool the player is either safe from damage, or after a few seconds starts "burning", which in current state of the game is fake damage displayed in the debug log.
