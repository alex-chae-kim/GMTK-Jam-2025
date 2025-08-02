using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPopupPrefab;
    public SettingsPopup settingsPopupInScene;
    private GameObject popupInstance;
    private bool isPopupOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!settingsPopupInScene.gameObject.activeSelf)
            {
                settingsPopupInScene.OpenPopup();
            }
            else
            {
                settingsPopupInScene.ClosePopup();
            }
        }
    }

    void OpenPopup()
    {
        if (popupInstance == null)
            popupInstance = Instantiate(settingsPopupPrefab, FindObjectOfType<Canvas>().transform);

        var popupScript = popupInstance.GetComponent<SettingsPopup>();

        popupInstance.SetActive(true);
        popupInstance.GetComponent<SettingsPopup>().OpenPopup();
        isPopupOpen = true;
    }

    void ClosePopup()
    {
        if (popupInstance != null)
        {
            popupInstance.GetComponent<SettingsPopup>().ClosePopup();
            isPopupOpen = false;
        }
    }
}
