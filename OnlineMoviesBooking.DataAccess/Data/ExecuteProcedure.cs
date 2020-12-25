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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;

namespace OnlineMoviesBooking.DataAccess.Data
{
    public class ExecuteProcedure
    {
        private readonly string cs;


        public  ExecuteProcedure(string _cs)
        {
            cs = _cs;

        }

        //-------------------------------MOVIE
        
        public int GetCountMovieNow()  
        {
            // to paging
            int count = 0;
            using SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand com = new SqlCommand("SELECT dbo.UF_GetNumOfMovieNow()", con);
            count= int.Parse(com.ExecuteScalar().ToString());
            return count;
        }
        public int GetCountMovieComing()   
        {
            // to paging
            using SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand com = new SqlCommand("SELECT dbo.UF_GetNumOfMovieComing()", con);
            return int.Parse(com.ExecuteScalar().ToString());
        }
        public List<Movie> ExecuteMovieGetAll() 
        {
            var lst = new List<Movie>();
            using SqlConnection con = new SqlConnection(cs);
            con.Open();
            // TêN STORE
            SqlCommand com = new SqlCommand("USP_GetAllMovie", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            SqlDataReader rdr = com.ExecuteReader();

            while (rdr.Read())
            {
                lst.Add(new Movie
                {
                    Id = (rdr["Id"].ToString()),
                    Name = (rdr["Name"].ToString()),
                    Genre = (rdr["Genre"].ToString()),
                    Director = (rdr["Director"].ToString()),
                    Casts = (rdr["Casts"].ToString()),
                    Rated = (rdr["Rated"].ToString()),
                    Description = (rdr["Description"].ToString()),
                    Trailer = (rdr["Trailer"].ToString()),
                    ReleaseDate = DateTime.Parse(rdr["ReleaseDate"].ToString()),
                    RunningTime = int.Parse(rdr["RunningTime"].ToString()),
                    Poster = (rdr["Poster"].ToString())
                });

            }
            return lst;

        }
        public Movie ExecuteMovieDetail(string id)  
        {
            var movie = new Movie();
            using SqlConnection con = new SqlConnection(cs);
            con.Open();
            // TêN STORE
            SqlCommand com = new SqlCommand("USP_GetDetailMovie", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Id", id);
            SqlDataReader rdr = com.ExecuteReader();

            while (rdr.Read())
            {
                movie = new Movie
                {
                    Id = (rdr["Id"].ToString()),
                    Name = (rdr["Name"].ToString()),
                    Genre = (rdr["Genre"].ToString()),
                    Director = (rdr["Director"].ToString()),
                    Casts = (rdr["Casts"].ToString()),
                    Rated = (rdr["Rated"].ToString()),
                    Description = (rdr["Description"].ToString()),
                    Trailer = (rdr["Trailer"].ToString()),
                    ReleaseDate = DateTime.Parse(rdr["ReleaseDate"].ToString()),
                    RunningTime = int.Parse(rdr["RunningTime"].ToString()),
                    Poster = (rdr["Poster"].ToString())
                };

            }
            return movie;
        }
        public string ExecuteInsertMovie(Movie movie)       
        {
            string error = "";
            try
            {
                using SqlConnection con = new SqlConnection(cs);
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_InsertMovie", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", movie.Id);
                com.Parameters.AddWithValue("@Name", movie.Name ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Genre", movie.Genre ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Director", movie.Director ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Casts", movie.Casts ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Rated", movie.Rated ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Description", movie.Description ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Trailer", movie.Trailer ?? Convert.DBNull);
                com.Parameters.AddWithValue("@ReleaseDate", movie.ReleaseDate ?? Convert.DBNull);
                com.Parameters.AddWithValue("@RunningTime", movie.RunningTime ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Poster", movie.Poster ?? Convert.DBNull);
                com.ExecuteNonQuery();
            }
            catch(SqlException s)
            {
                error = s.Message.ToString();
            }
            return error;
           
        }
        public void ExecuteDeleteMovie(string id)   
        {
            using SqlConnection con = new SqlConnection(cs);
            con.Open();
            // TêN STORE
            SqlCommand com = new SqlCommand("USP_DeleteMovie", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Id", id);

            com.ExecuteNonQuery();
        }
        public string ExecuteUpdateMovie(Movie movie) 
        {
            string s = "";
            try
            {
                using SqlConnection con = new SqlConnection(cs);
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_UpdateMovie", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", movie.Id);
                com.Parameters.AddWithValue("@Name", movie.Name ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Genre", movie.Genre ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Director", movie.Director ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Casts", movie.Casts ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Rated", movie.Rated ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Description", movie.Description ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Trailer", movie.Trailer ?? Convert.DBNull);
                com.Parameters.AddWithValue("@ReleaseDate", movie.ReleaseDate ?? Convert.DBNull);
                com.Parameters.AddWithValue("@RunningTime", movie.RunningTime ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Poster", movie.Poster ?? Convert.DBNull);
                com.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                s = e.Message.ToString();
            }
            return s;
        }
        public string ExecuteGetImageMovie(string id)   
        {
            string pos = "";
            using SqlConnection con = new SqlConnection(cs);
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
        public List<Movie> ExecuteGetMovieNow(int skip, int take)   
        {
            var lst = new List<Movie>();
            using SqlConnection con = new SqlConnection(cs);
            con.Open();
            // TêN STORE
            SqlCommand com = new SqlCommand("USP_PagingMovieNow", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Skip", skip);
            com.Parameters.AddWithValue("@Take", take);
            SqlDataReader rdr = com.ExecuteReader();

            while (rdr.Read())
            {
                lst.Add(new Movie
                {
                    Id = (rdr["Id"].ToString()),
                    Name = (rdr["Name"].ToString()),
                    Genre = (rdr["Genre"].ToString()),
                    Director = (rdr["Director"].ToString()),
                    Casts = (rdr["Casts"].ToString()),
                    Rated = (rdr["Rated"].ToString()),
                    Description = (rdr["Description"].ToString()),
                    Trailer = (rdr["Trailer"].ToString()),
                    ReleaseDate = DateTime.Parse(rdr["ReleaseDate"].ToString()),
                    RunningTime = int.Parse(rdr["RunningTime"].ToString()),
                    Poster = (rdr["Poster"].ToString())
                });

            }
            return lst;

        }
        public List<Movie> ExecuteGetMovieComingSoon(int skip, int take)    
        {
            var lst = new List<Movie>();
            using SqlConnection con = new SqlConnection(cs);
            con.Open();
            // TêN STORE
            SqlCommand com = new SqlCommand("USP_PagingMovieComing", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Skip", skip);
            com.Parameters.AddWithValue("@Take", take);
            SqlDataReader rdr = com.ExecuteReader();

            while (rdr.Read())
            {
                lst.Add(new Movie
                {
                    Id = (rdr["Id"].ToString()),
                    Name = (rdr["Name"].ToString()),
                    Genre = (rdr["Genre"].ToString()),
                    Director = (rdr["Director"].ToString()),
                    Casts = (rdr["Casts"].ToString()),
                    Rated = (rdr["Rated"].ToString()),
                    Description = (rdr["Description"].ToString()),
                    Trailer = (rdr["Trailer"].ToString()),
                    ReleaseDate = DateTime.Parse(rdr["ReleaseDate"].ToString()),
                    RunningTime = int.Parse(rdr["RunningTime"].ToString()),
                    Poster = (rdr["Poster"].ToString())
                });

            }
            return lst;
        }
        //----------------------THEATER
        public List<Theater> ExecuteTheaterGetAll()
        {
            var lst = new List<Theater>();
            using SqlConnection con = new SqlConnection(cs);
            con.Open();
            // TêN STORE
            SqlCommand com = new SqlCommand("USP_GetAllTheater", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataReader rdr = com.ExecuteReader();

            while (rdr.Read())
            {
                lst.Add(new Theater
                {
                    Id = (rdr["Id"].ToString()),
                    Name = (rdr["Name"].ToString()),
                    Address = (rdr["Address"].ToString()),
                    Hotline = (rdr["Hotline"].ToString())
                });

            }
            return lst;
        }       
        public Theater ExecuteDetailTheater(string id)
        {
            var theater = new Theater();
            using SqlConnection con = new SqlConnection(cs);
            con.Open();
            // TêN STORE
            SqlCommand com = new SqlCommand("USP_GetDetailTheater", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Id", id);
            SqlDataReader rdr = com.ExecuteReader();

            while (rdr.Read())
            {
                theater = new Theater
                {
                    Id = (rdr["Id"].ToString()),
                    Name = (rdr["Name"].ToString()),
                    Address = (rdr["Address"].ToString()),
                    Hotline = (rdr["Hotline"].ToString())
                };
            }
            return theater;
        }   
        public string ExecuteInsertTheater(string id, string name, string address, string hotline)  // cheked
        {
            string mess = "";
            try 
            {
                using SqlConnection con = new SqlConnection(cs);
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_InsertTheater", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", id);
                com.Parameters.AddWithValue("@Name", name ??Convert.DBNull);
                com.Parameters.AddWithValue("@Address", address ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Hotline", hotline ?? Convert.DBNull);

                com.ExecuteScalar();

            }
            catch(SqlException s)
            {
                 mess = s.Message.ToString();
            }
            return mess;
        }
        public string ExecuteUpdateTheater(Theater theater)
        {
            string mess = "";
            try
            {
                using SqlConnection con = new SqlConnection(cs);
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_UpdateTheater", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", theater.Id);
                com.Parameters.AddWithValue("@Name", theater.Name ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Address", theater.Address ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Hotline", theater.Hotline ?? Convert.DBNull);

                com.ExecuteScalar();

            }
            catch (SqlException s)
            {
                mess = s.Message.ToString();
            }
            return mess;
        }       
        public string ExecuteDeleteTheater(string id)     
        {
            string s = "";
            using SqlConnection con = new SqlConnection(cs);
            con.Open();
            // TêN STORE
            SqlCommand com = new SqlCommand("USP_DeleteThreater", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Id", id);

            SqlDataReader rdr = com.ExecuteReader();

            while (rdr.Read())
            {
                s = (rdr["ErrorMessage"]).ToString();

            }
            return s;
        }
        //-----------------------Screen
        public List<Screen_Theater> ExecuteScreenGetAllwithTheater()
        {
            /// Id, Name, Name Theater
            var lst = new List<Screen_Theater>();
            using SqlConnection con = new SqlConnection(cs);
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
                    IdTheater = rdr["Id_Theater"].ToString(),
                    NameTheater = rdr["Theater"].ToString()
                });
            }
            return lst;

        }       
        public string ExecuteInsertScreen(Screen screen)      // EDIT HERE AFTER USE TRANSACTION
        {
            string mess = "";
            using SqlConnection con = new SqlConnection(cs);
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
                mess = (rdr["ErrorMessage"]).ToString();
            }
            return mess;  // 3609 : trigger

        }
        public string CheckNameScreen(string name, string id)
        {
            string result = "";

            using SqlConnection con = new SqlConnection(cs);
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
        public ScreenViewModel ExecuteGetDetailScreen_Theater(string id)    
        {
            /// Id, Name, Name Theater
            ScreenViewModel lst = new ScreenViewModel();
            using SqlConnection con = new SqlConnection(cs);
            con.Open();
            // TêN STORE
            SqlCommand com = new SqlCommand("USP_GetDetailScreenwithTheater", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Id", id);
            SqlDataReader rdr = com.ExecuteReader();
            while (rdr.Read())
            {
                lst = new ScreenViewModel
                {
                    Id = (rdr["Id"]).ToString(),
                    Name = rdr["Name"].ToString(),
                    IdTheater = rdr["Id_Theater"].ToString(),
                    NameTheater = rdr["Theater"].ToString(),
                    Address = (rdr["Address"]).ToString()
                };
            }
            return lst;
        }
        public void ExecuteUpdateScreen(Screen screen)      
        {
            using SqlConnection con = new SqlConnection(cs);
            con.Open();
            // TêN STORE
            SqlCommand com = new SqlCommand("USP_UpdateScreen", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Id", screen.Id);
            com.Parameters.AddWithValue("@Name", screen.Name);
            com.Parameters.AddWithValue("@Id_Theater", screen.IdTheater);

            com.ExecuteScalar();
        }
        public string ExecuteDeleteScreen(string id)
        {
            string result = "";
            using SqlConnection con = new SqlConnection(cs);
            con.Open();
            // TêN STORE
            SqlCommand com = new SqlCommand("dbo.USP_DeleteSeatandScreen", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@IdScreenIn", id);
            SqlDataReader rdr = com.ExecuteReader();
            while (rdr.Read())
            {
                result = (rdr["ErrorMessage"]).ToString();
            }
            return result;  // 3609 : trigger
        }   
        public List<Screen_Theater> SearchScreenwithTheater(string id)  
        {
            List<Screen_Theater> lst = new List<Screen_Theater>();
            using SqlConnection con = new SqlConnection(cs);
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
        //--------------------TypesOfSeat
        public void CreateTypeOfSeat()
        {
            using SqlConnection con = new SqlConnection(cs);
            con.Open();
            // TêN STORE
            SqlCommand com = new SqlCommand("USP_CreateTypeOfSeat", con);
            com.CommandType = CommandType.StoredProcedure;
            com.ExecuteScalar();
        }
        public List<TypesOfSeat> GetAllTypesOfSeat()
        {
            var lst = new List<TypesOfSeat>();
            using SqlConnection con = new SqlConnection(cs);
            con.Open();
            // TêN STORE
            SqlCommand com = new SqlCommand("USP_GetAllTypesOfSeat", con);
            com.CommandType = CommandType.StoredProcedure;

            SqlDataReader rdr = com.ExecuteReader();

            while (rdr.Read())
            {
                lst.Add(new TypesOfSeat
                {
                    Id = rdr["Id"].ToString(),
                    Name = rdr["Name"].ToString(),
                    Cost = int.Parse(rdr["Cost"].ToString()),
                    Num = int.Parse(rdr["Num"].ToString())
                });
            }
            return lst;
        }       
        public string ExecuteUpdateTypesOfSeat(TypesOfSeat s)
        {
            string result = "";
            try
            {
                using SqlConnection con = new SqlConnection(cs);
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_UpdateTypesOfSeat", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", s.Id);
                com.Parameters.AddWithValue("@Name", s.Name);
                com.Parameters.AddWithValue("@Cost", s.Cost);
                com.ExecuteNonQuery();
            }
            catch(SqlException e)
            {
                result = e.Message.ToString();
            }
            return result;
        }   
        public TypesOfSeat ExecGetDetailTypeOfSeat(string id)   
        {
            var type = new TypesOfSeat();
            using SqlConnection con = new SqlConnection(cs);
            con.Open();
            // TêN STORE
            SqlCommand com = new SqlCommand("USP_GetDetailTypeOfSeat", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Id", id);
            SqlDataReader rdr = com.ExecuteReader();

            while (rdr.Read())
            {
                type = new TypesOfSeat
                {
                    Id = rdr["Id"].ToString(),
                    Name = rdr["Name"].ToString(),
                    Cost = int.Parse(rdr["Cost"].ToString()),
                    Num = int.Parse(rdr["Num"].ToString())
                };
            }
            return type;
        }
               //------------------SHOW
        public List<ShowViewModel> ExecuteGetAllShow()  
        {
            List<ShowViewModel> lstShow = new List<ShowViewModel>();
            using SqlConnection con = new SqlConnection(cs);
            con.Open();
            // TêN STORE
            SqlCommand com = new SqlCommand("USP_GetAllShowwithMovieandScreen", con);
            com.CommandType = CommandType.StoredProcedure;
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
        public List<ShowViewModel> ExecuteGetAllShowTheater(string id)  
        {
            List<ShowViewModel> lstShow = new List<ShowViewModel>();
            using SqlConnection con = new SqlConnection(cs);
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
       

        public List<ShowViewModel> ExecuteGetAllShowisUsed()
        {
            List<ShowViewModel> lstShow = new List<ShowViewModel>();
            using SqlConnection con = new SqlConnection(cs);
            con.Open();
            // TêN STORE
            SqlCommand com = new SqlCommand("SELECT * FROM V_GetShowisUsed", con);
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
        public List<ShowViewModel> ExecuteGetAllShowisComing()
        {
            List<ShowViewModel> lstShow = new List<ShowViewModel>();
            using SqlConnection con = new SqlConnection(cs);
            con.Open();
            // TêN STORE
            SqlCommand com = new SqlCommand("SELECT * FROM V_GetShowisComing", con);
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
        public string ExecuteInsertShow(Show show)
        {
            string result = "";
            try
            {
                using SqlConnection con = new SqlConnection(cs);
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_InsertShow", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", show.Id);
                com.Parameters.AddWithValue("@Languages", show.Languages);
                com.Parameters.AddWithValue("@TimeStart", show.TimeStart);
                com.Parameters.AddWithValue("@Id_Movie", show.IdMovie);
                com.Parameters.AddWithValue("@Id_Screen", show.IdScreen);

                com.ExecuteNonQuery();
            }
            catch(SqlException s)
            {
                result = s.Message.ToString();
            }
            return result;
        }   
        public ShowViewModel ExecuteGetDetailShow(string id)    
        {
            ShowViewModel show = new ShowViewModel();
            using SqlConnection con = new SqlConnection(cs);
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
                    IdMovie = rdr["Id_Movie"].ToString(),
                    IdScreen = rdr["Id_Screen"].ToString(),
                    IdTheater = rdr["IdTheater"].ToString(),
                    MovieName = rdr["MovieName"].ToString(),
                    Poster = rdr["Poster"].ToString(),
                    ScreenName = rdr["ScreenName"].ToString(),
                    TheaterName = rdr["TheaterName"].ToString()
                };
            }
            return show;
        }   
        public ShowViewModel ExecuteGetDetailShowEdit(string id)
        {
            ShowViewModel show = new ShowViewModel();
            using SqlConnection con = new SqlConnection(cs);
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
                    IdTheater = rdr["IdTheater"].ToString()
                };
            }
            return show;
        }
        public string ExecuteUpdateShow(ShowViewModel show)
        {
            string result = "";

            try
            {
                using SqlConnection con = new SqlConnection(cs);
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_UpdateShow", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", show.Id);
                com.Parameters.AddWithValue("@Languages", show.Languages);
                com.Parameters.AddWithValue("@TimeStart", show.TimeStart);
                com.Parameters.AddWithValue("@Id_Movie", show.IdMovie);
                com.Parameters.AddWithValue("@Id_Screen", show.IdScreen);

                com.ExecuteNonQuery();
            }
            catch (SqlException s)
            {
                result = s.Message;
            }
            return result;
        }   
        public string ExecuteDeleteShow(string id)
        {
            string s = "";
            try
            {
                using SqlConnection con = new SqlConnection(cs);    
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_DeleteShow", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IdShow", id);

                com.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                s = e.Message.ToString();
            }
            return s;
        }
        public object ExecuteFindTheaterShow(string idmovie, string date)   
        {
            List<object> theater= new List<object>();

            using SqlConnection con = new SqlConnection(cs);
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

                theater.Add(new
                {
                    Id = rdr["Id_Theater"].ToString(),
                    Name = rdr["Name"].ToString(),
                    Times = times
                });
            }
            return theater;
        }
        public object ExecuteFindTimeofTheater(string idmovie, string date,string idtheater)
        {
            List<object> times = new List<object>();

            using SqlConnection con = new SqlConnection(cs);
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
            catch (SqlException e)
            {
                string ss = e.Message;
            }
            return times;
        }   
            
       
        
        //------------------DISCOUNT
        public List<Discount> ExecuteGetAllDiscount()   
        {
            List<Discount> lstDiscount = new List<Discount>();

            using SqlConnection con = new SqlConnection(cs);
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
                    Code = rdr["Code"].ToString(),
                    Description = rdr["Description"].ToString(),
                    PercentDiscount = rdr["PercentDiscount"].ToString() == "" ? 0 : int.Parse(rdr["PercentDiscount"].ToString()),
                    MaxCost = rdr["MaxCost"].ToString() == "" ? 0 : int.Parse(rdr["MaxCost"].ToString()),
                    DateStart = DateTime.Parse(rdr["DateStart"].ToString()),
                    DateEnd = DateTime.Parse(rdr["DateEnd"].ToString()),
                    ImageDiscount = rdr["ImageDiscount"].ToString(),
                    Point = rdr["Point"].ToString() == "" ? 0 : int.Parse(rdr["Point"].ToString()),
                    Used = rdr["Used"].ToString() == "" ? 0 : int.Parse(rdr["Used"].ToString()),
                });

            }
            return lstDiscount;
        }
        public Discount ExecuteGetDetailDiscount(string id)
        {
            Discount d = new Discount();
            using SqlConnection con = new SqlConnection(cs);
            con.Open();
            // TêN STORE
            SqlCommand com = new SqlCommand("USP_GetDetailDiscount", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Id", id);
            SqlDataReader rdr = com.ExecuteReader();

            while (rdr.Read())
            {
                d = new Discount
                {
                    Id = rdr["Id"].ToString(),
                    Name = rdr["Name"].ToString(),
                    Code = rdr["Code"].ToString(),
                    Description = rdr["Description"].ToString(),
                    PercentDiscount = rdr["PercentDiscount"].ToString() == "" ? 0 : int.Parse(rdr["PercentDiscount"].ToString()),
                    MaxCost = rdr["MaxCost"].ToString() == "" ? 0 : int.Parse(rdr["MaxCost"].ToString()),
                    DateStart = DateTime.Parse(rdr["DateStart"].ToString()),
                    DateEnd = DateTime.Parse(rdr["DateEnd"].ToString()),
                    ImageDiscount = rdr["ImageDiscount"].ToString(),
                    Point = rdr["Point"].ToString() == "" ? 0 : int.Parse(rdr["Point"].ToString()),
                    Used = rdr["Used"].ToString() == "" ? 0 : int.Parse(rdr["Used"].ToString()),
                };

            }
            return d;
        }   
        public string ExecuteInsertDiscount(Discount discount)
        {
            string result = "";
           
            try
            {
                using SqlConnection con = new SqlConnection(cs);
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_InsertDiscount", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", discount.Id);
                com.Parameters.AddWithValue("@Name", discount.Name);
                com.Parameters.AddWithValue("@Code", discount.Code);
                com.Parameters.AddWithValue("@Description", discount.Description);
                com.Parameters.AddWithValue("@PercentDiscount", discount.PercentDiscount ?? Convert.DBNull);
                com.Parameters.AddWithValue("@MaxCost", discount.MaxCost ?? Convert.DBNull);
                com.Parameters.AddWithValue("@DateStart", discount.DateStart);
                com.Parameters.AddWithValue("@DateEnd", discount.DateEnd);
                com.Parameters.AddWithValue("@ImageDiscount", discount.ImageDiscount);
                com.Parameters.AddWithValue("@Point", discount.Point ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Used", discount.Used);
                com.ExecuteNonQuery();
            }
            catch (SqlException s)
            {
                result = s.Message;
            }
            return result;
        }   

        public string ExecCheckDiscount(string code)
        {
            using SqlConnection con = new SqlConnection(cs);
            con.Open();
            // TêN STORE
            SqlCommand com = new SqlCommand("SELECT dbo.UF_CheckDiscount(@Code)", con);
            com.Parameters.AddWithValue("@Code", code);

            return  com.ExecuteScalar().ToString();
        }
        public string ExecuteGetImageDiscount(string id)
        {
            string pos = "";
            using SqlConnection con = new SqlConnection(cs);
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
        public string ExecuteUpdateDiscount(Discount discount)
        {
            string result = "";
            try
            {
                using SqlConnection con = new SqlConnection(cs);
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_UpdateDiscount", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", discount.Id);
                com.Parameters.AddWithValue("@Name", discount.Name);
                com.Parameters.AddWithValue("@Code", discount.Code);
                com.Parameters.AddWithValue("@Description", discount.Description);
                com.Parameters.AddWithValue("@PercentDiscount", discount.PercentDiscount ?? Convert.DBNull);
                com.Parameters.AddWithValue("@MaxCost", discount.MaxCost ?? Convert.DBNull);
                com.Parameters.AddWithValue("@DateStart", discount.DateStart);
                com.Parameters.AddWithValue("@DateEnd", discount.DateEnd);
                com.Parameters.AddWithValue("@ImageDiscount", discount.ImageDiscount);
                com.Parameters.AddWithValue("@Point", discount.Point ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Used", discount.Used);
                com.ExecuteNonQuery();
            }
            catch (SqlException s)
            {
                result = s.Message;
            }
            return result;
        }   
        public string ExecuteDeleteDiscount(string id)    
        {
            string s = "";
            try
            {
                using SqlConnection con = new SqlConnection(cs);
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_DeleteDiscount", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", id);
                com.ExecuteNonQuery();
            }
            catch(SqlException e)
            {
                s = e.Message.ToString();
            }
            return s;
        }
        //==========SEAT============
        public List<SeatViewModel> ExecGetAllSeat(string idShow)
        {
            List<SeatViewModel> lstSeat = new List<SeatViewModel>();
            using SqlConnection con = new SqlConnection(cs);
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
                    Status = rdr["Status"].ToString()
                });

            }
            return lstSeat;
        }   
        public Seat ExecCheckIdSeat(string idseat, string idshow)   
        {
            // lấy seat và kiểm tra seat chưa đặt
            var obj = new Seat();
            using SqlConnection con = new SqlConnection(cs);
            con.Open();
            // TêN STORE
            SqlCommand com = new SqlCommand("USP_GetDetailSeat", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@IdSeat", idseat);
            com.Parameters.AddWithValue("@IdShow", idshow);
            SqlDataReader rdr = com.ExecuteReader();

            while (rdr.Read())
            {
                obj = new Seat
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
        //=================CHECKOUT

        public CheckoutViewModel TestCheckout(string idaccount,string pointuse)
        {

            using SqlConnection con = new SqlConnection(cs);
            con.Open();
            // TêN STORE
            SqlCommand com = new SqlCommand("USP_Checkout", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@IdAccount", idaccount);
            com.Parameters.AddWithValue("@PointUse", pointuse);
            SqlDataReader rdr = com.ExecuteReader();

            while (rdr.Read())
            {
                return new CheckoutViewModel
                {
                    MovieName = rdr["MovieName"].ToString(),
                    No = rdr["No"].ToString(),
                    TotalPer = rdr["PricePer"].ToString() == "" ? (int?)null : int.Parse(rdr["PricePer"].ToString()),
                    PointPer = rdr["PointPer"].ToString() == "" ? (int?)null : int.Parse(rdr["PointPer"].ToString()),
                    TotalCost = rdr["PriceCost"].ToString() == "" ? (int?)null : int.Parse(rdr["PriceCost"].ToString()),
                    PointCost = rdr["PointCost"].ToString() == "" ? (int?)null : int.Parse(rdr["PointCost"].ToString()),
                    TheaterName = rdr["TheaterName"].ToString(),
                    Date = DateTime.Parse(rdr["TimeStart"].ToString())
                };
            }
            return null;
        }   

        //==================BILL        checked
        public string ExecInsertTickets(List<string> seatVM, string idAccount, string idShow, string iddiscount)
        {
            string result = "";
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_InsertTickets", con); 
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id_Seat1", seatVM[0] ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Id_Seat2", seatVM[1] ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Id_Seat3", seatVM[2] ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Id_Seat4", seatVM[3] ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Id_Seat5", seatVM[4] ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Id_Seat6", seatVM[5] ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Id_Seat7", seatVM[6] ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Id_Seat8", seatVM[7] ?? Convert.DBNull);
                com.Parameters.AddWithValue("@Id_Account", idAccount);
                com.Parameters.AddWithValue("@Id_Show", idShow);
                com.Parameters.AddWithValue("@Id_Discount", iddiscount ?? Convert.DBNull);
                SqlDataReader rdr = com.ExecuteReader();

                while (rdr.Read())
                {
                    result = rdr["Results"].ToString();
                }

            }
            return result;
        }
        public TicketViewModel ExecGetTicketDetail(string idaccount, string idshow) 
        {
            var bill = new TicketViewModel();
            using SqlConnection con = new SqlConnection(cs);
            con.Open();

            // TêN STORE
            SqlCommand com = new SqlCommand("USP_GetDetailTicketVM", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Id_Account", idaccount);
            com.Parameters.AddWithValue("@Id_Show", idshow);
            SqlDataReader rdr = com.ExecuteReader();

            while (rdr.Read())
            {
                bill = new TicketViewModel
                {
                    IdShow = rdr["IdShow"].ToString(),
                    IdAccount = rdr["IdAccount"].ToString(),
                    No = int.Parse(rdr["No"].ToString()),
                    TotalPrice = double.Parse(rdr["TotalPrice"].ToString()),
                    MovieName = rdr["MovieName"].ToString(),
                    RunningTime = int.Parse(rdr["RunningTime"].ToString()),
                    TimeStart = DateTime.Parse(rdr["TimeStart"].ToString()),
                    TimeEnd = DateTime.Parse(rdr["TimeEnd"].ToString()),
                    TheaterName = (rdr["TheaterName"].ToString()),
                    ScreenName = (rdr["ScreenName"].ToString()),
                    Languages = (rdr["Languages"].ToString()),
                    Address = (rdr["Address"].ToString()),
                    Point = int.Parse(rdr["PointVM"].ToString())
                };
            }
            return bill;
        }

        public string ExecDeleteTicketStatus0(string idaccount) 
        {
            string s = "";
            try
            {
                using SqlConnection con = new SqlConnection(cs);
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_DeleteTicketStatus0", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IdAccount", idaccount ?? Convert.DBNull);
                com.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                s = e.Message.ToString();
            }
            return s;
        }
        public void ExecUpdateTicketStatus(string idaccount,int point)  
        {
            using SqlConnection con = new SqlConnection(cs);
            con.Open();
            // TêN STORE
            SqlCommand com = new SqlCommand("USP_ChangeTicketStatus", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@IdAccount", idaccount);
            com.Parameters.AddWithValue("@Point", point);       // point = 0 nếu không dùng
            com.ExecuteNonQuery();
        }
        public object ExecUseDiscount(string idaccount, string code)    
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_UseDiscount", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IdAccount", idaccount);
                com.Parameters.AddWithValue("@Code", code);
                SqlDataReader rdr = com.ExecuteReader();

                while (rdr.Read())
                {
                    return  new 
                    {
                        IdAccount = rdr["IdAccount"].ToString(),
                        Price = double.Parse(rdr["Price"].ToString()),
                        No = int.Parse(rdr["No"].ToString()),
                        NameDiscount = rdr["Name"].ToString(),
                        PercentDiscount=rdr["PercentDiscount"].ToString(),
                        MaxCost=rdr["MaxCost"].ToString(),
                        Point=rdr["Point"].ToString(),
                    };
                }
            }
            return null;
        }
        public void ExecAddDiscount(string idaccount, string code)  
        {
            using SqlConnection con = new SqlConnection(cs);
            con.Open();
            // TêN STORE
            SqlCommand com = new SqlCommand("USP_AddDiscount", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@IdAccount", idaccount);
            com.Parameters.AddWithValue("@Code", code);
            com.ExecuteNonQuery();
        }
        public string ExecCheckPoint(string idaccount, int point)   
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_GetPoint", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IdAccount", idaccount);
                com.Parameters.AddWithValue("@Point", point);
                SqlDataReader rdr = com.ExecuteReader();

                while (rdr.Read())
                {
                    return rdr["Result"].ToString();
                }
            }
            return "";
        }
        public string ExecAddPoint(string idaccount, string point)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // TêN STORE
                SqlCommand com = new SqlCommand("USP_Addpoint", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IdAccount", idaccount);
                com.Parameters.AddWithValue("@Point", point);
                SqlDataReader rdr = com.ExecuteReader();

                while (rdr.Read())
                {
                    return rdr["Result"].ToString();
                }
            }
            return "";
        }
    }
}
