using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class Assignment_submit
    {
        private double marks;
        private string path;
        private DateTime date;
        

        public double Marks
        {
            get
            {
                return marks;
            }
            set
            {
                if (value >= 0 && value <= 10.1)
                {
                    marks = value;
                }

                else
                {
                    throw new Exception("Marks should be between 0 till 10");
                }
            }
        }

        public string Answer
        {
            get
            {
                return path;
            }
            set
            {
                if (value.Equals(""))
                {
                    throw new Exception("Invalid path");
                }
                else
                {
                    path = value;
                }
            }
        }

        public DateTime Date {
            get
            {
                return date;
            }
            set
            {
                value = DateTime.Now;
                date = value;
            }
        }

        public int ID { get; set; }
        public int Fac_id { get; set; }
        
    }
}
