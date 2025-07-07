using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing.Printing;

namespace Mint.Code
{
    public class Print
    {
        public delegate void StatusUpdateDelegate(string message);
        public static event StatusUpdateDelegate StatusUpdate;

        public void PrintAndDeletePDFsAsync(string folderPath)
        {
            try
            {
                // Vérifie si le dossier existe
                if (Directory.Exists(folderPath))
                {
                    // Récupère la liste des fichiers PDF dans le dossier
                    string[] pdfFiles = Directory.GetFiles(folderPath, "*.pdf");

                    if (pdfFiles.Length > 0)
                    {
                        int count = 1;
                        foreach (string pdfFile in pdfFiles)
                        {
                            StatusUpdate?.Invoke($"Impression des documents : {count}/{pdfFiles.Length}");

                            // Imprime le fichier PDF
                            if (PrintPDF(pdfFile) == true)
                            {
                                System.Threading.Thread.Sleep(1000);
                            }
                            // Supprime le fichier PDF après l'impression
                            File.Delete(pdfFile);
                            count++;
                        }
                        StatusUpdate?.Invoke(null);
                    }
                    else
                    {
                        MessageBox.Show("Aucun fichier PDF trouvé dans le dossier.");
                    }
                }
                else
                {
                    MessageBox.Show("Le dossier spécifié n'existe pas.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite : " + ex.Message);
            }
        }

       

        private bool PrintPDF(string pdfFilePath)
        {
            // Crée un processus pour imprimer le PDF
            ProcessStartInfo psi = new ProcessStartInfo
            {
                //Methode avec imprimeur par défaut (donc potentiellement Adobe Reader)
                FileName = pdfFilePath,
                Verb = "print",
                UseShellExecute = true,
                CreateNoWindow = true
            };

            Process printProcess = new Process { StartInfo = psi };
            printProcess.Start();
            //printProcess.WaitForExit();
            //printProcess.WaitForInputIdle();

            //try to close the main window until we get to no error
            int waitcount = 0;
            while (waitcount < 30)
            {
                try
                {
                    if (true == printProcess.CloseMainWindow())
                    {
                        printProcess.WaitForInputIdle();
                        printProcess.Kill();
                        System.Threading.Thread.Sleep(3000);
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show("Le processus d'impression n'a pas pu être fermé.");
                        System.Threading.Thread.Sleep(1000);
                        waitcount++;
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception (optional)
                    System.Threading.Thread.Sleep(1000);
                    MessageBox.Show(printProcess.MainWindowTitle.ToString() + " - reponse " + printProcess.Responding.ToString() + " - exception - " + ex.Message);
                    waitcount++;
                }
            }

            // Si nous avons atteint 30 tentatives sans succès, alors nous tuons le processus
            if (!printProcess.HasExited)
            {
                printProcess.Kill();
                System.Threading.Thread.Sleep(3000);
            }
            //Et on renvoie false pour dire que l'impression a échoué
            return false;
        }
    }
}

