using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Project1
{
     public class Faculty
    {
        private string fac_name;
        private string fac_qualification;
        private string fac_email;
        private DateTime fac_date;
        private string fac_des;
        private string fac_gen;
        private int fac_salary;
        private byte [] img;

        public int ID { get; set; }

        public string Name
        {
            get
            {
                return fac_name;
            }
            set
            {
                if (value.Equals(""))
                {
                    throw new Exception("Name cannot be Empty");
                }
                else if (value.Length<3)
                {
                    throw new Exception("Length of Name should be of 4 characters");
                }
                else
                {
                    fac_name = value;
                }
            }
        }

        public string Qualification
        {
            get
            {
                return fac_qualification;
            }
            set
            {
                if (value.Equals(""))
                {
                    throw new Exception("Kindly select any qualification");
                }
                else
                {
                    fac_qualification = value;
                }
            }
        }

        public string Gender
        {
            get
            {
                return fac_gen;
            }
            set
            {
                if (value.Equals(""))
                {
                    throw new Exception("Kindly select any gender");
                }
                else
                {
                    fac_gen = value;
                }
            }
        }

        public string Email
        {
            get
            {
                return fac_email;
            }
            set
            {
                Regex email = new Regex(@"^[a-z][\w\.-]{2,28}[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
                if (value.Equals(""))
                {
                    throw new Exception("Email field is Empty");
                }
                else if (!email.IsMatch(value))
                {
                    throw new Exception("Invalid Email");

                }
                fac_email = value;
            }
        }

        public DateTime Date
        {
            get
            {
                return fac_date;
            }
            set
            {
                if (((DateTime.Now.Year - value.Year) > 23) && ((DateTime.Now.Year - value.Year) < 55))
                {
                    fac_date = value;
                }
                else
                {
                    throw new Exception("Teacher's Age should be greater than 23");
                }
            }
        }

        public int Age
        {
            get
            {
                return DateTime.Now.Year - this.fac_date.Year;
            }
        }

        public int Course { get; set; }
        public int Program { get; set; }

        public string Designation
        {
            get
            {
                return fac_des;
            }
            set
            {
                if (value.Equals(""))
                {
                    throw new Exception("Designation cannot be Empty");
                }
                else
                {
                    fac_des = value;
                }
            }
        }

        public int Salary
        {
            get
            {
                return fac_salary;
            }
            set
            {
                if (value.Equals("") || value<=50000)
                {
                    throw new Exception("Salary cannot be less than 50000");
                }
                else
                {
                    fac_salary = value;
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
    }
}
