using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;


namespace Weather.Models
{
    public class Index
    {
        [Required(ErrorMessage = "Please, enter the city")]
        public string ResCityName { get; set; }      //название города для отправки
        public bool last { get; set; } = false;         
        public List<string> Cookie { get; set; } = new List<string>();  //прошлые города
        public List<SelectList> countries { get; set; } = new List<SelectList>();
    }
}