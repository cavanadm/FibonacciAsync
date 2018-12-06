//Program By : Dylan Cavanaugh
//Working with the Fibonacci Sequence

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class AsyncForm : Form
    {
        public AsyncForm()
        {
            InitializeComponent();
        }

        private async void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                //Make sure the input is within bounds
                if (Convert.ToInt32(inputTextBox.Text) > 0 && Convert.ToInt32(inputTextBox.Text) < 100)
                {
                    outputTextbox.Text =
                       $"Starting Task to calculate Fibonacci({inputTextBox.Text})\r\n";

                    //Create Task to perform Fibonacci(x+1) calculation in a thread
                    Task<TimeData> task1 = Task.Run(() => StartFibonacci(Convert.ToInt32(inputTextBox.Text)));


                    outputTextbox.AppendText(
                        $"Starting to calculate Fibonacci({(Convert.ToInt32(inputTextBox.Text) + 1)})\r\n");

                    Task<TimeData> task2 = Task.Run(() => StartFibonacci(Convert.ToInt32(inputTextBox.Text) + 1));

                    //create Task to perform Fibonacci(x+2) calculation in a thread
                    outputTextbox.AppendText(
                        $"Starting to calculate Fibonacci({(Convert.ToInt32(inputTextBox.Text) + 2)})\r\n");

                    Task<TimeData> task3 = Task.Run(() => StartFibonacci(Convert.ToInt32(inputTextBox.Text) + 2));



                    TimeData[] times = await Task.WhenAll(task1, task2, task3); //await for all to complete
                    double totalMins = 0;

                    //Using Array of TimeData from tasks, calculate the total time for three tasks
                    foreach (var time in times)
                    {
                        totalMins += (time.EndTime - time.StartTime).TotalMinutes;
                    }
                    outputTextbox.AppendText(
                        $"Total calculation time = {totalMins:F6} minutes\r\n");
                }
                //If input is above 100 or below 1, output MessageBox
                else
                {
                    MessageBox.Show("Number Out Of Bounds, Try Again");
                }
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Please Enter A Number");
            }



            //starts a call to Fibonacci and captures start/end times
            TimeData StartFibonacci(int n)
            {
                //create a TimeData object to store start/end time
                var result = new TimeData();
                

                AppendText($"Calculating Fibonacci({n})");
                result.StartTime = DateTime.Now;
                long fibonacciValue = Fibonacci(n);
                result.EndTime = DateTime.Now;

                //Output the Fib number and calculated time for each
                AppendText($"Fibonacci({n}) = {fibonacciValue} \r\n Calculation time = " +
                    $"{(result.EndTime - result.StartTime).TotalMinutes:F6} minutes");

                return result;

            }
        }
        //Recursively calculates Fibonacci numbers
        public long Fibonacci(long n)
        {
            if (n == 0 || n == 1)
            {
                return n;
            }
            else
            {
                return Fibonacci(n - 1) + Fibonacci(n - 2);
            }
        }

        //append text to outputTextBox in UI thread
        public void AppendText(String text)
        {
            if(InvokeRequired)//not GUI thread, so add to GUI thread
            {
                Invoke(new MethodInvoker(() => AppendText(text)));
            }
            else //GUI thread so append text
            {
                outputTextbox.AppendText(text + "\r\n");
            }
        }
    }
}
