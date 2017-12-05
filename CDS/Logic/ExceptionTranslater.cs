using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDS.Logic
{
    class ExceptionTranslater : Exception
    {

        private string _message;
        private string _stackTrace;
        private int _number;
        public ExceptionTranslater(Exception objEx)
        {
            string message = "";
            int exNum;
            if (objEx.GetType().ToString().Equals("System.Data.SqlClient.SqlException"))
            {
                exNum = ((System.Data.SqlClient.SqlException)objEx).Number;
                switch (exNum)
                {
                    case 17:
                        message = "Database server could not be reached.";
                        break;
                    case 18456:
                        message = "Invalid user name or password.";
                        break;
                    case 4060:
                        message = "Could not open connection with the database server.";
                        break;
                    case 208:
                        message = "Could not find the requested catalog table.";
                        break;
                    case 2627:
                    case 2601:
                        message = "Can not create duplicate records.";
                        break;
                    case 547:
                        message = "The record cannot be deleted/modified/created. Operation violates the data integrity.";
                        break;
                    case 201:
                        message = "Stored Procedure requires some parameters which were either not supplied or are invalid.";
                        break;
                    case 2812:
                        message = "Invalid procedure call.";
                        break;
                    case 8144:
                        message = "Stored Procedure has extra arguments specified.";
                        break;
                    case 8143:
                        message = "Duplicate parameters supplied while calling a procedure.";
                        break;
                    case 8145:
                        message = "Invalid parameter name supplied.";
                        break;
                    case 8114:
                        message = "Wrong data type supplied.";
                        break;
                    case 170:
                        message = "The query has an invalid syntax.";
                        break;
                    default:
                        message = objEx.Message;
                        break;
                }
                _number = exNum;
            }
            else
            {
                _number = -1;
                message = objEx.Message;
            }
            this._message = message;
            this.Source = objEx.Source;
            this._stackTrace = objEx.StackTrace;
            WriteToWindowsEvents(objEx);
        }
        public override string Message
        {
            get
            {
                return _message;
            }
        }
        public override string StackTrace
        {
            get
            {
                return _stackTrace;
            }
        }
        public int Number
        {
            get
            {
                return _number;
            }
        }

        private void WriteToWindowsEvents(System.Exception ex)
        {
            // Make sure the Eventlog Exists
            //if (EventLog.SourceExists("CCT"))
            //{
            //    // Create an EventLog instance and assign its source.
            //    EventLog myLog = new EventLog("CCT Log");
            //    myLog.Source = "CCT";

            //    // Write the error entry to the event log.    
            //    myLog.WriteEntry(ex.Message);
            //}
        }
    }// end of class
}