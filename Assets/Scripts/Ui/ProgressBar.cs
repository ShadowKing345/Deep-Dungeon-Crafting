using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ProgressBar : MonoBehaviour
{
    public int current;
    public int maximum = 100;
    
    public Image mask;

    private void Start()
    {
        mask ??= GameObject.Find("Mask").GetComponent<Image>();
    }

    private void Update()
    {
        GetFill();
    }

    private void GetFill()
    {
        mask.fillAmount = (float) current / maximum;
    }
}
