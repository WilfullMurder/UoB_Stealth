using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UoBStealthGame.Highscores;


namespace UoBStealthGame.Highscores
{
    public class NewHighScoreEntry_UI : MonoBehaviour
    {


        [SerializeField] TextMeshProUGUI Char1;
        [SerializeField] TextMeshProUGUI Char2;
        [SerializeField] TextMeshProUGUI Char3;

        public Button Up1;
        public Button Up2;
        public Button Up3;

        public Button Down1;
        public Button Down2;
        public Button Down3;

        public HighScoreTable HST;

        private Button PressedButton;

        private char StringAsChar = 'A';

        public int NewHighScore = 0;

        public void IncreaseNameChar(Button LastPressed)
        {
            string currentcharacter = "";


            if (LastPressed == Up1)
            {
                currentcharacter = Char1.text;

                for (int i = 0; i < currentcharacter.Length; i++)
                {
                    if (currentcharacter.Length > 1)
                    {
                        return;
                    }
                    else
                    {
                        StringAsChar = LoopCurrentText(currentcharacter);
                        StringAsChar++;
                    }
                }


                Char1.text = StringAsChar.ToString();
                Char1.UpdateFontAsset();
            }


            else if (LastPressed == Up2)
            {

                currentcharacter = Char2.text;

                for (int i = 0; i < currentcharacter.Length; i++)
                {
                    if (currentcharacter.Length > 1)
                    {
                        return;
                    }
                    else
                    {
                        StringAsChar = LoopCurrentText(currentcharacter);
                        StringAsChar++;
                    }
                }
                Char2.text = StringAsChar.ToString();
                Char2.UpdateFontAsset();
            }
            else
            {

                currentcharacter = Char3.text;

                for (int i = 0; i < currentcharacter.Length; i++)
                {
                    if (currentcharacter.Length > 1)
                    {
                        return;
                    }
                    else
                    {
                        StringAsChar = LoopCurrentText(currentcharacter);
                        StringAsChar++;
                    }
                }
                Char3.text = StringAsChar.ToString();
                Char3.UpdateFontAsset();
            }

        }

        public void DecreaseNameChar(Button LastPressed)
        {
            string currentcharacter = "";



            if (LastPressed == Down1)
            {

                currentcharacter = Char1.text;

                for (int i = 0; i < currentcharacter.Length; i++)
                {
                    if (currentcharacter.Length > 1)
                    {
                        return;
                    }
                    else
                    {
                        StringAsChar = LoopCurrentText(currentcharacter);
                        StringAsChar--;
                    }
                }
                Char1.text = StringAsChar.ToString();
                Char1.UpdateFontAsset();
            }

            else if (LastPressed == Down2)
            {
                currentcharacter = Char2.text;

                for (int i = 0; i < currentcharacter.Length; i++)
                {
                    if (currentcharacter.Length > 1)
                    {
                        return;
                    }
                    else
                    {
                        StringAsChar = LoopCurrentText(currentcharacter);
                        StringAsChar--;
                    }
                }
                Char2.text = StringAsChar.ToString();
                Char2.UpdateFontAsset();
            }
            else
            {
                currentcharacter = Char3.text;

                for (int i = 0; i < currentcharacter.Length; i++)
                {
                    if (currentcharacter.Length > 1)
                    {
                        return;
                    }
                    else
                    {
                        StringAsChar = LoopCurrentText(currentcharacter);
                        StringAsChar--;
                    }
                }
                Char3.text = StringAsChar.ToString();
                Char3.UpdateFontAsset();
            }

        }

        public void SetPressedButton(Button LastPressed)
        {

            if (LastPressed == Up1 || LastPressed == Up2 || LastPressed == Up3)
            {
                IncreaseNameChar(LastPressed);
            }
            else
            {
                DecreaseNameChar(LastPressed);
            }
        }

        public char LoopCurrentText(string TextToLoop)
        {

            for (int i = 0; i < TextToLoop.Length; i++)
            {
                if (TextToLoop.Length > 1)
                {
                    break;
                }
                else
                {
                    StringAsChar = TextToLoop[i];

                }
            }
            return StringAsChar;
        }

        public void SaveNewname()
        {
            string String1 = Char1.text.ToString();
            string String2 = Char2.text.ToString();
            string String3 = Char3.text.ToString();
            string NameText = String1 + String2 + String3;
            
            HST.SaveNewHighScoreEntry(HST.GetPlayerTrackedScore(), NameText);
        }


    }
}

