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

        public TextDocument(string PathName, TextBox TextInput)
        {
            _pathName = PathName;
            _textInput = TextInput;
        }

        public TextDocument(TextBox TextInput)
        {
            _textInput = TextInput;
        }

        public void OpenText()
        {
            TextInput.Text = File.ReadAllText(PathName);
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

    }
}
