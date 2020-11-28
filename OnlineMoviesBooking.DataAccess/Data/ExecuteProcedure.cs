using Microsoft.EntityFrameworkCore;
using OnlineMoviesBooking.Models.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Data;
using OnlineMoviesBooking.Models.ViewModels;
using Microsoft.IdentityModel.Protocols;

namespace OnlineMoviesBooking.DataAccess.Data
{
    public class ExecuteProcedure
    {
        private string cs;
        private readonly CinemaContext _context;

        public ExecuteProcedure()
        {
             
        }

        public ExecuteProcedure(CinemaContext context)
        {
            _context = context;                 // xem lại
            cs = "Server=db.c1q99xmhvjrm.ap-southeast-1.rds.amazonaws.com,1433;Initial " +
               "Catalog=Cinema;MultipleActiveResultSets=true;User Id=admin;Password=thuyety12315?!";
        }
        //-------------------------------MOVIE
        public List<Movie> ExecuteMovieGetAll()
        {
            var obj = _context.Movie.FromSqlRaw($"SELECT * FROM Movie").ToList();
            return obj;

        }
        public Movie ExecuteMovieDetail(string id)
        {
            var sqlParam = new SqlParameter("@Id", id);
            var obj = _context.Movie.FromSqlRaw("EXEC USP_GetDetailMovie @Id",sqlParam).ToList();
            return obj[0];
        }
        public void ExecuteInsertMovie(Movie movie)
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
                new SqlParameter("@RunningTime",movie.RunningTime),
                new SqlParameter("@Poster",movie.Poster)
             };

