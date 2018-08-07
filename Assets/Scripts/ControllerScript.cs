using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using System.Net.Http;

public class ControllerScript : MonoBehaviour
{

    public Text[] buttonList;
    public GameObject victoryPanel;
//    public Text victoryText;
    private bool side; //True==X, False==O
    //private static readonly HttpClient client = new HttpClient();
    private string json = @"{
        'test?':'true', 
        'foo':'bar', 
        'creator':'Alexi'
    }";


    public void SetButtonControllers() {
        for (int i = 0; i < buttonList.Length;i++) {
            buttonList[i].GetComponentInParent<XbuttonScript>().SetCRef(this);
        }
    }

	private void Awake()
	{
        SetButtonControllers();
        side = true;
	}

    public bool GetSide() {
        return side;
    }

    private bool IsGameOver() {
        if (buttonList[0].text==buttonList[1].text && buttonList[0].text==buttonList[2].text && buttonList[0].text != "" ||
            buttonList[3].text == buttonList[4].text && buttonList[4].text == buttonList[5].text && buttonList[5].text != "" ||
            buttonList[6].text == buttonList[7].text && buttonList[7].text == buttonList[8].text && buttonList[8].text != ""||
            buttonList[0].text == buttonList[4].text && buttonList[0].text == buttonList[8].text && buttonList[8].text != ""||
            buttonList[6].text == buttonList[2].text && buttonList[2].text == buttonList[4].text && buttonList[4].text != ""||
            buttonList[0].text == buttonList[3].text && buttonList[0].text == buttonList[6].text && buttonList[6].text != ""||
            buttonList[1].text == buttonList[4].text && buttonList[1].text == buttonList[7].text && buttonList[7].text != ""||
            buttonList[5].text == buttonList[8].text && buttonList[8].text == buttonList[2].text && buttonList[2].text != "") {
            return true;
        }
        return false;
    }

    public void EndTurn() {
        if(IsGameOver()) {
            for (int i = 0; i < buttonList.Length;i++) {
                buttonList[i].GetComponentInParent<XbuttonScript>().b.interactable = false;
            }
            victoryPanel.SetActive(true);
            doPost();

        }
        side = !side;
//        Debug.Log("Not ready yet. Still have work to do.");
    }

/*    private void postGameEnd(string winner) {
        var values = new Dictionary<string, string> {
            { "Game", "TicTacToe" },
            { "Winner", "X" },
        };

        var content = new FormUrlEncodedContent(values);

        var response = await client.PostAsync("http://54.149.126.189:14056/logs", content);

        var responseString = await response.Content.ReadAsStringAsync();
    }*/

    void doPost() {
        string URL = "http://54.149.126.189:14056/logs";
        string myAccessKey = "myAccessKey";
        string mySecretKey = "mySecretKey";

        //Auth token for http request
        string accessToken;
        //Our custom Headers
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        //Encode the access and secret keys
        accessToken = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(myAccessKey + ":" + mySecretKey));
        //Add the custom headers
        //parameters.Add("Authorization", "Basic " + accessToken);
        parameters.Add("Content-Type", "application/json");
        //parameters.Add("AnotherHeader", "AnotherData");
        parameters.Add("new-header", "WhyEverNot");
        //parameters.Add("Content-Length", json.Length.ToString());
        //Replace single ' for double " 
        //This is usefull if we have a big json object, is more easy to replace in another editor the double quote by singles one
        json = json.Replace("'", "\"");
        //Encode the JSON string into a bytes
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(json);
        //Now we call a new WWW request
        WWW www = new WWW(URL, postData, parameters);
        //And we start a new co routine in Unity and wait for the response.
        StartCoroutine(WaitForRequest(www));
    }
    //Wait for the www Request
    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        if (www.error == null)
        {
            //Print server response
            Debug.Log(www.text);
        }
        else
        {
            //Something goes wrong, print the error response
            Debug.Log(www.error);
        }
    }

}
