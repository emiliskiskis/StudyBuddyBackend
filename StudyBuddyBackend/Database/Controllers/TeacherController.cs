using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyBuddyBackend.Database.Entities;
using StudyBuddyBackend.Identity;

namespace StudyBuddyBackend.Database.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/teachers")]
    public class TeacherController : ControllerBase
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly IIdentityService _identityService;

        public TeacherController(IDatabaseContext databaseContext, IIdentityService identityService)
        {
            _databaseContext = databaseContext;
            _identityService = identityService;
        }

        [HttpPost("{username}")]
        public ActionResult MakeTeacher(string username)
        {
            if (username != _identityService.GetUsername(HttpContext)) return Unauthorized();

            if (TeacherExists(username)) return Conflict();

            // Add the teacher and return the created teacher
            _databaseContext.TeacherInfo.Add(new TeacherInfo(username));
            User user = _databaseContext.Users.Find(username);

            if (user == null)
            {
                return NotFound();
            }

            user.IsTeacher = true;
            _databaseContext.Users.Update(user);
            _databaseContext.SaveChanges();
            return Ok();
        }

        private bool TeacherExists(string username)
        {
            // Does any teacher with specified username exist?
            return _databaseContext.TeacherInfo.Any(u => u.Username == username);
        }

        [HttpGet]
        public ActionResult<IEnumerable<object>> GetAllSubjects(int size, string q = "")
        {
            // Return all subjects or subjects that starts with specific leters
            return _databaseContext.Subjects.Where(subject => subject.Name.Trim()
                                                                          .ToLower()
                                                                          .StartsWith(q.Trim().ToLower()))
                                            .Take(size)
                                            .Select(subject => new { subject.Name })
                                            .ToList();
        }

        [HttpPost("{username}/subjects")]
        public IActionResult AddSubjectToTeacher(Subject subject, string username)
        {
            if (username != _identityService.GetUsername(HttpContext)) return Unauthorized();

            TeacherInfo teacher = _databaseContext.TeacherInfo.Find(username);

            if (teacher == null)
            {
                return NotFound();
            }

            _databaseContext.TeacherSubjects.Add(new TeacherSubject(teacher, subject));
            _databaseContext.SaveChanges();
            return Ok();
        }
    }
}
