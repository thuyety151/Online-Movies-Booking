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
        
        public int GetCountMovieNow()
        {
            // to paging
            int pos = 0;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_GetNumOfMovieNow", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    pos = int.Parse((rdr["Num"]).ToString());
                }
                return pos;
            }
        }
        public int GetCountMovieComing()
        {
            // to paging
            int pos = 0;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_GetNumOfMovieComing", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataReader rdr = com.ExecuteReader();

                while (rdr.Read())
                {
                    pos = int.Parse((rdr["Num"]).ToString());
                }
                return pos;
            }
        }
        public List<Movie> ExecuteMovieGetAll()
        {
            var obj = _context.Movie.FromSqlRaw($"SELECT * FROM Movie").ToList();
            return obj;

        }
        public Movie ExecuteMovieDetail(string id)
        {
            try
            {
                var sqlParam = new SqlParameter("@Id", id);
                var obj = _context.Movie.FromSqlRaw("EXEC USP_GetDetailMovie @Id", sqlParam).ToList();
                return obj[0];
            }
            catch
            {
                return new Movie();
            }
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

            _context.Database.ExecuteSqlRaw("EXEC USP_InsertMovie @Id, @Name, @Genre, @Director," +
                " @Casts , @Rated , @Description , @Trailer , @ReleaseDate  ," +
                "@RunningTime , @Poster", sqlParam);
           
        }
        public int ExecuteDeleteMovie(string id)
        {
            var sqlParam = new SqlParameter("@Id", id);
            return _context.Database.ExecuteSqlRaw("EXEC USP_DeleteMovie @Id", sqlParam);
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
        public List<Movie> ExecuteGetMovieNow(int skip, int take)
        {
            return _context.Movie.FromSqlRaw("EXEC USP_PagingMovieNow @Skip= "+skip +", @Take= "+take).ToList();

        }
        public List<Movie> ExecuteGetMovieComingSoon(int skip, int take)
        {
            return _context.Movie.FromSqlRaw("EXEC USP_PagingMovieComing @Skip = "+skip +", @Take = "+take).ToList();
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
                 mess = s.Number.ToString();
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
        public string ExecuteInsertScreen(Screen screen)      // EDIT HERE AFTER USE TRANSACTION
        {
            string result = "";
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("dbo.USP_CreateSeatandScreen", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IdScreenIn", screen.Id);
                com.Parameters.AddWithValue("@NameIn", screen.Name);
                com.Parameters.AddWithValue("@IdTheaterIn", screen.IdTheater);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    result = (rdr["ErrorNumber"]).ToString();
                }
                return result;  // 3609 : trigger
            }

        }
        public string CheckNameScreen(string name, string id)
        {
            string result = "";
            var sqlParam = new SqlParameter[]
            {
                new SqlParameter("@Name",name),
                new SqlParameter("@Id",id)
            };
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_CheckSreenName", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Name", name);
                com.Parameters.AddWithValue("@Id_Theater", id);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    result = (rdr["Result"]).ToString();
                }
                return result;  // 3609 : trigger
            }
        }
        public ScreenViewModel ExecuteGetDetailScreen_Theater(string id)
        {
            /// Id, Name, Name Theater
            string cs = "Server=db.c1q99xmhvjrm.ap-southeast-1.rds.amazonaws.com,1433;Initial " +
                "Catalog=Cinema;MultipleActiveResultSets=true;User Id=admin;Password=thuyety12315?!";
            ScreenViewModel lst = new ScreenViewModel();
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
                    lst=new ScreenViewModel
                    {
                        Id = (rdr["Id"]).ToString(),
                        Name = rdr["Name"].ToString(),
                        IdTheater = rdr["Id_Theater"].ToString(),
                        NameTheater=rdr["Theater"].ToString(),
                        Address=(rdr["Address"]).ToString()
                    };
                }
                return lst;
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
        public string ExecuteDeleteScreen(string id)
        {
            string result = "";
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("dbo.USP_DeleteSeatandScreen", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IdScreenIn", id);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    result = (rdr["ErrorNumber"]).ToString();
                }
                return result;  // 3609 : trigger
            }
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
        public List<ShowViewModel> ExecuteGetAllShowMovie(string id)
        {
            List<ShowViewModel> lstShow = new List<ShowViewModel>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_GetAllShowMovie", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IdMovie", id);
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
        public List<ShowViewModel> ExecuteGetAllShowDate(DateTime date)
        {
            List<ShowViewModel> lstShow = new List<ShowViewModel>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_GetAllShowDate", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@DateStart", date);
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
                        IdMovie=rdr["Id_Movie"].ToString(),
                        IdScreen=rdr["Id_Screen"].ToString(),
                        IdTheater=rdr["IdTheater"].ToString(),
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
        public object ExecuteFindTheaterShow(string idmovie, string date)
        {
            List<object> theater= new List<object>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_FindTheaterofShow", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IdMovie", idmovie);
                com.Parameters.AddWithValue("@Date", date);
                com.Parameters.AddWithValue("@TimeNow", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    var times = ExecuteFindTimeofTheater(idmovie, date, rdr["Id_Theater"].ToString());

                    theater.Add( new
                    {
                        Id = rdr["Id_Theater"].ToString(),
                        Name = rdr["Name"].ToString(),
                        Times=times
                    });
                }
                return theater;
            }
        }
        public object ExecuteFindTimeofTheater(string idmovie, string date,string idtheater)
        {
            List<object> times = new List<object>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_FindTimeofTheater", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IdMovie", idmovie);
                com.Parameters.AddWithValue("@Date", date);
                com.Parameters.AddWithValue("@IdTheater", idtheater);
                com.Parameters.AddWithValue("@TimeNow", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                string s = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SqlDataReader rdr = com.ExecuteReader();
                try
                {
                    while (rdr.Read())
                    {
                        times.Add(new
                        {
                            Id = rdr["Id"].ToString(),
                            Times = DateTime.Parse(rdr["Time"].ToString()).ToShortTimeString()
                        });
                    }
                    return times;
                }
                catch(SqlException e)
                {
                    string ss = e.Message;
                }
                return times;
            }
        }

        //-------- front end
        public List<ShowViewModel> ExecuteGetShowByDate(string idMovie, string date)
        {
            List<ShowViewModel> lstShow = new List<ShowViewModel>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_GetShowByDate", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IdMovie", idMovie);
                com.Parameters.AddWithValue("@Date",date);
                SqlDataReader rdr = com.ExecuteReader();

                while (rdr.Read())
                {
                    lstShow.Add(new ShowViewModel
                    {
                        Id = rdr["Id"].ToString(),
                        Languages=rdr["Languages"].ToString(),
                        TimeStart=DateTime.Parse((rdr["TimeStart"]).ToString()),
                        TimeEnd=DateTime.Parse((rdr["TimeEnd"]).ToString()),
                        IdMovie=rdr["Id_Movie"].ToString(),
                        IdScreen=rdr["Id_Screen"].ToString(),

                    });

                }
                return lstShow;
            }
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
                        NoTicket = int.Parse(rdr["NoTicket"].ToString()),
                        Point = int.Parse(rdr["Point"].ToString()),
                        Used = int.Parse(rdr["Used"].ToString())
                    });

                }
                return lstDiscount;
            }
        }
        public Discount ExecuteGetDetailDiscount(string id)
        {
            Discount d = new Discount();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_GetDetailDiscount", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", id);
                SqlDataReader rdr = com.ExecuteReader();

                while (rdr.Read())
                {
                    d=new Discount
                    {
                        Id = rdr["Id"].ToString(),
                        Name = rdr["Name"].ToString(),
                        Description = rdr["Description"].ToString(),
                        PercentDiscount = int.Parse(rdr["PercentDiscount"].ToString()),
                        MaxCost = int.Parse(rdr["MaxCost"].ToString()),
                        DateStart = DateTime.Parse(rdr["DateStart"].ToString()),
                        DateEnd = DateTime.Parse(rdr["DateEnd"].ToString()),
                        ImageDiscount = rdr["ImageDiscount"].ToString(),
                        NoTicket = int.Parse(rdr["NoTicket"].ToString()),
                        Point = int.Parse(rdr["Point"].ToString()),
                        Used = int.Parse(rdr["Used"].ToString())
                    };

                }
                return d;
            }
        }
        public string ExecuteInsertDiscount(Discount discount)
        {
            string result = "";
            var sqlParam = new SqlParameter[]
            {
                new SqlParameter("@Id",discount.Id),
                new SqlParameter("@Name",discount.Name),
                new SqlParameter("@Description",discount.Description),
                new SqlParameter("@PercentDiscount",discount.PercentDiscount),
                new SqlParameter("@MaxCost",discount.MaxCost),
                new SqlParameter("@DateStart",discount.DateStart),
                new SqlParameter("@DateEnd",discount.DateEnd),
                new SqlParameter("@ImageDiscount",discount.ImageDiscount),
                new SqlParameter("@NoTicket",discount.NoTicket),
                new SqlParameter("@Point",discount.Point),
                new SqlParameter("@Used",discount.Used)
            };
            try
            {
                _context.Database.ExecuteSqlRaw("EXEC USP_InsertDiscount @Id, @Name ,@Description," +
                    " @PercentDiscount,  @MaxCost ,@DateStart,@DateEnd,@ImageDiscount,@NoTicket,@Point," +
                    "@Used  ", sqlParam);
            }
            catch (SqlException s)
            {
                result = s.Message;
            }
            return result;
        }
        public string ExecuteGetImageDiscount(string id)
        {
            string pos = "";
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_GetImageDiscount", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", id);
                SqlDataReader rdr = com.ExecuteReader();

                while (rdr.Read())
                {
                    pos = (rdr["ImageDiscount"]).ToString();

                }
                return pos;
            }
        }
        public string ExecuteUpdateDiscount(Discount discount)
        {
            string result = "";
            var sqlParam = new SqlParameter[]
            {
                new SqlParameter("@Id",discount.Id),
                new SqlParameter("@Name",discount.Name),
                new SqlParameter("@Description",discount.Description),
                new SqlParameter("@PercentDiscount",discount.PercentDiscount),
                new SqlParameter("@MaxCost",discount.MaxCost),
                new SqlParameter("@DateStart",discount.DateStart),
                new SqlParameter("@DateEnd",discount.DateEnd),
                new SqlParameter("@ImageDiscount",discount.ImageDiscount),
                new SqlParameter("@NoTicket",discount.NoTicket),
                new SqlParameter("@Point",discount.Point),
                new SqlParameter("@Used",discount.Used)
            };
            try
            {
                _context.Database.ExecuteSqlRaw("EXEC USP_UpdateDiscount @Id, @Name ,@Description," +
                    " @PercentDiscount,  @MaxCost ,@DateStart,@DateEnd,@ImageDiscount,@NoTicket,@Point," +
                    "@Used  ", sqlParam);
            }
            catch (SqlException s)
            {
                result = s.Message;
            }
            return result;
        }
        public void ExecuteDeleteDiscount(string id)
        {
            _context.Database.ExecuteSqlRaw("EXEC USP_DeleteDiscount @Id " ,new SqlParameter("@Id", id));
        }
        //==========SEAT============
        public List<SeatViewModel> ExecGetAllSeat(string idShow)
        {
            List<SeatViewModel> lstSeat = new List<SeatViewModel>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_GetSeat", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IdShow", idShow);
                SqlDataReader rdr = com.ExecuteReader();

                while (rdr.Read())
                {
                    lstSeat.Add(new SeatViewModel
                    {
                        Id = rdr["Id"].ToString(),
                        IdTypesOfSeat = rdr["Id_TypesOfSeat"].ToString(),
                        IdScreen = rdr["Id_Screen"].ToString(),
                        Row = rdr["Row"].ToString(),
                        No = int.Parse(rdr["No"].ToString()),
                        Status=rdr["Status"].ToString()
                    });

                }
                return lstSeat;
            }
        }
        public List<object> ExecGetChoosedSeat(string idShow, string idScreen)
        {
            List<object> lstSeat = new List<object>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_GetSeatChoosed", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IdScreen", idScreen);
                com.Parameters.AddWithValue("@IdShow", idShow);
                SqlDataReader rdr = com.ExecuteReader();

                while (rdr.Read())
                {
                    lstSeat.Add(new Seat
                    {
                        Id = rdr["Id_Seat"].ToString()
                    });

                }
                return lstSeat;
            }
            
        }
        public Seat ExecCheckIdSeat(string idseat, string idshow)
        {
            // lấy seat và kiểm tra seat chưa đặt
            var obj = new Seat();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_GetDetailSeat", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IdSeat", idseat);
                com.Parameters.AddWithValue("@IdShow", idshow);
                SqlDataReader rdr = com.ExecuteReader();

                while (rdr.Read())
                {
                    obj= new Seat
                    {
                        Id = rdr["Id"].ToString(),
                        IdTypesOfSeat = rdr["Id_TypesOfSeat"].ToString(),
                        IdScreen = rdr["Id_Screen"].ToString(),
                        Row = rdr["Row"].ToString(),
                        No = int.Parse(rdr["No"].ToString())
                    };
                }
                return obj;
            }
        }
        //=================CHECKOUT
        public int FGetPrice(string idseat)
        {
            SqlConnection con = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand("SELECT dbo.UF_Price(@IdSeat)", con);
            // cmd.CommandType=CommandType.StoredProcedure;  
            cmd.Parameters.AddWithValue("@IdSeat", idseat);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int str = int.Parse(dt.Rows[0][0].ToString());
            return str;
        }
    }
}
