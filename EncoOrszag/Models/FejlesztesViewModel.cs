using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EncoOrszag.Models
{
    public class FejlesztesViewModel
    {


        public int Fejleszt { get; set; }

        public List<FejlesztesFejlesztesListViewModel> Fejlesztesek { get; set; }


    }
    public class FejlesztesFejlesztesListViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Ido { get; set; }

        public int Hatravan { get; set; }

        public bool Kifejlesztve { get; set; }


    }

}