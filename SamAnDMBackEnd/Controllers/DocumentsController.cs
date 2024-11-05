using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SamAnDMBackEnd.Attributes;
using SamAnDMBackEnd.DTO;
using SamAnDMBackEnd.Model;
using SamAnDMBackEnd.Service;

namespace SamAnDMBackEnd.Controllers
{
    [ApiController]
    [Authorize]
    [Permission("Documents")]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentsController(IDocumentService documentService)
        {
            _documentService = documentService;
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


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(int id, [FromForm] DocumentsUploadDto documentUploadDto)
        {
            await _documentService.UpdateDocumentAsync(id, documentUploadDto);
            return Ok("Documento actualizado con éxito");
        }


        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocuments([FromForm] DocumentsUploadDto documentsUploadDto)
        {
            await _documentService.UploadDocumentsAsync(documentsUploadDto);
            return Ok("Documento subido con éxito");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            await _documentService.SoftDeleteDocumentAsync(id);
            return Ok("Documento eliminado con éxito");
        }
    }
}
