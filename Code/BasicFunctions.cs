using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mint.Code
{
    internal static class BasicFunctions
    {
        public static string ChoosePath(string sTitle, string sType = "Folder")
        {
            string sPath = "";
            if (sType == "Folder")
            {
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                fbd.Description = sTitle;
                fbd.ShowDialog();
                sPath = fbd.SelectedPath;
            }
            else if (sType == "File")
            {
                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                ofd.Title = sTitle;
                ofd.ShowDialog();
                sPath = ofd.FileName;
            }
            return sPath;
        }
        public static string ConvertDataTableToHtml(DataTable dataTable)
        {
            // Crée une chaîne pour stocker le contenu HTML
            string htmlTable = "<table border='1'><tr>";

            // Ajoute les en-têtes de colonne
            foreach (DataColumn column in dataTable.Columns)
            {
                htmlTable += "<th>" + column.ColumnName + "</th>";
            }

            htmlTable += "</tr>";

            // Ajoute les données de la DataTable
            foreach (DataRow row in dataTable.Rows)
            {
                htmlTable += "<tr>";

                foreach (DataColumn column in dataTable.Columns)
                {
                    htmlTable += "<td>" + row[column] + "</td>";
                }

                htmlTable += "</tr>";
            }

            // Termine la table HTML
            htmlTable += "</table>";

            return htmlTable;
        }
        public static int CountStringInString(string sSearched, string sToCount)
        {
            int iStart = 0;
            int iCount = 0;

            // Iterate as long as iStart is smaller than the length of sSearched
            while (iStart < sSearched.Length)
            {
                // Find the next occurrence of sToCount starting from iStart
                int index = sSearched.IndexOf(sToCount, iStart);

                // If no more occurrences are found, break out of the loop
                if (index == -1)
                {
                    break;
                }

                // Increment the count
                iCount++;

                // Move the starting index to the character after the last occurrence
                iStart = index + sToCount.Length;
            }

            return iCount;

        }
        public static bool IsNumeric(string value)
        {
            double num;
            return double.TryParse(value.Trim(), out num);
        }
        public static string ReplaceSpecialChars(string inputString)
        {
            string specialChars = "àáâäçèéêëìíîïòóôöùúûü";
            string normVariants = "aaaaceeeeiiiioooouuuu";

            // Créez un tableau de correspondance pour les caractères spéciaux et leurs variantes normales
            Dictionary<char, char> charMap = new Dictionary<char, char>();
            for (int i = 0; i < specialChars.Length; i++)
            {
                charMap[specialChars[i]] = normVariants[i];
            }

            // Créez un tableau de caractères pour stocker le résultat
            char[] resultChars = inputString.ToCharArray();

            // Parcourez chaque caractère dans la chaîne d'entrée
            for (int i = 0; i < resultChars.Length; i++)
            {
                char currentChar = resultChars[i];
                // Vérifiez si le caractère actuel est dans le tableau de correspondance
                if (charMap.ContainsKey(currentChar))
                {
                    // Si c'est le cas, remplacez-le par sa variante normale
                    resultChars[i] = charMap[currentChar];
                }
            }

            // Créez une nouvelle chaîne à partir du tableau de caractères modifié
            string resultString = new string(resultChars);
            return resultString;
        }

    }
}
