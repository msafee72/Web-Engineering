using System;

namespace ElectronicVotingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("\n**********Welcome to Online Voting System**********");
                VotingMachine vm = new VotingMachine();
                int choice;
                do
                {
                    Console.WriteLine("\nSelect Option\n");
                    Console.WriteLine("1. Add Voter\n2. Update Voter\n3. Delete Voter\n4. Display voters\n5. cast Vote");
                    Console.WriteLine("6. Insert Candidate\n7. Update Candidate\n8. Display Candidate\n9. Delete Candidate\n10. Declare Winner\n11. Quit");
                    Console.Write("\nChoice: ");
                    choice = int.Parse(Console.ReadLine());
                    while (choice < 1 || choice > 11)
                    {
                        Console.Write("Invalid Choice...!\nSelect Again..\nChoice: ");
                        choice = int.Parse(Console.ReadLine());
                    }
                    Console.WriteLine('\n');
                    switch (choice)
                    {
                        case 1:
                            vm.addVoter();
                            break;
                        case 2:
                            string cnic;
                            Console.Write("Enter CNIC of Voter\nCNIC: ");
                            cnic = Console.ReadLine();
                            vm.updateVoter(cnic);
                            break;
                        case 3:
                            Console.Write("Enter CNIC of Voter\nCNIC: ");
                            cnic = Console.ReadLine();
                            vm.deleteVoter(cnic);
                            break;
                        case 4:
                            vm.displayVoters();
                            break;
                        case 5:
                            vm.castVote();
                            break;
                        case 6:
                            vm.insertCandidate();
                            break;
                        case 7:
                            int id;
                            Console.Write("Enter ID of Candidate\nID: ");
                            id = int.Parse(Console.ReadLine());
                            vm.updateCandidate(id);
                            break;
                        case 8:
                            vm.displayCandidates();
                            break;
                        case 9:
                            Console.Write("Enter ID of Candidate\nID: ");
                            id = int.Parse(Console.ReadLine());
                            vm.deleteCandidate(id);
                            break;
                        case 10:
                            vm.declareWinner();
                            break;
                    }

                } while (choice != 11);

                Console.WriteLine("\n\t\t\t\t\tBye Bye..Have A Nice Day");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }


        }
    }
}





