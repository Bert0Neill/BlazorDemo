using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWASMDemo.Shared.Models
{
    public class CustomExceptionResponse
    {
        public CustomExceptionResponse()
        {

        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Stack { get; set; }
    }
}
