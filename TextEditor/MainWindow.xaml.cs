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
        TextDocument textDocument;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenAction(object sender, RoutedEventArgs e)
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
                textDocument.OpenText();
            }
            else
            {
                return;
            }
            
        }

        private void SaveAsAction(object sender, RoutedEventArgs e)
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

        private void NewFileAction(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "text files (*.txt)|*.txt";
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.CheckFileExists = true;
            saveFileDialog.AddExtension = true;
            saveFileDialog.FileName = "untitled";
            saveFileDialog.Title = "Save As";
            textDocument = new TextDocument(textBox);
            if (!string.IsNullOrEmpty(textDocument.TextInput.Text))
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

        private void SaveFileAction(object sender, RoutedEventArgs e) 
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "text files (*.txt)|*.txt";
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.CheckFileExists = true;
            saveFileDialog.AddExtension = true;
            saveFileDialog.FileName = "untitled";
            saveFileDialog.Title = "Please Select Your File To Save Over";
            textDocument = new TextDocument(textBox);           
            if (!string.IsNullOrEmpty(textDocument.TextInput.Text))
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
            return;
        }


        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            // this.OnClosing
            this.Close();

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
            MessageBox.Show("Closing called");
         //   if (this.isDataDirty)
          //  {
                MessageBoxResult result =
                  MessageBox.Show("save", "Data App", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    // If user doesn't want to close, cancel closure
                    e.Cancel = true;
                }
           // }
        }
    }
}
