using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mint.Code;

namespace Mint.Code
{
    public static class Mods
    {
        public static DataTable ModLibrary = new DataTable();

        static Mods()
        {
            // Create the columns for the ModInfo table
            ModLibrary.Columns.Add("Name", typeof(string));
            ModLibrary.Columns.Add("FormName", typeof(string));
            ModLibrary.Columns.Add("Image", typeof(string));
            ModLibrary.Columns.Add("Label", typeof(string));

            // Add the rows to the ModInfo table
            ModLibrary.Rows.Add("HEXOA_ORDERS", "Orders", "HEXOA_ORDERS","Commandes");
            ModLibrary.Rows.Add("HEXOA_DATABASE", "Database", "HEXOA_DATABASE","Afficher la DB");
            ModLibrary.Rows.Add("HEXOA_ACCOUNTS", "Accounts", "HEXOA_ACCOUNTS","Utilisateurs");
            ModLibrary.Rows.Add("HEXOA_PRODUCTS", "Products", "HEXOA_PRODUCTS", "Produits");
        }


    }
}
