using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineMoviesBooking.Models
{
    public class Question
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Phai nhap du lieu")]
        public string Email { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string IdAdmin { get; set; }

    }
}
