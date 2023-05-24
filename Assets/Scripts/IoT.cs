using UnityEngine;
using KyleDulce.SocketIo;
using UnityEngine.UI;

public class IoT : MonoBehaviour
{
    private bool runLocal = true;
    Socket socket;

    private string currentPotValue = "0";
    private float potRotation = 0.0f;
    public Button upBtn;
    public Button dwnBtn;
    public Button allUpBtn;
    public Button allDownBtn;
    public Button lightBtn;
    public Text lightModeTxt;

    // Start is called before the first frame update
    void Start()
    {
        /*--------------               Connect to Server                                    ----------------------*/
        if (runLocal)
        {
            Debug.Log("Connect to Local Server");
            socket = SocketIo.establishSocketConnection("ws://localhost:3000");
        }
        else
        {
            Debug.Log("Connect to Online Server");
            socket = SocketIo.establishSocketConnection("ws://sdu-e22-iot-v1.eu-4.evennode.com");
        }

        //Connect to server
        socket.connect();
        Debug.Log("Socket IO - Connected");



        /*--------------               Receive Updates                                    ----------------------*/

        //On "CurrentPotentiometerValue"
        socket.on("CurrentPotentiometerValue", SetCurrentPotentiometerValue);
    }

    void SetCurrentPotentiometerValue(string data)
    {
        currentPotValue = data;
        Debug.Log("CurrentPotValue Received: " + currentPotValue);
        //convert the potentiometers min/max to unitys Min/max
        float oldRange = 1023 - 0;
        float newRange = 4.13f - 2.09f;

        float newValue = ((float.Parse(currentPotValue) - 0) * newRange / oldRange) + 2.09f;

        //set the value of the curtain to what it is currently, min = 2.09 (Curtain down) Max = 4.13 (Curtain up)
        this.transform.position = new Vector3(0, 3);
        

    }

    void Update()
    {
        if(this.transform.position.y <= 2.09f) {
            this.transform.position = new Vector3(0, 2.09f);
            dwnBtn.interactable = false;
        }

        if (this.transform.position.y >= 4.13f) {
            this.transform.position = new Vector3(0, 4.13f);
            upBtn.interactable = false;
        }
    }

    public void CurtainUp() {
        this.transform.position = new Vector3(0, this.transform.position.y + 0.1f);
        if (!dwnBtn.interactable) {
            dwnBtn.interactable = true;
        }
    }
    public void CurtainDown() {
        this.transform.position = new Vector3(0, this.transform.position.y - 0.1f);
        if (!upBtn.interactable) {
            upBtn.interactable = true;
        }
    }

    public void CurtainAllUp() {
        //insert code for take the curtain all the way up and disable buttons
    }

    public void CurtainAllDown() {
        //insert code for take the curtain all the way down and disable buttons
    }

    public void LightMode() {
        string currentText = lightModeTxt.text;
        //insert code for toggling light mode
        Debug.Log(currentText);

        if(currentText == "Light Mode is ON") {
            lightModeTxt.text = "Light Mode is OFF";
        } else {
            lightModeTxt.text = "Light Mode is ON";
        }
    }

}

