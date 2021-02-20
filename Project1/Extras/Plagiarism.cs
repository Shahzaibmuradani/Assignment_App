using Copyleaks.SDK.API;
using Copyleaks.SDK.API.Exceptions;
using Copyleaks.SDK.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project1
{
    class Plagiarism
    {
        public void Scan(string email, string apiKey)
        {
            Facmng mng = new Facmng();
            Accmng accmng = new Accmng();
            string path = @"E:\cscolor\result.txt";
            var file = File.CreateText(path);
            file.Close();
            StreamWriter sw = new StreamWriter(path);            
 
            CopyleaksCloud copyleaks = new CopyleaksCloud(eProduct.Businesses);
            CopyleaksProcess createdProcess;
            ProcessOptions scanOptions = new ProcessOptions();
            scanOptions.SandboxMode = true; // Sandbox mode --> Read more https://api.copyleaks.com/documentation/headers/sandbox
            ResultRecord[] results;
            try
            {
                #region Login to Copyleaks cloud

                //Console.Write("Login to Copyleaks cloud...");
                copyleaks.Login(email, apiKey);
                //Console.WriteLine("Done!");

                #endregion

                #region Checking account balance

                //Console.Write("Checking account balance...");
                uint creditsBalance = copyleaks.Credits;
                //Console.WriteLine("Done ({0} credits)!", creditsBalance);
                if (!scanOptions.SandboxMode && creditsBalance == 0)
                {
                    MessageBox.Show("ERROR: You do not have enough credits to complete this scan. Your balance is {0}).",Convert.ToString(creditsBalance));

                    Environment.Exit(2);
                }

                #endregion

                #region callbacks

                // add a URL address to get notified using callbacks once the scan results are ready. 
                //Read more https://api.copyleaks.com/documentation/headers/http-callback
                //scanOptions.HttpCallback = new Uri("http://callbackurl.com?pid={PID}");
                //scanOptions.InProgressResultsCallback = new Uri("http://callbackurl.com?pid={PID}");

                #endregion

                #region Submitting a new scan process to the server

                // Insert here the URL that you'd like to scan for plagiarism
                createdProcess = copyleaks.CreateByUrl(new Uri("http://cnn.com/"), scanOptions);

                // Insert here the file that you'd like to scan for plagiarism
                Addfac add = new Addfac();
                createdProcess = copyleaks.CreateByFile(new FileInfo(add.ansreturn()), scanOptions);
                

                //Console.WriteLine("Done (PID={0})!", createdProcess.PID);

                #endregion

                #region Waiting for server's process completion

                // Use this if you are not using callback
                sw.WriteLine("Scanning... ");
                ushort currentProgress;
                while (!createdProcess.IsCompleted(out currentProgress))
                {
                    sw.WriteLine(currentProgress + "%");
                    Thread.Sleep(5000);
                }
                sw.WriteLine("Done.");

                #endregion

                #region Processing finished. Getting results

                results = createdProcess.GetResults();
                if (results.Length == 0)
                {
                   sw.WriteLine("No results.");
                }
                else
                {
                   
                    for (int i = 0; i < results.Length; ++i)
                    {
                        if (results[i].URL != null)
                        {
                            sw.WriteLine("Url: {0}", results[i].URL);
                        }
                        sw.WriteLine("Information: {0} copied words ({1}%)", results[i].NumberOfCopiedWords, results[i].Percents);
                        sw.WriteLine("Comparison report: {0}", results[i].ComparisonReport);
                        //Console.WriteLine("Title: {0}", results[i].Title);
                        //Console.WriteLine("Introduction: {0}", results[i].Introduction);
                        ////Console.WriteLine("Embeded comparison: {0}", results[i].EmbededComparison);
                        //Console.ReadKey();
                    }
                }

                #endregion
            }
            catch (UnauthorizedAccessException)
            {
                sw.WriteLine("Failed!");
                sw.WriteLine("Authentication with the server failed!");
                sw.WriteLine("Possible reasons:");
                sw.WriteLine("* You did not log in to Copyleaks cloud");
                sw.WriteLine("* Your login token has expired");
                Console.ReadKey();
            }
            catch (CommandFailedException theError)
            {
                sw.WriteLine("Failed!");
                sw.WriteLine("*** Error {0}:", theError.CopyleaksErrorCode);
                sw.WriteLine("{0}", theError.Message);
                Console.ReadKey();
            }

            sw.Close();
        }

       
    }
}
