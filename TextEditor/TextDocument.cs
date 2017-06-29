using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;

namespace TextEditor
{
    class TextDocument
    {
        string _pathName;
        TextBox _textInput;
        string _currentText;

        public TextDocument(string PathName, TextBox TextInput)
        {
            _pathName = PathName;
            _textInput = TextInput;
        }

        public TextDocument(string PathName, string CurrentText)
        {
            _pathName = PathName;
            _currentText = CurrentText;
        }

        public TextDocument(TextBox TextInput)
        {
            _textInput = TextInput;
        }

        public TextDocument(string CurrentText)
        {
            _currentText = CurrentText;
        }

        public void OpenText()
        {
            TextInput.Text = File.ReadAllText(PathName);
        }
        public void ReadText()
        {
            _currentText = File.ReadAllText(PathName);
        }

        public void SaveText()
        {
            File.WriteAllText(PathName, TextInput.Text);
        }

        public void NewTextFile()
        {
            TextInput.Text = string.Empty;
        }

        public string PathName
        {
            get { return _pathName; }
            set { _pathName = value; }
        }

        public TextBox TextInput
        {
            get { return _textInput; }
            set { _textInput = value; }
        }

        public string CurrentText
        {
            get { return _currentText; }
            set { _currentText = value; }
        }
    }
}
