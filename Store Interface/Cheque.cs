using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Store_Interface
{
    class Cheque
    {
        public void Print(List<StoreItem> shoppingcart, double total)
        {
            foreach (var item in shoppingcart)
            {
                total += (item as StoreItem).Price;
                File.WriteAllText("Cheque.txt", "Your total is: " + total.ToString() + " euro");
                File.AppendAllText("Cheque.txt", Environment.NewLine);
            }
            Process.Start("Cheque.txt");
        }
    }
}
