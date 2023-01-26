namespace SantaBackEnd.Database
{
    public class DataGenerator
    {
        internal void generataElves(int minindex, int maxIndex)
        {
            using (var reader = new StreamReader(@"names.csv"))
            {
                List<string> firstNames = new List<string>();
                List<string> lastNames = new List<string>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(' ');

                    firstNames.Add(values[0]);
                    lastNames.Add(values[1]);
                }
                Random firstRand = new Random();
                Random secondRand = new Random();
                int allNames = firstNames.Count;
                int firstIndex = 0;
                int secondIndex = 0;
                string firstName = "";
                string lastName = "";
                DateOnly birthday = new DateOnly(2022, 1, 1);
                DateOnly registration = new DateOnly(2022, 1, 1);
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                string password = "";
                string filename = "insertUsers.txt";
                string filename2 = "insertElves.txt";

                for (int i = minindex; i < maxIndex; i++)
                {
                    birthday = new DateOnly(1998, 1, 1);
                    registration = new DateOnly(2022, 1, 1);
                    firstIndex = firstRand.Next(allNames);
                    secondIndex = secondRand.Next(allNames);
                    firstName = firstNames[firstIndex];
                    lastName = lastNames[secondIndex];
                    birthday = birthday.AddDays(firstRand.Next(2000));
                    registration = registration.AddDays(secondRand.Next(380));
                    password = new string(Enumerable.Repeat(chars, 8)
                        .Select(s => s[firstRand.Next(s.Length)]).ToArray());
                    string insert = $"INSERT INTO public.users (userid, firstname, lastname, password, dateofbirth, dateofregistration) " +
                        $"VALUES( {i}, \'{firstName}\', \'{lastName}\', \'{password}\', \'{birthday}\', \'{registration}\');\n";
                    File.AppendAllText(filename, insert);
                    insert = $"INSERT INTO public.elf(roleid, userid) VALUES(\'{firstRand.Next(1, 2)}\', \'{i}\');\n";
                    File.AppendAllText(filename, insert);

                }
            }
        }
    }
}
