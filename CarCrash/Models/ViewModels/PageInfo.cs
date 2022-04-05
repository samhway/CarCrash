using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarCrash.Models
{
    public class PageInfo  //I was wrong, a model that is specific to a view is called a ViewModel
    {
        public int TotalNumCrashes { get; set; }
        public int CrashesPerPage { get; set; }
        public int CurrentPage { get; set; }
        //Figures out how many pages there will be as it is accessed
        public int TotalPages => (int)Math.Ceiling((double)TotalNumCrashes / CrashesPerPage); //When you put the type inside parentheses in front of it, you are casting it as that variable type
    }
}
