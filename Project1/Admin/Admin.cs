using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class Admin
    {
        private string name;
        private byte [] img;

        public int ID { get; set; }
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (value.Equals("") && value.Length<3)
                {
                    throw new Exception("Name should be atleast 3 characters");
                }
                else
                {
                    this.name = value;
                }
            }
        }
        public byte [] Image
        { 
            get
            {
                return this.img;
            }
            set
            {
                if (value.Equals(""))
                {
                    throw new Exception("Please upload image");
                }
                else
                {
                    this.img = value;
                }
            }
        }

        public string Email { get; set; }
    }
}
