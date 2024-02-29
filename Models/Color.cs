using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jido.Models
{
    public class Color
    {
        public byte[] RGB { get; set; } = [0, 0, 0];

        public string Name { get; set; } = "Default";
    }
}
