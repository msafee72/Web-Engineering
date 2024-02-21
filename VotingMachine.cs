using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text.Json;

namespace ElectronicVotingSystem
{
    static class ConnectionString 
    {
        public static string constr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DBVotingSystem;Integrated Security=True";
    }

    class VotingMachine
    {
        private List<Candidate> candidates;
        public VotingMachine()
        {
            candidates = new List<Candidate>();
        }
        private void castVote(Voter v)
        {
            if (v.hasVoted(v.Cnic))
            {
                Console.WriteLine("Vote can't cast\nEither Voter is not Registered or already cast vote.. ");
                return;
            }

            //update database
            SqlConnection connection = new SqlConnection(ConnectionString.constr);
            try
            {
                connection.Open();
                string query2 = "update  Candidates set votes=votes+1  where Party ='" + v.SelectedPartyName + "'";
                SqlCommand cmd2 = new SqlCommand(query2, connection);
                int count = cmd2.ExecuteNonQuery();
                if (count == 0)  //selected party not exist
                {
                    Console.WriteLine("Party: " + v.SelectedPartyName + " does not exist");
                    return;
                }

                string query = "update  Voters set SelectedPartyName='" + v.SelectedPartyName + "' where CNIC='" + v.Cnic + "'";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                Console.WriteLine("\nVote Cast Successfully\n");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                System.Environment.Exit(0);

            }
            finally
            {
                connection.Close();
            }


            //Update files
            try
            {
                StreamWriter sw = new StreamWriter("temp.txt", true);
                StreamReader sr = new StreamReader("Candidates.txt");


                string s = sr.ReadLine();
                while (s != null)
                {
                    Candidate candidate = JsonSerializer.Deserialize<Candidate>(s);

                    if (candidate.Party == v.SelectedPartyName)
                    {
                        candidate.incrementVotes();
                    }
                    string jstr = JsonSerializer.Serialize(candidate);
                    sw.WriteLine(jstr);
                    s = sr.ReadLine();
                }
                sw.Close();
                sr.Close();
                File.Copy("temp.txt", "Candidates.txt", true);
                File.Delete("temp.txt");

                sw = new StreamWriter("temp.txt", true);
                sr = new StreamReader("Voters.txt");
                s = sr.ReadLine();
                while (s != null)
                {
                    Voter voter = JsonSerializer.Deserialize<Voter>(s);
                    if (voter.Cnic == v.Cnic)
                    {
                        voter.SelectedPartyName = v.SelectedPartyName;
                    }
                    string jstr = JsonSerializer.Serialize(voter);
                    sw.WriteLine(jstr);
                    s = sr.ReadLine();
                }
                sw.Close();
                sr.Close();
                File.Copy("temp.txt", "Voters.txt", true);
                File.Delete("temp.txt");
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine("Error: " + ex.Message);
                System.Environment.Exit(0);
            }
        }

