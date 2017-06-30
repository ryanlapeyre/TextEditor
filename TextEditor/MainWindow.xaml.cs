using Microsoft.Win32;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.IO; //only used to get filepath

namespace TextEditor
{
    /*  Ryan Lapeyre
     *  Assignment 4
     *  TextEditor 
     */
     //http://www.wpf-tutorial.com/tabcontrol/using-the-tabcontrol/ /\/ for next time


    public partial class MainWindow : Window
    {
        TextDocument textDocument = null;
        TextDocument backupTextDocument = null;
        const string APP_TITLE = "TextPad";
        const string DEFAULT_FILENAME = "untitled";
        const string OPEN_ACTION_NAME = "Open";
        const string SAVE_AS_NAME = "Save As";
        const string FILE_FILTER = "Text Documents (*.txt)|*.txt";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenAction(object sender, RoutedEventArgs e) 
        {
            OpenFunction();
        }

        private void SaveAsAction(object sender, RoutedEventArgs e) 
        {
            SaveAsFunction();
        }

        private void SaveFileAction(object sender, RoutedEventArgs e)
        {
            SaveFunction();
        }

        private void NewFileAction(object sender, RoutedEventArgs e)
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
            try
            {
                textDocument = new TextDocument(textBox);
                if (!string.IsNullOrWhiteSpace(textDocument.TextInput.Text) && backupTextDocument == null)
                {
                    SaveWarning();
                    return;
                }
                if (System.String.Compare(textDocument.TextInput.Text, backupTextDocument.CurrentText, false) != 0 && !string.IsNullOrEmpty(textDocument.TextInput.Text))
                {
                    SaveWarning();
                    return;
                }
                if (string.IsNullOrWhiteSpace(textDocument.TextInput.Text) && backupTextDocument == null)
                {
                    return;
                }
                if (backupTextDocument == null)
                {
                    return;
                }
            }
            catch(System.NullReferenceException) //whenever i would start a new document and try and close the app, null refs would happen. strange...
            {
                return;
            }
        }

        private void AboutMeAction(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("TextPad was made by Ryan Lapeyre for IT4400 SS2017. When not coding, Ryan is a nerd who plays videogames.", "AboutMe", MessageBoxButton.OK, MessageBoxImage.None);
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
            textDocument = new TextDocument(textBox);

            if (!string.IsNullOrWhiteSpace(textDocument.TextInput.Text) && backupTextDocument == null)
            {
                SaveWarning();
                openFileDialog.ShowDialog();
                if (!string.IsNullOrEmpty(openFileDialog.FileName))
                {
                    textDocument = new TextDocument(openFileDialog.FileName, textBox);
                    textDocument.OpenText();
                    backupTextDocument = new TextDocument(openFileDialog.FileName, textBox.Text);
                    backupTextDocument.ReadText();
                    ChangeApplicationTitle(Path.GetFileNameWithoutExtension(textDocument.PathName));
                    return;
                }
                else
                {
                    return;
                }
            }
            if(backupTextDocument == null)
            {
                openFileDialog.ShowDialog();
                if (!string.IsNullOrEmpty(openFileDialog.FileName))
                {
                    textDocument = new TextDocument(openFileDialog.FileName, textBox);
                    backupTextDocument = new TextDocument(openFileDialog.FileName, textBox.Text);
                    textDocument.OpenText();
                    backupTextDocument.ReadText();
                    ChangeApplicationTitle(Path.GetFileNameWithoutExtension(textDocument.PathName));
                    return;
                }
                else
                {
                    return;
                }
            }
            if (System.String.Compare(textDocument.TextInput.Text, backupTextDocument.CurrentText) != 0 && !string.IsNullOrEmpty(textDocument.TextInput.Text))
            {
                SaveWarning();
                openFileDialog.ShowDialog();
                if (!string.IsNullOrEmpty(openFileDialog.FileName))
                {
                    textDocument = new TextDocument(openFileDialog.FileName, textBox);
                    backupTextDocument = new TextDocument(openFileDialog.FileName, textBox.Text);
                    textDocument.OpenText();
                    backupTextDocument.ReadText();
                    ChangeApplicationTitle(Path.GetFileNameWithoutExtension(textDocument.PathName));
                    return;
                }
                else
                {
                    return;
                }
            }
            openFileDialog.ShowDialog();
            if (!string.IsNullOrEmpty(openFileDialog.FileName))
            {
                textDocument = new TextDocument(openFileDialog.FileName, textBox);
                backupTextDocument = new TextDocument(openFileDialog.FileName, textBox.Text);
                textDocument.OpenText();
                backupTextDocument.ReadText();
                ChangeApplicationTitle(Path.GetFileNameWithoutExtension(textDocument.PathName));
                return;
            }
            else
            {
                return;
            }
        }

        private void ChangeApplicationTitle(string PathName)
        {
            if (string.IsNullOrEmpty(PathName))
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
            if (textDocument == null || string.IsNullOrWhiteSpace(textDocument.PathName))
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
                backupTextDocument = new TextDocument(textBox.Text);
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
            backupTextDocument = new TextDocument(textBox.Text);
            if (textDocument.PathName == DEFAULT_FILENAME)
            {
                saveFileDialog.ShowDialog();
                if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                {
                    textDocument = new TextDocument(saveFileDialog.FileName, textBox);
                    backupTextDocument = new TextDocument(textBox.Text);
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
            if (backupTextDocument == null)
            {
               if(!string.IsNullOrWhiteSpace(textDocument.TextInput.Text))
                {
                    SaveWarning();
                }
                textDocument.NewTextFile();
                ChangeApplicationTitle(DEFAULT_FILENAME);
                return;
            }
            if (System.String.Compare(textDocument.TextInput.Text, backupTextDocument.CurrentText) != 0 && !string.IsNullOrEmpty(textDocument.TextInput.Text))
            {
                SaveWarning();
            }
            textDocument.NewTextFile();
            ChangeApplicationTitle(DEFAULT_FILENAME);
            return;
        }

        private void SaveWarning()
        {
            MessageBoxResult result = MessageBox.Show("Do you wish to save your progress?", "Save Warning", MessageBoxButton.YesNo, MessageBoxImage.None);
            if (result == MessageBoxResult.Yes)
            {
                SaveAsFunction();
            }
        }

    }
}
