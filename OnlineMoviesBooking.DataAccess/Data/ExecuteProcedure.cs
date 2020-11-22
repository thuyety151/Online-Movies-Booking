using Microsoft.EntityFrameworkCore;
using OnlineMoviesBooking.Models.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Data;

namespace OnlineMoviesBooking.DataAccess.Data
{
    public class ExecuteProcedure
    {
        private readonly CinemaContext _context;

        public ExecuteProcedure()
        {
        }

        public ExecuteProcedure(CinemaContext context)
        {
            _context = context;                 // xem lại
        }
        //-------------------------------MOVIE
        public List<Movie> ExecuteMovieGetAll()
        {
            var obj = _context.Movie.FromSqlRaw($"SELECT * FROM Movie").ToList();
            return obj;

        }
        public Movie ExecuteMovieDetail(string id)
        {
            var obj = _context.Movie.FromSqlRaw("EXEC USP_GetDetailMovie @Id= "+ id).ToList();
            return obj[0];
        }
        public int CheckNameMovie(string name)
        {

            var sqlParam = new SqlParameter("@Name", name);
            var i = _context.Movie.FromSqlRaw("EXEC USP_CheckNameMovie @Name ", sqlParam).ToList();
            return i.Count();
        }
        public int ExecuteInsertMovie(Movie movie)
        {
            var sqlParam = new SqlParameter[]
             {
                new SqlParameter("@Id",movie.Id),
                new SqlParameter("@Name",movie.Name),
                new SqlParameter("@Genre",movie.Genre),
                new SqlParameter("@Director",movie.Director),
                new SqlParameter("@Casts",movie.Casts),
                new SqlParameter("@Rated",movie.Rated),
                new SqlParameter("@Description",movie.Description),
                new SqlParameter("@Trailer",movie.Trailer),
                new SqlParameter("@ReleaseDate",movie.ReleaseDate),
                new SqlParameter("@ExpirationDate",movie.ExpirationDate),
                new SqlParameter("@RunningTime",movie.RunningTime),
                new SqlParameter("@Poster",movie.Poster)
             };

            var i=_context.Database.ExecuteSqlCommand("EXEC USP_InsertMovie @Id, @Name, @Genre, @Director," +
                " @Casts , @Rated , @Description , @Trailer , @ReleaseDate , @ExpirationDate ," +
                "@RunningTime , @Poster", sqlParam);
            return i;
        }
        public int ExecuteDeleteMovie(string id)
        {
            var sqlParam = new SqlParameter("@Id", id);
            return _context.Database.ExecuteSqlCommand("EXEC USP_DeleteMovie @Id", sqlParam);
        }
        public int ExecuteUpdateMovie(Movie movie)
        {
            var sqlParam = new SqlParameter[]
            {
                new SqlParameter("@Id",movie.Id),
                new SqlParameter("@Name",movie.Name),
                new SqlParameter("@Genre",movie.Genre),
                new SqlParameter("@Director",movie.Director),
                new SqlParameter("@Casts",movie.Casts),
                new SqlParameter("@Rated",movie.Rated),
                new SqlParameter("@Description",movie.Description),
                new SqlParameter("@Trailer",movie.Trailer),
                new SqlParameter("@ReleaseDate",movie.ReleaseDate),
                new SqlParameter("@ExpirationDate",movie.ExpirationDate),
                new SqlParameter("@RunningTime",movie.RunningTime),
                new SqlParameter("@Poster",movie.Poster)
            };
            var i = _context.Database.ExecuteSqlCommand("EXEC USP_UpdateMovie @Id, @Name, @Genre, @Director," +
                " @Casts , @Rated , @Description , @Trailer , @ReleaseDate , @ExpirationDate ," +
                "@RunningTime , @Poster", sqlParam);
            return i;
        }
        //----------------------THEATER
        public List<Theater> ExecuteTheaterGetAll()
        {
            var x = _context.Theater.FromSqlRaw("EXEC USP_GetAllTheater").ToList();
            return x;
        }
        public int ExecuteInsertTheater(string id, string name, string address, string hotline)
        {
            var sqlParam = new SqlParameter[]
            {
                new SqlParameter("@Id",id),
                new SqlParameter("@Name",name),
                new SqlParameter("@Address",address),
                new SqlParameter("@Hotline",hotline)
            };
            return _context.Database.ExecuteSqlCommand("USP_InsertTheater @Id, @Name, @Address, @Hotline", sqlParam);
        }
        public int ExecuteDeleteTheater(string id)
        {
            var sqlParam = new SqlParameter("@Id", id);
            return _context.Database.ExecuteSqlCommand("USP_DeleteThreater @Id", sqlParam);
        }
        //-----------------------Screen
        public void ExecuteScreenGetAllwithTheater()
        {
             _context.Database.ExecuteSqlRaw("EXEC USP_GetAllScreenwithTheater");
        }
    }
}
