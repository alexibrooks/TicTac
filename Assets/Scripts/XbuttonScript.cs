using UnityEngine;
using UnityEngine.UI;

public class XbuttonScript : MonoBehaviour
{

    public Button b;
    public Text btext;
    private ControllerScript c;

    public void SetSpace()
    {
        bool side = c.GetSide();
        if (side)
        {
            btext.text = "X";
        }
        else
        {
            btext.text = "O";
        }
        c.registerClick(b.GetInstanceID());
        b.interactable = false;
        //side = !side;
        c.EndTurn();
    }

    public void SetCRef(ControllerScript cs) {
        c = cs;
    }

}
