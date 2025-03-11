using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.Base
{
    public class CustomException
    {
        public DateTime ExceptionDate { get; set; }

        public int CurrentUserId { get; set; }

        public string Description { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }

        /// <summary>
        /// create a new instance of CustomException class
        /// </summary>
        /// <param name="currentUserId">code of current logged in user</param>
        /// <param name="description">description</param>
        /// <param name="e">exception occured</param>

        public CustomException(int currentUserId, string description, Exception e)
        {
            ExceptionDate = DateTime.Now;
            CurrentUserId = currentUserId;
            Description = string.Format("Description = {0}, Source = {1}, InnerException = {2}", description == null ? string.Empty : description, e.Source == null ? string.Empty : e.Source, e.InnerException == null ? string.Empty : e.InnerException.Message);// description;
            StackTrace = e.StackTrace;
            Message = e.Message;
        }

    }
}
