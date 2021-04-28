using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SarojWebApp.Models
{
    public class UserNote
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }
    }
}
