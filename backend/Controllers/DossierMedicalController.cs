using Microsoft.AspNetCore.Mvc;
using projet_1.Models;
using projet_1.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Font;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using iText.Layout.Font;
using iText.IO.Font.Constants;



namespace projet_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DossierMedicalController : ControllerBase
    {
        private readonly DossierMedicalService _dossierMedicalService;
        private readonly EncryptionService _encryptionService;
        private readonly string _pdfDirectory = Path.Combine(Directory.GetCurrentDirectory(), "GeneratedPdfs");

        public DossierMedicalController(DossierMedicalService dossierMedicalService, EncryptionService encryptionService)
        {
            _dossierMedicalService = dossierMedicalService;
            _encryptionService = encryptionService;

            if (!Directory.Exists(_pdfDirectory))
                Directory.CreateDirectory(_pdfDirectory);
        }

        // GET: api/DossierMedical
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DossierMedical>>> GetDossierMedical()
        {
            var dossiers = await _dossierMedicalService.GetAllDossierMedicalAsync();
            foreach (var dossier in dossiers) DecryptFields(dossier);
            return Ok(dossiers);
        }

        // GET: api/DossierMedical/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DossierMedical>> GetDossierMedical(int id)
        {
            var dossier = await _dossierMedicalService.GetDossierMedicalByIdAsync(id);
            if (dossier == null) return NotFound();
            DecryptFields(dossier);
            return Ok(dossier);
        }

        // POST: api/DossierMedical
        [HttpPost]
        public async Task<ActionResult<DossierMedical>> CreateDossierMedical(DossierMedical dossierMedical)
        {
            try
            {
                EncryptFields(dossierMedical);
                var created = await _dossierMedicalService.CreateDossierMedicalAsync(dossierMedical);
                DecryptFields(created);
                await GeneratePdf(created);
                return CreatedAtAction(nameof(GetDossierMedical), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/DossierMedical/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDossierMedical(int id, DossierMedical dossierMedical)
        {
            if (id != dossierMedical.Id)
                return BadRequest("L'ID ne correspond pas.");

            // Encrypt fields before updating
            EncryptFields(dossierMedical);

            // Update the dossier
            var result = await _dossierMedicalService.UpdateDossierMedicalAsync(dossierMedical);

            if (!result)
                return NotFound("Dossier non trouvé.");

            // Decrypt fields after updating
            DecryptFields(dossierMedical);

            // Generate PDF after updating
            await GeneratePdf(dossierMedical);

            return Ok(new { message = "Dossier mis à jour et PDF généré." });
        }


        // DELETE: api/DossierMedical/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDossierMedical(int id)
        {
            var result = await _dossierMedicalService.DeleteDossierMedicalAsync(id);
            if (!result) return NotFound();
            var pdfPath = GetPdfPath(id);
            if (System.IO.File.Exists(pdfPath))
                System.IO.File.Delete(pdfPath);
            return NoContent();
        }

        // GET: api/DossierMedical/pdf/5
        [HttpGet("pdf/{id}")]
        public IActionResult GetPdf(int id)
        {
            var filePath = GetPdfPath(id);
            if (!System.IO.File.Exists(filePath))
                return NotFound("PDF non trouvé.");
            var pdfBytes = System.IO.File.ReadAllBytes(filePath);
            return File(pdfBytes, "application/pdf", $"dossier_{id}.pdf");
        }

        // Helper method to get the PDF path
        private string GetPdfPath(int id)
        {
            return Path.Combine(_pdfDirectory, $"dossier_{id}.pdf");
        }

        // PDF generation
        private async Task GeneratePdf(DossierMedical dossier)
        {
            var filePath = GetPdfPath(dossier.Id);

            using (var writer = new PdfWriter(filePath))
            using (var pdf = new iText.Kernel.Pdf.PdfDocument(writer))
            using (var document = new Document(pdf))
            {
                // Set the font for bold text (using built-in font for simplicity)
                //var font = PdfFontFactory.CreateFont(iText.Kernel.Font.PdfFontFactory.HELVETICA_BOLD); // Correct reference
                var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                // Add content to the PDF
                document.Add(new Paragraph("Dossier Médical").SetFont(font).SetFontSize(18));
                document.Add(new Paragraph($"ID: {dossier.Id}"));
                document.Add(new Paragraph($"Taille: {dossier.Taille}"));
                document.Add(new Paragraph($"Poids: {dossier.Poids}"));
                document.Add(new Paragraph($"Groupe Sanguin: {dossier.GroupeSanguin}"));
                document.Add(new Paragraph($"Antécédents: {dossier.Antecedents}"));
                document.Add(new Paragraph($"Traitements: {dossier.Traitements}"));
                document.Add(new Paragraph($"Allergies: {dossier.Allergies}"));
                document.Add(new Paragraph($"Observations: {dossier.Observations}"));
                document.Add(new Paragraph($"Date de génération: {DateTime.Now}"));
            }

            await Task.CompletedTask;
        }


        private void EncryptFields(DossierMedical d)
        {
            d.Taille = _encryptionService.Encrypt(d.Taille ?? string.Empty);
            d.Poids = _encryptionService.Encrypt(d.Poids ?? string.Empty);
            d.GroupeSanguin = _encryptionService.Encrypt(d.GroupeSanguin ?? string.Empty);
            d.Antecedents = _encryptionService.Encrypt(d.Antecedents ?? string.Empty);
            d.Traitements = _encryptionService.Encrypt(d.Traitements ?? string.Empty);
            d.Allergies = _encryptionService.Encrypt(d.Allergies ?? string.Empty);
            d.Observations = _encryptionService.Encrypt(d.Observations ?? string.Empty);
        }


        private void DecryptFields(DossierMedical d)
        {
            d.Taille = _encryptionService.Decrypt(d.Taille);
            d.Poids = _encryptionService.Decrypt(d.Poids);
            d.GroupeSanguin = _encryptionService.Decrypt(d.GroupeSanguin);
            d.Antecedents = _encryptionService.Decrypt(d.Antecedents);
            d.Traitements = _encryptionService.Decrypt(d.Traitements);
            d.Allergies = _encryptionService.Decrypt(d.Allergies);
            d.Observations = _encryptionService.Decrypt(d.Observations);
        }
        // GET: api/DossierMedical/pdf
        [HttpGet("pdf")]
        public IActionResult GetAllPdfs()
        {
            if (!Directory.Exists(_pdfDirectory))
                return NotFound("Le dossier PDF n'existe pas.");

            var pdfFiles = Directory.GetFiles(_pdfDirectory, "*.pdf");

            var pdfList = new List<object>();

            foreach (var file in pdfFiles)
            {
                var fileName = Path.GetFileName(file);
                var fileUrl = $"{Request.Scheme}://{Request.Host}/api/DossierMedical/pdf/{ExtractIdFromFilename(fileName)}";

                pdfList.Add(new
                {
                    FileName = fileName,
                    DownloadUrl = fileUrl
                });
            }

            return Ok(pdfList);
        }

        private int ExtractIdFromFilename(string fileName)
        {
            // Ex: dossier_5.pdf ? 5
            var parts = fileName.Split(new[] { '_', '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2 && int.TryParse(parts[1], out int id))
                return id;
            return 0;
        }

    }
}
