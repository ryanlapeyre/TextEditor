using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary> http://www.wpf-tutorial.com/common-interface-controls/menu-control/
    public partial class MainWindow : Window
    {
        TextDocument textDocument = null;
        TextDocument loadedTextDocument = null;

        //http://www.wpf-tutorial.com/tabcontrol/using-the-tabcontrol/
        //http://www.wpf-tutorial.com/dialogs/the-messagebox/
        //https://msdn.microsoft.com/en-us/library/system.windows.messageboximage.aspx
        //https://msdn.microsoft.com/en-us/library/system.windows.messagebox(v=vs.110).aspx

        //do a check for input file name
        //fix modularity of methods, DRY
        //figure out the noal coalescene operating you dunce

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenAction(object sender, RoutedEventArgs e) //do a check if text is in the textbox for saving?
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "text files (*.txt)|*.txt";
            openFileDialog.AddExtension = true;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Title = "Please Select Your File";
            openFileDialog.ShowDialog();

            if (!string.IsNullOrEmpty(openFileDialog.FileName))
            {
                textDocument = new TextDocument(openFileDialog.FileName, textBox);
                loadedTextDocument = new TextDocument(openFileDialog.FileName, textBox);
                textDocument.OpenText();
                loadedTextDocument.ReadText();
            }
            else
            {
                return;
            }
        }

        private void SaveAsAction(object sender, RoutedEventArgs e) // input the filename if doc is already created
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Documents (*.txt)|*.txt";
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.AddExtension = true;
            saveFileDialog.Title = "Save As";
            saveFileDialog.ShowDialog();
            if (!string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                textDocument = new TextDocument(saveFileDialog.FileName, textBox);
                textDocument.SaveText();
            }
            else
            {
                return;    
            }
        }

        private void SaveFileAction(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "text files (*.txt)|*.txt";
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.AddExtension = true;
            if(textDocument != null)
            {
                saveFileDialog.FileName = textDocument.PathName;
            }
            else
            {
                saveFileDialog.FileName = "untitled";
            }
            saveFileDialog.Title = "Please Select Your File To Save Over";
            textDocument = new TextDocument(saveFileDialog.FileName, textBox);
            MessageBox.Show(saveFileDialog.FileName);
            if (textDocument.PathName == "untitled")
            {
                saveFileDialog.ShowDialog();
                if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                {
                    textDocument = new TextDocument(saveFileDialog.FileName, textBox);
                    textDocument.SaveText();
                }

            }
            textDocument.SaveText();
            return;
        }

        private void NewFileAction(object sender, RoutedEventArgs e)//check if the object exists first before using untitled
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "text files (*.txt)|*.txt";
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.CheckFileExists = true;
            saveFileDialog.AddExtension = true;
            saveFileDialog.FileName = "untitled";
            saveFileDialog.Title = "Save As";
            textDocument = new TextDocument(textBox);
            if(loadedTextDocument == null)
            {
                textDocument.NewTextFile();
                return;
            }
            if (System.String.Compare(textDocument.TextInput.Text, loadedTextDocument.CurrentText) != 0)
            {
                MessageBox.Show("Please think about saving your data before creating a new document!");
                saveFileDialog.ShowDialog();
                if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                {
                    textDocument = new TextDocument(saveFileDialog.FileName, textBox);
                    textDocument.SaveText();
                }
                else
                {
                    textDocument.NewTextFile();
                    return;
                }
            }
            textDocument.NewTextFile();
        }  

 /*       private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
        */
        private void CloseCommandHandler(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void MaximizedScreenHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                return;
            }
            this.WindowState = WindowState.Normal;
            return;
        }
        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox.Text) && loadedTextDocument == null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text Documents (*.txt)|*.txt";
                saveFileDialog.CheckPathExists = true;
                saveFileDialog.AddExtension = true;
                saveFileDialog.Title = "Save As";
                saveFileDialog.ShowDialog();
                if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                {
                    textDocument = new TextDocument(saveFileDialog.FileName, textBox);
                    textDocument.SaveText();
                }
                else
                {
                    return;
                }

            }
            if (textDocument == null || loadedTextDocument == null)
            {
                return;
            }
            if (System.String.Compare(textDocument.TextInput.Text, loadedTextDocument.CurrentText) != 0)
            {
                MessageBoxResult result =
                  MessageBox.Show("Do you wish to save your progress?", "Save Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Text Documents (*.txt)|*.txt";
                    saveFileDialog.CheckPathExists = true;
                    saveFileDialog.AddExtension = true;
                    saveFileDialog.Title = "Save As";
                    saveFileDialog.ShowDialog();
                    if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                    {
                        textDocument = new TextDocument(saveFileDialog.FileName, textBox);
                        textDocument.SaveText();
                    }
                    else
                    {
                        return;
                    }
                }
                return;
            }
        }
    }
}
