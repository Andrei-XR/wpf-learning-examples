using System;
using System.Windows;
using System.IO;
using System.Windows.Documents;
using System.Windows.Markup;
using Microsoft.Win32;
using System.Xml;

namespace FindText
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadFile(object sender, RoutedEventArgs args)
        {
            var openFile = new OpenFileDialog()
            {
                Filter = "FlowDocument Files (*.xaml)|*.xaml|All Files(*.*)|*.*",
                InitialDirectory = "Content"
            };

            if (openFile.ShowDialog().HasValue)
            {
                FlowDocument content = null;

                try
                {

                    var xamlFile = openFile.OpenFile() as FileStream;

                    if (xamlFile == null) return;

                    content = (FlowDocument)XamlReader.Load(xamlFile);

                    if (content == null)
                        throw (new XamlParseException("O arquivo especificado não pode ser carregado como um FlowDocument."));
                }
                catch (XamlParseException e)
                {
                    var error = "There was a problem parsing the specified file:\n\n";
                    error += openFile.FileName;
                    error += "Exception details:\n\n";
                    error += e.Message;
                    MessageBox.Show(error);
                    return;
                }
                catch (Exception e)
                {
                    var error = "There was a problem loading the specified file:\n\n";
                    error += openFile.FileName;
                    error += "Exception details:\n\n";
                    error += e.Message;
                    MessageBox.Show(error);
                    return;
                }

                FlowDocRdr.Document = content;
            }
        }

        private void SaveFile(object sender, RoutedEventArgs args)
        {
            var saveFile = new SaveFileDialog();
            FileStream xamlFile = null;
            saveFile.Filter = "FlowDocument Files (*.xaml)|*.xaml|All Files (*.*)|*.*";
            if (saveFile.ShowDialog().HasValue)
            {
                try
                {
                    xamlFile = saveFile.OpenFile() as FileStream;
                }
                catch (Exception e)
                {
                    var error = "There was a opening the file:\n\n";
                    error += saveFile.FileName;
                    error += "\n\nException details:\n\n";
                    error += e.Message;
                    MessageBox.Show(error);
                    return;
                }
                if (xamlFile == null) return;
                XamlWriter.Save(FlowDocRdr.Document, xamlFile);
                xamlFile.Close();
            }
        }

        private void Clear(object sender, RoutedEventArgs args)
        {
            FlowDocRdr.Document = null;
        }

        private void Exit(object sender, RoutedEventArgs args)
        {
            Close();
        }
    }
}
