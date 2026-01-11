using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

            return Created($"/notes/{note.Id}", new
            {
                id = note.Id,
                content = note.Content
            });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateNote(int id, CreateNoteRequest request)
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var note = _context.Notes
                .FirstOrDefault(n => n.Id == id && n.UserId == userId);

            if (note == null)
            {
                return NotFound();
            }

            note.Content = request.Content;
            _context.SaveChanges();

            return Ok(new
            {
                id = note.Id,
                content = note.Content
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteNote(int id)
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var note = _context.Notes
                .FirstOrDefault(n => n.Id == id && n.UserId == userId);

            if (note == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(note);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
