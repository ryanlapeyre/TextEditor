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

        static public void ReadText(TextBox textBox, string PathName)
        {
            textBox.Text = File.ReadAllText(PathName);
          
        }

        static public void SaveText(TextBox textBox, string PathName)
        {
           File.WriteAllText(PathName, textBox.Text);
        }

        static public void NewTextFile(TextBox textBox)
        {
            textBox.Text = string.Empty;
        }
    }
}