            _context.Database.ExecuteSqlCommand("EXEC USP_InsertMovie @Id, @Name, @Genre, @Director," +
                " @Casts , @Rated , @Description , @Trailer , @ReleaseDate  ," +
                "@RunningTime , @Poster", sqlParam);
           
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
                new SqlParameter("@RunningTime",movie.RunningTime),
                new SqlParameter("@Poster",movie.Poster)
            };
            var i = _context.Database.ExecuteSqlRaw("EXEC USP_UpdateMovie @Id, @Name, @Genre, @Director," +
                " @Casts , @Rated , @Description , @Trailer , @ReleaseDate  ," +
                "@RunningTime , @Poster", sqlParam);
            return i;
        }
        public string ExecuteGetImageMovie(string id)
        {
            string cs = "Server=db.c1q99xmhvjrm.ap-southeast-1.rds.amazonaws.com,1433;Initial " +
               "Catalog=Cinema;MultipleActiveResultSets=true;User Id=admin;Password=thuyety12315?!";
            string pos = "";
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_GetImageMovie", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", id);
                SqlDataReader rdr = com.ExecuteReader();
                
                while (rdr.Read())
                {
                    pos = (rdr["Poster"]).ToString();

                }
                return pos;
            }
        }
        //----------------------THEATER
        public List<Theater> ExecuteTheaterGetAll()
        {
            var x = _context.Theater.FromSqlRaw("EXEC USP_GetAllTheater").ToList();
            return x;
        }
        public Theater ExecuteDetailTheater(string id)
        {
            var obj= _context.Theater.FromSqlRaw("EXEC USP_GetDetailTheater @Id", new SqlParameter("@Id", id)).ToList();
            return obj[0];
        }
        public string ExecuteInsertTheater(string id, string name, string address, string hotline)
        {
            string mess = "";
            var sqlParam = new SqlParameter[]
            {
                new SqlParameter("@Id",id),
                new SqlParameter("@Name",name),
                new SqlParameter("@Address",address),
                new SqlParameter("@Hotline",hotline)
            };
            try 
            {
                _context.Database.ExecuteSqlRaw("USP_InsertTheater @Id, @Name, @Address, @Hotline", sqlParam);

            }
            catch(SqlException s)
            {
                 mess = s.Message;
            }
            return mess;
        }
        public void ExecuteDeleteTheater(string id)
        {
            var sqlParam = new SqlParameter("@Id", id);
            _context.Database.ExecuteSqlRaw("USP_DeleteThreater @Id", sqlParam);
        }
        //-----------------------Screen
        public List<Screen_Theater> ExecuteScreenGetAllwithTheater()
        {
            /// Id, Name, Name Theater
            string cs = "Server=db.c1q99xmhvjrm.ap-southeast-1.rds.amazonaws.com,1433;Initial " +
                "Catalog=Cinema;MultipleActiveResultSets=true;User Id=admin;Password=thuyety12315?!";
            List<Screen_Theater> lst = new List<Screen_Theater>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_GetAllScreenwithTheater", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Screen_Theater
                    {
                        Id = (rdr["Id"]).ToString(),
                        Name = rdr["Name"].ToString(),
                        IdTheater=rdr["Id_Theater"].ToString(),
                        NameTheater = rdr["Theater"].ToString()
                    }) ;
                }
                return lst;
            }

        }
        public void ExecuteInsertScreen(Screen screen)
        {
            var sqlParam = new SqlParameter[]
            {
                new SqlParameter("@Id",screen.Id),
                new SqlParameter("@Name",screen.Name),
                new SqlParameter("@IdTheater",screen.IdTheater)
            };
            _context.Database.ExecuteSqlRaw("EXEC USP_InsertScreen @Id, @Name , @IdTheater ", sqlParam);
        }
        public int CheckNameScreen(string name, string id)
        {
            var sqlParam = new SqlParameter[]
            {
                new SqlParameter("@Name",name),
                new SqlParameter("@Id",id)
            };
            var obj = _context.Screen.FromSqlRaw("EXEC USP_CheckSreenName @Name , @Id", sqlParam).ToList();
            return obj.Count();
        }
        public Screen ExecuteGetDetailScreen_Theater(string id)
        {
            /// Id, Name, Name Theater
            string cs = "Server=db.c1q99xmhvjrm.ap-southeast-1.rds.amazonaws.com,1433;Initial " +
                "Catalog=Cinema;MultipleActiveResultSets=true;User Id=admin;Password=thuyety12315?!";
            List<Screen> lst = new List<Screen>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_GetDetailScreenwithTheater", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", id);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Screen
                    {
                        Id = (rdr["Id"]).ToString(),
                        Name = rdr["Name"].ToString(),
                        IdTheater = rdr["Id_Theater"].ToString()
                    });
                }
                return lst[0];
            }
        }
        public void ExecuteUpdateScreen(Screen screen)
        {
            var sqlParam = new SqlParameter[]
            {
                new SqlParameter("@Id",screen.Id),
                new SqlParameter("@Name",screen.Name),
                new SqlParameter("@Id_Theater",screen.IdTheater)
            };
            _context.Database.ExecuteSqlRaw("EXEC USP_UpdateScreen @Id ,@Name ,@Id_Theater", sqlParam);
        }
        public void ExecuteDeleteScreen(string id)
        {
            _context.Database.ExecuteSqlRaw("EXEC USP_DeleteScreen @Id", new SqlParameter("@Id", id));
        }
        public List<Screen_Theater> SearchScreenwithTheater(string id)
        {
            string cs = "Server=db.c1q99xmhvjrm.ap-southeast-1.rds.amazonaws.com,1433;Initial " +
                "Catalog=Cinema;MultipleActiveResultSets=true;User Id=admin;Password=thuyety12315?!";
            List<Screen_Theater> lst = new List<Screen_Theater>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_FindScreenwithTheater", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IdThreater", id);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Screen_Theater
                    {
                        Id = (rdr["Id"]).ToString(),
                        Name = rdr["Name"].ToString(),
                        IdTheater = rdr["Id_Theater"].ToString(),
                        NameTheater = rdr["Theater"].ToString()
                    });
                }
                return lst;
            }
        }
        //--------------------TypesOfSeat
        public List<TypesOfSeat> GetAllTypesOfSeat()
        {
            return _context.TypesOfSeat.FromSqlRaw("EXEC USP_GetAllTypesOfSeat").ToList();
        }
        public void ExecuteUpdateTypesOfSeat(TypesOfSeat s)
        {
            var sqlParam = new SqlParameter[]
            {
                new SqlParameter("@Id",s.Id),
                new SqlParameter("@Name",s.Name),
                new SqlParameter("@Cost",s.Cost)
            };
            _context.Database.ExecuteSqlRaw("EXEC USP_UpdateTypesOfSeat @Id, @Name, @Cost ", sqlParam);
        }
        public int CheckToSeatName(string id, string name)
        {
            var sqlParam = new SqlParameter[]
           {
                new SqlParameter("@Id",id),
                new SqlParameter("@Name",name)
           };
            var ls = _context.TypesOfSeat.FromSqlRaw("EXEC USP_CheckToSeatName @Id , @Name ", sqlParam).ToList();
            return ls.Count();
        }
        //------------------SHOW
        public List<ShowViewModel> ExecuteGetAllShow()
        {
            List<ShowViewModel> lstShow = new List<ShowViewModel>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_GetAllShowwithMovieandScreen", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataReader rdr = com.ExecuteReader();

                while (rdr.Read())
                {
                    lstShow.Add(new ShowViewModel { 
                        Id=rdr["Id"].ToString(),
                        Languages=rdr["Languages"].ToString(),
                        TimeStart= DateTime.Parse(rdr["TimeStart"].ToString()),
                        TimeEnd= DateTime.Parse(rdr["TimeEnd"].ToString()),
                        MovieName=rdr["MovieName"].ToString(),
                        Poster=rdr["Poster"].ToString(),
                        ScreenName=rdr["ScreenName"].ToString(),
                        TheaterName=rdr["TheaterName"].ToString()
                    });

                }
                return lstShow;
            }
        }
        public List<ShowViewModel> ExecuteGetAllShowTheater(string id)
        {
            List<ShowViewModel> lstShow = new List<ShowViewModel>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_GetAllShowTheater", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IdTheater", id);
                SqlDataReader rdr = com.ExecuteReader();

                while (rdr.Read())
                {
                    lstShow.Add(new ShowViewModel
                    {
                        Id = rdr["Id"].ToString(),
                        Languages = rdr["Languages"].ToString(),
                        TimeStart = DateTime.Parse(rdr["TimeStart"].ToString()),
                        TimeEnd = DateTime.Parse(rdr["TimeEnd"].ToString()),
                        MovieName = rdr["MovieName"].ToString(),
                        Poster = rdr["Poster"].ToString(),
                        ScreenName = rdr["ScreenName"].ToString(),
                        TheaterName = rdr["TheaterName"].ToString()
                    });

                }
                return lstShow;
            }
        }
        public string ExecuteInsertShow(Show show)
        {
            string result = "";
            var sqlParam = new SqlParameter[]
            {
                new SqlParameter("@Id",show.Id),
                new SqlParameter("@Languages",show.Languages),
                new SqlParameter("@TimeStart",show.TimeStart),
                //new SqlParameter("@TimeEnd",show.TimeEnd),
                new SqlParameter("@Id_Movie",show.IdMovie),
                new SqlParameter("@Id_Screen",show.IdScreen)
            };
            try
            {
                _context.Database.ExecuteSqlRaw("EXEC USP_InsertShow @Id, @Languages ,@TimeStart," +
                    " @Id_Movie,  @Id_Screen", sqlParam);
            }
            catch(SqlException s)
            {
                result = s.Message;
            }
            return result;
        }
        public ShowViewModel ExecuteGetDetailShow(string id)
        {
            ShowViewModel show = new ShowViewModel();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_GetDetailShowViewModel", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IdShow", id);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    show= new ShowViewModel
                    {
                        Id = rdr["Id"].ToString(),
                        Languages = rdr["Languages"].ToString(),
                        TimeStart = DateTime.Parse(rdr["TimeStart"].ToString()),
                        TimeEnd = DateTime.Parse(rdr["TimeEnd"].ToString()),
                        MovieName = rdr["MovieName"].ToString(),
                        Poster = rdr["Poster"].ToString(),
                        ScreenName = rdr["ScreenName"].ToString(),
                        TheaterName = rdr["TheaterName"].ToString()
                    };
                }
                return show;
            }
        }
        public ShowViewModel ExecuteGetDetailShowEdit(string id)
        {
            ShowViewModel show = new ShowViewModel();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_GetDetailShowViewModel", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IdShow", id);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    show = new ShowViewModel
                    {
                        Id = rdr["Id"].ToString(),
                        Languages = rdr["Languages"].ToString(),
                        TimeStart = DateTime.Parse(rdr["TimeStart"].ToString()),
                        TimeEnd = DateTime.Parse(rdr["TimeEnd"].ToString()),
                        MovieName = rdr["MovieName"].ToString(),
                        ScreenName = rdr["ScreenName"].ToString(),
                        TheaterName = rdr["TheaterName"].ToString(),
                        IdMovie = rdr["Id_Movie"].ToString(),
                        IdScreen = rdr["Id_Screen"].ToString(),
                        IdTheater=rdr["IdTheater"].ToString()
                    };
                }
                return show;
            }
        }
        public string ExecuteUpdateShow(ShowViewModel show)
        {
            string result = "";
            var sqlParam = new SqlParameter[]
            {
                new SqlParameter("@Id",show.Id),
                new SqlParameter("@Languages",show.Languages),
                new SqlParameter("@TimeStart",show.TimeStart),
                //new SqlParameter("@TimeEnd",show.TimeEnd),
                new SqlParameter("@Id_Movie",show.IdMovie),
                new SqlParameter("@Id_Screen",show.IdScreen)
            };
            try
            {
                _context.Database.ExecuteSqlRaw("EXEC USP_UpdateShow @Id, @Languages ,@TimeStart," +
                    " @Id_Movie,  @Id_Screen", sqlParam);
            }
            catch (SqlException s)
            {
                result = s.Message;
            }
            return result;
        }
        public void ExecuteDeleteShow(string id)
        {
            _context.Database.ExecuteSqlRaw("EXEC USP_DeleteShow @IdShow", new SqlParameter("@IdShow", id));
        }
        //------------------DISCOUNT
        public List<Discount> ExecuteGetAllDiscount()
        {
            List<Discount> lstDiscount = new List<Discount>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_GetAllDiscount", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataReader rdr = com.ExecuteReader();

                while (rdr.Read())
                {
                    lstDiscount.Add(new Discount
                    {
                        Id = rdr["Id"].ToString(),
                        Name = rdr["Name"].ToString(),
                        Description = rdr["Description"].ToString(),
                        PercentDiscount = int.Parse(rdr["PercentDiscount"].ToString()),
                        MaxCost = int.Parse(rdr["MaxCost"].ToString()),
                        DateStart = DateTime.Parse(rdr["DateStart"].ToString()),
                        DateEnd = DateTime.Parse(rdr["DateEnd"].ToString()),
                        ImageDiscount = rdr["ImageDiscount"].ToString(),
                        //NoTicket = int.Parse(rdr["NoTicket"].ToString()),
                        //Point = int.Parse(rdr["Point"].ToString())
                    });

                }
                return lstDiscount;
            }
        }
    }
}
