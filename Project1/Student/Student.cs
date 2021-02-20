using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Project1
{
    class Student
    {
        private string name;
        private string stu_email;
        private string gender;
        private DateTime age;
        private byte[] img;

        public int ID { get; set; }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (value.Equals(""))
                {
                    throw new Exception("Name cannot be Empty");
                }
                else if (value.Length < 3)
                {
                    throw new Exception("Length of Name should be of 4 characters");
                }
                else
                {
                    this.name = value;
                }
            }
        }

        public string Email
        {
            get
            {
                return this.stu_email;
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
                this.stu_email = value;
            }
        }

        public string Gender
        {
            get
            {
                return this.gender;
            }
            set
            {
                if (value.Equals(""))
                {
                    throw new Exception("Kindly select any gender");
                }
                else
                {
                    this.gender = value;
                }
            }
        }

        public DateTime Age
        {
            get
            {
                return this.age;
            }
            set
            {
                if (((DateTime.Now.Year - value.Year) > 15) && ((DateTime.Now.Year - value.Year) < 60))
                {
                    this.age = value;
                }
                else
                {
                    throw new Exception("Student's Age should be greater than 15");
                }
            }
        }

        public int Semester { get; set; }
        public int Program { get; set; }

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
