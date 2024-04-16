using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using MvcCoreSasStorage.Helpers;
using MvcCoreSasStorage.Models;

namespace MvcCoreSasStorage.Controllers
{
    public class MigracionController : Controller
    {
        private HelperXml helper;

        public MigracionController(HelperXml helper)
        {
            this.helper = helper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string algo)
        {
            string azureKeys =
                "UseDevelopmentStorage=true";
            TableServiceClient serviceClient =
                new TableServiceClient(azureKeys);
            TableClient tableClient =
                serviceClient.GetTableClient("alumnos");
            await tableClient.CreateIfNotExistsAsync();
            List<Alumno> alumnos = this.helper.GetAlumnos();
            foreach (Alumno alum in alumnos)
            {
                await tableClient.AddEntityAsync<Alumno>(alum);
            }
            ViewData["MENSAJE"] = "Migración completa en Azure";
            return View();
        }

    }
}
