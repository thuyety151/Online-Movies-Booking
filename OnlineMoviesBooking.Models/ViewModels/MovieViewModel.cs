﻿using Microsoft.AspNetCore.Http;
using OnlineMoviesBooking.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineMoviesBooking.Models.ViewModels
{
    public class MovieViewModel
    {
        public MovieViewModel(Movie movie)
        {
            this.Id = movie.Id;
            this.Name = movie.Name;
            this.Genre = movie.Genre;
            this.Director = movie.Director;
            this.Casts = movie.Casts;
            this.Rated = movie.Rated;
            this.Description = movie.Description;
            this.Trailer = movie.Trailer;
            this.ReleaseDate = movie.ReleaseDate;
            this.RunningTime = movie.RunningTime;
            this.Poster = movie.Poster;
        }
        public string Id { get; set; }
        [Display(Name="Tên phim")]
        [Required]
        public string Name { get; set; }
        [Display(Name ="Thể loại")]
        [Required]
        public string Genre { get; set; }
        [Display(Name = "Đạo diễn")]
        [Required]
        public string Director { get; set; }
        [Display(Name = "Diễn viên")]
        [Required]
        public string Casts { get; set; }
        [Display(Name = "Qui định")]
        public string Rated { get; set; }
        [Display(Name = "Mô tả")]
        [Required]
        public string Description { get; set; }
        [Display(Name = "Link trailer")]
        [Required]
        public string Trailer { get; set; }
        [Display(Name = "Ngày khởi chiếu")]
        [Required]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Thời lượng")]
        [Required]
        public int RunningTime { get; set; }
        [Display(Name = "Poster")]
        public string Poster { get; set; }

        // property tren code
        public IFormFile ImageFile { get; set; }
        public virtual ICollection<Show> Show { get; set; }
    }
}