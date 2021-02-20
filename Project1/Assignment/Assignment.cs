using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class Assignment
    {
        private DateTime due_date;
        public string assign_name;
        public string assign_question;
        

        public int ID { get; set; }
        public int Course_id  { get; set; }
        public string Previous { get; set; }

        public DateTime Due_date {
            get
            {
                return due_date;
            }
            set
            {
                if (DateTime.Today.Day < value.Day)
                {
                    this.due_date = value;
                }
                else
                {
                    throw new Exception("Due to should be greater than 1 days");
                }
            }
        }

        public string Name {
            get
            {
                return assign_name;
            }
            set
            {
                if(value.Equals(""))
                {
                    throw new Exception("Kindly Upload File");
                }
                else
                {
                    this.assign_name = value;
                }
            }
        }

        public string Question
        {
            get
            {
                return assign_question;
            }
            set
            {
                if (value.Equals(""))
                {
                    throw new Exception("Kindly Upload File");
                }
                else
                {
                    this.assign_question = value;
                }
            }
        }
    }
}
