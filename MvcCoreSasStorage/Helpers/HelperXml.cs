using MvcCoreSasStorage.Models;
using System.Xml.Linq;

namespace MvcCoreSasStorage.Helpers
{
    public class HelperXml
    {
        private XDocument document;
        public HelperXml()
        {
            string assemblyPath =
"MvcAzureCoreSaSTablesStorage.Documents.alumnos_tables.xml";
            Stream stream =
                this.GetType().Assembly
                .GetManifestResourceStream(assemblyPath);
            this.document = XDocument.Load(stream);
        }

        public List<Alumno> GetAlumnos()
        {
            var consulta = from datos in
                               this.document.Descendants("alumno")
                           select new Alumno
                           {
                               IdAlumno =
                               int.Parse(datos.Element("idalumno").Value),
                               Nombre = datos.Element("nombre").Value,
                               Apellidos = datos.Element("apellidos").Value,
                               Curso = datos.Element("curso").Value,
                               Nota = int.Parse(datos.Element("nota").Value)
                           };
            return consulta.ToList();
        }

    }
}
