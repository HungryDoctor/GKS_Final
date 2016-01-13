using GKS;
using GKS.GKS.Additions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace GKS6
{
    /// <summary>
    /// Interaction logic for Page0.xaml
    /// </summary>
    public partial class Page0 : Page
    {
        public event PropertyChangedEventHandler MainDataCreated;
        public event PropertyChangedEventHandler MainDataLoaded;

        private string text;

        private MainData mainData;
        private bool loaded;

        public MainData MainData { get { return mainData; } private set { mainData = value; OnMainDataCreated(); } }
        public bool IsMainDataLoaded { get { return loaded; } private set { loaded = value; if (loaded == true) OnMainDataLoaded(); } }


        public Page0()
        {
            InitializeComponent();

            IsMainDataLoaded = false;
            button_Continue.IsEnabled = false;
        }



        private void button_Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Text Files|*.txt";
            fileDialog.Multiselect = false;
            if (fileDialog.ShowDialog() == true)
            {
                Dictionary<int, string> initialStrings = IO.ReadStringsFromFile(fileDialog.FileName);

                textBox_Input.Clear();
                textBox_Input.Text = "";
                foreach (var item in initialStrings)
                {
                    textBox_Input.Text += item.Value + Environment.NewLine;
                }
            }
        }

        private void button_Continue_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<int, string> initialStrings = new Dictionary<int, string>();
            foreach (var item in textBox_Input.Text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
            {
                initialStrings.Add(initialStrings.Count, item.Trim());
            }

            if (textBox_Input.Text != text)
            {
                MainData = new MainData(initialStrings);

                IsMainDataLoaded = true;

                text = textBox_Input.Text;

                button_Continue.IsEnabled = false;
            }
        }


        private void OnMainDataCreated([CallerMemberName] String propertyName = "")
        {
            if (MainDataCreated != null)
            {
                MainDataCreated(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnMainDataLoaded([CallerMemberName] String propertyName = "")
        {
            if (MainDataLoaded != null)
            {
                MainDataLoaded(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void textBox_Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox_Input.Text != text && String.IsNullOrWhiteSpace(textBox_Input.Text) == false)
            {
                button_Continue.IsEnabled = true;
            }
            else
            {
                button_Continue.IsEnabled = false;
            }
        }

    }
}
