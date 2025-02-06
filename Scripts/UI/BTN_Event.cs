using UnityEngine;

public class BTN_Event : MonoBehaviour
{
    [field: SerializeField] public string name { get; private set; }
    
    public void BTNclickEvent()
    {

        switch (name)
        {
            case "MainBattle":
                UIManager.Instance.OnStage();
                break;
            default:
                break;
        }
    }
}
