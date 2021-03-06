﻿using System.Collections;
using System.Collections.Generic;
using Luke;
using UnityEngine;
using UnityEngine.UI;
using Matthew;

namespace Zach
{
    public class NoteBehaviour : MonoBehaviour
    {
        public GameObjectVariable NoteUI;
        public NoteScriptable note;

        public void SetNoteUIActive(bool on)
        {
            NoteUI.Value.transform.GetChild(0).gameObject.SetActive(on);
            NoteUI.Value.transform.GetChild(1).gameObject.SetActive(on);
            NoteUI.Value.transform.GetChild(2).gameObject.SetActive(on);
        }

        public void SetUIText()
        {
            note.SetIsEnabled(true);
            var textUI = NoteUI.Value.transform.GetChild(1);
            var text = textUI.GetComponent<Text>();
            text.text = note.data;
        }
    }
}