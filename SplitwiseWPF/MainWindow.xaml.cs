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
using Microsoft.Win32;

namespace SplitwiseWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        List<Friend> friends = new List<Friend>();
        List<String> output = new List<string>();
        private void inputCsvFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "CSV Files (*.csv)|*.csv";

            if (fileDialog.ShowDialog() == true)
            {
                string[] csvLines = System.IO.File.ReadAllLines(fileDialog.FileName);

                Calculate(csvLines, friends, output);

                displayOutput.ItemsSource = output;

            }
            else
            {
                MessageBox.Show("Error : Please select csv file");
            }
        }

        private static void Calculate(string[] csvLines, List<Friend> friends, List<string> output)
        {
            int count = 0;

            for (int i = 1; i < csvLines.Length; i++)
            {
                Friend fd = new Friend(csvLines[i]);
                if (fd.name.Length == 0)
                {
                    count++;
                }
                friends.Add(fd);
            }
            count++;
            float[] totalAmountPerGroup = new float[count];
            int groupCount = 0;
            int j = 0;
            float amountPerGroup = 0;
            while (j < friends.Count)
            {
                if (friends[j].name.Length == 0)
                {
                    totalAmountPerGroup[groupCount] = amountPerGroup;
                    amountPerGroup = 0;
                    groupCount++;
                }
                else
                {
                    amountPerGroup += friends[j].spent;
                }
                j++;
            }
            totalAmountPerGroup[groupCount] = amountPerGroup;

            groupCount = 0;

            output.Add("Group 1 calculation");
            for (int i = 0; i < friends.Count; i++)
            {
                string op = "";
                if (friends[i].name.Length == 0)
                {
                    groupCount++;
                    op = "Group " + (groupCount+1).ToString() + " Calculation";
                }
                else
                {
                    float toBeSpent = (friends[i].share * totalAmountPerGroup[groupCount]) / 100;
                    float toBeRecieved = friends[i].spent - toBeSpent;
                    string recieveOrPay = "";
                    if (toBeRecieved < 0)
                    {
                        recieveOrPay = " should pay ";
                    }
                    else
                    {
                        recieveOrPay = " should recieve ";
                    }

                    op = friends[i].name + recieveOrPay + Math.Abs(toBeRecieved).ToString();
                }
                output.Add(op);
            }
        }
    }
}
