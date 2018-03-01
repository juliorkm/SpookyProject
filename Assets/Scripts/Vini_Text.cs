using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Globalization;

public class Vini_Text : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI dialogText;
    [SerializeField]
    AudioSource audioManager;
    [SerializeField]
    Image sign1, sign2;
    [SerializeField]
    TextMeshProUGUI atk, item;

    bool skip_display = false;
    bool next_dialog = false;
    bool text_running = false;
    bool dialog_active = false;

    public IEnumerator WaitForEndOfFrames(int frames) {
        for (int i = 0; i < frames; i++) {
            yield return new WaitForEndOfFrame();
        }
    }

    public bool pressButton() {
        return Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.KeypadEnter) ||
            Input.GetKeyDown(KeyCode.JoystickButton0);
    }

    public bool pressSkipButton() {
        return Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7);
    }

    public IEnumerator toTitle() {
        float fadeTime = FindObjectOfType<FadeInOut>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(1);
    }

    void Start() {
        StartCoroutine(Text());
        StartCoroutine(SkipText());

        if (!PlayerPrefs.HasKey("atk")) PlayerPrefs.SetString("atk", "z");

        if (!PlayerPrefs.HasKey("item")) PlayerPrefs.SetString("item", "x");

        if (!PlayerPrefs.HasKey("atkJ")) PlayerPrefs.SetString("atkJ", "joystick button 2");

        if (!PlayerPrefs.HasKey("itemJ")) PlayerPrefs.SetString("itemJ", "joystick button 3");

        atk.text = PlayerPrefs.GetString("atk").ToUpper() + " / <sprite name=\"" + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(PlayerPrefs.GetString("atkJ")) + "\">";
        item.text = PlayerPrefs.GetString("item").ToUpper() + " / <sprite name=\"" + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(PlayerPrefs.GetString("itemJ")) + "\">";
    }

    IEnumerator SkipText() {
        yield return new WaitUntil(() => pressSkipButton());
        yield return toTitle();
    }

    IEnumerator Tutorial() {
        dialogText.text = "";
        dialogText.transform.localPosition = new Vector2(0, 150);
        while (90 - sign2.transform.eulerAngles.x < 89.5f) {
            sign1.transform.localRotation = Quaternion.Lerp(sign1.transform.localRotation, Quaternion.identity, .11f);
            if (sign1.transform.eulerAngles.x < 60f) sign2.transform.localRotation = Quaternion.Lerp(sign2.transform.localRotation, Quaternion.identity, .11f);
            yield return new WaitForSeconds(.02f);
        }
        yield return Display_String("Use isso para enfrentá-los", 4);
        yield return new WaitUntil(() => pressButton());
    }

    IEnumerator Text() {
        List<string> aux = new List<string> {
            "Bia...",
            "Já faz tanto tempo",
            "Por que você não acorda?",
            "Você sempre foi uma criança tão feliz",
            "E seu medo te fez isso",
            "Você ao menos lembra <b><u>como</u></b> veio parar aqui?",
            "...",
            "Eu não quero te perder...",
            "Não deixe que seus medos te conquistem",
            "<b><u>Você</u></b> deve conquistar seus medos."
        };


        for (int i = 0; i < aux.Count; i++) {
            yield return Display_String(aux[i], 4);
            yield return new WaitUntil(() => pressButton());
        }

        yield return Tutorial();

        StartCoroutine(toTitle());
    }

    IEnumerator Display_String(string text, int speed) {
        int current_character = 0;
        text_running = true;

        while (true) {
            var aux = text;
            if (current_character == text.Length ||
                skip_display) {
                break;
            }

            while (text[current_character] == '<') {
                while (text[current_character] != '>') current_character++;
                current_character++;
            }

            dialogText.text = aux.Insert(current_character++,"<alpha=#00>");
            audioManager.Play();
            yield return WaitForEndOfFrames(speed);
        }

        skip_display = false;
        text_running = false;
        dialogText.text = text;
    }
}