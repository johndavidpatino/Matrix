using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixNotificationSevice
{
    class Mail
    {
        private string _To;
        private string _From;
        private string _Subject;
        private string _Body;
         

        public string To 
        {
            get
            {
                return this._To;
            }
            set
            {
                this._To = value;
            }
        }

        public string From
        {
            get
            {
                return this._From;
            }
            set
            {
                this._From = value;
            }
        }

        public string Subject
        {
            get
            {
                return this._Subject;
            }
            set
            {
                this._Subject = value;
            }
        }


        public string Body
        {
            get
            {
                return this._Body;
            }
            set
            {
                this._Body = value;
            }
        }




    }
}
