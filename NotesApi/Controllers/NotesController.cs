using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApi.Data;
using NotesApi.Dtos;
using NotesApi.Models;
using System.Security.Claims;

namespace NotesApi.Controllers
{
    [ApiController]
    [Route("notes")]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetNotes()
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var notes = _context.Notes
                .Where(n => n.UserId == userId)
                .Select(n => new
                {
                    id = n.Id,
                    content = n.Content
                })
                .ToList();

            return Ok(notes);
        }

        [HttpPost]
        public IActionResult CreateNote(CreateNoteRequest request)
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );
            
            var note = new Note
            {
                Content = request.Content,
                UserId = userId
            };

            _context.Notes.Add(note);
            _context.SaveChanges();

            return Ok(new
            {
                id = note.Id,
                content = note.Content
            });
        }
    }
}
