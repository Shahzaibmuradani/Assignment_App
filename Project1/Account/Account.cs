using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Project1
{
    public class Account
    {
        private string pass;

        public string Password
        {
            get
            {
                return pass;
            }
            set
            {
                if(value.Equals(""))
                {
                    throw new Exception("Not a Valid Password");
                }

                else
                {
                    Regex five = new Regex(@".{5,50}");
                    Regex number = new Regex(@"(?=.*\d)");
                    Regex upper = new Regex(@"(?=.*[A-Z])");
                    Regex special = new Regex(@"(?=.*\W)");

                    if(!five.IsMatch(value))
                    {
                        throw new Exception("Atleast 5 characters"); 
                    }
                    else if (!number.IsMatch(value))
                    {
                        throw new Exception("One Digit");
                    }
                    else if (!upper.IsMatch(value))
                    {
                        throw new Exception("One Uppercase character");
                    }
                    else if (!special.IsMatch(value))
                    {
                        throw new Exception("One Special symbol");
                    }

                    else
                    {
                       pass = value;
                    }
                }
            }
        }

        public int Admin_id { get; set; }
        public int Fac_id { get; set; }
        public int Stu_id { get; set; }
        public int Pro_id { get; set; }

    }
}
