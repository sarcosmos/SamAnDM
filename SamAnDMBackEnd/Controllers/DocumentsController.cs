using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SamAnDMBackEnd.Attributes;
using SamAnDMBackEnd.DTO;
using SamAnDMBackEnd.Model;
using SamAnDMBackEnd.Service;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace SamAnDMBackEnd.Controllers
{
    [ApiController]
    [Authorize]
    [Permission("Documents")]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IHistoricService _historicService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DocumentsController(IDocumentService documentService, IHistoricService historicService, IHttpContextAccessor httpContextAccessor)
        {
            _documentService = documentService;
            _historicService = historicService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Documents>>> GetAllDocuments()
        {
            var documents = await _documentService.GetAllDocumentsAsync();
            return Ok(documents);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocument(int id)
        {
            var document = await _documentService.GetDocumentByIdAsync(id);
            if (document == null) return NotFound();

            return File(document.DocumentContent, "application/octet-stream", document.DocumentName);
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<Documents>>> GetUserDocuments()
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var documents = await _documentService.GetUserDocumentsAsync(userId);
            return Ok(documents);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserDocumentById(int id)
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var document = await _documentService.GetUserDocumentByIdAsync(id, userId);
            if (document == null) return NotFound("Documento no encontrado o no pertenece al usuario.");

            return File(document.DocumentContent, "application/octet-stream", document.DocumentName);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(int id, [FromForm] DocumentsUpdateDto documentUpdateDto)
        {
        
            // Llamar a UpdateDocumentAsync y obtener el documento actualizado
            var document = await _documentService.UpdateDocumentAsync(id, documentUpdateDto);

            if (document == null)
            {
                return NotFound("Documento no encontrado.");
            }

            // Obtener el userId desde el contexto HTTP
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("No se encontró el ID de usuario en el token.");
            }

            int userId = int.Parse(userIdClaim.Value);

            // Registrar en histórico
            await _historicService.RegisterHistoricAsync(document.DocumentId, "UPDATE", userId, "Referencia de actualización de documento");

            return Ok(document);
        }


        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocuments([FromForm] DocumentsUploadDto documentsUploadDto)
        {
            var document = await _documentService.UploadDocumentsAsync(documentsUploadDto);

            if (document == null)
            {
                return BadRequest("Error al crear el documento.");
            }

            // Obtener el userId desde el contexto HTTP
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("No se encontró el ID de usuario en el token.");
            }

            int userId = int.Parse(userIdClaim.Value);

            // Registrar en histórico
            await _historicService.RegisterHistoricAsync(document.DocumentId, "CREATE", userId, "Referencia de creación");

            return CreatedAtAction(nameof(GetDocument), new { id = document.DocumentId }, document);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            await _documentService.SoftDeleteDocumentAsync(id);

            // Registrar en histórico
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await _historicService.RegisterHistoricAsync(id, "DELETE", userId, "Referencia de eliminación");
            return Ok("Documento eliminado con éxito");
        }
    }
}
