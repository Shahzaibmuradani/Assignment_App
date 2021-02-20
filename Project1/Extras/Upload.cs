using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class Upload
    {
        private string name;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value.Equals(""))
                {
                    throw new Exception("Please Select File");
                }
                else
                {
                    this.name = value;
                }
            }
        }
    }
}
