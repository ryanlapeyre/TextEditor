using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
                TextDocument.ReadText(textBox, openFileDialog.FileName);
            }
            else
            {
                return;
            }
            
        }

        private void SaveAsAction(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "text files (*.txt)|*.txt";
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.AddExtension = true;
            saveFileDialog.Title = "Please Select Your File To Save Over";
            saveFileDialog.ShowDialog();
            if (!string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                TextDocument.SaveText(textBox, saveFileDialog.FileName);
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
            saveFileDialog.Title = "Please Select Your File To Save Over";

            if(!string.IsNullOrEmpty(textBox.Text))
            {
                MessageBox.Show("Please think about saving your data before creating a new document");
                saveFileDialog.ShowDialog();
                if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                {
                    TextDocument.SaveText(textBox, saveFileDialog.FileName);
                }
                else
                {
                    return;
                }

            }

            TextDocument.NewTextFile(textBox);


        }

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
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

    }
}
