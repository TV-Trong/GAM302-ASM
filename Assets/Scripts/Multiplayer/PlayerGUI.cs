using UnityEngine;

public class PlayerGUI : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != transform.parent && collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerNetworkProperties>().TogglePlayerGUI(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != transform.parent && collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerNetworkProperties>().TogglePlayerGUI(false);
        }
    }
}
