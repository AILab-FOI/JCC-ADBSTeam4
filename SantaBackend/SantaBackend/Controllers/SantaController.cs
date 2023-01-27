using Microsoft.AspNetCore.Mvc;
using Npgsql;
using SantaBackend.DTO;
using SantaBackEnd;
using SantaBackEnd.Database;
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

        [HttpGet("listElvesWorkshop")]
        public ActionResult listElvesWorkshop(int workshopID)
        {
            var elves = (from e in _context.Elves
                         where e.Workshopid == workshopID
                         select e).ToList();
            return Ok(elves);
        }

        [HttpPost("addElvesWorkshop")]
        public ActionResult addElvesWorkshop(int workshopID, int numOfElves)
        {
            var elves = (from e in _context.Elves
                         where e.Workshopid == null
                         select e).ToList();
            for (int i = 0; i < numOfElves; i++)
            {
                elves[i].Workshopid = workshopID;
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("generateElfs")]
        public ActionResult generateElfs(int minindex, int maxIndex)
        {
            var generator = new DataGenerator();
            generator.generataElves(minindex, maxIndex);
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

        [HttpPost("LoginElf")]
        public ActionResult LoginElf([Required] string username, [Required] string password)
        {
            User? user = _context.Users.FirstOrDefault(s => s.username == username);
            if (user != null)
            {
                Elf? foundElf = _context.Elves.FirstOrDefault(s => s.Elfid == user.Userid);
                if (foundElf != null)
                {
                    if (user.Password == password)
                    {
                        return Ok(user.Userid);
                    }
                }
            }
            return BadRequest();
        }

        [HttpPost("LoginSanta")]
        public ActionResult LoginSanta([Required] string username, [Required] string password)
        {
            User? user = _context.Users.FirstOrDefault(s => s.username == username);
            if (user != null)
            {
                Santaclau? foundSanta = _context.Santaclaus.FirstOrDefault(s => s.Santaclausid == user.Userid);
                if (foundSanta != null)
                {
                    if (user.Password == password)
                    {
                        return Ok(foundSanta);
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

            _context.Users.Add(newUser);
            _context.SaveChanges();
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
                                    join sb in _context.Shiftblocks on e.Elfid equals sb.Elfid
                                    where (ub.Starttime.CompareTo(datetimeStart) > 0 && ub.Endtime.CompareTo(datetimeEnd) > 0 &&
                                    ub.Starttime.CompareTo(datetimeStart) < 0 && ub.Endtime.CompareTo(datetimeEnd) < 0 &&
                                    ub.Starttime.CompareTo(datetimeStart) != 0 && ub.Endtime.CompareTo(datetimeEnd) != 0 &&
                                    !sb.Date.Equals(startDate))
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

        [HttpPost("AddShift")]
        public ActionResult AddShift(int elfID, DateOnly date, int shiftTypeID)
        {
            Elf? Elf = _context.Elves.FirstOrDefault(e => e.Elfid == elfID);
            if (Elf != null)
            {
                bool unavailable = _context.Shiftblocks.Select(s => s.Date == date).FirstOrDefault();
                if (unavailable)
                {
                    TimeOnly shiftStart = _context.Shifttypes.FirstOrDefault(s => s.Shifttypeid == shiftTypeID).Starttime;
                    TimeOnly shiftEnd = _context.Shifttypes.FirstOrDefault(s => s.Shifttypeid == shiftTypeID).Endtime;
                    DateTime start = date.ToDateTime(shiftStart);
                    DateTime end = date.ToDateTime(shiftEnd);
                    var blocked = (from u in _context.Unavailableblocks
                                   where (u.Elfid == elfID && date == DateOnly.FromDateTime(u.Starttime) &&
                                   u.Starttime.CompareTo(start) > 0 && u.Endtime.CompareTo(end) > 0 &&
                                    u.Starttime.CompareTo(start) < 0 && u.Endtime.CompareTo(end) < 0 &&
                                    u.Starttime.CompareTo(start) != 0 && u.Endtime.CompareTo(end) != 0)
                                   select u);
                    if (blocked != null)
                    {
                        Shiftblock newShift = new Shiftblock()
                        {
                            Elfid = elfID,
                            Date = date,
                            Shifttypeid = shiftTypeID,
                        };
                        _context.Shiftblocks.Add(newShift);
                        _context.SaveChanges();
                        return Ok(newShift);
                    }

                }
            }
            return BadRequest();
        }

        [HttpPost("RemoveShift")]
        public ActionResult RemoveShift(int shiftID)
        {
            Shiftblock? block = _context.Shiftblocks.FirstOrDefault(s => s.Shiftid == shiftID);
            if (block == null)
            {
                return BadRequest();
            }
            else
            {
                _context.Shiftblocks.Remove(block);
                _context.SaveChanges();
                return Ok();
            }
        }

        [HttpGet("addUnavailable")]
        public ActionResult addUnavailable(int elfID, DateTime start, DateTime end)
        {
            Unavailableblock unavail = new Unavailableblock()
            {
                Elfid = elfID,
                Starttime = start,
                Endtime = end
            };
            _context.Unavailableblocks.Add(unavail);
            _context.SaveChanges();
            return Ok(unavail);
        }

        [HttpGet("getElfShifts")]
        public ActionResult getElfShifts(int elfID, DateOnly start)
        {
            var startDate = start;
            var weekDay = startDate.DayOfWeek;
            if (weekDay != DayOfWeek.Monday)
            {
                startDate = startDate.AddDays(8 - ((int)weekDay));
            }
            var endDate = startDate.AddDays(7);

            Elf? elf = _context.Elves.FirstOrDefault(e => e.Elfid == elfID);
            if (elf != null)
            {
                var shifts = (from s in _context.Shiftblocks
                              where (s.Elfid == elfID && s.Date >= startDate || s.Date <= endDate)
                              select s);
                var unavail = (
                         from u in _context.Unavailableblocks
                         where (u.Elfid == elfID && DateOnly.FromDateTime(u.Starttime) >= startDate && DateOnly.FromDateTime(u.Endtime) <= endDate)
                         select u);
                elf.Shiftblocks = (ICollection<Shiftblock>)shifts;
                elf.Unavailableblocks = (ICollection<Unavailableblock>)unavail;
                return Ok(elf);
            }
            return BadRequest();
        }

        [HttpGet("getWorkshopShifts")]
        public ActionResult getWorkshopShifts(int workshopID, DateOnly start)
        {
            var startDate = start;
            var weekDay = startDate.DayOfWeek;
            if (weekDay != DayOfWeek.Monday)
            {
                startDate = startDate.AddDays(8 - ((int)weekDay));
            }
            var endDate = startDate.AddDays(7);

            Workshop? workshop = _context.Workshops.FirstOrDefault(w => w.Workshopid == workshopID);
            if (workshop != null)
            {
                var elfs = (from e in _context.Elves
                            where e.Workshopid == workshopID
                            select e);

                foreach (var elf in elfs)
                {
                    var shifts = (from s in _context.Shiftblocks
                                  where (s.Elfid == elf.Elfid && s.Date >= startDate || s.Date <= endDate)
                                  select s);
                    elf.Shiftblocks = (ICollection<Shiftblock>)shifts;
                }
                workshop.Elves = (ICollection<Elf>)elfs;
                return Ok(workshop);
            }
            return BadRequest();
        }
    }
}
