using EJ.SistemaAgendamentos.Data;
using EJ.SistemaAgendamentos.Migrations;
using EJ.SistemaAgendamentos.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Org.BouncyCastle.Tls;

namespace EJ.SistemaAgendamentos.Controllers
{
    public class AgendamentosController : Controller
    {
        private readonly MyDbContext _context;
        public AgendamentosController(MyDbContext context)
        {
            _context = context;
        }
        // GET: AgendamentosController
        public ActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllAsJSON(){
            var agendamentos = _context.Agendamentos.ToListAsync();

            return Json(agendamentos);
        }

        public async Task<IActionResult> Add(){
            return View(new Agendamento(){
                Start = DateTime.Today,
                End = DateTime.Today.AddHours(1),
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([FromForm]Agendamento agendamento){
            if(ModelState.IsValid){
                if(agendamento.Start > agendamento.End)
                {
                    TempData["Errors"] = "Data inicial não pode ser maior que data final.";

                    return View(agendamento);
                }

                var overlappingAgendamento = await _context.Agendamentos
                                                            .Where(a => agendamento.Start >= a.Start && agendamento.Start < a.End
                                                                        || (agendamento.End >= a.Start && agendamento.End < a.End))
                                                            .FirstOrDefaultAsync();

                if(overlappingAgendamento != null){
                    TempData["Errors"] = "Já Existe um agendamento para essa data e hora";

                    return View(agendamento);
                }

                _context.Agendamentos.Add(agendamento);
                await _context.SaveChangesAsync();
                await NotificarPorEmail(agendamento);
                
                return RedirectToAction("Index");
            }

            return View(agendamento);

        }

        public async Task<IActionResult> Details([FromRoute]int id){
            var agendamento = await _context.Agendamentos.FindAsync(id);

            if(agendamento != null){
                return PartialView("DetalhesPartial", agendamento);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm]int id){
            var agendamento = await _context.Agendamentos.FindAsync(id);

            if(agendamento != null){
                _context.Entry(agendamento).State = EntityState.Deleted;

                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("Index", "Agendamentos");
        }

        private async Task NotificarPorEmail(Agendamento agendamento)
        {
            var message = new MimeMessage ();
            message.From.Add (new MailboxAddress ("No-Reply", "eduardo.dias092@gmail.com"));
            message.To.Add (new MailboxAddress ("Admin", "eduardo.dias092@gmail.com"));
            message.Subject = "Novo Agendamento";

            message.Body = new TextPart ("html") {
                Text = $@"<body style='font-family: sans-serif'>
                <h1>Um novo agendamento foi criado:</h1>
                <dl>
                <dt>Título:</dt> <dd>{agendamento.Title}</dd>
                <dt>Data:</dt> <dd>{agendamento.Start:dd/MM/yyyy HH:mm} - {agendamento.End:dd/MM/yyyy HH:mm}</dd>
                <dt>Descrição:</dt> <dd>{agendamento.Description}</dd>
                
                -- 
                </body>
                "
            };

            using var client = new SmtpClient();
            client.Connect("sandbox.smtp.mailtrap.io", 465, SecureSocketOptions.StartTls);
            client.Authenticate("109cf7c4fa376c", "277e8654cd9ae4");
            await client.SendAsync(message);
        }

    }
}
