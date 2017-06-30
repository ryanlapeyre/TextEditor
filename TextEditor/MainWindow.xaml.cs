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
using System.IO; //only used to get filepath

namespace TextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary> http://www.wpf-tutorial.com/common-interface-controls/menu-control/
    public partial class MainWindow : Window
    {
        TextDocument textDocument = null;
        TextDocument loadedTextDocument = null;
        const string APP_TITLE = "TextPad";
        const string DEFAULT_FILENAME = "untitled";
        const string OPEN_ACTION_NAME = "Open";
        const string SAVE_AS_NAME = "Save As";
        const string FILE_FILTER = "Text Documents (*.txt)|*.txt";

        //http://www.wpf-tutorial.com/tabcontrol/using-the-tabcontrol/
        //http://www.wpf-tutorial.com/dialogs/the-messagebox/
        //https://msdn.microsoft.com/en-us/library/system.windows.messageboximage.aspx
        //https://msdn.microsoft.com/en-us/library/system.windows.messagebox(v=vs.110).aspx

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenAction(object sender, RoutedEventArgs e) //do a check if text is in the textbox for saving?
        {
            OpenFunction();
        }

        private void SaveAsAction(object sender, RoutedEventArgs e) // input the filename if doc is already created
        {
            SaveAsFunction();
        }

        private void SaveFileAction(object sender, RoutedEventArgs e)
        {
            SaveFunction();
        }

        private void NewFileAction(object sender, RoutedEventArgs e)//check if the object exists first before using untitled
        {
            NewFileFunction();
        }

        private void CloseCommandHandler(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MaximizedScreenHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
                return;
            }
            WindowState = WindowState.Normal;
            return;
        }

        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(textBox.Text))
            {
                return;
            }
            if (!string.IsNullOrWhiteSpace(textBox.Text) && loadedTextDocument == null)
            {
                SaveAsFunction();
                return;
            }
            if (textDocument == null && loadedTextDocument == null)
            {
                return;
            }
            if (System.String.Compare(textDocument.TextInput.Text, loadedTextDocument.CurrentText) != 0 && !string.IsNullOrEmpty(textDocument.TextInput.Text))
            {
                MessageBoxResult result =
                  MessageBox.Show("Do you wish to save your progress?", "Save Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    SaveAsFunction();
                }
                return;
            }
            return;
        }

        private void AboutMeAction(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("TextPad was made by Ryan Lapeyre for IT4400 SS2017. When not coding, Ryan is a nerd who plays videogames.", "AboutMe", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        private void OpenFunction()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = FILE_FILTER;
            openFileDialog.AddExtension = true;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Title = OPEN_ACTION_NAME;
            openFileDialog.ShowDialog();

            if (!string.IsNullOrEmpty(openFileDialog.FileName))
            {
                textDocument = new TextDocument(openFileDialog.FileName, textBox);
                loadedTextDocument = new TextDocument(openFileDialog.FileName, textBox);
                textDocument.OpenText();
                loadedTextDocument.ReadText();
                ChangeApplicationTitle(Path.GetFileNameWithoutExtension(textDocument.PathName));
            }
            else
            {
                return;
            }
        }

        private void ChangeApplicationTitle(string PathName)
        {
            if(string.IsNullOrEmpty(PathName))
            {
                PathName = DEFAULT_FILENAME;
            }
            Title = APP_TITLE + " - " + PathName;
            return;
        }

        private void SaveAsFunction()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = FILE_FILTER;
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.AddExtension = true;
            saveFileDialog.Title = SAVE_AS_NAME;
            if (textDocument == null)
            {
                saveFileDialog.FileName = DEFAULT_FILENAME;
            }
            else
            {
                saveFileDialog.FileName = Path.GetFileNameWithoutExtension(textDocument.PathName);
            }

            saveFileDialog.ShowDialog();
            if (!string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                textDocument = new TextDocument(saveFileDialog.FileName, textBox);
                textDocument.SaveText();
                ChangeApplicationTitle(Path.GetFileNameWithoutExtension(textDocument.PathName));
            }
            else
            {
                return;
            }
        }

        private void SaveFunction()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = FILE_FILTER;
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.AddExtension = true;
            if (textDocument != null)
            {
                saveFileDialog.FileName = textDocument.PathName ?? DEFAULT_FILENAME;
            }
            else
            {
                saveFileDialog.FileName = DEFAULT_FILENAME;
            }
            saveFileDialog.Title = SAVE_AS_NAME;
            textDocument = new TextDocument(saveFileDialog.FileName, textBox);
            if (textDocument.PathName == DEFAULT_FILENAME)
            {
                saveFileDialog.ShowDialog();
                if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                {
                    textDocument = new TextDocument(saveFileDialog.FileName, textBox);
                    textDocument.SaveText();
                    ChangeApplicationTitle(Path.GetFileNameWithoutExtension(textDocument.PathName));
                }
            }
            textDocument.SaveText();
            return;
        }
        
        private void NewFileFunction()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = FILE_FILTER;
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.CheckFileExists = true;
            saveFileDialog.AddExtension = true;
            saveFileDialog.FileName = DEFAULT_FILENAME;
            saveFileDialog.Title = SAVE_AS_NAME;
            textDocument = new TextDocument(textBox);
            if (loadedTextDocument == null)
            {
                textDocument.NewTextFile();
                ChangeApplicationTitle(string.Empty);
                return;
            }
            if (System.String.Compare(textDocument.TextInput.Text, loadedTextDocument.CurrentText) != 0 && !string.IsNullOrEmpty(textDocument.TextInput.Text))
            {
                MessageBox.Show("Please think about saving your data before creating a new document!");
                saveFileDialog.ShowDialog();
                if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                {
                    textDocument = new TextDocument(saveFileDialog.FileName, textBox);
                    textDocument.SaveText();
                    ChangeApplicationTitle(Path.GetFileNameWithoutExtension(textDocument.PathName));
                }
                else
                {
                    textDocument.NewTextFile();
                    ChangeApplicationTitle(string.Empty);
                    return;
                }
            }
            textDocument.NewTextFile();
            ChangeApplicationTitle(string.Empty);
        }
    }
}