        public void addVoter()
        {
            string name;
            string cnic;
            Console.WriteLine("Enter voter Details:");
            Console.Write("Name: ");
            name = Console.ReadLine();
            Console.Write("CNIC: ");
            cnic = Console.ReadLine();
            Voter v = new Voter(name, cnic, string.Empty);

            //add in database

            SqlConnection connection = new SqlConnection(ConnectionString.constr);
            try
            {
                connection.Open();
                string query = "insert into Voters values('" + v.Cnic + "','" + v.VoterName.ToUpper() + "',NULL)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine("Error: " + ex.Message);
                System.Environment.Exit(0);
            }
            finally
            {
                // Ensure the connection is closed
                connection.Close();
            }

            //add in file

            try
            {
                StreamWriter sw = new StreamWriter("Voters.txt", true);
                string jstr = JsonSerializer.Serialize(v);
                sw.WriteLine(jstr);
                sw.Close();
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine("Error: " + ex.Message);
                System.Environment.Exit(0);
            }
            Console.WriteLine("\nVoter added Successfully\n");
        }
        public void updateVoter(string cnic)
        {
            string name;
            Console.Write("Enter new details of Voter:\nName: ");
            name = Console.ReadLine();

            //update in database

            SqlConnection connection = new SqlConnection(ConnectionString.constr);
            try
            {
                connection.Open();
                string query = "update  Voters set VoterName='" + name.ToUpper() + "'  where CNIC='" + cnic + "'";
                SqlCommand cmd = new SqlCommand(query, connection);
                int count = cmd.ExecuteNonQuery();
                if (count == 0)  //Voter not exist
                {
                    Console.WriteLine("CNIC: " + cnic + " does not exist");
                    return;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                System.Environment.Exit(0);

            }
            finally
            {
                connection.Close();
            }


            //Update in files
            try
            {
                StreamWriter sw = new StreamWriter("temp.txt", true);
                StreamReader sr = new StreamReader("Voters.txt");


                string s = sr.ReadLine();
                while (s != null)
                {
                    Voter voter = JsonSerializer.Deserialize<Voter>(s);
                    if (voter.Cnic == cnic)
                    {
                        voter.VoterName = name;
                    }
                    string jstr = JsonSerializer.Serialize(voter);
                    sw.WriteLine(jstr);
                    s = sr.ReadLine();
                }
                sw.Close();
                sr.Close();
                File.Copy("temp.txt", "Voters.txt", true);
                File.Delete("temp.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                System.Environment.Exit(0);
            }
            Console.WriteLine("Voter Updated Successfully..!");

        }
        public void deleteVoter(string cnic)
        {
            //delete from database

            SqlConnection connection = new SqlConnection(ConnectionString.constr);
            try
            {
                connection.Open();
                string query = "delete  from Voters  where CNIC='" + cnic + "'";
                SqlCommand cmd = new SqlCommand(query, connection);
                int count = cmd.ExecuteNonQuery();
                if (count == 0)  //Voter not exist
                {
                    Console.WriteLine("CNIC: " + cnic + " does not exist");
                    return;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                System.Environment.Exit(0);

            }
            finally
            {
                connection.Close();
            }


            //delete from files
            try
            {
                StreamWriter sw = new StreamWriter("temp.txt", true);
                StreamReader sr = new StreamReader("Voters.txt");


                string s = sr.ReadLine();
                while (s != null)
                {
                    Voter voter = JsonSerializer.Deserialize<Voter>(s);
                    if (voter.Cnic == cnic)
                    {
                        s = sr.ReadLine();
                        continue;
                    }
                    string jstr = JsonSerializer.Serialize(voter);
                    sw.WriteLine(jstr);
                    s = sr.ReadLine();
                }
                sw.Close();
                sr.Close();
                File.Copy("temp.txt", "Voters.txt", true);
                File.Delete("temp.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                System.Environment.Exit(0);
            }
            Console.WriteLine("Voter Deleted Successfully..!");
        }
        public void displayVoters()
        {
            Console.WriteLine("List of Voters:\n");
            SqlConnection connection = new SqlConnection(ConnectionString.constr);
            try
            {
                connection.Open();
                string query = "select CNIC,Votername from Voters ";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.HasRows)  //data not exist
                {
                    Console.WriteLine("No data found..!");
                    dr.Close();
                    return;
                }
                int count = 1;
                while (dr.Read())
                {
                    Console.WriteLine(count + "- " + dr["VoterName"] + " CNIC- " + dr["CNIC"]);
                    count++;
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                System.Environment.Exit(0);

            }
            finally
            {
                connection.Close();
            }
        }
        public void castVote()
        {
            string name;
            string cnic;
            string party;
            try
            {
                Console.WriteLine("Enter voter Details:");
                Console.Write("Name: ");
                name = Console.ReadLine();
                Console.Write("CNIC: ");
                cnic = Console.ReadLine();
                Console.Write("Selected Party: ");
                party = Console.ReadLine();
                Voter v = new Voter(name, cnic, party.ToUpper());

                castVote(v);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                System.Environment.Exit(0);

            }



        }
        public void insertCandidate()
        {
            string name;
            string party;
            Console.WriteLine("Enter Candidate Details:");
            Console.Write("Name: ");
            name = Console.ReadLine();
            Console.Write("Party: ");
            party = Console.ReadLine();
            Candidate candidate = new Candidate(name, party);
            candidates.Add(candidate);

            //add in database

            SqlConnection connection = new SqlConnection(ConnectionString.constr);
            try
            {
                connection.Open();
                string query = "select party from Candidates where Party='" + candidate.Party.ToUpper() + "'";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)   //if party is not unique
                {
                    Console.WriteLine("Request denied..!\nCandidate of " + candidate.Party + " is already registered");
                    dr.Close();
                    return;
                }
                dr.Close();
                query = "insert into Candidates values('" + candidate.CandidateId + "','" + candidate.Name + "','" + candidate.Party.ToUpper() + "','" + candidate.Votes + "')";
                cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                System.Environment.Exit(0);

            }
            finally
            {
                // Ensure the connection is closed
                connection.Close();
            }

            //add in file

            try
            {
                StreamWriter sw = new StreamWriter("Candidates.txt", true);
                string jstr = JsonSerializer.Serialize(candidate);
                sw.WriteLine(jstr);
                sw.Close();
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine("Error: " + ex.Message);
                System.Environment.Exit(0);
            }
            Console.WriteLine("\nCandidate Inserted Successfully\n");
        }
        public void updateCandidate(int Id)
        {
            string name;
            string party;
            Console.Write("Enter new details of Candidate:\nName: ");
            name = Console.ReadLine();
            Console.Write("party: ");
            party = Console.ReadLine();

            //update in database

            SqlConnection connection = new SqlConnection(ConnectionString.constr);
            try
            {
                connection.Open();
                string query = "select party from Candidates where Party='" + party.ToUpper() + "'";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)   //if party is not unique
                {
                    Console.WriteLine("Request denied..!\nCandidate of " + party + " is already registered");
                    dr.Close();
                    return;
                }
                dr.Close();

                query = "update  Candidates set Name='" + name + "' ,Party='" + party.ToUpper() + "' where CandidateId='" + Id + "'";
                cmd = new SqlCommand(query, connection);
                int count = cmd.ExecuteNonQuery();
                if (count == 0)  //Candidate not exist
                {
                    Console.WriteLine("Request Denied..!\nID: " + Id + " does not exist");
                    return;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                System.Environment.Exit(0);

            }
            finally
            {
                connection.Close();
            }


            //Update in files
            try
            {
                StreamWriter sw = new StreamWriter("temp.txt", true);
                StreamReader sr = new StreamReader("Candidates.txt");


                string s = sr.ReadLine();
                while (s != null)
                {
                    Candidate candidate = JsonSerializer.Deserialize<Candidate>(s);
                    if (candidate.CandidateId == Id)
                    {
                        candidate.Name = name;
                        candidate.Party = party;
                    }
                    string jstr = JsonSerializer.Serialize(candidate);
                    sw.WriteLine(jstr);
                    s = sr.ReadLine();
                }
                sw.Close();
                sr.Close();
                File.Copy("temp.txt", "Candidates.txt", true);
                File.Delete("temp.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                System.Environment.Exit(0);
            }
            Console.WriteLine("Candidate Updated Successfully..!");
        }
        public void readCandidate(int id)
        {
            bool flag = true;
            Console.WriteLine("Read candidate details from Database..\n");
            SqlConnection connection = new SqlConnection(ConnectionString.constr);
            try
            {
                connection.Open();
                string query = "select * from Candidates where CandidateId='" + id + "' ";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.HasRows)  //data not exist
                {
                    Console.WriteLine("No data found..!");
                    dr.Close();
                    flag = false;

                }
                else
                {
                    Console.WriteLine("---------------------------------------");
                    Console.WriteLine("ID    | Name       | Party      | Votes");
                    Console.WriteLine("---------------------------------------");
                    dr.Read();

                    Console.WriteLine(string.Format("{0,-5} | {1,-10} | {2,-10} | {3,-5}", dr[0], dr[1], dr[2], dr[3]));

                    Console.WriteLine("---------------------------------------");
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                System.Environment.Exit(0);

            }
            finally
            {
                connection.Close();
            }

            Console.WriteLine("\nRead candidate details from File..\n");
            if (!flag)
            {
                Console.WriteLine("No data found..!");
                return;
            }
            try
            {
                StreamReader sr = new StreamReader("Candidates.txt");
                string s = sr.ReadLine();
                while (s != null)
                {
                    Candidate candidate = JsonSerializer.Deserialize<Candidate>(s);
                    if (candidate.CandidateId == id)
                    {
                        Console.WriteLine("Candidate Id: " + candidate.CandidateId + "\nName: " + candidate.Name);
                        Console.WriteLine("Party: " + candidate.Party + "\nVotes: " + candidate.Votes);
                        break;
                    }
                    s = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                System.Environment.Exit(0);
            }
        }
        public void displayCandidates()
        {
            Console.WriteLine("List of Candidates:\n");
            SqlConnection connection = new SqlConnection(ConnectionString.constr);
            try
            {
                connection.Open();
                string query = "select * from Candidates ";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.HasRows)  //data not exist
                {
                    Console.WriteLine("No data found..!");
                    return;
                }
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("ID    | Name       | Party      | Votes");
                Console.WriteLine("---------------------------------------");
                while (dr.Read())
                {
                    Console.WriteLine(string.Format("{0,-5} | {1,-10} | {2,-10} | {3,-5}", dr[0], dr[1], dr[2], dr[3]));
                }
                Console.WriteLine("---------------------------------------");
                dr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                System.Environment.Exit(0);

            }
            finally
            {
                connection.Close();
            }
        }
        public void deleteCandidate(int Id)
        {
            //delete from database

            SqlConnection connection = new SqlConnection(ConnectionString.constr);
            try
            {
                connection.Open();
                string query = "delete  from Candidates  where CandidateId='" + Id + "'";
                SqlCommand cmd = new SqlCommand(query, connection);
                int count = cmd.ExecuteNonQuery();
                if (count == 0)  //Candidate not exist
                {
                    Console.WriteLine("Request denied..!\nID: " + Id + " does not exist");
                    return;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                System.Environment.Exit(0);

            }
            finally
            {
                connection.Close();
            }


            //delete from files
            try
            {
                StreamWriter sw = new StreamWriter("temp.txt", true);
                StreamReader sr = new StreamReader("Candidates.txt");


                string s = sr.ReadLine();
                while (s != null)
                {
                    Candidate candidate = JsonSerializer.Deserialize<Candidate>(s);
                    if (candidate.CandidateId == Id)
                    {
                        s = sr.ReadLine();
                        continue;
                    }
                    string jstr = JsonSerializer.Serialize(candidate);
                    sw.WriteLine(jstr);
                    s = sr.ReadLine();
                }
                sw.Close();
                sr.Close();
                File.Copy("temp.txt", "Candidates.txt", true);
                File.Delete("temp.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                System.Environment.Exit(0);
            }
            Console.WriteLine("Candidate Deleted Successfully..!");

        }
        public void declareWinner()
        {
            SqlConnection connection = new SqlConnection(ConnectionString.constr);
            try
            {
                connection.Open();
                string query = "select *  from Candidates  where Votes=(select max(Votes) from Candidates ) ";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.HasRows)
                {
                    Console.WriteLine("No data found..!");
                    return;
                }
                Console.WriteLine("Winner Candidate:");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("ID    | Name       | Party      | Votes");
                Console.WriteLine("---------------------------------------");
                while (dr.Read())
                {
                    Console.WriteLine(string.Format("{0,-5} | {1,-10} | {2,-10} | {3,-5}", dr[0], dr[1], dr[2], dr[3]));
                }
                Console.WriteLine("---------------------------------------");
                dr.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                System.Environment.Exit(0);

            }
            finally
            {
                connection.Close();
            }
        }
    }
}
