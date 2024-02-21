using System;
using System.Data.SqlClient;
using System.Text.Json.Serialization;

namespace ElectronicVotingSystem
{

    class Candidate
    {
        private int candidateId;
        private string name;
        private string party;
        private int votes;

        [JsonConstructor]
        private Candidate(int CandidateId, string Name, string Party, int Votes)
        {
            candidateId = CandidateId;
            name = Name;
            party = Party;
            votes = Votes;
        }
        public Candidate(string newName, string newParty)
        {
            name = newName;
            party = newParty;
            votes = 0;
            candidateId = generateCandidateId();
        }

        private int generateCandidateId()
        {
            SqlConnection connection = new SqlConnection(ConnectionString.constr);
            try
            {
                connection.Open();

                string query = "select top 1 CandidateId  from Candidates order by CandidateID DESC";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.HasRows)
                    return 1; //first record
                dr.Read();  

                int id = Convert.ToInt32(dr[0]) + 1;
                dr.Close();
                return id;


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                System.Environment.Exit(0);
                return -1;
            }

            finally
            {   
                connection.Close();
            }


        }
        public int CandidateId
        {
            get { return candidateId; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Party
        {
            get { return party; }
            set { party = value; }
        }
        public int Votes
        {
            get { return votes; }

        }
        public void incrementVotes()
        {
            votes += 1;
        }
        public override string ToString()
        {
            return $"Candidate Id: {candidateId} Name: {name} Party: {party} Votes: {votes}";
        }

    }
}

