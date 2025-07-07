using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mint.Controls
{
    public class ComboBoxItemWithTag
    {
        public string Text { get; set; }
        public string Tag { get; set; }

        public ComboBoxItemWithTag(string text, string tag)
        {
            Text = text;
            Tag = tag;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
