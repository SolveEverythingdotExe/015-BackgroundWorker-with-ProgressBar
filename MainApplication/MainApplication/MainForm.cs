using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainApplication
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            //set this first
            backgroundWorker.WorkerReportsProgress = true;
        }

        //first lets create a function to check if number is prime
        public bool IsPrime(int number)
        {
            bool isPrime = true;

            int limit = (int)Math.Sqrt(number);

            for (int i = 2; i <= limit; i++)
            {
                if (number % i == 0)
                    return false;
            }

            return isPrime;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            //lets run the thread
            backgroundWorker.RunWorkerAsync();

            //BUG FIX =======> disable the button to avoid user of clicking it again while the backgroundworker is still running
            btnFind.Enabled = false;
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //perform work

            int number = 2; //lets start on 2 since 1 is not prime
            int maxPrimeCount = 1000; //our target
            int currentPrimeCount = 0;
            int percentageOfWork = 0;

            while (true)
            {
                if (IsPrime(number))
                {
                    //if number is prime lets add our count
                    currentPrimeCount++;

                    //lets show on status label
                    statusLabel.Text = currentPrimeCount.ToString() + "th prime number: " + number.ToString();

                    //lets compute the amount of work done
                    float result = (float)currentPrimeCount / (float)maxPrimeCount;
                    percentageOfWork = (int)(result * 100);

                    //set the progress
                    backgroundWorker.ReportProgress(percentageOfWork);
                }

                //ENHANCEMENT =======> code transferred on the if condition for optimization purposes
                ////lets compute the amount of work done
                //float result = (float)currentPrimeCount / (float)maxPrimeCount;
                //percentageOfWork = (int)(result * 100);

                ////set the progress
                //backgroundWorker.ReportProgress(percentageOfWork);

                //exit the loop once we reached our target
                if (currentPrimeCount == maxPrimeCount)
                    break;

                number++;
            }
        }
        //It works

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //if task is complete show a message
            MessageBox.Show("Done!");
            btnFind.Enabled = true;
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //lets update the progress bar
            progressBar.Value = e.ProgressPercentage;
        }
    }
}
