using Microsoft.AspNetCore.Mvc;
using Npgsql;
using SantaBackend.DTO;
using SantaBackEnd;
using System.ComponentModel.DataAnnotations;

namespace SantaBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SantaController : ControllerBase
    {
        private const string CONNECTION_STRING = "Host=localhost:5432;Username=postgres;Password=postgres;Database=postgres";
        private NpgsqlConnection connection;
        private readonly postgresContext _context;

        public SantaController(postgresContext context)
        {
            connection = new NpgsqlConnection(CONNECTION_STRING);
            connection.Open();
            _context = context;
        }

        // GET: SantaController
        [HttpGet("test")]
        public ActionResult Index()
        {
            string command = $"select* from public.SantaClaus";
            using (NpgsqlCommand cmd = new NpgsqlCommand(command, connection))
            {

                NpgsqlDataReader reader = cmd.ExecuteReader();




            }
            return Ok();
        }

        // GET: SantaController/Details/5
        [HttpGet("Details")]
        public ActionResult<SantaDetailDTO> Details([Required] int santaID)
        {
            Santaclau? foundSanta = _context.Santaclaus.FirstOrDefault(s => s.Santaclausid == santaID);
            if (foundSanta != null)
            {
                User? user = _context.Users.FirstOrDefault(s => s.Userid == foundSanta.Userid);
                if (user != null)
                {
                    List<Workshop> foundWorkshops = _context.Workshops.Where(w => w.Santaclausid == foundSanta.Santaclausid).ToList();
                    List<WorkshopDTO> wokrshopDTOs = new List<WorkshopDTO>();
                    foreach (Workshop w in foundWorkshops)
                    {
                        wokrshopDTOs.Add(new WorkshopDTO()
                        {
                            location = w.Location,
                            workshopID = w.Workshopid,
                            workshopName = w.Name
                        });
                    }

                    SantaDetailDTO santaDTO = new SantaDetailDTO()
                    {
                        firstName = user.Firstname,
                        lastName = user.Lastname,
                        santaClausID = foundSanta.Santaclausid,
                        userID = foundSanta.Userid,
                        workshops = wokrshopDTOs
                    };
                    return Ok(santaDTO);

                }
            }
            return BadRequest();
        }

        [HttpPost("LoginSanta")]
        public ActionResult LoginSanta([Required] int santaID, [Required] string password)
        {
            Santaclau? foundSanta = _context.Santaclaus.FirstOrDefault(s => s.Santaclausid == santaID);
            if (foundSanta != null)
            {
                User? user = _context.Users.FirstOrDefault(s => s.Userid == foundSanta.Userid);
                if (user != null)
                {
                    if (user.Password == password)
                    {
                        return Ok();
                    }
                }
            }
            return BadRequest();
        }

        // GET: SantaController/Create
        [HttpPost("CreateSanta")]
        public ActionResult CreateSanta(string firstName, string lastName, string password, DateOnly birthday)
        {
            User newUser = new User()
            {
                Firstname = firstName,
                Lastname = lastName,
                Password = password,
                Dateofbirth = birthday,
                Dateofregistration = new DateOnly()
            };

            newUser = _context.Users.Add(newUser).Entity;
            Santaclau newSanta = new Santaclau()
            {
                Userid = newUser.Userid,
            };
            _context.Santaclaus.Add(newSanta);
            return Ok();
        }

        [HttpPost("CreateElf")]
        public ActionResult CreateElf(string firstName, string lastName, string password, DateOnly birthday, int workshop, int role)
        {
            User newUser = new User()
            {
                Firstname = firstName,
                Lastname = lastName,
                Password = password,
                Dateofbirth = birthday,
                Dateofregistration = new DateOnly()
            };
            newUser = _context.Users.Add(newUser).Entity;
            Elf newElf = new Elf()
            {
                Userid = newUser.Userid,
                Workshopid = workshop,
                Roleid = role
            };
            _context.Elves.Add(newElf);

            return Ok();
        }

        [HttpPost("CreateShifts")]
        public ActionResult CreateShifts(int numberOfElfs)
        {
            var startDate = new DateOnly();
            var weekDay = startDate.DayOfWeek;
            if (weekDay != DayOfWeek.Monday)
            {
                startDate = startDate.AddDays(8 - ((int)weekDay));
            }
            var endDate = startDate.AddDays(7);
            var shiftTimes = _context.Shifttypes;

            foreach (Shifttype shiftType in shiftTimes)
            {
                while (startDate.CompareTo(endDate) < 0)
                {
                    var datetimeStart = startDate.ToDateTime(shiftType.Starttime);
                    var datetimeEnd = startDate.ToDateTime(shiftType.Endtime);
                    var freeElfs = (from e in _context.Elves
                                    join ub in _context.Unavailableblocks on e.Elfid equals ub.Elfid
                                    where (ub.Starttime.CompareTo(datetimeStart) > 0 && ub.Endtime.CompareTo(datetimeEnd) > 0 &&
                                    ub.Starttime.CompareTo(datetimeStart) < 0 && ub.Endtime.CompareTo(datetimeEnd) < 0 &&
                                    ub.Starttime.CompareTo(datetimeStart) != 0 && ub.Endtime.CompareTo(datetimeEnd) != 0)
                                    select e).ToList();
                    for (int i = 0; i < numberOfElfs; i++)
                    {
                        Shiftblock newShift = new Shiftblock()
                        {
                            Date = startDate,
                            Elfid = freeElfs[i].Elfid,
                            Shifttypeid = shiftType.Shifttypeid,
                        };
                        _context.Shiftblocks.Add(newShift);
                    }
                    startDate.AddDays(1);
                }

            }






            return Ok();
        }

        // GET: SantaController/Edit/5
        [HttpPost("Edit")]
        public ActionResult Edit(int id)
        {
            return Ok();
        }

        // GET: SantaController/Delete/5
        [HttpPost("Delete")]
        public ActionResult Delete(int id)
        {
            return Ok();
        }

    }
}
