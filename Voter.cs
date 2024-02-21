using System;
using System.Data.SqlClient;

namespace ElectronicVotingSystem
{
    class Voter
    {
        private string voterName;
        private string cnic;
        private string selectedPartyName;
        public Voter(string VoterName, string Cnic, string SelectedPartyName)   
        {
            voterName = VoterName;
            cnic = Cnic;
            selectedPartyName = SelectedPartyName;
        }
        public string VoterName
        {
            get { return voterName; }
            set { voterName = value; }
        }
        public string Cnic
        {
            get { return cnic; }
            set { cnic = value; }
        }
        public string SelectedPartyName
        {
            get { return selectedPartyName; }
            set { selectedPartyName = value; }
        }
        public bool hasVoted(string cnic)
        {

            SqlConnection connection = new SqlConnection(ConnectionString.constr);
            try
            {
                connection.Open();

                string query = "select * from Voters where CNIC='" + cnic + "'";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dr = cmd.ExecuteReader();

                if (!dr.HasRows)  //voter not exist 
                {
                    dr.Close();
                    return true;  
                }
                dr.Read();
                if (dr["VoterName"].ToString() != voterName.ToUpper())
                    return true;
                if (string.IsNullOrEmpty(dr["SelectedPartyName"].ToString())) //if not cast Vote
                {
                    dr.Close();
                    return false;
                }
                else
                {
                    dr.Close();
                    return true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                System.Environment.Exit(0);
                return true;
            }
            finally
            {
                connection.Close();
            }

        }
    }
}
