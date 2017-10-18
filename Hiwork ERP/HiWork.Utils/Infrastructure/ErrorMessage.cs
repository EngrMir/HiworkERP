using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.Utils
{
    public class ErrorMessage
    {
        public const String FAULT_USER_ALREADY_LOGGED_IN = "User Already LoggedIn";
        public const String FAULT_DATABASE_OPERATION_FAILD = "Database operation failed";
        public const String FAULT_DATABASE_NOT_REACHABLE = "Database not reachable";
        public const String USER_NOT_FOUND = "No user account related to this e-mail address.";
        public const String ERROR_IN_DATA_ADDING = "Error on adding data for internal server error.";
        public const String ERROR_IN_DATA_EDITING = "Error on editing data for internal server error.";
        public const String ERROR_IN_DATA_DELETING = "Error on deleting data for CountryID used as reference key on another Table.";
    }
}
