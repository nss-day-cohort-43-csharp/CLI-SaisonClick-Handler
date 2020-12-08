﻿using System;
using System.Collections.Generic;
using System.Text;
using TabloidCLI.Models;
using Microsoft.Data.SqlClient;
using TabloidCLI.Repositories;

namespace TabloidCLI.Repositories
{
    public class JournalRepository : DatabaseConnector
    {
        public JournalRepository(string connectionString) : base(connectionString) { }

        public List<Journal> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT id, Title, Content, CreateDateTime FROM Journal";

                    List<Journal> journals = new List<Journal>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Journal journal = new Journal()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                        };
                        journals.Add(journal);
                    }

                    reader.Close();
                    return journals;

                }
            }
        }
        //public Journal Get(int id)
        //{
        //    throw new NotImplementedException();
        //}


        //public void Insert(Journal entry)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Update(Journal entry)
        //{
        //    throw new NotImplementedException();
        //}
        //public void Delete(int id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
